using UnityEngine;

public class ObstacleCollision : MonoBehaviour
{
    // Runs if the collider is set as solid
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.TakeDamage(25);
            }
        }
    }

    // Runs if the collider is set as a trigger (ghost)
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.TakeDamage(25);
            }
        }
    }
}