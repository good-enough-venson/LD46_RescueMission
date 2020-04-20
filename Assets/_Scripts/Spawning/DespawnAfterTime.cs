using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Despawner))]
public class DespawnAfterTime : MonoBehaviour
{
    Despawner _despawner;
    Despawner despawner => _despawner ? _despawner :
        _despawner = GetComponent<Despawner>();

    public float time;

    private float timer;

    private void OnEnable() {
        timer = time;
    }

    private void Update() {
        timer -= Time.deltaTime;
        if (timer <= 0) {
            despawner.Despawn();
        }
    }
}
