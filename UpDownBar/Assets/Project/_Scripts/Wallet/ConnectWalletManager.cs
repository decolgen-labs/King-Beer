using System;
using System.Collections;
using System.Collections.Generic;
using NOOD;
using UnityEngine;

namespace Game
{
    public static class PlayerData
    {
        public static string PlayerAddress;
        public static int InGamePoint;
        public static int SahPoint;
    }

    public class ConnectWalletManager : MonoBehaviorInstance<ConnectWalletManager>
    {
        [SerializeField] private Transform _gameUITransform;
        [SerializeField] private ConnectWalletUI _connectWalletUIPref;
        private ConnectWalletUI _connectWalletUI;
        private Action _onSuccess;

        public void StartConnectWallet(Action onSuccess)
        {
            _onSuccess = onSuccess;
            _connectWalletUI = Instantiate<ConnectWalletUI>(_connectWalletUIPref, _gameUITransform);
            _connectWalletUI.Open();
            _connectWalletUI.onArgentXClick = ConnectArgentX;
            _connectWalletUI.onBraavosClick = ConnectBraavos;
        }

        private void ConnectArgentX()
        {
            StartCoroutine(WalletConnectAsync(JSInteropManager.ConnectWalletArgentX));
        }
        private void ConnectBraavos()
        {
            StartCoroutine(WalletConnectAsync(JSInteropManager.ConnectWalletBraavos));
        }

        private IEnumerator WalletConnectAsync(Action walletAction)
        {
            walletAction?.Invoke();
            yield return new WaitUntil(() => JSInteropManager.IsConnected());

            PlayerData.PlayerAddress = JSInteropManager.GetAccount();
            _connectWalletUI.Close();
            _onSuccess.Invoke();
        }
    }
}
