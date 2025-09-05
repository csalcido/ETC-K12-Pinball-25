using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    public SerialManager serialManager;
    public TakePhotos takePhotos;
    public WebCamTest webCamTest;
    public TextPromptSelection textPromptSelection;

    public Button tutorialButton;
    public Button startButton;
    public Button photoButton;
    public Button promptSelectButton;
    public Button restartButton;

    [Tooltip("Reference to the OSC Message script to send data to TouchDesigner")]
    public OscMessage oscMessage;


    public enum ScreenState
    {
        StartMenu,
        Tutorial,
        PhotoZone,
        GameBoard,
        PowerUpStation,
        TextPrompt,
        EndScreen,
    }

    public enum GameMode
    {
        AdditiveColor,
        AiFilter
    }

    public ScreenState currentState = ScreenState.StartMenu;
    public GameMode currentMode = GameMode.AdditiveColor;

    //flags for temporary states in the photo zone and end screen

    [HideInInspector]
    public bool photoTaken = false;


    [HideInInspector]
    public bool randomSelectionFinished = false;

    [HideInInspector]
    public bool photoPrinted = false;

    public void GoToTutorial()
    {
        startButton.onClick.Invoke();
        currentState = ScreenState.Tutorial;
    }

    public void GoToPhotoZone()
    {
        tutorialButton.onClick.Invoke();
        currentState = ScreenState.PhotoZone;
    }

    public void GoToGameBoard()
    {
        currentState = ScreenState.GameBoard;
    }

    public void GoToPowerUpStation()
    {
        currentState = ScreenState.PowerUpStation;
    }

    public void GoToTextPrompt()
    {
        currentState = ScreenState.TextPrompt;
    }

    public void RestartGame()
    {
        restartButton.onClick.Invoke();
        currentState = ScreenState.StartMenu;
        webCamTest.StopCamera();
        oscMessage.gameOver = 0;
        photoPrinted = false;       // TODO: Does value need to come from Touch Designer
    }

    public void TakePhoto()
    {
        photoButton.onClick.Invoke();
        photoTaken = true;
    }

    public void SelectPrompt()
    {
        promptSelectButton.onClick.Invoke();
    }


    public void TransitionToEndScreen()
    {
        if (oscMessage.gameOver == 0)
        {
            oscMessage.gameOver = 1; //send game over signal to TouchDesigner
        }

        this.currentState = ScreenState.EndScreen;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || SerialManager.StartPressed)
        {
            switch (currentState)
            {
                case ScreenState.StartMenu:
                    GoToTutorial();
                    break;

                case ScreenState.Tutorial:
                    GoToPhotoZone();
                    break;

                case ScreenState.PhotoZone:
                    if (!photoTaken)
                    {
                        TakePhoto();
                    }
                    else {
                        if (currentMode == GameMode.AdditiveColor) //check which game it is
                        {
                            GoToPowerUpStation();

                        }
                        else if (currentMode == GameMode.AiFilter) //check which game it is
                        {
                            GoToTextPrompt();
                        }
                    }
                    
                    break;

                case ScreenState.PowerUpStation:
                    break;
                
                case ScreenState.TextPrompt:

                    if (randomSelectionFinished)
                    {
                        SelectPrompt();
                    }
                    break;
                
                case ScreenState.GameBoard:
                    GoToGameBoard();
                    break;

                case ScreenState.EndScreen:
                    if (photoPrinted) //only allow button clicks after photo is done
                    {
                        RestartGame();
                    }
                    break;
            }
            
        }
    }
    
}
