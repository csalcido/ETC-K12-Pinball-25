using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BallEffect : MonoBehaviour
{
    private GameObject impactEffect;

    public SoundController bumperSound;
    public SoundController slingshotSound;
    public SoundController metalSound;
    public SoundController plasticSound;
    public SoundController deadSound;
    public SoundController tunnelSound;
    public SoundController smashSound;

    public GameObject[] trails;

    //private Achievement achievementManager;
    public GameStateManager gameStateManager;

    //PAT EDIT: changing color of ball based on current color
    public TakePhotos takePhotosScript;
    public Material redMaterial;
    public Material greenMaterial;
    public Material blueMaterial;

    private Renderer ballRenderer;
    private Animator animator;
    
    //PAT end
    private Material mat;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //PAT EDIT
        ballRenderer = GetComponent<Renderer>();
          // Grab the Animator component attached to this ball
        animator = GetComponent<Animator>();
        
        if (takePhotosScript == null)
        {
            takePhotosScript = FindAnyObjectByType<TakePhotos>();
        }

        // LOCK THE BALL'S COLOR ONCE AT LAUNCH: Assign material based on assigned color from TakePhotos
        // Use Instantiate() to create unique material instances (prevents shared changes)
        if (takePhotosScript != null && ballRenderer != null)
        {
            Color ballColor = takePhotosScript.GetOrAssignColorForBall(this.gameObject);

            // Determine nearest primary for selection
            int chosenTrailIndex = -1;
            if (ballColor.r >= ballColor.g && ballColor.r >= ballColor.b)
            {
                ballRenderer.material = Instantiate(redMaterial);
                chosenTrailIndex = 0; // red trail index 
            }
            else if (ballColor.g >= ballColor.r && ballColor.g >= ballColor.b)
            {
                ballRenderer.material = Instantiate(greenMaterial);
                chosenTrailIndex = 1; // green trail index
            }
            else
            {
                ballRenderer.material = Instantiate(blueMaterial);
                chosenTrailIndex = 2; // blue trail index
            }

            ActivateTrail(chosenTrailIndex);
            string trailName = (trails != null && chosenTrailIndex >= 0 && chosenTrailIndex < trails.Length && trails[chosenTrailIndex] != null)
                ? trails[chosenTrailIndex].name
                : "(none)";
            Debug.Log($"Ball color locked at launch: {ballColor} | Material: {ballRenderer.material.name} | trailIndex: {chosenTrailIndex} | trailName: {trailName}");
        }
        //PAT END

        Transform impact = transform.Find("Ball_Impact");
        if (impact != null)
        {
            impactEffect = impact.gameObject;
        }
        else
        {
            Debug.LogWarning("Ball_Impact child not found on ball prefab");
        }
        //achievementManager = FindObjectOfType<Achievement>();

        // Start the self-destruct timer
        //StartCoroutine(SelfDestructAfterDelay(15f));
    }

    // Update is called once per frame
    void Update()
    {
        // REMOVED: Color-changing logic - balls now lock their color in Start() and don't change
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Bumper":
                if (bumperSound != null) bumperSound.PlaySound();
                StartCoroutine(EffectsTrigger(impactEffect, 0.3f));
                //achievementManager.RegisterBumperHit();
                break;

            case "Slingshot":
                if (slingshotSound != null) slingshotSound.PlaySound();
                StartCoroutine(EffectsTrigger(impactEffect, 0.3f));
                break;

            case "Metal":
                if (metalSound != null) metalSound.PlaySound();
                break;

            case "Plastic":
                if (plasticSound != null) plasticSound.PlaySound();
                break;

            case "Smash":
                if (smashSound != null) smashSound.PlaySound();
                break;

            case "Drop":
                if (bumperSound != null) bumperSound.PlaySound();
                //achievementManager.regiserTarget();
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Dead":
                if (deadSound != null) deadSound.PlaySound();
                break;

            case "Tunnel":
                if (tunnelSound != null) tunnelSound.PlaySound();
                //achievementManager.RegisterTunnel();
                break;

            case "Gobbler":
                //achievementManager.registerGobble();
                break;
        }
    }

    private IEnumerator EffectsTrigger(GameObject obj, float seconds)
    {
        obj.SetActive(true);

        yield return new WaitForSeconds(seconds);

        obj.SetActive(false);
    }

    // PAT EDIT: changing trail color based on color mode

    // Helper method to change the color of all trails
   void ActivateTrail(int index)
{
    Debug.Log($"ActivateTrail requested index={index}, trails.Length={ (trails!=null?trails.Length:0) }");
    if (trails == null)
    {
        Debug.Log("ActivateTrail: trails array is null");
        return;
    }

    for (int i = 0; i < trails.Length; i++)
    {
        var t = trails[i];
        if (t == null)
        {
            Debug.Log($"ActivateTrail: trails[{i}] is null");
            continue;
        }
        bool shouldBeActive = (i == index);
        t.SetActive(shouldBeActive);
        Debug.Log($"ActivateTrail: trails[{i}]='{t.name}' setActive={shouldBeActive}");
    }

    // After toggling, log which ones are active for easier debugging
    string activeList = "";
    for (int i = 0; i < trails.Length; i++)
    {
        var t = trails[i];
        if (t != null && t.activeSelf) activeList += $"[{i}] {t.name} ";
    }
    Debug.Log($"ActivateTrail result active: {activeList}");
}

//PAT END
    public void SwitchTrail(Material material)
    {
        if (material == null)
        {
            Debug.Log("SwitchTrail called with null material");
            return;
        }

        Debug.Log($"SwitchTrail called with material.name='{material.name}'");
        // Log caller stack to help find what is reactivating the trails
        Debug.Log("SwitchTrail stack:\n" + new System.Diagnostics.StackTrace());
        int materialIndex = -1;
        // try numeric prefix first
        if (!string.IsNullOrEmpty(material.name) && char.IsDigit(material.name[0]))
        {
            materialIndex = int.Parse(material.name[0].ToString());
        }
        else
        {
            // fallback: detect by keywords in material name
            string lower = material.name.ToLowerInvariant();
            if (lower.Contains("red")) materialIndex = 0;
            else if (lower.Contains("green")) materialIndex = 1;
            else if (lower.Contains("blue")) materialIndex = 2;
            else
            {
                Debug.Log($"SwitchTrail: material name does not start with digit and no color keyword found: '{material.name}'");
                return;
            }
        }
        Debug.Log($"SwitchTrail: parsed materialIndex={materialIndex}, trails.Length={(trails!=null?trails.Length:0)}");
        if (trails == null || materialIndex < 0 || materialIndex >= trails.Length)
        {
            Debug.Log("SwitchTrail: invalid trail index or null trails array");
            return;
        }

        for (int i = 0; i < trails.Length; i++)
        {
            if (trails[i] != null)
            {
                trails[i].gameObject.SetActive(i == materialIndex);
                Debug.Log($"SwitchTrail: trails[{i}]='{trails[i].name}' setActive={(i==materialIndex)}");
            }
            else
            {
                Debug.Log($"SwitchTrail: trails[{i}] is null");
            }
        }
    }
}
