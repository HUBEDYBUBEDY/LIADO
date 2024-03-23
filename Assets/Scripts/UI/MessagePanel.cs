using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessagePanel : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private TextMeshProUGUI message;
    [SerializeField] private TextMeshProUGUI summary;
    [SerializeField] private Color backgroundDeath;
    [SerializeField] private Color textDeath;
    [SerializeField] private Color backgroundWin;
    [SerializeField] private Color textWin;

    private float duration = 300f;
    private float startTime;
    private float summaryStartTime;
    private bool startSummary = false;
    private Color targetBgColor;
    private Color targetTxtColor;

    void Awake() {
        background.color = Color.clear;
        message.color = Color.clear;
        summary.color = Color.clear;

        targetBgColor = Color.clear;
        targetTxtColor = Color.clear;
    }

    void Update() {
        background.color = Color.Lerp(background.color, targetBgColor, Mathf.Min((Time.time-startTime)/duration, 1f));
        message.color = Color.Lerp(message.color, targetTxtColor, Mathf.Min((Time.time-startTime)/duration, 1f));
        if(startSummary) {
            summary.color = Color.Lerp(summary.color, targetTxtColor, Mathf.Min((Time.time-summaryStartTime)/duration, 1f));
        }
    }

    private void StartSummary() {
        summaryStartTime = Time.time;
        startSummary = true;
    }

    public void OnSubmarineDrown(float duration, float summaryDelay) {
        message.text = "Mission Failure";
        summary.text = "Your crew ran out of Oxygen!";

        this.duration = duration;
        targetBgColor = backgroundDeath;
        targetTxtColor = textDeath;
        startTime = Time.time;
        Invoke("StartSummary", summaryDelay);
    }

    public void OnResurface(float duration, float summaryDelay, int finalScore) {
        message.text = "Mission Success!";
        summary.text = "You earned £" + finalScore*1000 + "!";

        this.duration = duration;
        targetBgColor = backgroundWin;
        targetTxtColor = textWin;
        startTime = Time.time;
        Invoke("StartSummary", summaryDelay);
    }
}
