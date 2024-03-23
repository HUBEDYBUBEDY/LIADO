using UnityEngine;

public class Air : MonoBehaviour
{
    // false if air in submarine, true if air source (like surface of water)
    [SerializeField] private bool isNatural = false;
    [SerializeField] private Rigidbody2D target = null;

    // Activate movement functions for players in air
    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Player") {
            col.gameObject.GetComponent<PlayerMovement>().enterAir(target);
        }
        if (col.gameObject.tag == "Submarine" && isNatural == true) {
            col.gameObject.GetComponent<SubmarineManager>().EnterAir();
        }
    }

     // Deactivate movement functions for players in air
    void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.tag == "Player") {
            col.gameObject.GetComponent<PlayerMovement>().exitAir(target);
        }
        if (col.gameObject.tag == "Submarine" && isNatural == true) {
            col.gameObject.GetComponent<SubmarineManager>().ExitAir();
        }
    }
}
