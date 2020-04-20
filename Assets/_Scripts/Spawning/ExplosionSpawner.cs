using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GenericGameObjectPool))]
public class ExplosionSpawner : MonoBehaviour
{
    private static ExplosionSpawner instance;

    public static void Explode(Vector2 pos, Vector3 scale)
    {
        Debug.Log("EXPLODE!!");

        if (instance == null) return;

        var obj = instance.pool.UnpoolItem<Transform>();

        if (obj) {
            obj.position = pos;
            obj.localScale = scale;
        }
    }

    private GenericGameObjectPool _pool;
    protected GenericGameObjectPool pool => _pool ? _pool :
        GetComponent<GenericGameObjectPool>();

    private void Awake() {
        if (instance == null) instance = this;
    }
}
