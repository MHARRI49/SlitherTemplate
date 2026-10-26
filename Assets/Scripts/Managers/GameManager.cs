/*
 * GAME 220: Slither Template -- GameManager
 * Spawns the AI snakes, tracks them in a List, and handles game over, victory, and restart.
 * Concepts: LOOPS_AND_LISTS_CONCEPTS.md section 7 -- for loop spawning, foreach + Clear() cleanup
 *
 * Session 5: read GameOver() and RestartGame()   Session 6: read SpawnAISnakes()
 */

using System.Collections.Generic;  // Required for List<T>
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // ---------- Inspector settings ----------

    [Header("Input")]
    [SerializeField] private InputAction restartAction =
        new InputAction("Restart", InputActionType.Button, "<Keyboard>/r");

    [Header("AI Settings")]
    public GameObject aiSnakePrefab;

    // TRY IT: set to 0, 10, then 20 (see section 9)
    public int numberOfAISnakes = 3;

    [Header("Spawn Settings")]
    public Vector2 spawnAreaMin = new Vector2(-20, -15);
    public Vector2 spawnAreaMax = new Vector2(20, 15);

    // ---------- State ----------

    private List<AISnakeController> aiSnakes = new List<AISnakeController>(); // Every live AI snake

    private bool gameOver = false;               // Once true, only restart input is accepted

    // ---------- Unity lifecycle ----------

    void Start()
    {
        Debug.Log("Game started!");

        // Spawn all AI snakes using a FOR loop
        SpawnAISnakes();
    }

    // Input actions must be enabled before they report anything
    void OnEnable()
    {
        restartAction.Enable();
    }

    void OnDisable()
    {
        restartAction.Disable();
    }

    void Update()
    {
        // Only check for restart if game is over
        if (gameOver && restartAction.WasPressedThisFrame())
        {
            RestartGame();
        }
    }

    // ---------- FOR loop: spawning AI ----------

    // Runs SpawnSingleAISnake() numberOfAISnakes times
    void SpawnAISnakes()
    {
        for (int i = 0; i < numberOfAISnakes; i++)
        {
            SpawnSingleAISnake();
        }

        Debug.Log($"Spawned {aiSnakes.Count} AI snakes");
    }

    // Creates one AI snake and adds it to the list
    void SpawnSingleAISnake()
    {
        // Get a random position for this AI
        Vector3 spawnPosition = GetRandomSpawnPosition();

        // Create the AI snake
        GameObject aiObj = Instantiate(aiSnakePrefab, spawnPosition, Quaternion.identity);

        // Get the AISnakeController component
        AISnakeController ai = aiObj.GetComponent<AISnakeController>();

        if (ai != null)
        {
            aiSnakes.Add(ai);                    // List.Add(): same pattern as FoodSpawner
        }
    }

    Vector3 GetRandomSpawnPosition()
    {
        // Use BoundsHelper to guarantee spawn is inside game bounds
        return BoundsHelper.GetRandomPosition();
    }

    // ---------- FOREACH + Clear(): game over cleanup ----------

    // Called when the player dies. Destroy every AI, then empty the list (you need both).
    public void GameOver()
    {
        // Prevent multiple calls (e.g., if player hits two things at once)
        if (gameOver) return;

        gameOver = true;
        Debug.Log("GAME OVER! Press R to restart");

        // Step 1: Destroy removes each snake from the game world
        foreach (AISnakeController ai in aiSnakes)
        {
            if (ai != null)                      // Some may already be destroyed
            {
                Destroy(ai.gameObject);
            }
        }

        // Step 2: the list still holds the destroyed entries, so empty it
        aiSnakes.Clear();

        // Show game over UI
        ScoreManager scoreManager = FindAnyObjectByType<ScoreManager>();
        if (scoreManager != null)
        {
            scoreManager.ShowGameOver();
        }
    }

    // ---------- Restart ----------

    // Reloading the scene resets the player, score, and every list
    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // ---------- AI tracking ----------

    public int GetAISnakeCount()
    {
        return aiSnakes.Count;
    }

    // Called by an AI snake when it dies. Keeps the list in sync and checks for a win.
    public void OnAISnakeDied(AISnakeController ai)
    {
        // Don't process if game is already over
        if (gameOver) return;

        aiSnakes.Remove(ai);                     // List.Remove(): removes just this one

        Debug.Log($"AI snake eliminated! Remaining: {aiSnakes.Count}");

        // WIN CONDITION: Check if all AI snakes are dead
        if (aiSnakes.Count == 0)
        {
            Victory();
        }
    }

    // ---------- Victory ----------

    void Victory()
    {
        gameOver = true;
        Debug.Log("VICTORY! All AI snakes eliminated!");

        // Show victory UI
        ScoreManager scoreManager = FindAnyObjectByType<ScoreManager>();
        if (scoreManager != null)
        {
            scoreManager.ShowVictory();
        }
    }

    // Older name, kept so existing calls still work
    public void RemoveAISnake(AISnakeController ai)
    {
        OnAISnakeDied(ai);
    }

    // ---------- Debugging ----------

    void OnDrawGizmos()
    {
        // Draw the spawn area as a cyan rectangle
        Gizmos.color = Color.cyan;

        Vector3 center = new Vector3(
            (spawnAreaMin.x + spawnAreaMax.x) / 2,
            (spawnAreaMin.y + spawnAreaMax.y) / 2,
            0
        );

        Vector3 size = new Vector3(
            spawnAreaMax.x - spawnAreaMin.x,
            spawnAreaMax.y - spawnAreaMin.y,
            0
        );

        Gizmos.DrawWireCube(center, size);
    }
}
