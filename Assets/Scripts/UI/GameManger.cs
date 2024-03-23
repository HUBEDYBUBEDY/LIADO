using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManger : MonoBehaviour
{
    [SerializeField] private AirBar airBar;
    [SerializeField] private MessagePanel messagePanel;
    [SerializeField] private float deathMessageDuration = 300f;
    [SerializeField] private float summaryDelay = 3f;
    [SerializeField] private float respawnDelay = 10f;

    [SerializeField] private bool hasStarted = false;

    private void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SetAir(float air) {
        airBar.SetAir(air);
    }

    public void SetMaxAir(float maxAir) {
        airBar.SetMaxAir(maxAir);
    }

    public void OnSubmarineDrown() {
        messagePanel.OnSubmarineDrown(deathMessageDuration, summaryDelay);
        Invoke("Restart", respawnDelay);
    }

    public void OnStart() {
        if(!hasStarted) {
            hasStarted = true;
        }
    }

    public void OnResurface(int finalScore) {
        if(hasStarted) {
            messagePanel.OnResurface(deathMessageDuration, summaryDelay, finalScore);
            Invoke("Restart", respawnDelay);
        }
    }
}
