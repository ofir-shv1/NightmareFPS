using UnityEngine;
using UnityEngine.SceneManagement; 

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int currentHP = 100;
    public int triggersCount = 0;
    public bool isWin = false;

    void Awake()
    {
        Instance = this;
    }

    public void TakeDamage(int damage)
    {
        if (currentHP > 0 && !isWin)
        {
            currentHP -= damage;
            if (currentHP <= 0) currentHP = 0;
        }
    }

    public void ActivateTrigger()
    {
        if (!isWin)
        {
            triggersCount++;
        }
    }

    void OnGUI()
    {
        GUI.skin.label.fontSize = 24;
        GUI.skin.button.fontSize = 20;

        GUI.Label(new Rect(20, 20, 200, 40), "HP: " + currentHP);
        GUI.Label(new Rect(20, 60, 200, 40), "Triggers: " + triggersCount + " / 4");

        // --- Win ---
        if (triggersCount >= 4 || isWin)
        {
            isWin = true;

            // Release the cursor so it's possible to click!
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            GUI.color = Color.green;
            GUI.Label(new Rect(Screen.width / 2 - 60, Screen.height / 2 - 60, 200, 40), "YOU WIN!");
            GUI.color = Color.white;

            if (GUI.Button(new Rect(Screen.width / 2 - 80, Screen.height / 2, 160, 40), "Play Again?"))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        // --- Lose ---
        if (currentHP <= 0)
        {
            // Release the cursor so it's possible to click!
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            GUI.color = Color.red;
            GUI.Label(new Rect(Screen.width / 2 - 70, Screen.height / 2 - 60, 200, 40), "GAME OVER");
            GUI.color = Color.white; 

            if (GUI.Button(new Rect(Screen.width / 2 - 80, Screen.height / 2, 160, 40), "Try Again"))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}