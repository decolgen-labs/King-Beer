using NOOD;
using NOOD.Data;

namespace Game
{
    public class MoneyManager : MonoBehaviorInstance<MoneyManager>
    {
        #region Variables
        const string SAVE_ID = "Money";
        private int _money;
        private int _tip;
        #endregion

        #region Unity functions
        protected override void ChildAwake()
        {
            Load();
        }
        #endregion

        #region Get money
        public int GetMoney() => _money;
        public int GetTip() => _tip;
        #endregion

        #region Pay money
        public bool TryPayMoney(int amount)
        {
            if (_money >= amount)
            {
                _money -= amount;
                UIManager.Instance.UpdateMoney();
                Save();
                return true;
            }
            else
                return false;
        }
        #endregion

        #region Add Remove money
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
        public void AddTipMoney(int amount)
        {
            _tip += amount;
            UIManager.Instance.UpdateMoney();
        }
        #endregion

        #region Save load
        public void Save()
        {
            _money.SaveWithId(SAVE_ID);
        }
        private void Load()
        {
            _money = DataManager<int>.LoadDataFromPlayerPrefWithGenId(SAVE_ID, 0);
        }
        #endregion
    }

}
