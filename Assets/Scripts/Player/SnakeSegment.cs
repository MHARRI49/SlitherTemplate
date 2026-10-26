/*
 * GAME 220: Slither Template -- SnakeSegment
 * One body piece. It follows the Transform in front of it (head, then the segment ahead) with MoveTowards.
 * Concepts: LOOPS_AND_LISTS_CONCEPTS.md section 2 -- following another object
 *
 * Session 2: read SetTarget() and FixedUpdate()
 */

using UnityEngine;

public class SnakeSegment : MonoBehaviour
{
    [Header("Following")]
    public float followDistance = 0.3f;
    public float moveSpeed = 20f;

    // The Transform of whatever is in front of us
    private Transform target;

    // Called once when the segment is created
    public void SetTarget(Transform newTarget, float distance)
    {
        target = newTarget;
        followDistance = distance;
    }

    // Called when head wraps around screen boundary
    public void Teleport(Vector3 offset)
    {
        transform.position += offset;
    }

    void FixedUpdate()
    {
        if (target == null) return;

        // Calculate direction to target
        Vector3 direction = (target.position - transform.position).normalized;

        // Calculate ideal position (followDistance behind target)
        Vector3 targetPosition = target.position - direction * followDistance;

        // Move toward that position at constant speed
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.fixedDeltaTime
        );
    }
}
