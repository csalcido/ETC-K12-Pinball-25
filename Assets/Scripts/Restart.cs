using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    // Hit both flippers to restart the game.
    private bool isRightFlipper = false;
    private bool isLeftFlipper = false;
    void Start()
    {

    }

    void Update()
    {
        if(isLeftFlipper && isRightFlipper)
        {
            RestartGame();
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    public void RegisterRightFlipper() {
        isRightFlipper = true;
    }

    public void RegisterLeftFlipper() {
        isLeftFlipper = true;
    }

    public void CancelRightFlipper() {
        isRightFlipper = false;
    }

    public void CancelLeftFlipper() {
        isLeftFlipper = false;
    }
}
