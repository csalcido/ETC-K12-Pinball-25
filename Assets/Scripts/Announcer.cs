using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEditor;

public class Announcer : MonoBehaviour
{
    public static Announcer instance;
    /*void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
        }
    }*/

    public GameObject[] bumpers;

    public GameObject[] theme0;
    public GameObject[] theme1;
    public GameObject[] theme2;

    public GameObject[] themeChangeUI;
    public GameObject[] themeChangeUI_Done;

    private GameObject[][] allThemes;

    public Animator[] animators;

    public Image imageDisplay;
    private Renderer announcerRenderer;

    public int currentThemeIndex = -1;

    public SoundController clapSound;

    private void Start()
    {
        announcerRenderer = GetComponent<Renderer>();
        allThemes = new GameObject[][] { theme0, theme1, theme2 };
        //DisableAllThemes();
    }

    private void Update()
    {
        CheckBumperMaterials();
    }

    private void CheckBumperMaterials()
    {
        if (bumpers == null) return;
        // check if all bumpers (and slingshots) have same materials
        List<Material> materials = new List<Material>();

        foreach (var bumper in bumpers)
        {
            Renderer bumperRenderer = bumper.GetComponent<Renderer>();
            if (bumperRenderer != null)
            {
                materials.Add(bumperRenderer.material);
            }
        }

        bool allMaterialsMatch = materials.All(material => material.name == materials[0].name);

        if (allMaterialsMatch && materials.Count > 0)
        {
            announcerRenderer.material = materials[0];
            SwitchImage(materials[0]);
        }
    }

    private void SwitchImage(Material material)
    {
        char firstChar = material.name[0];
        if (char.IsDigit(firstChar))
        {
            int materialIndex = int.Parse(firstChar.ToString());

            if (materialIndex == currentThemeIndex)
            {
                return;
            }

            DisableAllThemes();
            SetThemeActive(materialIndex, true);
            animators[0].SetBool("isFall", false);
            animators[1].SetBool("isFall", false);
            animators[2].SetBool("isFall", false);

            // theme change achievement UI is seperated coded here
            themeChangeUI[materialIndex].SetActive(true);
            clapSound.PlaySound();
            var cg = themeChangeUI[materialIndex].GetComponent<CanvasGroup>();
            if (cg != null) cg.alpha = 1f;
            StartCoroutine(FadeOutAndDisable(themeChangeUI[materialIndex]));
            themeChangeUI_Done[materialIndex].SetActive(true);

            currentThemeIndex = materialIndex;
        }
    }
    private IEnumerator FadeOutAndDisable(GameObject uiObject, float delay = 3f, float fadeDuration = 1f)
    {
        yield return new WaitForSeconds(delay);

        CanvasGroup cg = uiObject.GetComponent<CanvasGroup>();
        if (cg == null)
        {
            cg = uiObject.AddComponent<CanvasGroup>();
        }

        float elapsed = 0f;
        float startAlpha = cg.alpha;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            cg.alpha = Mathf.Lerp(startAlpha, 0f, elapsed / fadeDuration);
            yield return null;
        }

        cg.alpha = 0f;
        uiObject.SetActive(false);
    }

    public void DisableAllThemes()
    {
        for (int i = 0; i < allThemes.Length; i++)
        {
            SetThemeActive(i, false);
        }
        foreach (var ui in themeChangeUI) {
            ui.SetActive(false);
        }
    }

    private void SetThemeActive(int themeIndex, bool isActive)
    {
        GameObject[] theme = allThemes[themeIndex];
        foreach (GameObject obj in theme)
        {
            if (obj != null && obj.activeSelf != isActive)
            {
                obj.SetActive(isActive);
            }
        }
    }

    public int GetCurrentThemeIndex()
    {
        return currentThemeIndex;
    }
}
