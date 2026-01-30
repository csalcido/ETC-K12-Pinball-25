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
        //PAT END
        Transform impact = transform.Find("Ball_Impact");
        impactEffect = impact.gameObject;
        //achievementManager = FindObjectOfType<Achievement>();

        // Start the self-destruct timer
        //StartCoroutine(SelfDestructAfterDelay(15f));
    }
    /*
    private IEnumerator SelfDestructAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Play the pop animation
        if (animator != null)
        {
            animator.Play("pinballPopAnim"); // must match the state name in Animator
            yield return new WaitForSeconds(.5f);
    }
        Destroy(gameObject);
    }

*/
    // Update is called once per frame
    void Update()
    {
        //PAT EDIT
        if (takePhotosScript != null && ballRenderer != null)
        {
            Color nextColor = takePhotosScript.nextPinballColor;

            if (nextColor.r > 0.8f && nextColor.g < 0.2f && nextColor.b < 0.2f)
            {
                ballRenderer.material = redMaterial;
                ActivateTrail(3);
            }
            else if (nextColor.g > 0.8f && nextColor.r < 0.2f && nextColor.b < 0.2f)
            {

                ballRenderer.material = greenMaterial;
                ActivateTrail(4);
            }
            else if (nextColor.b > 0.8f && nextColor.r < 0.2f && nextColor.g < 0.2f)
            {
                ballRenderer.material = blueMaterial;
                ActivateTrail(5);
            }
        }

        //PAT END
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
