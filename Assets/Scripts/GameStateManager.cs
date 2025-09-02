using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    public SerialManager serialManager;
    public TakePhotos takePhotos;
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

        PowerUpStation,
        EndScreen,
    }

    public ScreenState currentState = ScreenState.StartMenu;

    //flags for temporary states in the photo zone and end screen

    [HideInInspector]
    public bool photoTaken = false;
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
        photoButton.onClick.Invoke();
        currentState = ScreenState.PowerUpStation;
    }

    public void RestartGame()
    {
        restartButton.onClick.Invoke();
        currentState = ScreenState.StartMenu;

    }

    public void TakePhoto()
    {
        photoButton.onClick.Invoke();
        

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

                case ScreenState.Tutorial:
                    GoToPhotoZone();
                    break;

               case ScreenState.PhotoZone:
                    if (!photoTaken)
                    {
                        TakePhoto();
                    } 
                    if (photoTaken)
                    {
                        GoToPowerUpStation();
                        photoTaken = false; // reset for next run
        
                    }
                    break;

                case ScreenState.PowerUpStation:
                    break;
                
                case ScreenState.GameBoard:
                    GoToGameBoard();
                    break;

                 case ScreenState.EndScreen:
                    if (photoPrinted) //only allow button clicks after photo is done
                    {
                        RestartGame();
                        photoPrinted = false; // reset
                    }
                    break;
            }

                

                    
                    



            
        }
    }
    

        
        
    
}
