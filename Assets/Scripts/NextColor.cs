using UnityEngine;
using TMPro;

public class UIColorDisplay : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI colorText;
    [SerializeField] private TakePhotos takePhotosScript;
    
    private void Start()
    {
        if (takePhotosScript == null)
        {
            takePhotosScript = FindAnyObjectByType<TakePhotos>();
        }
        
        if (takePhotosScript == null)
        {
            Debug.LogError("TakePhotos script not found! Please assign it in the inspector.");
        }
    }
    
    private void Update()
    {
        if (takePhotosScript != null && colorText != null)
        {
            Color nextColor = takePhotosScript.nextPinballColor;
            colorText.text = GetColorName(nextColor);
        }
    }
    
    private string GetColorName(Color color)
    {
        if (color.r > 0.8f && color.g < 0.2f && color.b < 0.2f)
            return "Color: Red";
        else if (color.g > 0.8f && color.r < 0.2f && color.b < 0.2f)
            return "Color: Green";
        else if (color.b > 0.8f && color.r < 0.2f && color.g < 0.2f)
            return "Color: Blue";
        else
            return "Unknown";
    }
}
