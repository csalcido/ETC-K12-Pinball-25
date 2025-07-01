using UnityEngine;
using System.Collections;

public class BuffEffect : MonoBehaviour
{
    private Vector3 originalScale;
    private bool isBuffed = false;
    
    void Start()
    {
        // save original size
        originalScale = transform.localScale;
    }
    
    public void ApplyBuff(float sizeMultiplier, float duration)
    {
        if (!isBuffed)
        {
            StartCoroutine(BuffCoroutine(sizeMultiplier, duration));
        }
    }
    
    private IEnumerator BuffCoroutine(float sizeMultiplier, float duration)
    {
        isBuffed = true;
        
        // make ball bigger
        transform.localScale = originalScale * sizeMultiplier;
        
        // wait for buff to end
        yield return new WaitForSeconds(duration);
        
        // make ball normal size again
        transform.localScale = originalScale;
        isBuffed = false;
    }
}
