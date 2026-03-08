using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class for general, helpful methods
/// </summary>
public static class Helpers
{
    // Transform Extensions

    public static void Move(this Transform transform, in Vector3 pos)
    {
        transform.position += pos;
    }

    // Rect Transform Extensions

    public static void SetLocalY(this RectTransform rectTransform, in float y)
    {
        Vector3 pos = rectTransform.localPosition;
        pos.y = y;
        rectTransform.localPosition = pos;
    }

    // List Extensions

    public static T GetRandomElement<T>(this List<T> list)
    {
        if (list.Count == 0)
        {
            Debug.Log("List was empty when trying to get a random element!");
            return default(T);
        }
        return list[UnityEngine.Random.Range(0, list.Count)];
    }

    /// Array Extensions

    public static T GetRandomElement<T>(this T[] list)
    {
        if (list.Length == 0)
        {
            Debug.Log("Array was empty when trying to get a random element!");
            return default(T);
        }
        return list[UnityEngine.Random.Range(0, list.Length)];
    }

    // Float extensions

    public static bool IsInBetween(this in float val, in float min, in float max)
    {
        return val <= max && val >= min;
    }

    // Vector 3 Extensions

    public static Vector3 NoY(this in Vector3 value)
    {
        return new Vector3(value.x, 0f, value.z);
    }

    // BoxCollider Extensions

    public static Vector3 GetRandomPointInsideCollider(this BoxCollider boxCollider)
    {
        Vector3 extents = boxCollider.size * 0.5f;

        Vector3 point = new Vector3(
            Random.Range(-extents.x, extents.x),
            Random.Range(-extents.y, extents.y),
            Random.Range(-extents.z, extents.z)
        ) + boxCollider.center;

        return boxCollider.transform.TransformPoint(point);
    }

    // BoxCollider2D Extensions

    public static Vector2 GetRandomPointInsideCollider(this BoxCollider2D boxCollider)
    {
        Vector2 extents = boxCollider.size * 0.5f;

        Vector2 localPoint = new Vector2(
            Random.Range(-extents.x, extents.x),
            Random.Range(-extents.y, extents.y)
        ) + boxCollider.offset;

        return boxCollider.transform.TransformPoint(localPoint);
    }

    // CanvasGroup extensions

    public static void SetCanvasGroupVisible(this CanvasGroup canvasGroup, bool isVisible)
    {
        if (isVisible)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        else
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    // CircleCollider2D Extensions

    public static Vector2 GetRandomPoint(this CircleCollider2D circleCollider)
    {

        float angle = Random.Range(0, Mathf.PI * 2);
        float distance = circleCollider.radius * Mathf.Sqrt(Random.Range(0f, 1f)); // sqrt for even distribution

        Vector2 localPoint = new Vector2(distance * Mathf.Cos(angle), distance * Mathf.Sin(angle)) + circleCollider.offset;

        return circleCollider.transform.TransformPoint(localPoint);
    }

    // Vector 3 Func

    /// <summary>
    /// Calculates the squared distance between two vectors (for faster calculations)
    /// </summary>
    public static float SqDistance(in Vector3 a, in Vector3 b)
    {
        float num1 = a.x - b.x;
        float num2 = a.y - b.y;
        float num3 = a.z - b.z;
        return num1 * num1 + num2 * num2 + num3 * num3;
    }
}
