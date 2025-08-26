using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public float countdownTime = 100f;

    public SoundController flashSound;
    public TextMeshProUGUI countdownText;
    public GameObject endScreenManager;
    public GameStateManager gameStateManager;

    private float nextBeepTime = 0f;
    

    private float timeRemaining;
    private bool timerIsRunning = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeRemaining = countdownTime;
        timerIsRunning = false;
        endScreenManager.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
         Debug.Log("Game State changed to: " + gameStateManager.currentState);
        if (gameStateManager.currentState == GameStateManager.ScreenState.GameBoard)
        {
            if (!timerIsRunning)
                timerIsRunning = true;
        }

        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerUI(timeRemaining);



                //flashSound.PlaySound();
                
          if (timeRemaining <= 5f)
            {
                float beepInterval = 1f; // default 1 second
                if (timeRemaining <= 2f) // last 2 seconds
                {
                    beepInterval = 0.5f; // faster beeps
                }

                if (Time.time >= nextBeepTime)
                {
                    flashSound.PlaySound();
                    nextBeepTime = Time.time + beepInterval;
                }
            }
            

            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                Time.timeScale = 0f;
                endScreenManager.SetActive(true);

                //change gamestate to end screen
                gameStateManager.currentState = GameStateManager.ScreenState.EndScreen;
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
