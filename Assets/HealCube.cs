using UnityEngine;

public class HealCube : MonoBehaviour
{
    public int healAmount = 50; // How many HP this cube restores

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that touched the cube is the player
        if (other.CompareTag("Player"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.currentHP += healAmount;

                // Cap HP at 100 (so we don't get an infinite-HP bug)
                if (GameManager.Instance.currentHP > 100)
                {
                    GameManager.Instance.currentHP = 100;
                }

                Debug.Log("Player healed! Current HP: " + GameManager.Instance.currentHP);
            }

            // Destroy the heal cube after the player picked it up
            Destroy(gameObject);
        }
    }
}