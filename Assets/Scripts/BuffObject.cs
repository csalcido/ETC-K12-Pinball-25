using UnityEngine;

public class BuffObject : MonoBehaviour
{
    public float buffDuration = 5f;
    public float sizeMultiplier = 1.5f;
    
    private void OnTriggerEnter(Collider other)
    {
        // check if ball hit the buff
        BuffEffect ballBuff = other.GetComponent<BuffEffect>();
        if (ballBuff != null)
        {
            ballBuff.ApplyBuff(sizeMultiplier, buffDuration);
        }
    }
}
