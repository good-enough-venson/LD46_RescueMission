using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RectExtensions
{
    public static Vector2 Point(this Rect rect) {
        return new Vector2 (
            Random.Range(rect.xMin, rect.xMax),
            Random.Range(rect.yMin, rect.yMax)
        );
    }
}
