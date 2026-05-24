using UnityEngine;

public class ObstacleCollision : MonoBehaviour
{
    // Tracks when this obstacle last dealt damage, to prevent both
    // OnCollisionEnter and OnTriggerEnter firing on the same frame and double-hitting.
    private float lastDamageTime = -999f;
    private const float damageCooldown = 0.1f;

    // Runs if the collider is set as solid
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ApplyDamage();
        }
    }

    // Runs if the collider is set as a trigger (ghost)
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ApplyDamage();
        }
    }

    private void ApplyDamage()
    {
        // Ignore repeated hits within the cooldown window
        if (Time.time - lastDamageTime < damageCooldown) return;
        lastDamageTime = Time.time;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.TakeDamage(25);
        }
    }
}
