using UnityEngine;

public class InputControllerPlayer : EntityInputBase
{
    protected override void InputMovement()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
    }

    protected override void InputButtons()
    {
        isFire1Pressed = Input.GetButton("Fire1") ? true : false;
    }
}
