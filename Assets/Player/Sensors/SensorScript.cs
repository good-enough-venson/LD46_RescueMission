using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorScript : MonoBehaviour
{
    public GenericGameObjectPool pool;

    [Space]
    public float sensorRange = 10;
    public Vector2 offset = Vector2.zero;

    [Space]
    public float displayStart = 2;
    public float displayEnd = 5;

    float displayDepth { get => displayEnd - displayStart; }

    Vector2 position { get => transform.position; }
    Vector2 center { get => position + offset; }

    List<Collider2D> targets;

    private void Start() {
        targets = new List<Collider2D>();
    }

    private void LateUpdate()
    {
        var markers = pool.GetAllActive<Transform>();
        var markerInd = 0;

        for (int i = targets.Count - 1; i >= 0; i--)
        {
            if (targets[i] == null) {
                targets.RemoveAt(i);
                continue;
            }

            var point = targets[i].ClosestPoint(center);
            var distance = Vector2.Distance(center, point);

            if (distance > sensorRange) {
                targets.RemoveAt(i);
                continue;
            }

            var marker = markerInd < markers.Count ? markers[markerInd] : pool.UnpoolItem<Transform>();
            if (marker == null) {
                Debug.LogWarning("Could not get marker!", this);
            }

            var markerPos = displayStart + distance / sensorRange * displayDepth;
            var angle = Vector2.SignedAngle(Vector2.right, point - center);
            Vector3 vector = GetVector(angle) * 2f;

            marker.position = Vector2.MoveTowards(center, point, markerPos);
            marker.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            markerInd++;
        }

        for (; markerInd < markers.Count; markerInd++) {
            pool.PoolItem(markers[markerInd]);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        targets.Add(collider);
    }
    
    private void OnTriggerExit2D(Collider2D collider) {
        targets.Remove(collider);
    }

    private Vector2 GetVector(float angle) {
        return new Vector2 (
            Round(Mathf.Cos(Mathf.Deg2Rad * angle), 0.01f),
            Round(Mathf.Sin(Mathf.Deg2Rad * angle), 0.01f)
        );
    }

    private float Round(float value, float place) {
        return (float)(System.Math.Round((double)value / place) * place);
    }
}
