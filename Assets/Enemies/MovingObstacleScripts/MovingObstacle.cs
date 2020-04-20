using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Despawner))]
[RequireComponent(typeof(Rigidbody2D))]
public class MovingObstacle : MonoBehaviour
{
    private Despawner _despawner;
    public Despawner despawner => _despawner ? _despawner :
           _despawner = GetComponent<Despawner>();

    public GameObject spawner { get; set; }

    [Label(preSpace: 10, "Appearance")]
    public bool canFlipX = true;
    public bool canFlipY = false;

    [Range(min: 0.1f, max: 10f)]
    public float minScale = 1;
    
    [Range(min: 0.1f, max: 10f)]
    public float maxScale = 1;

    private Vector3 initialScale;

    private void Awake() {
        initialScale = transform.localScale;
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (spawner == collision.gameObject)
            despawner.Despawn();
    }

    private void OnEnable() {
        var scale = transform.localScale;
        if (canFlipX) scale.x *= Random.Range(0, 100) > 50 ? 1 : -1;
        if (canFlipY) scale.y *= Random.Range(0, 100) > 50 ? 1 : -1;
        transform.localScale = scale * Random.Range(minScale, maxScale);
    }

    private void OnDisable() {
        transform.localScale = initialScale;
    }
}
