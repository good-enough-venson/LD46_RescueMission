using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LerpingScript : MonoBehaviour
{
    [SerializeField]
    protected AnimationCurve curve;
    [SerializeField]
    protected float updateRate = 0.02f;

    public List<Lerpable> lerpables;

    protected IEnumerator Lerperate(Lerpable lerpable)
    {
        while (lerpable.lerping) {
            lerpable.curve = curve;
            lerpable.Update();
            yield return new WaitForSeconds(updateRate);
        }

        lerpable.SetToEnd();

        //if (!string.IsNullOrEmpty(lerpable.message))
        //    RoyalHerald.TriggerEvent(lerpable.message);
        if (lerpables != null) lerpables.Remove(lerpable);
    }
}
