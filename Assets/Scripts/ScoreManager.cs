using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public TextMeshProUGUI scoreText;
    private int totalScore = 0;

    void Awake()
    {
        Instance = this;
    }

    public void AddScore(int amount, Vector3 worldPosition)
    {
        totalScore += amount;
        scoreText.text = "Score: " + totalScore;

        ScorePopupSpawner.Instance.SpawnPopup(amount, worldPosition);
    }
}
