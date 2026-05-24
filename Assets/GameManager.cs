using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Needed for the damage flash Image
using TMPro; // Needed for TextMeshProUGUI

public class GameManager : MonoBehaviour
{
    // Singleton instance - lets any script reach the GameManager via GameManager.Instance
    public static GameManager Instance;

    [Header("Game State")]
    public int currentHP = 100;
    public int triggersCount = 0;
    public bool isWin = false;

    [Header("HUD")]
    // Drag the HUD text elements (HP and Triggers) into these slots in the Inspector
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI triggersText;

    [Header("End Screens")]
    // Drag the WinPanel and LosePanel GameObjects from the Canvas into these slots
    public GameObject winPanel;
    public GameObject losePanel;

    [Header("Player")]
    // Drag the Player object's PlayerMovement script here so we can disable it on win/lose
    public PlayerMovement playerMovement;

    [Header("Damage Flash")]
    // Drag a full-screen red Image on the Canvas here to flash when the player takes damage
    public Image damageFlashImage;
    public float damageFlashDuration = 0.4f; // How long the flash takes to fade out
    public float damageFlashMaxAlpha = 0.5f; // How opaque the flash is at peak (0..1)

    // Internal countdown timer for the damage flash fade
    private float damageFlashTimer = 0f;

    // Internal flag so we only show the lose panel once
    private bool isLose = false;

    void Awake()
    {
        // Save this instance into the static field on game start
        Instance = this;
    }

    void Start()
    {
        // Make sure the end-game panels are hidden when the game begins
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);
    }

    void Update()
    {
        // Refresh the HUD text every frame so it always shows the current values
        if (hpText != null) hpText.text = "HP: " + currentHP;
        if (triggersText != null) triggersText.text = "Triggers: " + triggersCount + " / 4";

        // Fade out the damage flash over time
        UpdateDamageFlash();

        // --- Win check ---
        // If the player collected all 4 triggers and didn't already win/lose - show the win panel
        if (triggersCount >= 4 && !isWin && !isLose)
        {
            isWin = true;
            if (winPanel != null) winPanel.SetActive(true);
            FreezeGame();
        }

        // --- Lose check ---
        // If the player ran out of HP and didn't already win/lose - show the lose panel
        if (currentHP <= 0 && !isLose && !isWin)
        {
            isLose = true;
            if (losePanel != null) losePanel.SetActive(true);
            FreezeGame();
        }
    }

    // Called from ObstacleCollision / MutantAI when the player takes damage
    public void TakeDamage(int damage)
    {
        if (currentHP > 0 && !isWin)
        {
            currentHP -= damage;
            if (currentHP <= 0) currentHP = 0;

            // Trigger the red screen flash
            damageFlashTimer = damageFlashDuration;
        }
    }

    // Called from ObjectiveTrigger when the player collects a trigger
    public void ActivateTrigger()
    {
        if (!isWin)
        {
            triggersCount++;
        }
    }

    // Hooked up to the "Play Again" / "Try Again" buttons in the Inspector
    public void RestartGame()
    {
        // Reset time scale in case the game was paused
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Releases the mouse cursor so the player can click the end-screen buttons
    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Stops time and disables player movement when the game ends
    private void FreezeGame()
    {
        UnlockCursor();
        Time.timeScale = 0f;
        if (playerMovement != null) playerMovement.enabled = false;
    }

    // Lerps the damage flash alpha from max down to 0 over damageFlashDuration seconds
    private void UpdateDamageFlash()
    {
        if (damageFlashImage == null) return;

        if (damageFlashTimer > 0f)
        {
            damageFlashTimer -= Time.deltaTime;
            float t = Mathf.Clamp01(damageFlashTimer / damageFlashDuration);
            Color c = damageFlashImage.color;
            c.a = t * damageFlashMaxAlpha;
            damageFlashImage.color = c;
        }
    }
}
