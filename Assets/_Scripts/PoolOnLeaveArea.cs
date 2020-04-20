using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GenericGameObjectPool))]
public class PoolOnLeaveArea : MonoBehaviour
{
    private GenericGameObjectPool _pool;
    public GenericGameObjectPool pool => _pool ? _pool :
        _pool = GetComponent<GenericGameObjectPool>();

    public string tagged;

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == tagged && pool != null) {
            pool.PoolItem(other);
        }
    }
}
