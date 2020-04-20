using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector2Extensions
{
    public static Vector2 Rotate(this Vector2 vector, float degrees) {
        return new Vector2 (
            vector.x * Mathf.Cos(Mathf.Deg2Rad * degrees) - vector.y * Mathf.Sin(Mathf.Deg2Rad * degrees),
            vector.x * Mathf.Sin(Mathf.Deg2Rad * degrees) + vector.y * Mathf.Cos(Mathf.Deg2Rad * degrees)
        );
    }

    public static Vector2 V2(this Vector3 vector) {
        return new Vector2(vector.x, vector.y);
    }
}
