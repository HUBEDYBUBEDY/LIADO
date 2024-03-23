using UnityEngine;

public class ObjectGrabbable : MonoBehaviour
{
    [SerializeField] private Collider2D self;
    [SerializeField] private MonoBehaviour toolScript = null;

    [SerializeField] private bool throwable = false;
    [SerializeField] private bool isTool = false;
    [SerializeField] private float throwMagnitude = 10f;
    [SerializeField] private int value = 0;

    protected Rigidbody2D objectRigidbody;

    // Start is called before the first frame update
    void Awake()
    {
        objectRigidbody = GetComponent<Rigidbody2D>();
    }

    public void Grab(Transform objectGrabPointTransform, PlayerMovement playerMovement) {
        // Move object to grab point and set parent
        transform.position = objectGrabPointTransform.position;
        transform.SetParent(objectGrabPointTransform);

        // Prevent object from interacting with any other elements of scene
        // objectRigidbody.isKinematic = true;
        // objectRigidbody.interpolation = RigidbodyInterpolation2D.None;
        objectRigidbody.simulated = false;

        // Active tool script
        if(toolScript != null) {
            toolScript.enabled = true;
            toolScript.GetComponent<ControlStation>().Activate(playerMovement);
            playerMovement.UpdateTarget(null, toolScript.GetComponent<ControlStation>());
        } else {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        self.enabled = false;
    }

    public void Drop(Vector3 velocity, PlayerMovement playerMovement) {
        transform.SetParent(null);

        // Reactivate objects' interaction with the scene
        // objectRigidbody.isKinematic = false;
        // objectRigidbody.interpolation = RigidbodyInterpolation2D.Interpolate;
        objectRigidbody.simulated = true;

        // Add a force in the players current direction if throwable
        if(throwable) objectRigidbody.AddForceAtPosition(velocity * throwMagnitude, transform.position, ForceMode2D.Impulse);

        // Deactivate tool script
        if(toolScript != null) {
            toolScript.enabled = false;
            toolScript.GetComponent<ControlStation>().Deactivate();
            playerMovement.RemoveTarget(null, toolScript.GetComponent<ControlStation>());
        }

        self.enabled = true;
    }

    public void SetValue(int newValue) {
        value = newValue;
    }

    public int GetValue() {
        return value;
    }

    public bool GetIsTool() {
        return isTool;
    }
}
