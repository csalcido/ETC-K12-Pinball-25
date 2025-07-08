using UnityEngine;
using TMPro;

public class BuffPopup : MonoBehaviour
{
    public TextMeshProUGUI buffText;
    
    private float displayTime = 2f;
    private float fadeTime = 1f;
    private float timer = 0f;
    
    void Start()
    {}
    
    void Update()
    {
        timer += Time.deltaTime;
        
        // Start fading after display time
        if (timer > displayTime)
        {
            float fadeProgress = (timer - displayTime) / fadeTime;
            float alpha = 1f - fadeProgress;
            
            if (buffText != null)
            {
                Color textColor = buffText.color;
                textColor.a = alpha;
                buffText.color = textColor;
            }
            
            // Destroy when fade is done
            if (fadeProgress >= 1f)
            {
                Destroy(gameObject);
            }
        }
    }
    
    public void ShowBuff(string buffName)
    {
        if (buffText != null)
        {
            buffText.text = buffName + " ACTIVATED!";
        }
    }
}
