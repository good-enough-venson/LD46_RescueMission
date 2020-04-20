using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelScript : MonoBehaviour
{
    [Tooltip("Please note that this should be a normalized curve.")]
    public AnimationCurve flowByVolume;
    public float capacity = 100;
    public float volume = 100;

    [SerializeField, ReadOnly]
    private float availableFlow = 0f;

    public float flow => Mathf.Min(volume,
        volume * flowByVolume.Evaluate(volume / capacity));

    private void LateUpdate() {
        availableFlow = flow * Time.deltaTime;
    }

    public float GetFuelOverTime(float volume) {
        volume = Mathf.Min(volume, availableFlow);
        availableFlow -= volume;
        this.volume -= volume;
        return volume;
    }

    public float GetFuelOneShot(float volume) {
        volume = Mathf.Min(volume, flow);
        this.volume -= volume;
        return volume;
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.tag == "Ground") {
            volume = volume.Move(towards: capacity, by: Time.deltaTime);
        }
    }
}
