using System;
using UnityEngine;

namespace HopRogue
{
public class InputController
{
    public static event Action<int> MouseButtonClicked;
    public static void ListenForPlayerInput()
    {
        if (Input.GetMouseButtonDown(0))
            MouseButtonClicked?.Invoke(0);

        else if (Input.GetMouseButtonDown(1))
            MouseButtonClicked?.Invoke(1);

        else if (Input.GetMouseButtonDown(2))
            MouseButtonClicked?.Invoke(2);
    }
}
}