using UnityEngine;
using TMPro;

public class BuffPopupManager : MonoBehaviour
{
    public static BuffPopupManager instance;
    
    public GameObject buffPopupPrefab;
    public Canvas gameCanvas;
    
    void Awake()
    {
        // Allow only one instance of this manager
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void ShowBuffPopup(BuffType buffType)
    {
        if (buffPopupPrefab == null || gameCanvas == null)
        {
            Debug.LogWarning("BuffPopupManager: Missing prefab or canvas!");
            return;
        }
        
        // Create new popup at center of screen
        GameObject popup = Instantiate(buffPopupPrefab, gameCanvas.transform);
        
        // Get the popup script and set the text
        BuffPopup popupScript = popup.GetComponent<BuffPopup>();
        if (popupScript != null)
        {
            string buffName = GetBuffName(buffType);
            popupScript.ShowBuff(buffName);
        }
    }
    
    private string GetBuffName(BuffType buffType)
    {
        switch (buffType)
        {
            case BuffType.Size:
                return "SIZE BOOST";
            case BuffType.SpawnBalls:
                return "MULTI BALL";
            case BuffType.SpeedMultiplier:
                return "SPEED BOOST";
            default:
                return "POWER UP";
        }
    }
}
