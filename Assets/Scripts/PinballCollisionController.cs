using UnityEngine;

public class PinballCollisionManager : MonoBehaviour
{
    public bool disableCollisionsWithOtherBalls = false;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ball" && disableCollisionsWithOtherBalls)
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        }
    }
}
