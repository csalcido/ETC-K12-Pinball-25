using UnityEngine;

public class Bounce : MonoBehaviour
{
    public float forceMagnitude = 10f;

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 normal = collision.contacts[0].normal;

            Vector3 force = -normal * forceMagnitude;

            rb.AddForce(force, ForceMode.Impulse);
        }
    }
}
