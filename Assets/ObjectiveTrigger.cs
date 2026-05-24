using UnityEngine;

public class ObjectiveTrigger : MonoBehaviour
{
    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !activated)
        {
            activated = true;
            GameManager.Instance.ActivateTrigger();

            // Disable the whole trigger object - hides the crystal effect child
            // and prevents the player from collecting it again
            gameObject.SetActive(false);
        }
    }
}