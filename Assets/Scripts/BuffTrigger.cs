using UnityEngine;
using System.Collections;

public class BuffTrigger : MonoBehaviour
{
    [Header("Buff Configuration")]
    public BuffType buffType = BuffType.Size;
    public float buffDuration = 5f;
    public float buffValue = 1.5f;

    [Header("Visual Feedback")]

    Animator buffAnimator;
    public bool disableAfterUse = false;
    public float respawnTime = 10f;

    private bool isActive = true;

    public GameStateManager gameStateManager;

    private void Start()
    {
        buffAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActive) return;

        if (gameStateManager.currentState == GameStateManager.ScreenState.GameBoard) //only usable when game is happening
        {
            BuffEffect ballBuff = other.GetComponent<BuffEffect>();
            if (ballBuff != null)
            {
                ballBuff.ApplyBuff(buffType, buffValue, buffDuration);
                buffAnimator.SetBool("IsTriggered", true);

                Debug.Log("triggered!");

                if (disableAfterUse)
                {
                    StartCoroutine(DisableTemporarily());
                }
            }
        }
    }

    private IEnumerator DisableTemporarily()
    {
        isActive = false;
        GetComponent<Renderer>().enabled = false;
        GetComponent<Collider>().enabled = false;

        yield return new WaitForSeconds(respawnTime);

        isActive = true;
        GetComponent<Renderer>().enabled = true;
        GetComponent<Collider>().enabled = true;
    }
    

    
}
