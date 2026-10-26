/*
 * GAME 220: Slither Template -- BoundsHelper (STATIC CLASS)
 * Shared play-area edges plus helpers to check, wrap, clamp, and pick random positions.
 * Concepts: LOOPS_AND_LISTS_CONCEPTS.md section 6 -- static classes, wrapping vs clamping, Mathf helpers
 *
 * Session 5: read WrapPosition(), then try the wrap/clamp experiment (section 9)
 */

using UnityEngine;

// static: never instantiated or attached; call BoundsHelper.Method() directly
public static class BoundsHelper
{
    // ---------- Play area edges (static = shared by every script) ----------

    // Bounds sized to match a camera with orthographic size 10 (16:9 aspect)
    // This keeps all game action visible on screen
    public static Vector2 minBounds = new Vector2(-17, -10);
    public static Vector2 maxBounds = new Vector2(17, 10);

    // ---------- Bounds check ----------

    // true if the position is inside on all four sides
    public static bool IsInBounds(Vector3 position)
    {
        return position.x >= minBounds.x && position.x <= maxBounds.x &&
               position.y >= minBounds.y && position.y <= maxBounds.y;
    }

    // ---------- Wrapping (Pac-Man style) ----------

    // TRY IT: steer off each edge, then swap in ClampPosition (see section 9)
    public static Vector3 WrapPosition(Vector3 position)
    {
        // Start with the current position
        Vector3 wrappedPosition = position;

        // Horizontal edges

        // Too far LEFT? Teleport to RIGHT edge
        if (position.x < minBounds.x)
        {
            wrappedPosition.x = maxBounds.x;
        }
        // Too far RIGHT? Teleport to LEFT edge
        else if (position.x > maxBounds.x)
        {
            wrappedPosition.x = minBounds.x;
        }

        // Vertical edges

        // Too far DOWN? Teleport to TOP edge
        if (position.y < minBounds.y)
        {
            wrappedPosition.y = maxBounds.y;
        }
        // Too far UP? Teleport to BOTTOM edge
        else if (position.y > maxBounds.y)
        {
            wrappedPosition.y = minBounds.y;
        }

        return wrappedPosition;
    }

    // ---------- Clamping (stop at the wall) ----------

    // Mathf.Clamp(value, min, max) forces each axis into the range
    public static Vector3 ClampPosition(Vector3 position)
    {
        return new Vector3(
            Mathf.Clamp(position.x, minBounds.x, maxBounds.x),
            Mathf.Clamp(position.y, minBounds.y, maxBounds.y),
            0  // Keep Z at 0 for 2D
        );
    }

    // ---------- Random positions ----------

    // Random point anywhere in the play area (float Random.Range includes both ends)
    public static Vector3 GetRandomPosition()
    {
        return new Vector3(
            Random.Range(minBounds.x, maxBounds.x),
            Random.Range(minBounds.y, maxBounds.y),
            0
        );
    }

    // Random point in a smaller area, trimmed so it stays inside the play area
    public static Vector3 GetRandomPositionInArea(Vector2 areaMin, Vector2 areaMax)
    {
        areaMin.x = Mathf.Max(areaMin.x, minBounds.x);
        areaMin.y = Mathf.Max(areaMin.y, minBounds.y);
        areaMax.x = Mathf.Min(areaMax.x, maxBounds.x);
        areaMax.y = Mathf.Min(areaMax.y, maxBounds.y);

        return new Vector3(
            Random.Range(areaMin.x, areaMax.x),
            Random.Range(areaMin.y, areaMax.y),
            0
        );
    }

    // ---------- Distance and size ----------

    // Distance to the nearest edge (useful for AI edge avoidance)
    public static float GetDistanceToBounds(Vector3 position)
    {
        // Calculate distance to each edge
        float distanceToLeft = position.x - minBounds.x;
        float distanceToRight = maxBounds.x - position.x;
        float distanceToBottom = position.y - minBounds.y;
        float distanceToTop = maxBounds.y - position.y;

        // Smallest of the four
        return Mathf.Min(
            distanceToLeft,
            distanceToRight,
            distanceToBottom,
            distanceToTop
        );
    }

    public static Vector2 GetBoundsSize()
    {
        return new Vector2(
            maxBounds.x - minBounds.x,  // Width: 17 - (-17) = 34
            maxBounds.y - minBounds.y   // Height: 10 - (-10) = 20
        );
    }

    public static Vector3 GetBoundsCenter()
    {
        return new Vector3(
            (minBounds.x + maxBounds.x) / 2,  // (-17 + 17) / 2 = 0
            (minBounds.y + maxBounds.y) / 2,  // (-10 + 10) / 2 = 0
            0
        );
    }
}
