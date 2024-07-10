using System.Collections.Generic;
using NOOD;
using UnityEngine;

namespace Game
{
    public class CustomerManager : MonoBehaviorInstance<CustomerManager>
    {
        private Dictionary<Customer, Transform> _customerSeatDic = new Dictionary<Customer, Transform>();

        public Transform GetRandomChair(Customer getter)
        {
            // Get random seat
            if(TableManager.Instance.TryGetRandomSeat(out Transform resultSeat))
            {
                _customerSeatDic.Add(getter, resultSeat);
                return resultSeat;
            }
            return null;
        }
        public void ReturnChair(Customer customer)
        {
            if(_customerSeatDic.TryGetValue(customer, out Transform seat))
                TableManager.Instance.ReturnSeat(seat);
        }

    }
}
