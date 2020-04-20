using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingDamageable : Damageable
{
    public Transform explosionBlueprint;
    public Statbar display;

    public float damageThreshold = 0;
    public float initialDurability = 100;
    public float durability = 0;

    public override float DamageThreshold {
        set => damageThreshold = value;
        get => damageThreshold;
    }

    public override float Durability
    {
        set {
            durability = value;
            if(display) display.SetValue(durability / initialDurability);
        }

        get => durability;
    }

    public override bool TakeDamage(float amount)
    {
        if (base.TakeDamage(amount))
        {
            despawner.Despawn();

            if (explosionBlueprint) ExplosionSpawner.Explode (
                transform.position + explosionBlueprint.localPosition,
                explosionBlueprint.localScale
            );

            return true;
        }

        return false;
    }

    private void Start() {
        Durability = initialDurability;
    }

    private void OnEnable() {
        Durability = initialDurability;
    }
}
