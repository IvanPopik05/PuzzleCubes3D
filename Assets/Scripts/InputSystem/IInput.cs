using System;
using UnityEngine;

namespace InputSystem
{
    public interface IInput
    {
        event Action<Vector3> Pressed;
        void UpdateInput();
    }
}