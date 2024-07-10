using System.Collections;
using System.Collections.Generic;
using NOOD;
using UnityEngine;

namespace Game
{
    public class InputManager : MonoBehaviorInstance<InputManager>
    {
        public float GetVerticalInput()
        {
            float verticalInput = 0;
            if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                verticalInput = 1;
            }
            if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                verticalInput = -1;
            }
            return verticalInput;
        }
        public bool IsPlayerPressServe()
        {
            return Input.GetKeyDown(KeyCode.Space);
        }
        public bool IsPlayerPressEscape()
        {
            return Input.GetKeyDown(KeyCode.Escape);
        }
    }
}
