using UnityEngine;
using TMPro;

public class ScorePopupSpawner : MonoBehaviour
{
    public static ScorePopupSpawner Instance;

    public GameObject popupPrefab;
    public Canvas worldSpaceCanvas;
    public Camera mainCamera;

    void Awake()
    {
        Instance = this;
    }

    public void SpawnPopup(int score, Vector3 worldPos)
    {
        Vector3 screenPos = mainCamera.WorldToScreenPoint(worldPos);
        GameObject popup = Instantiate(popupPrefab, worldSpaceCanvas.transform);
        popup.transform.position = screenPos;

        popup.GetComponent<ScorePopup>().SetText("+" + score);
    }
}
