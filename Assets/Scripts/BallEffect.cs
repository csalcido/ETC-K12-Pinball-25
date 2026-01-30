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

        // LOCK THE BALL'S COLOR ONCE AT LAUNCH: Assign material based on nextPinballColor at this moment
        // Use Instantiate() to create unique material instances (prevents shared changes)
        if (takePhotosScript != null && ballRenderer != null)
        {
            Color ballColor = takePhotosScript.nextPinballColor;  // Capture the color for this ball

            if (ballColor.r > 0.8f && ballColor.g < 0.2f && ballColor.b < 0.2f)
            {
                ballRenderer.material = Instantiate(redMaterial);  // Unique red material
                ActivateTrail(3);
            }
            else if (ballColor.g > 0.8f && ballColor.r < 0.2f && ballColor.b < 0.2f)
            {
                ballRenderer.material = Instantiate(greenMaterial);  // Unique green material
                ActivateTrail(4);
            }
            else if (ballColor.b > 0.8f && ballColor.r < 0.2f && ballColor.g < 0.2f)
            {
                ballRenderer.material = Instantiate(blueMaterial);  // Unique blue material
                ActivateTrail(5);
            }
            Debug.Log("Ball color locked at launch: " + ballColor + " | Material: " + ballRenderer.material.name);  // Debug: Confirm assignment
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
    for (int i = 0; i < trails.Length; i++)
    {
        trails[i].SetActive(i == index);
    }
}

//PAT END
    public void SwitchTrail(Material material)
    {
        char firstChar = material.name[0];
        if (char.IsDigit(firstChar))
        {
            int materialIndex = int.Parse(firstChar.ToString());
            foreach (var trail in trails)
            {
                trail.gameObject.SetActive(false);
            }
            trails[materialIndex].gameObject.SetActive(true);
        }
    }
}
