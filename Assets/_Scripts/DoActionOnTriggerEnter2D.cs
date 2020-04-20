using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoActionOnTriggerEnter2D : MonoBehaviour
{
    //public LayerMask layerMask;
    public bool onEnter = true, onStay = false;
    public System.Action<Collider2D> action = (other) => { };

    private void OnTriggerEnter2D(Collider2D other) {
        if (onEnter && CheckLayer(other)) action(other);
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (onStay && CheckLayer(other)) action(other);
    }

    private bool CheckLayer(Collider2D other) {
        return true;
        //return ((1 << other.gameObject.layer) & layerMask) != 0;
    }
}
