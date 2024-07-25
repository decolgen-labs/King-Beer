using System;
using NOOD;
using NOOD.Data;
using UnityEngine;

namespace Game
{
    public class MoneyManager : MonoBehaviorInstance<MoneyManager>
    {
        const string SAVE_ID = "Money";

        public int CurrentTarget => _currentTarget;
        public int NextTarget => _currentTarget + TimeManager.Instance.CurrentDay * 10;
        public int CurrentTotalMoney => _totalMoney;
        
        private int _totalMoney;
        private int _currentTarget;

        protected override void ChildAwake()
        {
            _totalMoney = DataManager<int>.LoadDataFromPlayerPrefWithGenId(SAVE_ID, 0);
            Debug.Log(_totalMoney);
            _currentTarget = 150;
        }

        void Start()
        {
            GameplayManager.Instance.OnNextDay += GameplayManager_OnNextDayHandler;
        }


        void OnDisable()
        {
            NoodyCustomCode.UnSubscribeAllEvent(GameplayManager.Instance, this);
        }

        private void GameplayManager_OnNextDayHandler()
        {
            _currentTarget = NextTarget;
        }
        public bool PayMoney(int amount)
        {
            if (_totalMoney >= amount)
            {
                _totalMoney -= amount;
                UIManager.Instance.UpdateInDayMoney();
                return true;
            }
            else
                return false;
        }
        /// <summary>
        /// If buying stuff, use PayMoney(int) instead
        /// </summary>
        /// <param name="amount"></param>
        public void RemoveMoney(int amount)
        {
            _totalMoney = amount;
            UIManager.Instance.UpdateInDayMoney();
        }
        public void AddMoney(int amount)
        {
            _totalMoney += amount;
            UIManager.Instance.UpdateInDayMoney();
        }
        public void Save()
        {
            _totalMoney.SaveWithId(SAVE_ID);
        }
    }

}
