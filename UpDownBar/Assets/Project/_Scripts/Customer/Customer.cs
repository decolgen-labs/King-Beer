using System;
using DG.Tweening;
using NOOD;
using NOOD.Sound;
using Unity.VisualScripting;
using UnityEngine;

namespace Game
{

    public class Customer : MonoBehaviour
    {
        #region Events
        public Action OnCustomerEnterSeat;
        public Action OnCustomerReturn;
        #endregion

        #region Variables
        [Header("Component")]
        [SerializeField] private CustomerView _customerView;

        [Header("Waiting time")]
        [SerializeField] private WaitingUI _waitingUI;
        [SerializeField] private float _maxWaitingTime;
        [SerializeField] private float _waitLossSpeed = 1f;

        private float _waitTimer = 0;
        private float _waitingIntensity = 1;
        private bool _isWaiting;

        [SerializeField] private int _money = 10;
        private Vector3 _targetSeat;
        private float _speed = 4f;
        private float _rotateSpeed = 10;
        private bool _isRequestBeer;
        private bool _isServed;
        private bool _isReturnCalled;
        private Transform _seat = null;
        #endregion

        #region Unity functions
        void OnEnable()
        {
            GameplayManager.Instance.OnEndDay += OnEndDayHandler;
        }
        void Start()
        {
            _isReturnCalled = false;
            // Get random seat from Customer Manager and announce it this script get seat
            _seat = CustomerManager.Instance.GetRandomChair(this);
            SetTargetPosition(_seat.position);
        }
        private void Update()
        {
            if (_seat == null) return;
            Move();    
            if(_isWaiting)
            {
                UpdateWaitTimer();
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if(other.transform.TryGetComponent<BeerCup>(out BeerCup beerCup) && _isServed == false && _isRequestBeer == true)
            {
                Destroy(other.gameObject);
                Complete();
            }
        }
        void OnDisable()
        {
            NoodyCustomCode.UnSubscribeAllEvent<GameplayManager>(this);
        }
        #endregion

        #region Event functions
        private void OnEndDayHandler()
        {
            Return();
        }
        #endregion

        #region Move
        private void Move()
        {
            float distance = Vector3.Distance(this.transform.position, _targetSeat);
            if(distance > 0.1f)
            {
                Vector3 direction = _targetSeat - this.transform.position;
                direction = Vector3.Normalize(direction);
                this.transform.position += direction * _speed * TimeManager.DeltaTime;
                this.transform.DOKill(); // To stop rotate
                this.transform.forward = Vector3.Lerp(this.transform.forward, direction, TimeManager.DeltaTime* _rotateSpeed);
            }
            else
            {
                // Get to seat
                if(!_isRequestBeer)
                {
                    this.transform.DORotate(Vector3.zero, 0.5f); 
                    OnCustomerEnterSeat?.Invoke();
                    _isRequestBeer = true;
                    _isWaiting = true;
                }
            }
        }
        #endregion

        #region Waiting zone
        private void UpdateWaitTimer()
        {
            if(_isWaiting)
            {
                _waitTimer += TimeManager.DeltaTime * _waitLossSpeed;
                _waitingIntensity = _waitTimer / _maxWaitingTime;
                _waitingUI.UpdateWaitingUI(_waitingIntensity);        
            }
            if(_waitTimer >= _maxWaitingTime)
            {
                Return(); // Return and no money
            }
        }
        #endregion

        #region Set
        public void SetTargetPosition(Vector3 position)
        {
            _targetSeat = position;
        }
        #endregion

        #region Complete order
        public void Complete()
        {
            Return();
            // Pay money
            TextPopup.Show("+" + _money, this.transform.position, Color.yellow); // Show money at this customer position
            MoneyManager.Instance.AddMoney(_money);

            // Prepare tip if customer feel good
            int tip = 0;
            if(_waitingIntensity > 0.5f)
            {
                tip = (int)(_waitingIntensity * 10);
            }
            MoneyManager.Instance.AddTipMoney(tip);

            SoundManager.PlaySound(SoundEnum.ServeComplete, this.transform.position);
            _isWaiting = false;
        }
        private void Return()
        {
            if (_isReturnCalled == true) return;

            _isReturnCalled = true;
            _isServed = true;
            CustomerManager.Instance.ReturnChair(this); // Return seat
            SetTargetPosition(CustomerSpawner.Instance.transform.position); // Move out
            OnCustomerReturn?.Invoke();
            NoodyCustomCode.StartUpdater(this.gameObject, () =>
            {
                float distance = Vector3.Distance(this.transform.position, _targetSeat);
                if (distance <= 0.1f)
                {
                    DestroySelf();
                    return true;
                }
                return false;
            });
        }
        #endregion

        #region Support
        private void DestroySelf()
        {
            _customerView.StopAllAnimation();
            Destroy(this.gameObject, 1f);
        }
        #endregion
    }
}
