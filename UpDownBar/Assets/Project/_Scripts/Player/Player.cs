using System;
using System.Collections;
using System.Collections.Generic;
using NOOD;
using NOOD.Sound;
using UnityEngine;

namespace Game
{
    public class Player : MonoBehaviorInstance<Player>
    {
        public static Action<int> OnPlayerChangePosition;
        public static Action OnPlayerChangePositionSuccess;
        public static Action OnPlayerPressDeliverBeer;
        private int _index = 0;

        private void Update()
        {
            if(InputManager.Instance.IsPlayerPressServe())
            {
                BeerServeManager.Instance.ServeBeer();
                OnPlayerPressDeliverBeer?.Invoke();
                SoundManager.PlaySound(SoundEnum.ServeBeer);
            }
            if(InputManager.Instance.IsPlayerPressEscape())
            {
                GameplayManager.Instance.OnPausePressed?.Invoke();
            }

            Move();
        }
        
        private void Move()
        {
            CalculateStandIndex();

            Vector3 standPosition = new Vector3(this.transform.position.x, 0, TableManager.Instance.GetPlayerTablePosition().z);
            this.transform.position = standPosition;
        }

        private int CalculateStandIndex()
        {
            int oldIndex = _index;
            if(InputManager.Instance.GetVerticalInput() > 0)
            {
                // Move up
                _index++;
            }
            if(InputManager.Instance.GetVerticalInput() < 0)
            {
                // Move down
                _index--;
            }
            _index = Mathf.Clamp(_index, 0, TableManager.Instance.GetTableList().Count - 1);
            OnPlayerChangePosition?.Invoke(_index);
            if(oldIndex != _index)
            {
                OnPlayerChangePositionSuccess?.Invoke();
            }
            return _index;
        }
    }
}
