using UnityEngine;
using UnityEngine.InputSystem;

public class RepairTool : MonoBehaviour, ControlStation
{
    // Range of tool
    [SerializeField] float range = 3f;
    // Time taken to fix 1 leak in seconds
    [SerializeField] float fixRate = 1f;
    [SerializeField] private bool held;

    private Vector2 move;
    private PlayerMovement playerMovement;
    private ParticleSystem repairStream;

    void Awake() {
        repairStream = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if(held) {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, range, LayerMask.GetMask("Leak"));
            Debug.DrawRay(transform.position, transform.up, Color.red);
            if(hit) {
                hit.collider.gameObject.GetComponent<Leak>().Fix(fixRate*Time.deltaTime);
            }
        }
    }

    // Maps Unity Event to a function, passing context with lambda function
    public void OnMove(InputAction.CallbackContext ctx) {
        move = ctx.ReadValue<Vector2>();

        float angle = Vector2.SignedAngle(Vector2.up, move);
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    public void OnUse(InputAction.CallbackContext ctx) {
        if(ctx.performed) {
            held = true;
            repairStream.Play();
            playerMovement.sit(false);
        } else if (ctx.canceled) {
            held = false;
            repairStream.Stop();
            playerMovement.stand(false);
        }
    }

    public void Activate(PlayerMovement playerMovement) {
        this.playerMovement = playerMovement;
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    public void Deactivate() {
        this.playerMovement = null;
    }
}
