using NOOD.Data;
using UnityEngine;

namespace Game
{
    public class SeatUpgrade : UpgradeBase
    {
        #region Variables
        private Table _table;
        #endregion

        #region Unity Functions
        protected override void ChildAwake()
        {
            _table = this.GetComponent<Table>();
            _upgradeAction = new UpgradeAction(() =>
            {
                _table.UnlockSeat();
                _table.AvailableSeatNumber++;
            }, UpgradeComplete);
        }
        protected override void ChildOnEnable()
        {
            base.ChildOnEnable();
        }
        protected override void ChildStart()
        {
            Load();
        }
        protected override void ChildOnDisable()
        {
            base.ChildOnDisable();
        }
        #endregion

        #region Support
        private void MoveToNewPos()
        {
            Vector3 tempPos = _table.GetUpgradePosition();
            Vector3 newPos = new Vector3(tempPos.x, this.transform.position.y, tempPos.z);
            _upgradeUI.SetPosition(newPos);
        }
        #endregion

        public override void ShowUI()
        {
            base.ShowUI();
            MoveToNewPos();
        }

        #region Abstract functions override
        protected override void UpgradeComplete()
        {
            if(CheckAllUpgradeComplete())
            {
                HideUI();
            }
            else
            {
                MoveToNewPos();
            }
        }
        protected override bool CheckAllUpgradeComplete()
        {
            if (_table.AvailableSeatNumber == 4)
            {
                return true;
            }
            else return false;
        }
        protected override void UpdateUpgradeTime()
        {
            _upgradeTime = _table.AvailableSeatNumber;
        }
        protected override string GetSaveId()
        {
            string Id = typeof(SeatUpgrade).ToString() + TableManager.Instance.GetTableList().IndexOf(_table);
            return Id;
        }
        protected override void Save()
        {
            DataManager<int>.SaveToPlayerPrefWithGenId(_upgradeTime, GetSaveId());
        }
        protected override void Load()
        {
            _upgradeTime = DataManager<int>.LoadDataFromPlayerPref(keyName: GetSaveId(), defaultValue: 1);
        }
        #endregion
    }

}
