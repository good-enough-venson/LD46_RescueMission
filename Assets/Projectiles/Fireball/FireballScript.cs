using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballScript : Projectile
{
    public float relativeSpeedCap = 5;
    public float maximumDamage = 100;

    [Tooltip("The minimum allowed speed as a percent of the initial speed.")]
    [Range(0f, 1f)]
    public float minSpeed = 0.5f;
    [SerializeField, ReadOnly]
    private float initialSpeed;

    public Vector2 direction = Vector2.right;
    public Vector2 forward => transform
        .TransformDirection(direction).normalized;

    public override void OnFire(Vector2 pos, Vector2 vel, float angle) {
        base.OnFire(pos, vel, angle); initialSpeed = vel.magnitude;
        var self = GetComponent<Damageable>();
    }

    private void Update() {
        if (velocity.magnitude < initialSpeed * minSpeed) despawner.Despawn();
    }

    protected override void OnCollisionEnter2D(Collision2D collision) {
        Damageable other = collision.gameObject.GetComponent<Damageable>();
        if (other) if (other.TakeDamage(GetDamage(collision)))
            if (other.tag == "Monkey") GameScore.AddEnemy();

        Damageable self = collision.gameObject.GetComponent<Damageable>();
        if (self) self.TakeDamage(GetDamage(collision, selfInflicted: true));
    }

    protected override float GetDamage(Collision2D collision) {
        return GetDamage(collision);
    }

    protected float GetDamage(Collision2D collision, bool selfInflicted = false)
    {
        var tMass = collision.rigidbody.mass;
        var oMass = collision.otherRigidbody.mass;
        var colVel = collision.relativeVelocity;

        float impactSpeed = Vector2.Dot(colVel, forward.normalized).Abs();
        float relativeMass = selfInflicted ? (oMass / (oMass + tMass)) :
            (tMass / (tMass + oMass));

        var relativeSpeedModifier = Mathf.Min(
            impactSpeed, relativeSpeedCap) / relativeSpeedCap;

        var damage = relativeSpeedModifier * relativeMass * maximumDamage;

        //Debug.Log(string.Format (
        //    "Collided at {0}% speed, and {1}% mass difference, for a total damage of {2}.",
        //    relativeSpeedModifier, relativeMass, damage
        //));

        return damage;
    }
}
