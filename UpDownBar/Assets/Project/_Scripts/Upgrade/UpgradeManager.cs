using System;
using System.Collections.Generic;
using Codice.Client.BaseCommands;
using NOOD;

namespace Game
{
    public class UpgradeManager : MonoBehaviorInstance<UpgradeManager>
    {
        #region Events
        #endregion

        #region Variables
        private const string DO_NOT_ENOUGH_MONEY_ID = "DontHaveEnoughMoney";
        private List<UpgradeBase> _upgradeBases = new List<UpgradeBase>();
        #endregion

        #region Unity functions
        private void OnEnable()
        {
            UIManager.Instance.OnStorePhrase += UIManager_OnStorePhraseHandler;
            UIManager.Instance.OnNextDayPressed += UIManager_OnNextDayPressedHandler;
        }
        private void OnDisable()
        {
            NoodyCustomCode.UnSubscribeAllEvent<UIManager>(this);
        }
        #endregion

        #region Setup
        public void AddUpgradeBase(UpgradeBase upgradeBase)
        {
            _upgradeBases.Add(upgradeBase);
        }
        #endregion

        #region Event functions
        private void UIManager_OnNextDayPressedHandler()
        {
            HideAllUpgradeUI();
        }
        private void UIManager_OnStorePhraseHandler()
        {
            ShowAvailableUpgrade();
        }
        #endregion

        #region Upgrade functions
        public void Upgrade(UpgradeBase upgradeBase)
        {
            if (MoneyManager.Instance.TryPayMoney(upgradeBase.Price))
            {
                // Preform upgrade
                upgradeBase.PerformUpgrade();
                // Update new price for upgrade base
                upgradeBase.UpdatePrice();
            }
            else
            {
                NotifyManager.Instance.Show(DO_NOT_ENOUGH_MONEY_ID.GetText());
            }
        }
        #endregion

        #region Show Hide
        private void ShowAvailableUpgrade()
        {
            foreach(UpgradeBase upgradeBase in _upgradeBases)
            {
                if(!upgradeBase.IsUpgradeComplete())
                {
                    upgradeBase.ShowUI();
                }
            }
        }
        private void HideAllUpgradeUI()
        {
            foreach(UpgradeBase upgradeBase in _upgradeBases)
            {
                upgradeBase.HideUI();
            }
        }
        #endregion
    }

}
