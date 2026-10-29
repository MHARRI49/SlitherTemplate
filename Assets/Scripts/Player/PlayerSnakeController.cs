/*
 * GAME 220: Slither Template -- PlayerSnakeController
 * Steers the head toward the pointer and keeps the body in a List (HEAD -> Seg0 -> Seg1 -> ...).
 * Concepts: LOOPS_AND_LISTS_CONCEPTS.md section 1 -- input actions, Update vs FixedUpdate, List<T>, for/foreach
 *
 * Session 1: Update()/MoveHead()   Sessions 2-3: AddSegment()/Grow()   Session 5: CheckBoundaries()/Die()
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSnakeController : MonoBehaviour
{
    // ---------- Settings ----------

    // An InputAction names what the player does; expand it in the Inspector to see its bindings
    [Header("Input")]
    [SerializeField] private InputAction steerAction =
        new InputAction("Steer", InputActionType.Value, "<Pointer>/position", expectedControlType: "Vector2");

    [Header("Movement")]
    public float moveSpeed = 1100000000000000f;
    public float rotationSpeed = 1000000000f;


    [Header("Body")]
    public int startingSegments = 3;
    public GameObject segmentPrefab;
    public float segmentSpacing = 0.9f;

    // List<T> can grow and shrink, so it tracks every body segment
    private List<SnakeSegment> bodySegments = new List<SnakeSegment>();

    private Vector3 moveDirection;
    private bool isAlive = true;

    // ---------- Unity lifecycle ----------

    void Start()
    {
        moveDirection = transform.up;
        CreateStartingBody();
    }

    // Actions do nothing until enabled; OnEnable runs every time this object is switched on
    void OnEnable()
    {
        steerAction.Enable();
    }

    void OnDisable()
    {
        steerAction.Disable();
    }

    void Update()
    {
        if (!isAlive) return;

        // Get pointer position and calculate direction
        Vector2 pointerScreenPos = steerAction.ReadValue<Vector2>();
        Vector3 pointerWorldPos = Camera.main.ScreenToWorldPoint(pointerScreenPos);
        pointerWorldPos.z = 0;
        moveDirection = (pointerWorldPos - transform.position).normalized;
    }

    void FixedUpdate()
    {
        if (!isAlive) return;
        MoveHead();
        CheckBoundaries();
    }

    // ---------- Movement (Session 1) ----------

    void MoveHead()
    {
        transform.position += moveDirection * moveSpeed * Time.fixedDeltaTime;

        if (moveDirection != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg - 90f;
            float currentAngle = transform.eulerAngles.z;
            float newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, rotationSpeed * Time.fixedDeltaTime);
            transform.rotation = Quaternion.Euler(0, 0, newAngle);
        }
    }

    // ---------- Body list (Sessions 2-3) ----------

    // FOR LOOP: runs AddSegment() exactly startingSegments times
    void CreateStartingBody()
    {
        for (int i = 0; i < startingSegments; i++)
        {
            AddSegment();
        }
        Debug.Log($"Created snake with {bodySegments.Count} segments");
    }

    // Adds one segment to the end of the snake
    void AddSegment()
    {
        Vector3 spawnPosition;
        Transform targetToFollow;

        if (bodySegments.Count == 0)
        {
            // First segment follows the head
            spawnPosition = transform.position - transform.up * segmentSpacing;
            targetToFollow = transform;
        }
        else
        {
            // Additional segments follow the previous segment
            SnakeSegment lastSegment = bodySegments[bodySegments.Count - 1];
            spawnPosition = lastSegment.transform.position;
            targetToFollow = lastSegment.transform;
        }

        GameObject newSegmentObj = Instantiate(segmentPrefab, spawnPosition, Quaternion.identity);
        SnakeSegment newSegment = newSegmentObj.GetComponent<SnakeSegment>();

        if (newSegment != null)
        {
            // Tell this segment what to follow
            newSegment.SetTarget(targetToFollow, segmentSpacing);

            // Copy head color to segment
            SpriteRenderer headSprite = GetComponent<SpriteRenderer>();
            SpriteRenderer segmentSprite = newSegmentObj.GetComponent<SpriteRenderer>();
            if (headSprite != null && segmentSprite != null)
            {
                segmentSprite.color = headSprite.color;
            }

            // Tag and collider so AI can hit our body
            newSegmentObj.tag = "Player";
            CircleCollider2D collider = newSegmentObj.AddComponent<CircleCollider2D>();
            collider.radius = 0.4f;
            collider.isTrigger = true;

            // List.Add() - Adds item to end of list
            bodySegments.Add(newSegment);
        }
    }

    public void EatFood(FoodType food)
    {
        switch (food)
        {
            case FoodType.Normal:
                Grow(1);
                break;
            case FoodType.Super:
                Grow(3);
                break;
            case FoodType.Poison:
                Die();
                break;
        }
    }

    // Called by FoodPellet when snake eats food
    public void Grow(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            AddSegment();
        }
    }

    // ---------- Collision and edges (Session 5) ----------

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("AISnake"))
        {
            Die();
        }
    }

    void CheckBoundaries()
    {
        if (!BoundsHelper.IsInBounds(transform.position))
        {
            Vector3 oldPosition = transform.position;
            transform.position = BoundsHelper.WrapPosition(transform.position);
            Vector3 teleportOffset = transform.position - oldPosition;

            // FOREACH: Teleport all segments when head wraps
            foreach (SnakeSegment segment in bodySegments)
            {
                if (segment != null)
                {
                    segment.Teleport(teleportOffset);
                }
            }
        }
    }

    void Die()
    {
        isAlive = false;
        Debug.Log("Player died! Segments: " + bodySegments.Count);

        GameManager gameManager = FindAnyObjectByType<GameManager>();
        if (gameManager != null)
        {
            gameManager.GameOver();
        }
    }

    // FOREACH LOOP: Clean up all segments when destroyed
    void OnDestroy()
    {
        foreach (SnakeSegment segment in bodySegments)
        {
            if (segment != null)
            {
                Destroy(segment.gameObject);
            }
        }
    }
}
