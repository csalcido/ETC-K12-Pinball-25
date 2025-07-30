using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public float countdownTime = 100f;
    public TextMeshProUGUI countdownText;
    public GameObject resultPanel;

    private float timeRemaining;
    private bool timerIsRunning = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeRemaining = countdownTime;
        timerIsRunning = true;
        resultPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerUI(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                Time.timeScale = 0f;
                resultPanel.SetActive(true);
            }
        }
    }
    void UpdateTimerUI(float timeToDisplay)
    {
        timeToDisplay = Mathf.Max(timeToDisplay, 0f);
        int minutes = Mathf.FloorToInt(timeToDisplay / 60);
        int seconds = Mathf.FloorToInt(timeToDisplay % 60);
        countdownText.text = "time left: " + string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
