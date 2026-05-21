using UnityEngine;

public class HealCube : MonoBehaviour
{
    public int healAmount = 50; // כמה חיים הקובייה הזו תוסיף

    private void OnTriggerEnter(Collider other)
    {
        // בודק אם האובייקט שנגע בקובייה הוא השחקן
        if (other.CompareTag("Player"))
        {
            // מוצא את ה-GameManager שנמצא במשחק
            GameManager gameManager = FindObjectOfType<GameManager>();

            if (gameManager != null)
            {
                // מוסיף חיים לשחקן (שים לב: אם המשתנה ב-GameManager שלך כתוב קצת אחרת, תתקן בהתאם)
                gameManager.currentHP += healAmount;

                // שומר שהחיים לא יעברו את ה-100 (כדי שלא יהיה באג של חיים אינסופיים)
                if (gameManager.currentHP > 100)
                {
                    gameManager.currentHP = 100;
                }

                Debug.Log("Player healed! Current HP: " + gameManager.currentHP);
            }

            // מוחק את קוביית הריפוי מהעולם אחרי שהשחקן אסף אותה
            Destroy(gameObject);
        }
    }
}