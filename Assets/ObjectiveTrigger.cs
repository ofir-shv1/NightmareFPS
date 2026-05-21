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
            GetComponent<Renderer>().material.color = Color.green; // משתנה לירוק כשנוגעים בו
        }
    }
}