using UnityEngine;

namespace MicRogue
{
public abstract class EntityInputBase : MonoBehaviour
{
    protected float inputX, inputY;
    public float InputX => inputX;
    public float InputY => inputY;

    protected bool isFire1Pressed;
    public bool IsFire1Pressed => isFire1Pressed;

    protected virtual void Update()
    {
        InputMovement();
        InputButtons();
    }

    protected virtual void InputMovement()
    {
    }

    protected virtual void InputButtons()
    {
        isFire1Pressed = Input.GetButton("Fire1") ? true : false;
    }

    public virtual Vector2 DirectionOfTarget()
    {
        return Vector2.zero;
    }
}
}