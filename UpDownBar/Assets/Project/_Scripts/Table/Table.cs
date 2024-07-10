using System.Collections.Generic;
using UnityEngine;
using Game.Extension;
using NOOD;
using System.Linq;

namespace Game
{
    public class Table : MonoBehaviour
    {
        #region Variables
        [SerializeField] private List<Transform> _availableSeats = new List<Transform>();
        [SerializeField] private List<Transform> _unAvailableSeats = new List<Transform>();

        public int AvailableSeatNumber = 1;

        private Stack<Transform> _lockSeatStack = new Stack<Transform>();
        private bool _unlockAllSeats;
        #endregion

        #region Unity Functions
        private void OnEnable()
        {
            TableManager.Instance.OnReturnSeat += TableManager_OnReturnSeat;
        }
        private void Start()
        {
            while(_availableSeats.Count > AvailableSeatNumber && _unlockAllSeats == false)
            {
                // Deactivate all seats that not be unlocked
                Transform seat = _availableSeats.Last();
                seat.gameObject.SetActive(false);
                _lockSeatStack.Push(seat);
                _availableSeats.Remove(seat);
                Debug.Log($"Start {this.gameObject.name}: " + _lockSeatStack.Count);
            }
        }
        private void OnDestroy()
        {
            NoodyCustomCode.UnSubscribeAllEvent<TableManager>(this);
            NoodyCustomCode.UnSubscribeAllEvent<GameplayManager>(this);
            NoodyCustomCode.UnSubscribeAllEvent<UIManager>(this);
        }
        #endregion

        #region Upgrade Functions    
        public Vector3 GetUpgradePosition()
        {
            Debug.Log("lock seat: " + _lockSeatStack.Count);
            return _lockSeatStack.ToArray()[0].position.ToVector3XZ();
        }
        #endregion

        #region Event functions
        private void TableManager_OnReturnSeat(Transform seat)
        {
            // Check if this seat is belong to this table
            if(_unAvailableSeats.Contains(seat))
            {
                _availableSeats.Add(seat);
                _unAvailableSeats.Remove(seat);
            }
        }
        #endregion
        #region In game functions
        public void SetIsUnlockAllSeats(bool value)
        {
            _unlockAllSeats = value;
        }
        #endregion

        #region Get
        #endregion

        #region Seat
        public Transform GetRandomSeat()
        {
            int r = Random.Range(0, _availableSeats.Count - 1);
            Transform seat = _availableSeats[r];
            _availableSeats.Remove(seat);
            _unAvailableSeats.Add(seat);

            return seat;
        }
        public void UnlockSeat()
        {
            Transform seat = _lockSeatStack.Pop();
            seat.gameObject.SetActive(true);
            _availableSeats.Add(seat);
            TableManager.Instance.OnTableUpgrade?.Invoke(this);
        }
        public void ReturnSeat(Transform seat)
        {
            _unAvailableSeats.Remove(seat);
            _availableSeats.Add(seat);
        }
        #endregion

        public bool IsAvailable()
        {
            return _availableSeats.Count > 0;
        }
    }

}
