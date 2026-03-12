using System.Collections;
using System.Collections.Generic;
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
    public TextMeshProUGUI gameOverlayText;
    public TextMeshProUGUI endScreenOverlayText;

    public GameObject thirdPromptUI;

    [Header("Animated Components")]

    public Animator cameraAnimator;
    public Animator textAnimator;
    

    // List of available prompt options

    private string[] randomPromptOne = {"Pittsburgh"};
    private string[] randomPromptTwo = {""};
    private string[] promptOptions = {"SKYLINE", "WARHOL", "MR. ROGERS", "FALLINGWATER", "BRIDGES"};


    
    void Start()
    {
        thirdPromptUI.SetActive(false); 
        StartCoroutine(randomPromptSequence()); //randomize the first two prompts   
        // Set initial prompt text
        UpdatePromptText();
    }

    IEnumerator randomPromptSequence()
    {
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(randomizePrompt(randomPromptOne, randomPromptOneText));
        yield return StartCoroutine(randomizePrompt(randomPromptTwo, randomPromptTwoText)); 
        gameStateManager.randomSelectionFinished = true;
        thirdPromptUI.SetActive(true);

    }


    IEnumerator randomizePrompt(string[] promptList, TextMeshProUGUI textObject)
    {

        //selects random prompt from list
        int listLength = promptList.Count();
        int index = Random.Range(0, listLength);


        for (int i = 0; i < ((listLength * 2) + index); i++)
        {
            int wrappedIndex = i % listLength;
            textObject.text = (promptList[wrappedIndex]).ToUpper();
            yield return new WaitForSeconds(0.1f);

        }

        //update text object
        string displayText = (promptList[index]).ToUpper();
       
        textObject.text = displayText;
        

        yield return new WaitForSeconds(.2f);
        
        
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


        switch (partOne)
        {
            case "PITTSBURGH":
                partOne = " ";
                break;
        }

        switch (partTwo)
        {
            case "":
                partTwo = "";
                break;
        }

        switch (partThree)
        {
            case "SKYLINE":
                partThree = "with the Pittsburgh skyline in the background";
                break;
            case "WARHOL":
                partThree = "marilyn monroe pop art";
                break;
            case "MR. ROGERS":
                partThree = "on the set of Mr. Rogers Neighborhood";
                break;
            case "FALLINGWATER":
                partThree = "Fallingwater house Frank Lloyd Wright";
                break;
            case "BRIDGES":
                partThree = "with Pittsburgh classic yellow bridges";
                break;
        }
    

    //first part
        /*
        switch (partOne)
        {
            case "DISCO":
                partOne = "disco diva in neon lights and glitter";
                break;
            case "MAGICAL":
                partOne = "a magical fairy forest glowing with enchantment";
                break;
            case "FUTURISTIC":
                partOne = "a neon cyberpunk futuristic city full of robots";
                break;
            case "RETRO":
                partOne = "60s hippie flower vivid colors vibe";
                break;
            case "GALACTIC":
                partOne = "galatic space with stars and galaxies";
                break;
            case "URBAN":
                partOne = "urban city skyline and neon signs and buildings and people";
                break;
        }

        //second part 
        switch (partTwo)
        {
            case "COMIC BOOK":
                partTwo = "illustrated in bold comic book marvel style";
                break;
            case "WATERCOLOR":
                partTwo = "painted in dreamy watercolor textures";
                break;
            case "VINTAGE":
                partTwo = "styled as a vintage faded photograph in black and white";
                break;
            case "POP ART":
                partTwo = "rendered in pop art Andy Warhol style";
                break;
            case "ANIME":
                partTwo = "drawn in ghibli anime style";
                break;
            case "CARTOON":
                partTwo = "drawn in a disney cartoon style";
                break;
            case "16-BIT":
                partTwo = "retro 16-bit pixel art";
                break;
        }
        
        /*
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
            case "vampire":
                partTwo = "victorian era vampire in spooky mansion";
                break;
            case "robot":
                partTwo = "mechanical robot with gears and steam";
                break;
            case "alien":
                partTwo = "aliens in outer space";
                break;

        }
        
        
                */

        
        string fullPrompt = $"{partThree}, {partTwo}, {partOne}."; //turn into interpolated string
        Debug.Log(fullPrompt);
        return fullPrompt;
    }

    #endregion


    public void ConfirmSelection()
    {
        //translate prompts to TD prompts
        TdPrompt = TdPromptTranslate(randomPromptOneText.text, randomPromptTwoText.text, selectedPromptText.text);
        //apply it to the oscmessage object
        gameStateManager.oscMessage.promptText = TdPrompt;

        //update Game State
        gameStateManager.currentState = GameStateManager.ScreenState.GameBoard;
        gameStateManager.randomSelectionFinished = false; //reset flag

        //transition to gameboard
        cameraAnimator.SetBool("playGumballAnim", true);
        gumballManager.SetActive(true);
        //update text overlay on gameboard
        string onScreenPrompt = $"{randomPromptOneText.text} {randomPromptTwoText.text} in {selectedPromptText.text} style";
        string endScreenPrompt = $"{randomPromptOneText.text}'s {selectedPromptText.text}";
        onScreenPrompt = onScreenPrompt.ToUpper(); //capitalizing all letters
        endScreenPrompt = endScreenPrompt.ToUpper(); //capitalizing all letters
        gameOverlayText.text = endScreenPrompt;
        endScreenOverlayText.text = endScreenPrompt;
    }
    
    
    private bool isScrolling = false;

    // Update is called once per frame
    void Update()
    {

        //check gameState, use flipper controls to select last prompt
        if (gameStateManager.currentState == GameStateManager.ScreenState.TextPrompt && gameStateManager.randomSelectionFinished)
        {
            //play left flipper once
            if (Input.GetKeyDown(KeyCode.LeftArrow) || SerialManager.LeftFlipperPressed && !isScrolling)
            {
                StartCoroutine(MenuScrollDelay());
                OnLeftButton();
            }

            // play right flipper once
            if (Input.GetKeyDown(KeyCode.RightArrow) || SerialManager.RightFlipperPressed && !isScrolling)
            {
                StartCoroutine(MenuScrollDelay());
                OnRightButton();
            }
        
        }

    }
    IEnumerator MenuScrollDelay()
    {
        isScrolling = true;
        yield return new WaitForSeconds(0.5f);
        isScrolling = false;

        

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
