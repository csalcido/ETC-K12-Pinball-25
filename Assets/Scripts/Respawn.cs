using UnityEngine;
using System.Collections;

public class Respawn : MonoBehaviour
{
    public Transform leftRespawnPoint;
    public Transform rightRespawnPoint;
    public GameObject Pinball;

    private Vector3 leftInitialPosition;
    private Vector3 rightInitialPosition;

    void Start()
    {
        leftInitialPosition = leftRespawnPoint.position;
        rightInitialPosition = rightRespawnPoint.position;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Destroy(other.gameObject);
        }
    }

    public void SpawnBall(bool isLeftRespawnPoint)
    {
        Vector3 respawnPosition = isLeftRespawnPoint ? leftInitialPosition : rightInitialPosition;
        StartCoroutine(SpawnBallAfterDelay(respawnPosition, 0.5f));
    }

    private IEnumerator SpawnBallAfterDelay(Vector3 position, float delay)
    {
        yield return new WaitForSeconds(delay);
        Instantiate(Pinball, position, Quaternion.identity);
    }

}
