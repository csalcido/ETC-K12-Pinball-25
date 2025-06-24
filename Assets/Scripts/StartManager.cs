using UnityEngine;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour
{
    private bool isStarted = false;
    public GameObject timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isStarted)
        {
            timer.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }

    public void RegisterStart() {
        isStarted = true;
    }
}
