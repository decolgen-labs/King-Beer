using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NOOD;
using NOOD.Data;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class TableData
    {
        public List<int> SeatNumberList = new List<int>();
    }

    public class TableManager : MonoBehaviorInstance<TableManager>
    {
        #region Variables
        public Action<Table> OnTableUpgrade;
        public Action<Customer> OnCustomComplete;
        public Action<Transform> OnReturnSeat;
        [SerializeField] private List<Table> _tableList = new List<Table>();
        [SerializeField] private bool _unlockAllSeats;
        private int _currentTableIndex = 0;
        private TableData _tableData;
        #endregion

        #region Unity Functions
        protected override void ChildAwake()
        {
            _tableData = DataManager<TableData>.Data;
            for(int i = 0; i < _tableList.Count; i++)
            {
                if(i >= _tableData.SeatNumberList.Count)
                {
                    _tableData.SeatNumberList.Add(1);
                }
                _tableList[i].AvailableSeatNumber = _tableData.SeatNumberList[i];
            }

            OnTableUpgrade += OnTableUpgradeHandler;
        }
        void OnEnable()
        {
            Player.OnPlayerChangePosition += OnPlayerChangePositionHandler;
            foreach(Table table in _tableList)
            {
                table.SetIsUnlockAllSeats(_unlockAllSeats);
            }
        }
        void OnDisable()
        {
            Player.OnPlayerChangePosition -= OnPlayerChangePositionHandler;
        }
        void OnDestroy()
        {
            OnTableUpgrade -= OnTableUpgradeHandler;
        }
        #endregion

        #region Event functions
        private void OnTableUpgradeHandler(Table table)
        {
            int index = _tableList.IndexOf(table);
            _tableData.SeatNumberList[index] += 1;
            DataManager<TableData>.QuickSave();
        }
        private void OnPlayerChangePositionHandler(int index)
        {
            _currentTableIndex = index;
        }
        public void CustomerComplete(Customer customer)
        {
            OnCustomComplete?.Invoke(customer);
        }
        #endregion

        #region Get
        public bool TryGetRandomSeat(out Transform resultSeat)
        {
            resultSeat = null;
            List<Table> availableTable = _tableList.Where(table => table.IsAvailable() == true).ToList();
            if (availableTable != null && availableTable.Count > 0)
            {
                // Get random seat
                int r = UnityEngine.Random.Range(0, availableTable.Count - 1);
                Transform seat = availableTable[r].GetRandomSeat();
                // Check if seat valid
                resultSeat = seat;
                return true;
            }
            return false;
        }
        public List<Table> GetTableList()
        {
            return _tableList;
        }
        public Vector3 GetPlayerTablePosition()
        {
            return _tableList[_currentTableIndex].transform.position;
        }
        public Table GetTable(int index)
        {
            try
            {
                return _tableList[index];
            }catch
            {
                Debug.LogError("table index out of range");
                return null;
            }
        }
        public Table GetPlayerTable()
        {
            return _tableList[_currentTableIndex];
        }
        #endregion

        #region Seat
        public void ReturnSeat(Transform seat)
        {
            OnReturnSeat?.Invoke(seat);
        }
        #endregion

        #region Support
        public bool IsAnyAvailableSeat()
        {
            List<Table> availableTable = _tableList.Where(table => table.IsAvailable() == true).ToList();
            return availableTable != null && availableTable.Count > 0;
        }
        #endregion
    }

}













