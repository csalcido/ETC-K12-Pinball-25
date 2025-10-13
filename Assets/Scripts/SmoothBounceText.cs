using UnityEngine;
using TMPro;
using System.Collections;

public class SmoothBounceText : MonoBehaviour
{
   public TextMeshProUGUI textToBounce;
    public float bounceHeight = 20f; // how many units up and down
    public float bounceSpeed = 2f;   // how fast it bounces

    private RectTransform rectTransform;
    private float originalY;

    void Start()
    {
        if (textToBounce == null)
            textToBounce = GetComponent<TextMeshProUGUI>();

        rectTransform = textToBounce.rectTransform;
        originalY = rectTransform.anchoredPosition.y;
    }

    void Update()
    {
        // Smooth sinusoidal bounce
        float newY = originalY + Mathf.Sin(Time.time * bounceSpeed) * bounceHeight;
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, newY);
    }
}