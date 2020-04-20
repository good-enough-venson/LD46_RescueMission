using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStatbar: Statbar
{
    new public Animation animation;

    AnimationClip clip => animation.clip;

    public override void SetValue(float normalized) {
        if (animation == null || clip == null) return;
        clip.SampleAnimation(gameObject, normalized * clip.length);
    }
}
