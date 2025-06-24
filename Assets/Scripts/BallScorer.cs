using UnityEngine;

public class BallScorer : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        string tag = collision.collider.tag;
        int score = 0;

        switch (tag)
        {
            case "Bumper":
                score = 288;
                break;
            case "Slingshot":
                score = 288;
                break;
            case "Drop":
                score = 288;
                break;
            case "Tunnel":
                score = 500;
                break;
            case "Smash":
                score = 1666;
                break;
            default:
                return;
        }

        Vector3 hitPoint = collision.GetContact(0).point;
        ScoreManager.Instance.AddScore(score, hitPoint);
    }

    private void OnTriggerEnter(Collider other)
    {
        string tag = other.gameObject.tag;
        int score = 0;

        switch (tag)
        {
            case "Tunnel":
                score = 500;
                break;
            case "Gobbler":
                score = 5000;
                break;
            default:
                return;
        }

        Vector3 hitPoint = other.transform.position;
        ScoreManager.Instance.AddScore(score, hitPoint);
    }
}
