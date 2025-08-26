using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    public SerialManager serialManager;
    public Button tutorialButton;
    public Button startButton;
    public Button photoButton;
    public Button restartButton;

    public enum ScreenState
    {
        StartMenu,
        Tutorial,
        PhotoZone,
        PhotoIsTaken,
        GameBoard,
        EndScreen
    }

    public ScreenState currentState = ScreenState.StartMenu;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    public void clickButton()
    {


    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) ||  SerialManager.StartPressed)
        {
            switch (currentState)
            {
                case ScreenState.StartMenu:
                    startButton.onClick.Invoke();
                    //switch gamestate
                    currentState = ScreenState.Tutorial;

                    break;

                case ScreenState.Tutorial:
                    //switch gamestate
                    tutorialButton.onClick.Invoke();
                    currentState = ScreenState.PhotoZone;
                    break;

                case ScreenState.PhotoZone:
                    //switch gamestate
                    photoButton.onClick.Invoke();
                    currentState = ScreenState.PhotoIsTaken;
                    break;

                case ScreenState.PhotoIsTaken:
                    photoButton.onClick.Invoke();
                    currentState = ScreenState.GameBoard;
                    break;

                case ScreenState.EndScreen:
                    restartButton.onClick.Invoke();
                    currentState = ScreenState.StartMenu;
                    break;
            }

                

                    
                    



            
        }
    }
    

        
        
    
}
