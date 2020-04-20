using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnZone : MonoBehaviour
{
    public Vector2 size;
    public Vector2 offset;

    [Label(preSpace: 10, "Base Spawn Properties")]
    public float initialSpeed;

    [SerializeField, Range(270, -90)]
    private float _angle;
    public float initialAngle {
        get => _angle;
        set {
            _angle = Mathf.Repeat(value, 360);
            direction = _angle.GetVector();
        }
    }

    [SerializeField, ReadOnly]
    private Vector2 direction;

    public Vector2 position {
        get => new Vector2(
            transform.position.x,
            transform.position.y
        );
    }

    public Rect rect {
        get => new Rect(position + offset - size * 0.5f, size);
    }

    private void OnValidate() {
        direction = _angle.GetVector();
    }

    private void OnDrawGizmos()
    {
        var rect = this.rect;

        Gizmos.color = Color.white;
        DrawRect(rect);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(rect.center, rect.center + direction * 5);
    }

    private void DrawRect(Rect rect)
    {
        Vector3 tl = new Vector3(rect.xMin, rect.yMax),
            tr = new Vector3(rect.xMax, rect.yMax),
            bl = new Vector3(rect.xMin, rect.yMin),
            br = new Vector3(rect.xMax, rect.yMin);

        Gizmos.DrawLine(bl, br);
        Gizmos.DrawLine(tl, tr);
        Gizmos.DrawLine(bl, tl);
        Gizmos.DrawLine(br, tr);
    }
}
