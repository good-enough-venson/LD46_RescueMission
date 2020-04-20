using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScore : MonoBehaviour
{
    static GameScore instance;

    public static void AddShip(float shipDurability) {
        if (!instance) return;
        instance.cShips += shipDurability;
        instance.ships.SetValue(Mathf.Clamp01(instance.cShips / instance.tShips));
    }
    public static void AddCargo(float cargoDurability) {
        if (!instance) return;
        instance.cCargo += cargoDurability;
        instance.cargo.SetValue(Mathf.Clamp01(instance.cCargo / instance.tCargo));
    }
    public static void AddEnemy() {
        if (!instance) return;
        instance.cEnemies += 1f;
        instance.enemies.SetValue(Mathf.Clamp01(instance.cEnemies / instance.tEnemies));
    }

    public Statbar ships, cargo, enemies;

    public int tShips, tCargo, tEnemies;

    [SerializeField, ReadOnly]
    private float cShips, cCargo, cEnemies;

    private void Awake() {
        if (instance == null) instance = this;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print(collision.name + " Entering Home");

        var cargo = collision.GetComponent<CargoScript>();
        var ship = collision.GetComponentInParent<PoolingDamageable>();

        if (cargo && cargo.tag == "Cargo") {
            AddCargo(cargo.Durability / cargo.initialDurability);
        }
        else if (ship && ship.tag == "Barge") {
            print("I'm barging on in!");
            AddShip(ship.Durability / ship.initialDurability);
        }
    }
}
