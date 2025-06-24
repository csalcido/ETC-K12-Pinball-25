using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BumperEffect : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject[] impactEffects;
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("Ball"))
        {
            Renderer pinballRenderer = collision.gameObject.GetComponent<Renderer>();
            int themeIndex = GetThemeIndex(pinballRenderer.material);
            StartCoroutine(EffectsTrigger(impactEffects[themeIndex], 0.3f));
        }
    }

    private IEnumerator EffectsTrigger(GameObject obj, float seconds)
    {
        obj.SetActive(true);

        yield return new WaitForSeconds(seconds);

        obj.SetActive(false);
    }

    private int GetThemeIndex(Material material)
    {
        string materialName = material.name;
        string numberPart = materialName.Replace("Material", "").Replace(" (Instance)", "").Trim();

        int index = int.Parse(numberPart);

        return index;
    }
}
