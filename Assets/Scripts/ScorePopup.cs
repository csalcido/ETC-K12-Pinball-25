using UnityEngine;
using TMPro;

public class ScorePopup : MonoBehaviour
{
    public TextMeshProUGUI popupText;
    public float floatSpeed = 1f;
    public float fadeDuration = 1f;

    private float timer = 0f;
    private CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;
        timer += Time.deltaTime;

        if (canvasGroup != null)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
        }

        if (timer >= fadeDuration)
        {
            Destroy(gameObject);
        }
    }

    public void SetText(string text)
    {
        popupText.text = text;
    }
}
