using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class PlayerInfoPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _playerAddressText, _ingamePointText, _sahPointText;
        [SerializeField] private Button _claimBtn, _logOutBtn;

        void Awake()
        {
            _claimBtn.onClick.AddListener(OnClaimBtnClick);
            _logOutBtn.onClick.AddListener(OnLogOutBtnClick);
        }


        void Start()
        {
            UpdatePlayerAddress();
        }

        private void UpdatePoint()
        {
            _ingamePointText.text = PlayerData.InGamePoint.ToString();
            _sahPointText.text = PlayerData.SahPoint.ToString();
        }
        private void UpdatePlayerAddress()
        {
            _playerAddressText.text = PlayerData.PlayerAddress;
        }
        private void OnLogOutBtnClick()
        {
        }
        private void OnClaimBtnClick()
        {
        }

        public void Open()
        {
            this.transform.DOScale(1, 0.25f).SetEase(Ease.OutFlash);
        }
        public void Close()
        {
            this.transform.DOScale(0, 0.5f).SetEase(Ease.InFlash).onComplete = () => this.gameObject.SetActive(false);
        }
    }
}
