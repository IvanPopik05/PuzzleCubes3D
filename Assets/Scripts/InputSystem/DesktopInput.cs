using System;
using UnityEngine;

namespace InputSystem
{
    public class DesktopInput : IInput
    {
        public event Action<Vector3> Pressed;

        public void UpdateInput()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                Pressed?.Invoke(Vector3.left);
            if (Input.GetKeyDown(KeyCode.RightArrow))
                Pressed?.Invoke(Vector3.right);
            if (Input.GetKeyDown(KeyCode.UpArrow))
                Pressed?.Invoke(Vector3.forward);
            if (Input.GetKeyDown(KeyCode.DownArrow))
                Pressed?.Invoke(Vector3.back);
        }
    }
}