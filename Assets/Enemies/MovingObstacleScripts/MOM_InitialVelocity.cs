using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MOM_InitialVelocity : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    new public Rigidbody2D rigidbody {
        get => _rigidbody ? _rigidbody : _rigidbody = GetComponent<Rigidbody2D>();
        set => _rigidbody = value;
    }
    
    [Tooltip("The maximum change from the initial trajectory in degrees.")]
    [Range(min: 0, max: 45)]
    public float trajectoryDivergence = 0f;

    [Tooltip("The maximum change from the initial speed as a percent.")]
    [Range(min: 0f, max: 1f)]
    public float speedVariance = 0f;

    protected float speed;
    protected float trajectory;
    private bool ready = false;

    protected Vector2 vector { get => trajectory.GetVector(); }

    public virtual void Setup(float trajectory, float speed) {
        this.speed = speed * (1 + Random.Range(-speedVariance, speedVariance));
        this.trajectory = trajectory + Random.Range(-trajectoryDivergence, trajectoryDivergence);
        ready = true;
    }

    protected virtual void FixedUpdate() {
        if (!ready) return;
        rigidbody.AddForce(vector * speed, ForceMode2D.Impulse);
        ready = false;
    }

    protected virtual void OnDrawGizmos() {
        Vector3 direction = vector;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position,
            transform.position + direction);
    }

    protected void DrawArrow(Vector2 from, Vector2 to) {
        var vector = (to - from).normalized;
        Gizmos.DrawLine(from, to);
        Gizmos.DrawLine(to, to + vector.Rotate(45));
        Gizmos.DrawLine(to, to + vector.Rotate(-45));
    }
}
