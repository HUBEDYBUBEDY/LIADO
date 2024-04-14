using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PivotingStation : MonoBehaviour, ControlStation
{
    private Vector2 move;

    public void OnMove(InputAction.CallbackContext ctx) {
        move = ctx.ReadValue<Vector2>();

        Debug.Log("Moving");
        // float angle = Vector2.SignedAngle(Vector2.up, move);
        // transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    public abstract void OnUse(InputAction.CallbackContext ctx);

    public void Activate(PlayerMovement playerMovement) {}

    public void Deactivate() {}
}
