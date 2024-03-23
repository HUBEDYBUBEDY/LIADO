using UnityEditor;
using UnityEngine;

public class PickupDrop : MonoBehaviour
{
    [SerializeField] private Transform objectGrabPointTransform;
    [SerializeField] private Transform toolGrabPointTransform;
    [SerializeField] private Collider2D self;
    [SerializeField] private GameObject targetObject;
    [SerializeField] private GameObject grabbedObject;
    [SerializeField] private GameObject targetContainer;
    // Used for calculating throw velocity (can be removed later)
    [SerializeField] private Rigidbody2D playerRigidbody;

    private ObjectGrabbable objectGrabbable;
    private PlayerMovement playerMovement;

    void Awake() {
        playerMovement = GetComponentInParent<PlayerMovement>();
    }

    // Returns true if object was successfully grab, otherwise returns false
    public bool Grab() {
        // If currently holding an object next to a container, store it in that container
        if(grabbedObject) {
            if(targetContainer != null) {
                if(targetContainer.GetComponent<Container>() != null) {
                    // Store into loot container
                    if(targetContainer.GetComponent<Container>().GetManager().Store(grabbedObject)) {
                        Destroy(grabbedObject);
                    }
                } else {
                    // Store into tool holder
                    if(targetContainer.GetComponent<ToolHolder>().Store(grabbedObject)) {
                        grabbedObject.GetComponent<ObjectGrabbable>().Drop(playerRigidbody.velocity, playerMovement);
                        Destroy(grabbedObject);
                    }
                }
            }
        }
        // Grab currently targeted object if possible
        else if(targetObject) {
            grabbedObject = targetObject;
            objectGrabbable = targetObject.GetComponent<ObjectGrabbable>();
            // Grab as a tool or object
            if (objectGrabbable.GetIsTool() == true) {
                objectGrabbable.Grab(toolGrabPointTransform, playerMovement);
            } else {
                objectGrabbable.Grab(objectGrabPointTransform, playerMovement);
            }
            return true;
        }
        // Grab object from target container if possible
        else if(targetContainer != null) {
            if(targetContainer.GetComponent<ToolHolder>() != null) {
                targetObject = targetContainer.GetComponent<ToolHolder>().Spawn();
                if(targetObject != null) {
                    Grab();
                }
            }
        }

        return false;
    }

    // Drop currently held object
    public void Drop() {
        if(grabbedObject) {
            grabbedObject = null;
            objectGrabbable.Drop(playerRigidbody.velocity, playerMovement);
            objectGrabbable = null;
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        // Debug.Log("Collision detected with" + col.gameObject.ToString());

        if(col.gameObject.tag == "Pickup") {
            Physics2D.IgnoreCollision(self, col, true);
            targetObject = col.gameObject;
        } else if(col.gameObject.tag == "Container") {
            targetContainer = col.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D col) {
        if(col.gameObject == targetObject) {
            targetObject = null;
        } else if(col.gameObject == targetContainer) {
            targetContainer = null;
        }
    }

    // Return true if the actor is holding an item
    public bool IsHolding() {
        return grabbedObject != null;
    }
}
