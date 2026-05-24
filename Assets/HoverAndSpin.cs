using UnityEngine;

public class HoverAndSpin : MonoBehaviour
{
    public float spinSpeed = 100f; // Rotation speed
    public float hoverHeight = 0.25f; // How high the object will move up and down
    public float hoverSpeed = 2f; // Hover speed

    private Vector3 startPos;

    void Start()
    {
        // Save the starting position so the object doesn't fly off into space
        startPos = transform.position;
    }

    void Update()
    {
        // 1. Rotation (around the Y axis)
        transform.Rotate(Vector3.up * spinSpeed * Time.deltaTime);

        // 2. Hover (up and down in a wave motion)
        float newY = startPos.y + Mathf.Sin(Time.time * hoverSpeed) * hoverHeight;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}