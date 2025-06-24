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

    private Achievement achievementManager;

    private Material mat;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Transform impact = transform.Find("Ball_Impact");
        impactEffect = impact.gameObject;
        achievementManager = FindObjectOfType<Achievement>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Bumper":
                if (bumperSound != null) bumperSound.PlaySound();
                StartCoroutine(EffectsTrigger(impactEffect, 0.3f));
                achievementManager.RegisterBumperHit();
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
                achievementManager.regiserTarget();
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
                achievementManager.RegisterTunnel();
                break;

            case "Gobbler":
                achievementManager.registerGobble();
                break;
        }
    }

    private IEnumerator EffectsTrigger(GameObject obj, float seconds)
    {
        obj.SetActive(true);

        yield return new WaitForSeconds(seconds);

        obj.SetActive(false);
    }
    public void SwitchTrail(Material material)
    {
        char firstChar = material.name[0];
        if (char.IsDigit(firstChar))
        {
            int materialIndex = int.Parse(firstChar.ToString());
            foreach (var trail in trails) {
                trail.gameObject.SetActive(false);
            }
            trails[materialIndex].gameObject.SetActive(true);
        }
    }
}
