using System;
using UnityEngine;

namespace InputSystem
{
    public class MobileInput : IInput
    {
        public event Action<Vector3> Pressed;

        public void UpdateInput()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Moved)
                {
                    if (Mathf.Abs(touch.deltaPosition.x) > Mathf.Abs(touch.deltaPosition.y))
                        Pressed?.Invoke(touch.deltaPosition.x > 0 ? Vector3.right : Vector3.left);
                    else
                        Pressed?.Invoke(touch.deltaPosition.y > 0 ? Vector3.forward : Vector3.back);
                }
            }
        }
    }
}