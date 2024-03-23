using UnityEngine.InputSystem;

public interface ControlStation
{
    // Maps Unity Event to a function, passing context with lambda function
    public void OnMove(InputAction.CallbackContext ctx);

    public void OnUse(InputAction.CallbackContext ctx);

    public void Activate(PlayerMovement playerMovement);

    public void Deactivate();
}
