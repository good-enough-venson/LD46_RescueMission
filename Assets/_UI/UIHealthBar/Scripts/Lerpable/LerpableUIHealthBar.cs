using UnityEngine;

public class LerpableUIHealthBar : Lerpable
{
    public const float thresh = 0.01f;
    public UIHealthBar bar;
    public float from;
    public float to;

    public float current {
        get { return Mathf.Lerp(from, to, Mathf.Clamp01(curve.Evaluate(percent))); }
    }
    new public bool lerping {
        get { return base.lerping && Mathf.Abs(current - to) > thresh; }
    }

    public LerpableUIHealthBar(UIHealthBar bar, string key, float to, AnimationCurve curve, float lerpTime, string message = null)
    {
        name = key;
        this.message = message;
        startTime = Time.time;
        duration = lerpTime;
        endTime = startTime + duration;
        this.curve = curve;

        this.bar = bar;
        from = bar.GetValue(key);
        this.to = to;
    }

    public override void Update() { bar.SetValue(name, current); }
    public override void SetToEnd() { bar.SetValue(name, to); }

    public override string ToString() {
        return string.Format("{0} From: {1} To: {2} Current: {3} Lerping: {4}",
            base.ToString(), from, to, current, lerping);
    }
}
