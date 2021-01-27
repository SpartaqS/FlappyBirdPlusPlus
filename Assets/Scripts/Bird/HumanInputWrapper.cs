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
            return Input.GetKeyDown(KeyCode.Mouse0);
        }
    }
}