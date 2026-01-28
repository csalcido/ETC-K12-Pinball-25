using System.Collections;
using TMPro;
using UnityEngine;

public class Gumball : MonoBehaviour
{

    public GameObject[] spawnBuffs; //public array of prefabs to instantiate
    public Animator mainCameraAnimator;//Main camera animator
    public Animator leverAnimator;
    public GameObject lever;
    public TextMeshProUGUI buffText;
    public Animator textAnimator;

    public SoundController buffBeep;
    public Transform[] spawnLocations;

    public GameObject gumballDrop;
    public Animator gumballAnimator;
    public GameStateManager gameStateManager;
    public Transform gumballContainer;

    public GameObject pinballOne;
    public GameObject pinballTwo;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(startGumballMachine());
        
    }

    IEnumerator startGumballMachine()
    {
        yield return new WaitForSeconds(1.5f); //delay between camera transition and gumball generating 
        //play lever animation
        leverAnimator.Play("leverAnim");
       
        //play lever sound
        lever.GetComponent<AudioSource>().Play();

        for (int i = 0; i < spawnLocations.Length; i++)
        {
            //randomize buff
            var randomBuff = spawnBuffs[Random.Range(0, spawnBuffs.Length)];
           
            buffText.text = randomBuff.name; //update text popup
            
            

            //spawn buff at random
            Instantiate(randomBuff, spawnLocations[i]);
            


            //play animation and sound of it coming out of gumball machine
            gumballAnimator.Play("gumballDropAnim");
            buffBeep.PlaySound();

           

            GameObject obj = Instantiate(randomBuff, gumballDrop.transform); //this is all to get rid of weird transforms once gumball is instantiated
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
            obj.transform.localScale = Vector3.one;

             buffText.gameObject.SetActive(true);
            textAnimator.Play("textInflate", -1, 0f); // play bounce animation

            yield return new WaitForSeconds(1f);
            buffText.gameObject.SetActive(false);
            
            yield return new WaitForSeconds(.25f);
            
            if (obj != null)
            {
                obj.SetActive(false); //prevents flash of sphere after anim finishes
                Destroy(obj, 0.1f);
                gumballAnimator.Rebind();
            }
            
            Destroy(obj, 0.1f);
            
            gumballAnimator.Rebind();
            yield return new WaitForSeconds(0.1f);
            
        }

        mainCameraAnimator.SetBool("playBoardAnim", true);
        gameStateManager.currentState = GameStateManager.ScreenState.GameBoard;

        pinballOne.SetActive(true);
        pinballTwo.SetActive(true);

        yield return null;
    }

   

    // Update is called once per frame
    void Update()
    {
        

        
    }
}
