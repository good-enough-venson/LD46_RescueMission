using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargoScript : PoolingDamageable
{
    new public SpriteRenderer renderer;
    public List<Sprite> sprites;

    public float CargoValue => Durability / initialDurability;

    private void OnEnable() {
        if (renderer) renderer.sprite = sprites[Random.Range(0, sprites.Count)];
    }
}
