using UnityEngine;

public class ObstacleCollision : MonoBehaviour
{
    // עובד אם הכדור מוגדר כמוצק
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

    // עובד אם הכדור מוגדר כטריגר (רוח רפאים)
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