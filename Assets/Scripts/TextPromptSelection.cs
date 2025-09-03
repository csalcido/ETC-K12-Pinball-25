using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextPromptSelection : MonoBehaviour
{

    public SerialManager serialManager; //manages controls
    public GameStateManager gameStateManager;

    public Button leftButton;
    public Button RightButton;
    public Button SelectButton;
    public SoundController buttonSound;
    public SoundController selectionSound;


    private int currentPromptIndex = 0; //index of current prompt
    public TextMeshProUGUI selectedPromptText;
    public Animator textAnimator;

    // List of available prompt options
    private string[] promptOptions = { "Cartoony", "Magical", "Spooky", "Retro", "Sci-Fi" };


    
    void Start()
    {
        // Set initial prompt text
        UpdatePromptText();
    }

    private void UpdatePromptText()
    {
        selectedPromptText.text = promptOptions[currentPromptIndex]; //update text object

        
    }

    public void ChangePrompt(int change)
    {
        currentPromptIndex += change;
        // Wrap around so it loops
        if (currentPromptIndex < 0)
            currentPromptIndex = promptOptions.Length - 1;
        else if (currentPromptIndex >= promptOptions.Length)
            currentPromptIndex = 0;

        textAnimator.Play("textBounce", -1, 0f);
        UpdatePromptText();
    }

    private void ConfirmSelection()
    {
        //update Game State
        gameStateManager.currentState = GameStateManager.ScreenState.GameBoard;

        //show image and play sounds
        selectionSound.PlaySound();
        ShowFilteredImage();
        
        //transition to gameboard



    }

    private void ShowFilteredImage()
    {

    }

    // Update is called once per frame
    void Update()
    {

        //check gameState, use flipper controls to select prompt
        if (gameStateManager.currentState == GameStateManager.ScreenState.TextPrompt)
        {
            //play left flipper
            if (Input.GetKeyDown(KeyCode.LeftArrow) || SerialManager.LeftFlipperPressed)
            {
                OnLeftButton();
            }

            // play right flippers
            if (Input.GetKeyDown(KeyCode.RightArrow) || SerialManager.RightFlipperPressed)
            {
                OnRightButton();
            }
            //select prompt
            if (Input.GetKeyDown(KeyCode.Space) || SerialManager.StartPressed)
            {
                ConfirmSelection();
            }
        }

    }


    // Called by Left Button
    public void OnLeftButton()
    {
        ChangePrompt(-1);
        buttonSound.PlaySound();
    }

    // Called by Right Button
    public void OnRightButton()
    {
        ChangePrompt(1);
        buttonSound.PlaySound();
    }
}
