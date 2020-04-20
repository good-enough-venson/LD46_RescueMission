using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawner : MonoBehaviour
{
    public enum DeactivationOptions {
        Destroy, Disable
    }

    public GenericGameObjectPool pool;
    public DeactivationOptions alternateAction;

    public void Despawn() {
        if (pool) pool.PoolItem(gameObject);
        else switch (alternateAction)
        {
            case DeactivationOptions.Disable:
                gameObject.SetActive(false);
                break;

            case DeactivationOptions.Destroy:
                Debug.LogWarning("Destroying due to no pool found for \'" + gameObject.name + "\'!");
                Destroy(gameObject);
                break;
        }
    }
}
