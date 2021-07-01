using UnityEngine;

namespace HB {
public class HBPlayerInput : MonoBehaviour
{
    float inputX, inputY;
    public float InputX => inputX;
    public float InputY => inputY;

    bool isFire1Pressed;
    public bool IsFire1Pressed => isFire1Pressed;

    void Update()
    {
        InputKeys();
    }

    void InputKeys()
    {
        inputX = 0f;
        inputY = 0f;

        bool pressingUp    = Input.GetAxisRaw("Vertical")   == 1;
        bool pressingDown  = Input.GetAxisRaw("Vertical")   == -1;
        bool pressingRight = Input.GetAxisRaw("Horizontal") == 1;
        bool pressingLeft  = Input.GetAxisRaw("Horizontal") == -1;

        inputX = pressingRight ? 1f : pressingLeft ? -1f : 0f;
        inputY = pressingUp ? 1f : pressingDown ? -1f : 0f;
    }
}
}