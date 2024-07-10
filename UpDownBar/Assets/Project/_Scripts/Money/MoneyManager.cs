using NOOD;
using NOOD.Data;
using UnityEngine;

namespace Game
{
    public class MoneyManager : MonoBehaviorInstance<MoneyManager>
    {
        const string SAVE_ID = "Money";
        private int _money;
        protected override void ChildAwake()
        {
            _money = DataManager<int>.LoadDataFromPlayerPrefWithGenId(SAVE_ID, 0);
            Debug.Log(_money);
        }

        public int GetMoney() => _money;
        public bool PayMoney(int amount)
        {
            if (_money >= amount)
            {
                _money -= amount;
                UIManager.Instance.UpdateMoney();
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
            _money -= amount;
            UIManager.Instance.UpdateMoney();
        }
        public void AddMoney(int amount)
        {
            _money += amount;
            UIManager.Instance.UpdateMoney();
        }
        public void Save()
        {
            _money.SaveWithId(SAVE_ID);
        }
    }

}
