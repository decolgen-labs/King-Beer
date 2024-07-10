using System;

namespace Game
{
    public class UpgradeAction
    {
        #region Variables
        private Action _onComplete;
        private Action _action;
        #endregion
        
        #region Constructor
        public UpgradeAction(Action action, Action onComplete = null)
        {
            this._action = action;
            this._onComplete = onComplete;
        }
        #endregion

        #region Public functions
        public void Invoke()
        {
            _action?.Invoke();
            _onComplete?.Invoke();
        }
        public void OnComplete(Action onComplete)
        {
            this._onComplete = onComplete;
        }
        #endregion
    }
}
