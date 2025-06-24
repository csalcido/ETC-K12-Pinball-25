using UnityEngine;

public class SoundController : MonoBehaviour
{

    private AudioSource audioSource;
    public float minPitch = 0.9f;
    public float maxPitch = 1.1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlaySound()
    {
            audioSource.pitch = Random.Range(minPitch, maxPitch);
            audioSource.Play();
    }
    public void StopSound()
    {
        audioSource.Stop();
    }
}
