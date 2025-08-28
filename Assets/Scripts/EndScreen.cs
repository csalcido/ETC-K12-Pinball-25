using System.Collections;
using UnityEngine;

public class EndScreen : MonoBehaviour
{

    public Animator mainCameraAnimator;
    public GameStateManager gameStateManager;
    public SerialManager serialManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        StartCoroutine(PlayEndAnimation());
       

    }
    IEnumerator PlayEndAnimation()
    {
        yield return null;
        mainCameraAnimator.SetBool("playEndAnim", true); // MAKE ANIMATION FROM BOARD TO END

        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStateManager.currentState == GameStateManager.ScreenState.PhotoIsPrinted)
        {
            //if the photo has been printed , then player can restart
        }
        
    }
}
