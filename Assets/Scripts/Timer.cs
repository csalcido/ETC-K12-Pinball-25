using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public float countdownTime = 100f;

    public SoundController flashSound;
    public TextMeshProUGUI countdownText;
    public EndScreen endScreen;
    public GameStateManager gameStateManager;

    private float nextBeepTime = 0f;
    

    private float timeRemaining;
    private bool timerIsRunning = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeRemaining = countdownTime;
        timerIsRunning = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        
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


                //change gamestate to end screen
                gameStateManager.TransitionToEndScreen();
                endScreen.EndTheGame();     // TODO: Move this to GameStateManager
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
