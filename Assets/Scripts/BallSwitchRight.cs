using UnityEngine;
using System.Collections.Generic;

public class BallSwitchRight : MonoBehaviour
{
    public Material[] materials;
    public int currentMaterialIndex = 0;
    private List<GameObject> pinballs = new List<GameObject>();

    private Xbox xboxControls;

    public Achievement achievement;
    void Start()
    {
        xboxControls = new Xbox();
        xboxControls.Enable();
    }

    private void Update()
    {
        if (pinballs.Count > 0)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                currentMaterialIndex = (currentMaterialIndex + 1) % materials.Length;
                ApplyMaterialToAllPinballs();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow)|| xboxControls.Player.D.WasPressedThisFrame())
            {
                currentMaterialIndex = (currentMaterialIndex - 1 + materials.Length) % materials.Length;
                ApplyMaterialToAllPinballs();
                achievement.registerColorChanged();
            }
        }
    }

    private void ApplyMaterialToAllPinballs()
    {
        foreach (var pinball in pinballs)
        {
            if (pinball != null)
            {
                // REMOVED: pinball.GetComponent<Renderer>().material = newMat;
                // This was overriding the locked color from BallEffect. If you want dynamic ball colors, re-enable this and adjust BallEffect.

                var trailController = pinball.GetComponent<BallEffect>();
                if (trailController != null)
                {
                    trailController.SwitchTrail(materials[currentMaterialIndex]);  // Keep trail switching if desired
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            pinballs.Add(collision.gameObject);
            ApplyMaterialToAllPinballs();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            pinballs.Remove(collision.gameObject);
        }
    }
}