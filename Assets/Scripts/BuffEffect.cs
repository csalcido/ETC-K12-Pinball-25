using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuffEffect : MonoBehaviour
{
    private Dictionary<BuffType, Coroutine> buffCoroutines = new Dictionary<BuffType, Coroutine>();
    private Dictionary<BuffType, BuffBase> activeBuffs = new Dictionary<BuffType, BuffBase>();
    
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
            activeBuffs[buffType] = newBuff;

            // Show buff popup
            if (BuffPopupManager.instance != null)
            {
                BuffPopupManager.instance.ShowBuffPopup(buffType);
            }
            
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
            case BuffType.SpawnBalls:
                return new SpawnBallsBuff(gameObject, value, duration);
            case BuffType.SpeedMultiplier:
                return new SpeedMultiplier(gameObject, value, duration);
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
        
        // Remove the buff from the active buffs
        if (activeBuffs.ContainsKey(buffType))
        {
            activeBuffs[buffType].Remove();
            activeBuffs.Remove(buffType);
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
