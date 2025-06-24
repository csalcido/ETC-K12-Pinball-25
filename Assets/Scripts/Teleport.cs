using UnityEngine;

public class Teleport : MonoBehaviour
{
    // trandform the pinball to another position
    public Transform targetPosition;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            other.transform.position = targetPosition.position;
        }
    }
}
