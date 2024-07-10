using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game
{
    public class UpgradeUI : MonoBehaviour
    {
        #region Events
        public Action OnUpgradeButtonClick;
        #endregion

        #region Variables
        [SerializeField] private TextMeshProUGUI _moneyText;
        [SerializeField] private CustomButton _upgradeButton;
        private UpgradeBase _upgradeBase;
        #endregion
        
        #region Unity functions
        private void Start()
        {
            _upgradeButton.OnClick += () =>
            {
                UpgradeManager.Instance.Upgrade(_upgradeBase);
            };
        }
        #endregion

        #region Set
        public void SetUpgradeBase(UpgradeBase upgradeBase)
        {
            _upgradeBase = upgradeBase;
        }
        public void SetPosition(Vector3 position)
        {
            this.transform.position = position;
        }
        #endregion

        #region Update UI
        public void UpdateMoneyText()
        {
            _moneyText.text = _upgradeBase.Price.ToString();
        }
        #endregion
    }

}
