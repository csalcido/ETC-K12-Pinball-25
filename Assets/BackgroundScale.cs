using UnityEngine;

public class BackgroundScale : MonoBehaviour
{
    public GameStateManager gameStateManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStateManager.currentState == GameStateManager.ScreenState.GameBoard)
        {
        // Keep Y and Z the same, only change X
        Vector3 newScale = transform.localScale;
        newScale.x = 5.861918f;
        transform.localScale = newScale;
        }
        
    }
}
