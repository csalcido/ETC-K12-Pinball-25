using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuffEffect : MonoBehaviour
{
    private Dictionary<BuffType, Coroutine> buffCoroutines = new Dictionary<BuffType, Coroutine>();
    
    public void ApplyBuff(BuffType buffType, float buffValue, float duration)
    {
        if (buffCoroutines.ContainsKey(buffType))
        {
            return; // buff already active
        }
        
        BuffBase newBuff = CreateBuff(buffType, buffValue, duration);
        if (newBuff != null)
        {
            newBuff.Apply();
            
            Coroutine buffCoroutine = StartCoroutine(BuffTimer(newBuff, buffType, duration));
            buffCoroutines[buffType] = buffCoroutine;
        }
    }
    
    private BuffBase CreateBuff(BuffType buffType, float value, float duration)
    {
        switch (buffType)
        {
            case BuffType.Size:
                return new SizeBuff(gameObject, value, duration);
            case BuffType.Speed:
                return new SpeedBuff(gameObject, value, duration);
            case BuffType.SpawnBalls:
                return new SpawnBallsBuff(gameObject, value, duration);
            case BuffType.SlowMotion:
                return new SlowMotionBuff(gameObject, value, duration);
            default:
                Debug.LogWarning($"Buff type {buffType} not implemented");
                return null;
        }
    }
    
    private IEnumerator BuffTimer(BuffBase buff, BuffType buffType, float duration)
    {
        yield return new WaitForSeconds(duration);
        RemoveBuff(buffType);
    }
    
    private void RemoveBuff(BuffType buffType)
    {        
        if (buffCoroutines.ContainsKey(buffType))
        {
            StopCoroutine(buffCoroutines[buffType]);
            buffCoroutines.Remove(buffType);
        }
    }
    
    private void OnDestroy()
    {
        // Clean up any active buffs
        foreach (var buffCoroutine in buffCoroutines.Values)
        {
            StopCoroutine(buffCoroutine);
        }
        buffCoroutines.Clear();
    }
}
