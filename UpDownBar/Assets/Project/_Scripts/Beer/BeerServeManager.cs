using System;
using NOOD;
using NOOD.NoodCamera;
using NOOD.Sound;
using UnityEngine;

namespace Game
{

    public class BeerServeManager : MonoBehaviorInstance<BeerServeManager>
    {
        public Action<Vector3> OnServerFail;

        [SerializeField] private int _moneyLostOnFail = 20;
        [SerializeField] private Transform _beerCupPref;
        [SerializeField] private float _tableHeight = 0.8f;
        private Player _player;

        #region Unity functions
        void OnEnable()
        {
            OnServerFail += OnServerFailHandler;
        }
        void Start()
        {
            _player = Player.Instance;
        }
        void OnDisable()
        {
            OnServerFail -= OnServerFailHandler;
        }
        #endregion


        private void OnServerFailHandler(Vector3 failPosition)
        {
            MoneyManager.Instance.RemoveMoney(_moneyLostOnFail);
            TextPopup.Show("-" + _moneyLostOnFail, failPosition, Color.red);
            SoundManager.PlaySound(SoundEnum.ServeFail, failPosition);
            FlashingUI.Instance.Flash(Color.red);
            CustomCamera.Instance.Shake();
        }

        public void ServeBeer()
        {
            if (GameplayManager.Instance.IsEndDay == true) return;
            
            if(_player == null)
            {
                _player = Player.Instance;
            }
            Vector3 startPosition = _player.transform.position;
            startPosition.y = _tableHeight;

            BeerCup beerCup = Instantiate(_beerCupPref, startPosition, Quaternion.identity).GetComponent<BeerCup>();
            float moveSpeed = 5;
            NoodyCustomCode.StartUpdater(beerCup, () =>
            {
                if (beerCup == null) return true;

                beerCup.transform.position += Vector3.left * moveSpeed * TimeManager.DeltaTime;
                return false;
            });
        }
    }
}
