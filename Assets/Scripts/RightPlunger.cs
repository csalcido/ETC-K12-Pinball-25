using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class RightPlunger : MonoBehaviour
{
    public Respawn respawnScript;

    public Rigidbody ball;
    public float maxForce = 5f;
    public float chargeSpeed = 1f;
    public float minForce = 1f;
    public KeyCode plungerKey;
    public float closeTime = 1.5f;
    public bool isLaunching = false;
    private bool isWaitingForReturn = false;

    private float currentForce = 0f;
    private bool isCharging = false;
    private List<Rigidbody> ballsInContact = new List<Rigidbody>();

    public float minDistance = 4f;
    public float maxDistance = 7f;

    public SerialManager serialManager;

    public GameObject plungerClose;

    public GameObject[] plungerVFX;

    public SoundController plungerSound;

    private bool ballHasLeftPlunger = true;

    public BallSwitchRight ballSwitchRight;
    private int matIndex = 0;

    public StartManager startManager;

    void Update()
    {
        isLaunching = false;
        float distanceCM = serialManager.DistanceCMR;
        distanceCM = distanceCM > 9f ? 4f : distanceCM;
        matIndex = ballSwitchRight.currentMaterialIndex;

        if ((Input.GetKeyDown(plungerKey) && ballsInContact.Count > 0) || (distanceCM > maxDistance && ballsInContact.Count > 0))
        {
            isCharging = true;
        }

        if ((Input.GetKey(plungerKey) && isCharging) || (distanceCM > maxDistance && isCharging))
        {
            currentForce += chargeSpeed * Time.deltaTime;
            currentForce = Mathf.Clamp(currentForce, minForce, maxForce);
        }

        if ((Input.GetKeyUp(plungerKey) && isCharging) || (distanceCM < minDistance && isCharging))
        {
            isLaunching = true;
            startManager.RegisterStart();
            LaunchBalls();
            isCharging = false;
            currentForce = 0f;
        }
    }

    void LaunchBalls()
    {
        foreach (var ball in ballsInContact)
        {
            if (ball != null)
            {
                Vector3 force = transform.forward * currentForce;
                ball.AddForce(force, ForceMode.Impulse);
            }
        }
        ballHasLeftPlunger = true;

        ballsInContact.Clear();
        plungerSound.PlaySound();

        StartCoroutine(DisableForSeconds(plungerClose, closeTime));
        StartCoroutine(EffectsTrigger(plungerVFX[matIndex], 1f));
        StartCoroutine(CheckBallReturn());
    }

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody ballRb = collision.gameObject.GetComponent<Rigidbody>();
        if (ballRb != null && !ballsInContact.Contains(ballRb))
        {
            ballsInContact.Add(ballRb);
        }

        if (isWaitingForReturn && collision.gameObject.CompareTag("Ball"))
        {
            ballHasLeftPlunger = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        Rigidbody ballRb = collision.gameObject.GetComponent<Rigidbody>();
        if (ballRb != null && ballsInContact.Contains(ballRb))
        {
            ballsInContact.Remove(ballRb);
        }
    }

    private IEnumerator DisableForSeconds(GameObject obj, float seconds)
    {
        obj.SetActive(false);

        yield return new WaitForSeconds(seconds);

        obj.SetActive(true);
    }

    private IEnumerator EffectsTrigger(GameObject obj, float seconds)
    {
        obj.SetActive(true);

        yield return new WaitForSeconds(seconds);

        obj.SetActive(false);
    }

    IEnumerator CheckBallReturn()
    {
        isWaitingForReturn = true;
        ballHasLeftPlunger = true;

        float timer = 0f;
        while (timer < 3)
        {
            if (!ballHasLeftPlunger)
            {
                isWaitingForReturn = false;
                yield break;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        isWaitingForReturn = false;
        respawnScript.SpawnBall(false);
    }
}
