using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace FlappyBirdPlusPlus
{
    public class HumanInputWrapper : IInputWrapper
    {
        public bool IsTapped()
        {
            bool isATap;
            if (Application.isEditor)
            {
                isATap = Input.GetKeyDown(KeyCode.Mouse0);
            }
            else
            {
                isATap = Input.touchCount > 0 ? Input.GetTouch(0).phase == TouchPhase.Began : false;
            }
            return isATap;
        }
    }
}