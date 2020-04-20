using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct GunStats
{
    [Tooltip("The speed at which the projectile leaves the gun.")]
    [Range(min: 0, max: 30)]
    public float fireSpeed;

    [Tooltip("The lowest valid projectile speed.")]
    [Range(min: 0, max: 30)]
    public float minSpeed;

    [Tooltip("The minimum delay in seconds between shots.")]
    [Range(min: 0, max: 30)]
    public float fireDelay;

    [Tooltip("The amount of fuel this engine uses per shot.")]
    [Range(min: 0, max: 10)]
    public float fuelUse;
}

public class GunScript: AutoFiring
{
    Controls controls;

    [Label("External Links")]
    public GenericGameObjectPool projectilePool;
    new public Rigidbody2D rigidbody;
    public FuelScript fuelTank;
    public Statbar cooldownDisplay;

    [Label(preSpace: 10, "Gun")]
    public Vector2 direction = Vector2.up;
    public GunStats stats;

    [SerializeField, ReadOnly]
    private float cooldown = 0f;

    public Vector2 position => transform.position;
    public Vector2 forward => transform
        .TransformDirection(direction).normalized;

    protected void Fire(float speed) {
        if (projectilePool == null) return;
        var projectile = projectilePool.UnpoolItem<Projectile>();
        var velocity = forward * speed + (rigidbody ? rigidbody.velocity : Vector2.zero);
        projectile.OnFire(position, velocity, Vector2.SignedAngle(Vector2.right, forward));
        //Debug.Break();
    }

    private void Awake() {
        controls = new Controls();
        controls.InGame_Actions.Fire.performed += (val) => { FireShots(1000); };
        controls.InGame_Actions.Fire.canceled  += (val) => { FireShots(0); };
    }

    private void Update()
    {
        cooldown = cooldown.Move(towards: 0f, by: Time.deltaTime);
        if (cooldownDisplay) cooldownDisplay.SetValue(cooldown / stats.fireDelay);
        if (cooldown > 0f || shotsLeft == 0) return;

        float fuelModifier = 1f;
        if (fuelTank != null && stats.fuelUse > 0f) {
            // If we don't have enough fuel to shoot our projectiles fast enough, just don't shoot them at all.
            if (fuelTank.flow < stats.fuelUse * (stats.minSpeed / stats.fireSpeed)) return;
            fuelModifier = (fuelTank.GetFuelOneShot(stats.fuelUse) / stats.fuelUse);
        }

        if (stats.fireSpeed * fuelModifier < stats.minSpeed) return;
        Fire(stats.fireSpeed * fuelModifier);

        cooldown = stats.fireDelay;
        shotsLeft = Mathf.Max(0, shotsLeft - 1);
    }

    private void OnEnable() { controls.Enable(); }
    private void OnDisable() { controls.Disable(); }
}
