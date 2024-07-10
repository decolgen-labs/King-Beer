using System;
using NOOD;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using NOOD.Sound;
using EasyTransition;

namespace Game
{
    public class UIManager : MonoBehaviorInstance<UIManager>
    {
        #region Events
        public Action OnStorePhrase;
        public Action OnNextDayPressed;
        #endregion

        [Header("In game menu")]
        [SerializeField] private GameObject _ingameMenu;
        [SerializeField] private TextMeshProUGUI _moneyText;
        [SerializeField] private TextMeshProUGUI _timeText;
        [SerializeField] private Image _timeBG;
        [SerializeField] private TextMeshProUGUI _dayText;
        [SerializeField] private CustomButton _pauseGameBtn;
        private Color _timeOriginalColor;

        [Header("End day menu")]
        [SerializeField] private GameObject _endDayMenu;
        [SerializeField] private TextMeshProUGUI _money;
        [SerializeField] private CustomButton _shopBtn, _nextDayBtn;
        [SerializeField] private float _moneyIncreaseSpeed = 30;

        [Header("Pause game menu")]
        [SerializeField] private GameObject _pauseGameMenu;
        [SerializeField] private TextMeshProUGUI _pMoneyText, _pDayText;
        [SerializeField] private CustomButton _pResumeBtn, _pToMainMenuBtn;
        [SerializeField] private TransitionSettings _transitionSetting;

        [Header("Store")]
        [SerializeField] private GameObject _storeMenu;
        [SerializeField] private CustomButton _confirmButton;
        private bool _isStorePhrase;

        #region Unity functions
        protected override void ChildAwake()
        {
            _endDayMenu.SetActive(false);
            _pauseGameMenu.SetActive(false);
            _pauseGameBtn.OnClick += OnPauseButtonClickHandler;
            _pResumeBtn.OnClick += OnResumeClickHandler;
            _shopBtn.OnClick += () =>
            {
                ActiveStorePhrase();
                _isStorePhrase = true;
            };
            _pToMainMenuBtn.OnClick += OnMainMenuClickHandler;
            _nextDayBtn.OnClick += () =>
            {
                OnNextDayPressed?.Invoke();
                UpdateDayText();
            };
            _confirmButton.OnClick += () => 
            {
                OpenEndDayPanel();
                HideStoreMenu();
                _isStorePhrase = false;
            };
            HideStoreMenu();
            OnNextDayPressed += HideEndDayPanel;
            _timeOriginalColor = _timeBG.color;
        }
        private void Start()
        {
            UpdateDayText();
            UpdateMoney();
            _timeBG.color = _timeOriginalColor;
            TimeManager.Instance.OnTimeWarning += () => 
            { 
                _timeBG.color = Color.red; 
            };
            GameplayManager.Instance.OnNextDay += () =>
            {
                _timeBG.color = _timeOriginalColor;
            };
            GameplayManager.Instance.OnEndDay += OnEndDayHandler;
            TimeManager.OnTimePause += ShowPauseGameMenu;
            TimeManager.OnTimeResume += HidePauseGameMenu;
        }
        private void Update()
        {
            if (GameplayManager.Instance.IsEndDay) return;
            UpdateTime();
        }
        void OnDisable()
        {
            NoodyCustomCode.UnSubscribeAllEvent<GameplayManager>(this);
            NoodyCustomCode.UnSubscribeAllEvent<TimeManager>(this);
            NoodyCustomCode.UnSubscribeFromStatic(typeof(TimeManager), this);
        }
        private void OnDestroy()
        {
            OnNextDayPressed -= HideEndDayPanel;
            _shopBtn.OnClick -= ActiveStorePhrase;
            _endDayMenu.transform.DOKill();
        }
        #endregion

        #region In game
        public void UpdateDayText()
        {
            _dayText.text = "Day".GetText() + " " + TimeManager.Instance.GetCurrentDay().ToString("00");
        }
        public void UpdateMoney()
        {
            _moneyText.text = MoneyManager.Instance.GetMoney().ToString();
        }
        public void UpdateTime()
        {
            float hour = TimeManager.Instance.GetHour();
            float minute = TimeManager.Instance.GetMinute();

            _timeText.text = $"{hour.ToString("00")}:{minute.ToString("00")}";
        }
        private void HideIngameMenu()
        {
            _ingameMenu.SetActive(false);
        }
        private void ShowIngameMenu()
        {
            _ingameMenu.SetActive(true);
        }
        #endregion

        #region End game
        private void OnEndDayHandler()
        {
            OpenEndDayPanel();
            MoneyManager.Instance.Save();
        }
        private void OpenEndDayPanel()
        {
            PlayMoneyAnimation(MoneyManager.Instance.GetMoney());
            SoundManager.PlaySound(SoundEnum.MoneySound);
            _ingameMenu.SetActive(false);
            _endDayMenu.SetActive(true);
            _endDayMenu.transform.DOScale(Vector3.one, 0.7f);
            EventSystem.current.SetSelectedGameObject(null);
        }
        private void HideEndDayPanel()
        {
            _ingameMenu.SetActive(true);
            _endDayMenu.transform.DOScale(Vector3.zero, 0.7f).OnComplete(() => _endDayMenu.SetActive(false));
        }
        private void PlayMoneyAnimation(int money)
        {
            float time = 0;
            float temp = 0;
            NoodyCustomCode.StartUpdater(this, () =>
            {
                time += Time.unscaledDeltaTime * _moneyIncreaseSpeed;
                temp = Mathf.Lerp(0, money, time/SoundManager.GetSoundLength(SoundEnum.MoneySound));
                _money.text = temp.ToString("0");
                return temp > money;
            });
        }
        private void ActiveStorePhrase()
        {
            HideEndDayPanel();
            ShowStoreMenu();
            OnStorePhrase?.Invoke();
        }
        #endregion

        #region Store Menu
        private void ShowStoreMenu()
        {
            _storeMenu.SetActive(true);
        }
        private void HideStoreMenu()
        {
            _storeMenu.SetActive(false);
        }
        #endregion

        #region PauseGame
        private void ShowPauseGameMenu()
        {
            _pauseGameMenu.SetActive(true);
            HideIngameMenu();
            HideStoreMenu();
            UpdatePauseText();
        }
        private void HidePauseGameMenu()
        {
            _pauseGameMenu.SetActive(false);
            ShowIngameMenu();
            if(_isStorePhrase)
                ShowStoreMenu();
        }
        private void UpdatePauseText()
        {
            _pDayText.text = TimeManager.Instance.GetCurrentDay().ToString("00");
            _pMoneyText.text = MoneyManager.Instance.GetMoney().ToString("0");
        }
        #endregion
 
        #region ButtonZone
        private void OnPauseButtonClickHandler()
        {
            GameplayManager.Instance.OnPausePressed?.Invoke();
        }
        private void OnMainMenuClickHandler()
        {
            TransitionManager.Instance().Transition("MainMenu", _transitionSetting, 0.3f);
        }
        private void OnResumeClickHandler()
        {
            HidePauseGameMenu();
            GameplayManager.Instance.OnPausePressed?.Invoke();
        }
        #endregion   
    }
}
