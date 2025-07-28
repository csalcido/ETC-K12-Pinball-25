using System.Collections;
using UnityEngine;

public class Gumball : MonoBehaviour
{

    public GameObject[] spawnBuffs; //public array of prefabs to instantiate
    public Animator mainCameraAnimator;//Main camera animator
    public Animator leverAnimator;
    public Transform[] spawnLocations;

    public GameObject gumballDrop;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(startGumballMachine());
        
    }

    IEnumerator startGumballMachine()
    {
        yield return new WaitForSeconds(3f);


        //play lever animation

        //play lever sound

        for (int i = 0; i < spawnLocations.Length; i++)
        {
            //randomize buff
            var randomBuff = spawnBuffs[Random.Range(0, spawnBuffs.Length)];

            //spawn buff at random
            Instantiate(randomBuff, spawnLocations[i]);

            //play animation of it coming out of gumball machine
            Instantiate(randomBuff, gumballDrop.transform);

            yield return new WaitForSeconds(2f);
        }

        mainCameraAnimator.SetBool("playBoardAnim", true);

        yield return null;
    }

   

    // Update is called once per frame
    void Update()
    {
        

        
    }
}
