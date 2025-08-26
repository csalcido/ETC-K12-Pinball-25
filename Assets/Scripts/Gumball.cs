using System.Collections;
using UnityEngine;

public class Gumball : MonoBehaviour
{

    public GameObject[] spawnBuffs; //public array of prefabs to instantiate
    public Animator mainCameraAnimator;//Main camera animator
    public Animator leverAnimator;
    public GameObject lever;
    public Transform[] spawnLocations;

    public GameObject gumballDrop;
    public Animator gumballAnimator;
    public GameStateManager gameStateManager;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(startGumballMachine());
        
    }

    IEnumerator startGumballMachine()
    {
        yield return new WaitForSeconds(2f); //delay between camera transition and gumball generating
        Debug.Log("Playing leverAnim on " + leverAnimator);
        //play lever animation
        leverAnimator.Play("leverAnim");
       

        //play lever sound
        lever.GetComponent<AudioSource>().Play();


        for (int i = 0; i < spawnLocations.Length; i++)
        {
            //randomize buff
            var randomBuff = spawnBuffs[Random.Range(0, spawnBuffs.Length)];

            //spawn buff at random
            Instantiate(randomBuff, spawnLocations[i]);

            //play animation of it coming out of gumball machine
            gumballAnimator.Play("gumballDropAnim");


            GameObject obj = Instantiate(randomBuff, gumballDrop.transform); //this is all to get rid of weird transforms once gumball is instantiated
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
            obj.transform.localScale = Vector3.one;
            
            yield return new WaitForSeconds(2.5f);
            Destroy(obj, 0.1f);
            gumballAnimator.Rebind();
            
        }

        mainCameraAnimator.SetBool("playBoardAnim", true);
        gameStateManager.currentState = GameStateManager.ScreenState.GameBoard;

        yield return null;
    }

   

    // Update is called once per frame
    void Update()
    {
        

        
    }
}
