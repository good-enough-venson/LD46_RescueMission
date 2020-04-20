using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorStatbar: Statbar
{
    public Animator animator;

    public string statName = "stat variable";
    private int statIndex = -1;

    public bool invertValue = false;

    public override void SetValue(float normalized)
    {
        normalized = Mathf.Clamp01(normalized);
        if (statIndex != -1) animator.SetFloat (
            statIndex, invertValue ? 1f - normalized : normalized
        );

        else Debug.Log(string.Format (
            "No Animator Value: '{0}'.\r\nValue: {1}",
            statName, normalized), this
        );
    }

    private void Awake() {
        if (animator == null) return;
        var param = new List<AnimatorControllerParameter>(
            animator.parameters).Find(p => p.name == statName);
        if (param != null) statIndex = param.nameHash;
    }
}
