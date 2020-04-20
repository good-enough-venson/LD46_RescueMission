using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IControllableRocket {
    float cruiserThrottle { get; set; }
    float boosterThrottle { get; set; }
    float headingThrottle { get; set; }
}

public interface IObstacleModifier {
    void Send(MovingObstacle obstacle);
}

public abstract class Statbar: MonoBehaviour {
    public abstract void SetValue(float normalized);
}

[RequireComponent(typeof(Despawner))]
public abstract class Damageable: MonoBehaviour
{
    private Despawner _despawner;
    public Despawner despawner => _despawner ? _despawner :
        _despawner = GetComponent<Despawner>();
    public virtual float Durability { get; set; }
    public virtual float DamageThreshold { get; set; }
    public virtual bool TakeDamage(float amount) {
        amount = Mathf.Max(amount - DamageThreshold, 0);
        Durability = Mathf.Max(Durability - amount, 0);
        return Durability <= 0f;
    }
}

[RequireComponent(typeof(Despawner))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class Projectile: MonoBehaviour
{
    private Despawner _despawner;
    public Despawner despawner => _despawner ? _despawner :
        _despawner = GetComponent<Despawner>();

    private Rigidbody2D _rigidbody;
    new public Rigidbody2D rigidbody => _rigidbody ? _rigidbody :
        _rigidbody = GetComponent<Rigidbody2D>();

    protected virtual Vector2 position { set => transform.position = value;  get => transform.position; }
    protected virtual Vector2 velocity { set => rigidbody.velocity = value; get => rigidbody.velocity; }
    public virtual void OnFire(Vector2 pos, Vector2 vel, float angle) {
        position = pos; velocity = vel;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    protected abstract float GetDamage(Collision2D collision);
    protected virtual void OnCollisionEnter2D(Collision2D collision) {
        Damageable other = collision.gameObject.GetComponent<Damageable>();
        if (other) other.TakeDamage(GetDamage(collision));
    }
}

public abstract class Throttleable: MonoBehaviour
{
    new public Rigidbody2D rigidbody;
    public abstract float Throttle { get; set; }
    protected virtual void OnEnable() {
        if (!rigidbody) gameObject.SetActive(false);
    }
}

public abstract class AutoFiring: MonoBehaviour
{
    protected int shotsLeft = 0;

    /// <summary>
    /// Automatically fire the designated number of shots.
    /// A value less than zero will result in infinite firing.
    /// </summary>
    public virtual void FireShots(int shotCount = 1) {
        shotsLeft = shotCount;
    }
}
