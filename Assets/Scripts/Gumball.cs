using System.Collections;
using UnityEngine;

public class Gumball : MonoBehaviour
{

    public GameObject[] spawnBuffs; //public array of prefabs to instantiate
    public Animator mainCameraAnimator;//Main camera animator
    public Animator leverAnimator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        startGumballMachine();
        
    }

    IEnumerator startGumballMachine()
    {
        yield return new WaitForSeconds(3f);

        Debug.Log("gumballs started!");
        
        //play lever animation

        //play lever sound

        for (int i = 0; i < 4; i++)
        {
            //spawn 4 buffs at random
        }

        mainCameraAnimator.SetBool("playBoardAnim", true);

        yield return null;
    } 

    void SpawnBuff()
    {
        //buff gets selected for animation

        //buff gets instantiated on map

        
    }

    // Update is called once per frame
    void Update()
    {
        

        
    }
}
