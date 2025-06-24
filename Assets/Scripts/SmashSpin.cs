using UnityEngine;

public class SmashSpin : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0.0f, 45.0f * Time.deltaTime, 0.0f, Space.Self);
    }
}
