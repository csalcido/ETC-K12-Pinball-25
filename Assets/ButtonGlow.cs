using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonGlow : MonoBehaviour
{
    public Image buttonImage;       // Assign the button's Image in inspector
    public Color glowColor = Color.cyan; // The color of the glow
    public float glowSpeed = 2f;    // How fast it pulses
    public float minAlpha = 0.3f;   // Lowest brightness
    public float maxAlpha = 1f;     // Highest brightness

    private Color baseColor;

    void Start()
    {
        if (buttonImage == null)
            buttonImage = GetComponent<Image>();

        baseColor = buttonImage.color;
        StartCoroutine(GlowLoop());
    }

    private IEnumerator GlowLoop()
    {
        float t = 0f;

        while (true)
        {
            // PingPong goes 0→1→0 smoothly
            float alpha = Mathf.Lerp(minAlpha, maxAlpha, Mathf.PingPong(t * glowSpeed, 1f));
            buttonImage.color = new Color(glowColor.r, glowColor.g, glowColor.b, alpha);

            t += Time.deltaTime;
            yield return null;
        }
    }
}