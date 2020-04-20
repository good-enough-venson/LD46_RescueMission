using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SetCenterOfGravity : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    new public Rigidbody2D rigidbody {
        get => _rigidbody ? _rigidbody :
            _rigidbody = GetComponent<Rigidbody2D>();
        set => _rigidbody = value;
    }

    public Vector2 centerOfGravity;

    private void Awake() {
        rigidbody.centerOfMass = centerOfGravity;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(transform.position + transform.TransformVector(centerOfGravity), 1f);
    }
}
