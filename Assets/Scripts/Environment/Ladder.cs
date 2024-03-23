using UnityEngine;

public class Ladder : MonoBehaviour
{
    // Activate movement functions for players using the ladder
    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Player") {
            col.gameObject.GetComponent<PlayerMovement>().enterLadder();
        }
    }

    // Deactivate movement functions for players using the ladder
    void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.tag == "Player") {
            col.gameObject.GetComponent<PlayerMovement>().exitLadder();
        }
    }
}
