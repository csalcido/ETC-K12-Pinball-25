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
        GameBoard,
        EndScreen,
    }

    public ScreenState currentState = ScreenState.StartMenu;
    
    //pflags for temporary states in the photo zone and end screen
      private bool photoTaken = false;
    private bool photoPrinted = false;
    
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
        photoButton.onClick.Invoke();
        currentState = ScreenState.GameBoard;
        
    }

    public void RestartGame()
    {
        restartButton.onClick.Invoke();
        currentState = ScreenState.StartMenu;
        
    }


    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) ||  SerialManager.StartPressed)
        {
            switch (currentState)
            {
                case ScreenState.StartMenu:
                    GoToTutorial();
                    break;

               case ScreenState.PhotoZone:
                    if (!photoTaken)
                    {
                        // First press to take photo
                        photoButton.onClick.Invoke();
                        photoTaken = true;
                    }
                    else
                    {
                        // Already taken, then move to GameBoard
                        currentState = ScreenState.GameBoard;
                        photoTaken = false; // reset for next run
                    }
                    break;


                 case ScreenState.EndScreen:
                    if (!photoPrinted)
                    {
                        // Run print animation
                        photoPrinted = true;
                    }
                    else
                    {
                        RestartGame();
                        photoPrinted = false; // reset
                    }
                    break;
            }

                

                    
                    



            
        }
    }
    

        
        
    
}
