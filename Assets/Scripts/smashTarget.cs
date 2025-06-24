using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public class smashTarget : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject wholeModel;
    public GameObject halfBrokenModel;
    public GameObject fullBrokenModel;
    public GameObject vfx;

    public Achievement achievementManager;

    private int hitCount = 0;

    public int restoreMaterialIndex = 0;

    void Start()
    {
        hitCount = 0;
        wholeModel.SetActive(true);
        halfBrokenModel.SetActive(false);
        fullBrokenModel.SetActive(false);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Ball")) return;

        Renderer renderer = collision.gameObject.GetComponent<Renderer>();
        if (renderer == null) return;

        int materialIndex = GetThemeIndex(renderer.material);

        StartCoroutine(VFX(vfx, 1f));

        if (materialIndex == restoreMaterialIndex)
        {
            RestoreStage();
        }
        else
        {
            BreakStage();
        }
    }

    private int GetThemeIndex(Material material)
    {
        string materialName = material.name;
        string numberPart = materialName.Replace("Material", "").Replace(" (Instance)", "").Trim();

        int index = int.Parse(numberPart);

        return index;
    }

    void BreakStage()
    {
        hitCount++;

        if (hitCount == 1)
        {
            wholeModel.SetActive(false);
            halfBrokenModel.SetActive(true);
        }
        else if (hitCount == 2)
        {
            halfBrokenModel.SetActive(false);
            fullBrokenModel.SetActive(true);
        }
        else if (hitCount >= 3)
        {
            gameObject.SetActive(false);
            wholeModel.SetActive(true);
            halfBrokenModel.SetActive(false);
            fullBrokenModel.SetActive(false);
            achievementManager.GetComponent<Achievement>().RegisterSmash();
        }
    }

    void RestoreStage()
    {
        hitCount = Mathf.Max(hitCount - 1, 0);

        if (hitCount == 0)
        {
            wholeModel.SetActive(true);
            halfBrokenModel.SetActive(false);
            fullBrokenModel.SetActive(false);
        }
        else if (hitCount == 1)
        {
            wholeModel.SetActive(false);
            halfBrokenModel.SetActive(true);
            fullBrokenModel.SetActive(false);
        }
    }

    private IEnumerator VFX(GameObject obj, float seconds)
    {
        obj.SetActive(true);

        yield return new WaitForSeconds(seconds);

        obj.SetActive(false);
    }

}
