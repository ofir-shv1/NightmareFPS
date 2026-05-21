using UnityEngine;

public class HoverAndSpin : MonoBehaviour
{
    public float spinSpeed = 100f; // מהירות הסיבוב
    public float hoverHeight = 0.25f; // כמה גבוה האובייקט יעלה וירד
    public float hoverSpeed = 2f; // מהירות הריחוף

    private Vector3 startPos;

    void Start()
    {
        // שומרים את נקודת ההתחלה כדי שהאובייקט לא יעוף לחלל
        startPos = transform.position; 
    }

    void Update()
    {
        // 1. פקודת הסיבוב (סביב ציר ה-Y)
        transform.Rotate(Vector3.up * spinSpeed * Time.deltaTime);

        // 2. פקודת הריחוף (מעלה ומטה בתנועה גלית)
        float newY = startPos.y + Mathf.Sin(Time.time * hoverSpeed) * hoverHeight;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}