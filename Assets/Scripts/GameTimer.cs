using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameTimer : MonoBehaviour
{
    [Tooltip("UI Text to display elapsed time")]
    public TMP_Text timerText;

    private float startTime;
    private bool running = false;

    void Update()
    {
        if (!running) return;

        float t = Time.time - startTime;
        // format as mm:ss or with hundredths:
        int minutes = (int)(t / 60);
        float seconds = t % 60;
        timerText.text = string.Format("{0:00}:{1:00.00}", minutes, seconds);
    }

    public void StartTimer()
    {
        startTime = Time.time;
        running = true;
    }

    public void StopTimer()
    {
        running = false;
    }
}
