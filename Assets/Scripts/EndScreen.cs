using UnityEngine;

public class EndScreen : MonoBehaviour
{

    public Animator mainCameraAnimator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        //play end screen camera transition
        mainCameraAnimator.SetBool("playEnddAnim", true); // MAKE ANIMATION FROM BOARD TO END

        //particle confetti effect

        //ending sound, drum roll?


        //photo pop in animation 
        
        //delay, then ask to restart

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
