using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Despawner))]
public class ExplodeOnCollision : MonoBehaviour
{
    private Despawner _despawner;
    public Despawner despawner => _despawner ? _despawner :
        _despawner = GetComponent<Despawner>();

    private Rigidbody2D _rigidbody;
    new public Rigidbody2D rigidbody {
        get => _rigidbody ? _rigidbody : _rigidbody = GetComponent<Rigidbody2D>();
        set => _rigidbody = value;
    }

    [Label("Parameters")]
    public LayerMask aoeMask;

    [Tooltip("The force fall off for the area of effect. " +
        "Note, this should be normalized, but it can be negative.")]
    public AnimationCurve aoeFalloff;

    [Tooltip("If set higher than 0.0, this will apply forces not only to " +
        "the object collided with, but also to objects within this radius.")]
    [Range(0f, 50f)]
    public float areaOfEffect = 0f;

    [Tooltip("The force with which the colliding object is " +
        "pushed away from this object's center of mass.")]
    [Range(0f, 100f)]
    public float blastForce;

    [Tooltip("The force at which the explosion occurs.")]
    public float impactThreshold;

    [Tooltip("For every unit of force applied to a rigidbody, try to damage it by this much.")]
    public float damage = 1;

    private Vector2 center => rigidbody == null ?
        new Vector2(transform.position.x, transform.position.y) :
        rigidbody.worldCenterOfMass;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var impact = GetImpact(collision.contacts);
        print(string.Format("Collided with a force of: {0}",
            impact.relativeVelocity.magnitude));

        if (impact.relativeVelocity.magnitude < impactThreshold)
            return;

        if (blastForce >= 0f)
        {
            if (areaOfEffect > 0f)
            {
                var others = GetOthersAround(center);

                foreach (var other in others) ApplyForce (
                    to: other, at: other.position + other.centerOfMass
                );
            }

            else ApplyForce (
                to: impact.otherRigidbody,
                at: impact.point
            );
        }

        Damageable d = GetComponent<Damageable>();
        if (d) d.TakeDamage(d.Durability + 1f);
    }

    private void ApplyForce(Rigidbody2D to, Vector2 at)
    {
        if (to == null || center == at) return;
        var other = to; var point = at;

        Vector3 force = (point - center).normalized * (
            areaOfEffect <= 0f || aoeFalloff.length < 1 ? blastForce :
            aoeFalloff.Evaluate(Mathf.Clamp01 (
                Vector2.Distance(center, point) / areaOfEffect
            )) * blastForce
        );

        Debug.Log("BOOM @ " + force.magnitude);

        //other.AddForceAtPosition(Vector2.up * 10, point, ForceMode2D.Impulse);
        other.AddForceAtPosition(force, point, ForceMode2D.Impulse);

        var damageable = other.GetComponent<Damageable>();
        if (damageable) damageable.TakeDamage(force.magnitude * damage);
        //if (other.tag == "Projectile") GameScore.AddEnemy();
    }

    private List<Rigidbody2D> GetOthersAround(Vector2 center) {
        var cc = Physics2D.OverlapCircleAll(center, areaOfEffect, aoeMask);
        var rr = new List<Rigidbody2D>(cc.Length);
        foreach (var c in cc) if (c.attachedRigidbody != null)
            rr.Add(c.attachedRigidbody);
        //rr.TrimExcess();
        return rr;
    }

    /// <summary>For now, we're only looking for the point of hardest impact.</summary>
    private ContactPoint2D GetImpact(ContactPoint2D[] points) {
        ContactPoint2D best = new ContactPoint2D();
        foreach (var p in points) best = GetImpact(p, best);
        return best;
    }

    private ContactPoint2D GetImpact(ContactPoint2D a, ContactPoint2D b) {
        return a.relativeVelocity.sqrMagnitude > b.relativeVelocity.sqrMagnitude ? a : b;
    }

    private void OnDrawGizmosSelected() {
        if (areaOfEffect > 0f) Gizmos.DrawWireSphere(center, areaOfEffect);
    }
}
