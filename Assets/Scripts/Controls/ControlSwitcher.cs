using UnityEngine;

public class ControlSwitcher : MonoBehaviour
{
    // The target movement script to switch to
    [SerializeField] private MonoBehaviour controlStation;
    [SerializeField] private bool isUsing;
    [SerializeField] private bool standingStation = true;
    [SerializeField] private Transform station;
    [SerializeField] private Transform player;

    private PlayerMovement playerMovement;

    void Awake()
    {
        controlStation.enabled = false;
        isUsing = false;
    }

    // Enter control system, update player status and camera target
    public void Enter(Transform newTransform, PlayerMovement newMovement) {
        // Only if not currently being used and player is not holding an item
        if(!isUsing && !newMovement.IsHolding()) {
            isUsing = true;
            controlStation.enabled = true;
            controlStation.GetComponent<ControlStation>().Activate(null);

            player = newTransform;
            player.transform.SetParent(station);
            playerMovement = newMovement;
            playerMovement.sit(standingStation);

            // mainCamera.target = station.transform;
        }
    }

    // Exit control system, update player status and camera target
    public void Exit() {
        // Only if currently being used
        if(isUsing) {
            isUsing = false;
            controlStation.GetComponent<ControlStation>().Deactivate();
            controlStation.enabled = false;

            player.transform.SetParent(null);
            playerMovement.stand(standingStation);

            // mainCamera.target = player.transform;
        }
    }

    // Update object references if any player enters trigger collider
    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Player") {
            col.gameObject.GetComponent<PlayerMovement>().UpdateTarget(this, controlStation.GetComponent<ControlStation>());
        }
    }

    // Update object references if any player exits trigger collider
    void OnTriggerExit2D(Collider2D col) {
        if (!isUsing && col.gameObject.tag == "Player") {
            col.gameObject.GetComponent<PlayerMovement>().RemoveTarget(this, controlStation.GetComponent<ControlStation>());
        }
    }
}
