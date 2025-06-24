using UnityEngine;
using System.Collections.Generic;

public class Gobbler : MonoBehaviour
{
    private List<GameObject> absorbedPinballs = new List<GameObject>();

    public Transform ejectPoint;
    public int threshold = 5;

    public float ejectForce = 1f;

    public SoundController gobbleSound;
    public SoundController dogSound;
    public SoundController fishSound;
    public SoundController snakeSound;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            gobbleSound.PlaySound();
            other.gameObject.SetActive(false);
            absorbedPinballs.Add(other.gameObject);

            if (absorbedPinballs.Count >= threshold)
            {
                EjectStoredPinballs();
            }
        }
    }

    void EjectStoredPinballs()
    {
        foreach (GameObject pinball in absorbedPinballs)
        {
            pinball.transform.position = ejectPoint.position;
            pinball.SetActive(true);

            Rigidbody rb = pinball.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                Vector3 mainDirection = new Vector3(1f, 0f, -1f).normalized;
                float angleRange = 120f;
                float randomAngle = Random.Range(-angleRange, angleRange);
                Quaternion rotation = Quaternion.AngleAxis(randomAngle, Vector3.up);
                Vector3 randomDir = rotation * mainDirection;

                rb.AddForce(randomDir * ejectForce, ForceMode.Impulse);
            }
        }
        if (Announcer.instance.GetCurrentThemeIndex()==0) { dogSound.PlaySound(); }
        else if (Announcer.instance.GetCurrentThemeIndex() == 1) { snakeSound.PlaySound(); }
        else if (Announcer.instance.GetCurrentThemeIndex() ==2) { fishSound.PlaySound();}
        else { snakeSound.PlaySound(); }
        absorbedPinballs.Clear();
        Debug.Log("Ejected!");
    }
}
