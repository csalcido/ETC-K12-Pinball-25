using UnityEngine;
using System.Collections.Generic;

public class OneWayBarrier : MonoBehaviour
{
    [SerializeField] private float pushForce = 10f;
    
    private HashSet<int> ballsThatPassedThrough = new HashSet<int>();
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            int ballID = other.GetInstanceID();
            
            // If ball hasn't passed through before, let it pass and remember it
            if (!ballsThatPassedThrough.Contains(ballID))
            {
                ballsThatPassedThrough.Add(ballID);
                return; // Let it pass through
            }
            
            // Ball already passed through - push it away
            Rigidbody ballRb = other.GetComponent<Rigidbody>();
            if (ballRb != null)
            {
                Vector3 pushDirection = (other.transform.position - transform.position).normalized;
                ballRb.AddForce(pushDirection * pushForce, ForceMode.Impulse);
            }
        }
    }
}
