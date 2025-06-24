using UnityEngine;

public class Accelerator : MonoBehaviour
{
    public float accelerationForce = 10f;
    public ForceMode forceMode = ForceMode.Impulse;
    public Vector3 accelerationDirection = Vector3.forward;

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody ballRb = other.GetComponent<Rigidbody>();
        if (ballRb != null)
        {
            ballRb.linearVelocity = Vector3.zero;
            ballRb.angularVelocity = Vector3.zero;

            Vector3 dirNormalized = accelerationDirection.normalized;

            ballRb.AddForce(dirNormalized * accelerationForce, forceMode);
        }
    }
}
