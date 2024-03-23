using UnityEngine;

public class SurfaceChecker : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private float duration = 10f;

    private float startTime;
    private Color currentColor;
    private Color targetColor;
    private int players;

    void Awake() {
        currentColor = Color.clear;
        sprite.color = currentColor;
        players = 0;
    }

    void Update() {
        currentColor = Color.Lerp(currentColor, targetColor, Mathf.Min((Time.time-startTime)/duration, 1f));
        sprite.color = currentColor;
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Player") {
            players += 1;
            targetColor = Color.clear;
            startTime = Time.time;
        }
    }

    void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.tag == "Player") {
            players -= 1;
            if(players <= 0) {
                targetColor = Color.white;
                startTime = Time.time;
            }
        }
    }
}
