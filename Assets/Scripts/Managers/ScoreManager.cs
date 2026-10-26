/*
 * GAME 220: Slither Template -- ScoreManager
 * The one place the score lives: tracks it, updates the UI, saves the high score, shows end messages.
 * Concepts: LOOPS_AND_LISTS_CONCEPTS.md section 5 -- UI hookup, encapsulation, PlayerPrefs
 *
 * Session 4: read AddScore() and connect the UI text fields in the Inspector
 */

using UnityEngine;
using TMPro; // Required for TextMeshPro UI components

public class ScoreManager : MonoBehaviour
{
    // ---------- UI references (drag from the Hierarchy) ----------

    [Header("UI References")]
    public TMP_Text scoreText;      // Displays current score during gameplay
    public TMP_Text gameOverText;   // Displays game over message

    // ---------- Score data ----------

    [Header("Score Settings")]
    // Private so other scripts must go through AddScore()
    private int currentScore = 0;                // This game only
    private int highScore = 0;                   // Best ever, saved with PlayerPrefs

    // ---------- Unity lifecycle ----------

    void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0); // 0 if nothing is saved yet

        // Set initial display
        UpdateScoreDisplay();

        // Hide game over text at start
        if (gameOverText != null)
        {
            // SetActive(false) makes the GameObject invisible and inactive
            gameOverText.gameObject.SetActive(false);
        }
    }

    // ---------- Score ----------

    // The only way to change the score from outside, so the UI and high score always update
    public void AddScore(int points)
    {
        // Add points to current score
        currentScore += points;

        // Update the UI to show new score
        UpdateScoreDisplay();

        // Check if this is a new high score
        if (currentScore > highScore)
        {
            highScore = currentScore;

            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();                  // Write to disk now in case the game crashes
        }
    }

    // Private helper: keeps the UI in sync with the score
    void UpdateScoreDisplay()
    {
        // Only update if we have a reference to the text
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore.ToString(); // Same as $"Score: {currentScore}"
        }
    }

    // ---------- End-of-game messages ----------

    // Called by GameManager when the player dies
    public void ShowGameOver()
    {
        if (gameOverText != null)
        {
            // Make the text visible
            gameOverText.gameObject.SetActive(true);

            // $"" fills in {variables}; \n starts a new line
            gameOverText.text = $"GAME OVER!\nScore: {currentScore}\nHigh Score: {highScore}\n\nPress R to Restart";
        }
    }

    // Called by GameManager when all AI snakes are eliminated
    public void ShowVictory()
    {
        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(true);
            gameOverText.text = $"VICTORY!\nAll enemies eliminated!\n\nScore: {currentScore}\nHigh Score: {highScore}\n\nPress R to Play Again";
        }
    }

    // ---------- Getters (read the score, can't change it) ----------

    public int GetCurrentScore()
    {
        return currentScore;
    }

    public int GetHighScore()
    {
        return highScore;
    }

    // For restarting without reloading the scene, or for testing
    public void ResetScore()
    {
        currentScore = 0;
        UpdateScoreDisplay();
    }
}
