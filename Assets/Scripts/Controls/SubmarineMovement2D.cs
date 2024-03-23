using UnityEngine;
using UnityEngine.InputSystem;

public class SubmarineMovement2D : MonoBehaviour, ControlStation
{
    [SerializeField] private float xSpeed;
    [SerializeField] private float ySpeed;
    [SerializeField] private bool inAir;
    [SerializeField] private float water;

    [SerializeField] private float xAcceleration = 10f;
    [SerializeField] private float yAcceleration = 10f;
    [SerializeField] private float speedMin = 200f;
    [SerializeField] private float xSpeedMax = 1000f;
    [SerializeField] private float ySpeedMax = 1000f;

    private Rigidbody2D rb;
    private Vector2 move;

    void Awake() {
        inAir = false;
        rb = GetComponent<Rigidbody2D>();
        water = 0f;
    }

    void FixedUpdate() {
        float yInput = move.y;
        float xInput = move.x;

        // Don't allow submarine to move upwards if in air
        if(inAir) {
            Mathf.Clamp(yInput, -1f, 0f);
            if(transform.position.y > 0f) {
                rb.gravityScale = 1f;
            } else {
                rb.gravityScale = 0f;
            }
        } else {
            rb.gravityScale = 0f;
        }

        // Adds input acceleration to current speed
        ySpeed += yInput*yAcceleration*(1f-water);
        // Stops sub if speed is too low (making stopping easier)
        if(Mathf.Abs(ySpeed) <= speedMin && yInput == 0 && !inAir) ySpeed = 0f;
        // Keeps speed under max value in either direction
        ySpeed = Mathf.Clamp(ySpeed, -ySpeedMax*(1f-water), ySpeedMax*(1f-water));

        xSpeed += xInput*xAcceleration;
        if(Mathf.Abs(xSpeed) <= speedMin && xInput == 0) xSpeed = 0f;
        xSpeed = Mathf.Clamp(xSpeed, -xSpeedMax, xSpeedMax);

        rb.AddForce(transform.right * xSpeed);
        rb.AddForce(transform.up * ySpeed);
    }

    // Maps Unity Event to a function, passing context with lambda function
    public void OnMove(InputAction.CallbackContext ctx) => move = ctx.ReadValue<Vector2>();

    public void OnUse(InputAction.CallbackContext ctx) {
        if(ctx.performed) Debug.Log("Submarine pressed.");
        else Debug.Log("Submarine released.");
    }

    public void Activate(PlayerMovement playerMovement) {}

    public void Deactivate() {
        xSpeed = 0f;
        ySpeed = 0f;
    }

    public void EnterAir() {
        inAir = true;
    }

    public void ExitAir() {
        inAir = false;
    }

    public float GetYSpeedMax() {
        return ySpeedMax;
    }

    public void SetWater(float water) {
        this.water = water;
    }
}
