using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PickupDrop pickupDrop;

    [SerializeField] private float waterSpeed = 1f;
    [SerializeField] private float airSpeed = 2f;
    [SerializeField] private float gravity = 1f;

    [SerializeField] private bool onLadder;
    [SerializeField] private bool inAir;
    [SerializeField] private bool isStanding;
    [SerializeField] private ControlSwitcher switchTarget;
    [SerializeField] private ControlStation controlTarget;
    [SerializeField] private Rigidbody2D rbTarget;

    private Rigidbody2D rb;
    private Collider2D body;

    private Vector2 move;
    private float speed;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        body = GetComponent<Collider2D>();
        rbTarget = null;

        onLadder = false;
        inAir = false;
        isStanding = true;
        switchTarget = null;
        controlTarget = null;
    }

    // Disable gravity if on ladder or out of air, otherwise enable
    void Update() {
        if(onLadder || !inAir) {
            rb.gravityScale = 0f;
        }
        else {
            rb.gravityScale = gravity;
        }
    }

    void FixedUpdate() {
        float xInput = move.x;
        float yInput;

        // Set speed if in/out of air and disable y input if in air
        if(onLadder || !inAir) {
            yInput = move.y;
            if(!inAir) speed = waterSpeed;
            else speed = airSpeed;
        }
        else {
            yInput = 0f;
            speed = airSpeed;
        }

        // Only move if standing
        if(rbTarget != null) {
            Vector2 targetVelocity = rbTarget.velocity;
            rb.velocity = targetVelocity + new Vector2(xInput * speed, yInput * speed);
        } else {
            rb.velocity = new Vector2(xInput * speed, yInput * speed);
        }

    }

    // Stop player from moving or being able to interact with anything while sitting
    public void sit(bool onStation = true) {
        isStanding = false;

        if(onStation == true) {
            rb.isKinematic = true;
            body.enabled = false;
            if(pickupDrop.IsHolding()) pickupDrop.Drop();
        }
    }

    // Reactivate movement and interaction with player when standing
    public void stand(bool onStation = true) {
        isStanding = true;

        if(onStation == true) {
            rb.isKinematic = false;
            body.enabled = true;
        }
    }

    public void enterLadder() {
        onLadder = true;
    }

    public void exitLadder() {
        onLadder = false;
    }

    public void enterAir(Rigidbody2D target = null) {
        inAir = true;
        if (target != null) {
            rbTarget = target;
        }
    }

    public void exitAir(Rigidbody2D target = null) {
        if (target == rbTarget) {
            inAir = false;
            rbTarget = null;
        }
    }

    public bool IsHolding() {
        return pickupDrop.IsHolding();
    }

    // Update the switch and control target when overlapping a new station
    // <switcher> <station> means ready to use control switcher
    // <null> <station> means tool is currently active
    public void UpdateTarget(ControlSwitcher newSwitcher, ControlStation newStation) {
        // Only update target if not holding tool
        if(switchTarget != null || controlTarget == null) {
            switchTarget = newSwitcher;
            controlTarget = newStation;
        }
    }

    public void RemoveTarget(ControlSwitcher oldSwitcher, ControlStation oldStation) {
        if(switchTarget == oldSwitcher && controlTarget == oldStation) {
            switchTarget = null;
            controlTarget = null;
        }
    }

    public void OnMove(InputAction.CallbackContext ctx) {
        if(isStanding) {
            // If standing, move as normal
            move = ctx.ReadValue<Vector2>();
        } else if(controlTarget != null) {
            // If sitting with an active control target, move with control target instead
            move = Vector2.zero;
            controlTarget.OnMove(ctx);
        } else {
            Debug.Log("Sitting with no target!");
        }
    }

    public void OnUse(InputAction.CallbackContext ctx) {
        if(ctx.performed) {
            // Call when pressed (not released)
            if(isStanding) {
                // If standing, attempt to grab an object before switching
                bool grabbed = pickupDrop.Grab();
                if(switchTarget != null && !grabbed) {
                    // If there is active switch target and did not just grab an object, attempt to enter
                    switchTarget.Enter(transform, this);
                } else if(controlTarget != null && !grabbed) {
                    // Only use tool if haven't just picked it up
                    controlTarget.OnUse(ctx);
                }
            } else if(controlTarget != null) {
                // Use control target if sitting
                controlTarget.OnUse(ctx);
            }
        } else if(controlTarget != null) {
            // Let control target handle release
            controlTarget.OnUse(ctx);
        }
    }

    public void OnExit(InputAction.CallbackContext ctx) {
        // Only call when button pressed (not released)
        if(ctx.performed) {
            if(isStanding) {
                // If standing, attempt to drop
                pickupDrop.Drop();
            } else if(controlTarget != null) {
                // If sitting with active control target, exit control station
                switchTarget.Exit();
            }
        }
    }

    public PickupDrop GetPickupDrop() {
        return this.pickupDrop;
    }
}
