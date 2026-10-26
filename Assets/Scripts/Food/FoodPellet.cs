/*
 * GAME 220: Slither Template -- FoodPellet
 * One food item. When a snake touches it, the snake grows, the player scores, and the spawner replaces it.
 * Concepts: LOOPS_AND_LISTS_CONCEPTS.md section 4 -- triggers, CompareTag, component communication
 *
 * Session 4: read OnTriggerEnter2D, then try the growthAmount experiment (section 9)
 */

using System;
using UnityEngine;

public class FoodPellet : MonoBehaviour
{
    // ---------- Settings ----------

    [Header("Pellet Settings")]
    // TRY IT: set growthAmount to 5, then 0 (see section 9)
    public int growthAmount = 1;                 // Segments a snake gains from this food
    [SerializeField] private FoodType foodType = FoodType.Normal;

    // ---------- References ----------

    private FoodSpawner spawner;                 // The spawner that made us, so we can report back

    // Called by FoodSpawner right after Instantiate() so we can call it back later
    public void SetSpawner(FoodSpawner foodSpawner)
    {
        spawner = foodSpawner;
    }

    // ---------- Collision ----------

    // Needs a Collider2D on both objects, Is Trigger on one, and a Rigidbody2D on one
    void OnTriggerEnter2D(Collider2D other)
    {
        // Player snake: grow, score, and tell the spawner
        if (other.CompareTag("Player"))          // CompareTag is faster than tag == "Player"
        {
            PlayerSnakeController player = other.GetComponent<PlayerSnakeController>(); // null if the script isn't there

            if (player != null)
            {

                player.EatFood(foodType);  //this now handles eating but doesn't tell the controller what to do

                // Spawner removes us from its list, destroys us, and spawns a replacement
                if (spawner != null)
                {
                    spawner.OnFoodCollected(this);
                }
                else
                {
                    Destroy(gameObject);         // No spawner (placed by hand): clean up ourselves
                }

                // FindAnyObjectByType is slower than a cached reference, but simpler to read
                ScoreManager scoreManager = FindAnyObjectByType<ScoreManager>();
                if (scoreManager != null)
                {
                    scoreManager.AddScore(10);
                }
            }
        }

        // AI snake: same pattern, but no score
        if (other.CompareTag("AISnake"))
        {
            AISnakeController ai = other.GetComponent<AISnakeController>();

            if (ai != null)
            {
                ai.Grow(growthAmount);

                if (spawner != null)
                {
                    spawner.OnFoodCollected(this);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    // ---------- Visual effect ----------

    // Bobbing is visual only, so it runs in Update for the smoothest motion
    void Update()
    {
        float bobAmount = 0.1f;    // How far up/down to bob
        float bobSpeed = 2f;       // How fast to bob

        // Sine wave swings between -1 and 1 over time
        float newY = transform.position.y + Mathf.Sin(Time.time * bobSpeed) * bobAmount * Time.deltaTime;

        // Update only the Y position, keep X and Z the same
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    // ---------- Food type ----------

    private void Start()
    {
        // Tint the pellet by food type (could also change size or effects)
        switch (foodType)
        {
            case FoodType.Normal:
                GetComponent<SpriteRenderer>().color = Color.blue;
                break;
            case FoodType.Super:
                // Change color to indicate super food
                GetComponent<SpriteRenderer>().color = Color.orange;
                break;
            case FoodType.Poison:
                // Change color to indicate poison food
                GetComponent<SpriteRenderer>().color = Color.green;
                break;
        }
    }

    public FoodType GetFoodType()
    {
        return foodType;
    }

    public void SetFoodType(FoodType type)
    {
        foodType = type;

        // Update visual appearance based on food type
        switch (foodType)
        {
            case FoodType.Normal:
                GetComponent<SpriteRenderer>().color = Color.blue;
                break;
            case FoodType.Super:
                GetComponent<SpriteRenderer>().color = Color.orange;
                break;
            case FoodType.Poison:
                GetComponent<SpriteRenderer>().color = Color.green;
                break;
        }
    }
}
