using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FloatExtensions
{
    public static float Abs(this float value) {
        return Mathf.Abs(value);
    }

    public static float Round(this float value, float place) {
        return (float)(System.Math.Round((double)value / place) * place);
    }

    public static float Move(this float value, float towards, float by) {
        if (towards > value) { return Mathf.Clamp(value + by, value, towards); }
        else if (towards < value) { return Mathf.Clamp(value - by, towards, value); }
        else return towards;
    }

    public static Vector2 GetVector(this float degrees) {
        return new Vector2(Mathf.Cos(Mathf.Deg2Rad * degrees),
            Mathf.Sin(Mathf.Deg2Rad * degrees));
    }
}
