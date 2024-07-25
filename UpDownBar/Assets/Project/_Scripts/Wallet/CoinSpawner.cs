using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class CoinSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _coinPref;
        private bool _isSpawnCoin;

        void Start()
        {
            CustomerSpawner.Instance.onCustomerSpawn += CustomerSpawner_OnCustomerSpawn;
            if(Application.isEditor)
            {
                Debug.Log("Subscribe spawnCoin");
                if(SocketConnectManager.Instance)
                    SocketConnectManager.Instance.OnEvent("spawnCoin", SpawnCoin);
            }
            else
            {
                JsSocketConnect.OnEvent("spawnCoin", this.gameObject.name, nameof(SpawnCoin));            
            }
        }

        private void SpawnCoin(string data)
        {
            Debug.Log("spawnCoin");
            _isSpawnCoin = true;
        }

        private void CustomerSpawner_OnCustomerSpawn(Customer customer)
        {
            if(_isSpawnCoin)
            {
                GameObject coin = Instantiate(_coinPref, customer.transform);
                coin.transform.localPosition = new Vector3(0, 2.7f, 0);
                customer.SetCoin(coin.transform);
                _isSpawnCoin = false;
            }
        }
    }
}
