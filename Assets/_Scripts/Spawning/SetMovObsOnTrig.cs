using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMovObsOnTrig : MonoBehaviour, IObstacleModifier
{
    public enum DamageType { single, continuous }

    [Tooltip("If this is null, the script will look for a Damageable componenet on the other object.")]
    public Damageable victim;
    public DamageType damageType;
    public float damageAmount;
    
    void IObstacleModifier.Send(MovingObstacle obstacle) {
        var subject = obstacle.gameObject.AddComponent<DoActionOnTriggerEnter2D>();
        subject.onStay = damageType == DamageType.continuous;
        subject.onEnter = damageType == DamageType.single;
        subject.action = OnPlayerEnter;
    }

    void OnPlayerEnter(Collider2D player)
    {
        Damageable _victim;
        
        // Check if we already have a victim or if the player is damageable.
        if (_victim = (victim ? victim : player.GetComponent<Damageable>())) {
            _victim.TakeDamage(damageType == DamageType.single ?
                damageAmount : damageAmount * Time.deltaTime);
        }
    }
}
