using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GenericGameObjectPool))]
[RequireComponent(typeof(BoxCollider2D))]
public class MovingObstacleSpawner : MonoBehaviour
{
    private BoxCollider2D _collider;
    new public BoxCollider2D collider {
        get => _collider ? _collider : _collider = GetComponent<BoxCollider2D>();
        set => _collider = value;
    }

    private GenericGameObjectPool _pool;
    protected GenericGameObjectPool pool => _pool ? _pool :
        _pool = GetComponent<GenericGameObjectPool>();

    private SpawnZone[] _spawnZones;
    public SpawnZone[] spawnZones {
        get => _spawnZones != null ? _spawnZones :
            _spawnZones = GetComponents<SpawnZone>();
        set => _spawnZones = value;
    }

    private IObstacleModifier[] _obstacleModifiers;
    public IObstacleModifier[] obstacleModifiers {
        get => _obstacleModifiers != null ? _obstacleModifiers :
            _obstacleModifiers = GetComponents<IObstacleModifier>();
        set => _obstacleModifiers = value;
    }

    [Range(min: 0.2f, max: 120f)]
    public float minDelay = 1f, maxDelay = 1f;

    public int prespawn = 0;
    public float nextSpawnTime;

    public Rect rect {
        get => new Rect (
            transform.position.V2() + collider.offset -
            collider.size * 0.5f, collider.size
        );
    }

    private void Start() {
        for (int c = 0; c < prespawn; c++) {
            Spawn(false);
        }
    }

    public void Update() {
        if (Time.time >= nextSpawnTime) {
            nextSpawnTime = Time.time + Random.Range(minDelay, maxDelay);
            Spawn();
        }
    }

    public void Spawn(bool inZone = true)
    {
        var zone = spawnZones.Length <= 0 ? null :
            spawnZones[Random.Range(0, spawnZones.Length)];

        if (zone == null) {
            Debug.LogWarning("Cannot Spawn Object: No Spawn Zones!", this);
            return;
        }

        var obstacle = pool.UnpoolItem<MovingObstacle>();
        if (obstacle == null) {
            Debug.LogWarning("Cannot Spawn Object: No Suitable Prefab!", this);
            return;
        }

        obstacle.transform.position = inZone ? zone.rect.Point() : rect.Point();
        obstacle.spawner = gameObject;

        var ivc = obstacle.GetComponent<MOM_InitialVelocity>();
        if (ivc) ivc.Setup(zone.initialAngle, zone.initialSpeed);

        foreach (var moddr in obstacleModifiers) {
            moddr.Send(obstacle);
        }
    }

    public void Despawn(MovingObstacle obstacle) {
        pool.PoolItem(obstacle);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        DrawRect(new Rect(
            new Vector2(transform.position.x, transform.position.y) +
            collider.offset - collider.size * 0.5f, collider.size
        ) );
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
