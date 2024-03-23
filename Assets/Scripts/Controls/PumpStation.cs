using UnityEngine;
using UnityEngine.InputSystem;

public class PumpStation : MonoBehaviour, ControlStation
{
    [SerializeField] private float drainRate = 0.1f;

    private ControlSwitcher switcher;
    private SubmarineManager manager;

    void Awake() {
        switcher = GetComponent<ControlSwitcher>();
        manager = GetComponentInParent<SubmarineManager>();
    }

    // Maps Unity Event to a function, passing context with lambda function
    public void OnMove(InputAction.CallbackContext ctx) {

    }

    public void OnUse(InputAction.CallbackContext ctx) {
        if (ctx.canceled) {
            manager.Drain(-drainRate);
            switcher.Exit();
        }
    }

    public void Activate(PlayerMovement playerMovement) {
        manager.Drain(drainRate);
    }

    public void Deactivate() {

    }
}
