using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class CoinSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _coinPref;
        private int _customerNumberToSpawnCoin;

        void Start()
        {
            CustomerSpawner.Instance.onCustomerSpawn += CustomerSpawner_OnCustomerSpawn;
            _customerNumberToSpawnCoin = UnityEngine.Random.Range(10, 20);
        }

        private void CustomerSpawner_OnCustomerSpawn(Customer customer)
        {
            if(_customerNumberToSpawnCoin-- == 0)
            {
                GameObject coin = Instantiate(_coinPref, customer.transform);
                coin.transform.localPosition = new Vector3(0, 2.7f, 0);
                customer.SetCoin(coin.transform);
                _customerNumberToSpawnCoin = UnityEngine.Random.Range(10, 20);
            }            
        }
    }
}
