using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NOOD;
using UnityEngine;

namespace Game
{
    public class CustomerSpawner : MonoBehaviorInstance<CustomerSpawner>
    {
        [SerializeField] private Transform _customerPref;
        [SerializeField] private float _spawnTime;

        private void Start()
        {
            StartCoroutine(SpawnerCustomer());
        }

        IEnumerator SpawnerCustomer()
        {
            while(true)
            {
                yield return new WaitForSeconds(_spawnTime);

                // Check if is any available seat
                if (TableManager.Instance.IsAnyAvailableSeat() && GameplayManager.Instance.IsEndDay == false)
                {
                    // Spawn customer
                    Customer customer = Instantiate(_customerPref, this.transform.position, Quaternion.identity).GetComponent<Customer>();
                }
            }
        }
    }

}
