using UnityEngine;
using UnityEngine.InputSystem; // Adds support for the new Input System

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 0.5f; // Sensitivity tuned for the new Input System

    private CharacterController controller;
    private float xRotation = 0f;
    private int framesToSkipMouse = 3; // Skip a few frames to let the Input System settle - prevents the initial mouse delta spike

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
    }

    void Update()
    {
        // The Input System can return huge mouse delta values during the first frames
        // after the cursor is locked. Reading those would snap-rotate the player.
        // Solution: skip mouse input entirely for the first few frames.
        bool readMouse = true;
        if (framesToSkipMouse > 0)
        {
            framesToSkipMouse--;
            readMouse = false;
        }

        // 1. Camera rotation with the mouse (using the new Input System)
        if (readMouse && Mouse.current != null)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue() * mouseSensitivity;

            // Extra safety: ignore frames where the delta is unreasonably large (still a spike)
            const float MAX_DELTA_PER_FRAME = 30f;
            if (Mathf.Abs(mouseDelta.x) > MAX_DELTA_PER_FRAME || Mathf.Abs(mouseDelta.y) > MAX_DELTA_PER_FRAME)
            {
                mouseDelta = Vector2.zero;
            }

            float mouseX = mouseDelta.x;
            float mouseY = mouseDelta.y;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }

        // 2. WASD keyboard movement (using the new Input System)
        float x = 0f;
        float z = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed) z = 1f;
            if (Keyboard.current.sKey.isPressed) z = -1f;
            if (Keyboard.current.dKey.isPressed) x = 1f;
            if (Keyboard.current.aKey.isPressed) x = -1f;
        }

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move.normalized * moveSpeed * Time.deltaTime);
    }
}