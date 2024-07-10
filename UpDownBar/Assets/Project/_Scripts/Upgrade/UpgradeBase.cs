using NOOD;
using UnityEditor;
using UnityEngine;

namespace Game
{
    public abstract class UpgradeBase : MonoBehaviour
    {
        #region Variable
        [HideInInspector] public int Price = 100;
        public float PriceMultipler = 200f;
    
        [SerializeField] protected UpgradeUI _upgradeUI;
        [SerializeField] protected UpgradeAction _upgradeAction;
        protected int _upgradeTime = 1;
        #endregion

        #region Unity functions
        protected void Awake()
        {
            _upgradeUI.SetUpgradeBase(this);
            HideUI();
            ChildAwake();
        }
        protected void OnEnable()
        {
            UpgradeManager.Instance.AddUpgradeBase(this);
            ChildOnEnable();
        }
        protected void Start()
        {
            ChildStart();
        }
        protected void OnDisable()
        {
            ChildOnDisable();
        }
        protected virtual void ChildAwake(){}
        protected virtual void ChildStart(){}
        protected virtual void ChildOnDisable(){}
        protected virtual void ChildOnEnable(){}
        #endregion

        #region Abstract functions
        protected abstract void Save();
        protected abstract void Load();
        protected abstract string GetSaveId();
        protected abstract bool CheckAllUpgradeComplete();
        /// <summary>
        /// This function is inherited by child to b/c it is set by child
        /// </summary>
        protected abstract void UpdateUpgradeTime();
        protected abstract void UpgradeComplete();
        #endregion

        #region Virtual functions
        public virtual void PerformUpgrade()
        {
            if(!CheckAllUpgradeComplete())
                _upgradeAction.Invoke();
        }
        /// <summary>
        /// Update price base on own setting of the child upgradeTime
        /// </summary>
        public virtual void UpdatePrice()
        {
            UpdateUpgradeTime();
            Price = (int) (_upgradeTime * PriceMultipler) + 50;
            Save();
        }
        #endregion

        #region Support
        public bool IsUpgradeComplete()
        {
            return CheckAllUpgradeComplete();
        }
        #endregion

        #region Show Hide UI
        public virtual void HideUI()
        {
            _upgradeUI.gameObject.SetActive(false);
        }
        public virtual void ShowUI()
        {
            UpdatePrice();
            _upgradeUI.gameObject.SetActive(true);
            _upgradeUI.UpdateMoneyText();
        }
        #endregion
    }
}








