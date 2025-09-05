using System.Collections;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TextPromptSelection : MonoBehaviour
{

    public SerialManager serialManager; //manages controls
    public GameStateManager gameStateManager;
    public GameObject gumballManager;

    [Header("Buttons")]

    public Button leftButton;
    public Button RightButton;
    public Button SelectButton;
    public SoundController buttonSound;
    public SoundController selectionSound;

    [Header("Info for Touchdesigner")]
    public string TdPrompt = "full prompt here";


    private int currentPromptIndex = 0; //index of current prompt

    [Header ("Text Objects")]
    public TextMeshProUGUI randomPromptOneText;
    public TextMeshProUGUI randomPromptTwoText;
    public TextMeshProUGUI selectedPromptText;

    [Header("Animated Components")]

    public Animator cameraAnimator;
    public Animator textAnimator;
    public Animator curtainAnimator;
    public Animator filteredImageAnimator;

    // List of available prompt options

    private string[] randomPromptOne = {"medieval","magical","futuristic" };
    private string[] randomPromptTwo = {"cowboy", "wizard", "mermaid", "pirate"};
    private string[] promptOptions = { "Comic Book", "Watercolor", "Vintage", "Hyperrealistic", "Lego", "Cartoon", "16-Bit" };


    
    void Start()
    {
        //randomize the first two prompts
        StartCoroutine(randomizePrompt(randomPromptOne, randomPromptOneText));
        StartCoroutine(randomizePrompt(randomPromptTwo, randomPromptTwoText));
        gameStateManager.randomSelectionFinished = true;
        // Set initial prompt text
        UpdatePromptText();
    }


    IEnumerator randomizePrompt(string[] promptList, TextMeshProUGUI textObject)
    {
        //selects random prompt from list
        int listLength = promptList.Count();
        int index = Random.Range(0, listLength);

        //update text object
        textObject.text = promptList[index];

        yield return new WaitForSeconds(2f);
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
    
    
    

    #region TouchDesigner Prompts
    
    public string TdPromptTranslate(string partOne, string partTwo, string partThree)
    {
        //first part
        switch (partOne)
        {
            case "medieval":
                partOne = "a fantasy medieval kingdom with castles and knights";
                break;
            case "magical":
                partOne = "a magical fairy forest glowing with enchantment";
                break;
            case "futuristic":
                partOne = "a neon cyberpunk futuristic city full of robots";
                break;
        }

        //second part 
        switch (partTwo)
        {
            case "cowboy":
                partTwo = "inhabited by cowboys in a dusty desert western setting";
                break;
            case "wizard":
                partTwo = "with a wise wizard casting powerful spells";
                break;
            case "mermaid":
                partTwo = "featuring mystical mermaids swimming in crystal waters";
                break;
            case "pirate":
                partTwo = "sailing with pirates on stormy seas";
                break;
        }

        //third part
        switch (partThree)
        {
            case "Comic Book":
                partThree = "illustrated in bold comic book style";
                break;
            case "Watercolor":
                partThree = "painted in dreamy watercolor textures";
                break;
            case "Vintage":
                partThree = "styled as a vintage faded photograph";
                break;
            case "Hyperrealistic":
                partThree = "rendered in hyperrealistic detail";
                break;
            case "Lego":
                partThree = "constructed entirely from colorful Lego bricks";
                break;
            case "Cartoon":
                partThree = "drawn as a playful Saturday morning cartoon";
                break;
            case "16-Bit":
                partThree = "depicted in retro 16-bit pixel art";
                break;
        }
        string fullPrompt = $"{partOne}, {partTwo}, {partThree}."; //turn into interpolated string

        return fullPrompt;
    }

    #endregion


    public void ConfirmSelection()
    {
        //translate prompts to TD prompts
        TdPrompt = TdPromptTranslate(randomPromptOneText.text, randomPromptTwoText.text, selectedPromptText.text);
        //apply it to the oscmessage object
        gameStateManager.oscMessage.promptText = TdPrompt;

        //play animation coroutine
        StartCoroutine(ImageTransformation());


        ShowFilteredImage();

        //update Game State
        gameStateManager.currentState = GameStateManager.ScreenState.GameBoard;
        gameStateManager.randomSelectionFinished = false; //reset flag

        //transition to gameboard
        cameraAnimator.SetBool("playGumballAnim", true);
        gumballManager.SetActive(true);



    }

    IEnumerator ImageTransformation()
    {
        //first have text animation 
        textAnimator.Play("textTransform", -1, 0f);
        selectionSound.PlaySound();
        
      
        //have text and image go behind curtain and do sparkle thing

        yield return new WaitForSeconds(2f);
    }

    private void ShowFilteredImage()
    {

    }

    // Update is called once per frame
    void Update()
    {

        //check gameState, use flipper controls to select last prompt
        if (gameStateManager.currentState == GameStateManager.ScreenState.TextPrompt && gameStateManager.randomSelectionFinished)
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
