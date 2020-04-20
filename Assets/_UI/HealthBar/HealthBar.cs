using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBar : Statbar 
{
	public Image bar;
	public float fullWidth;
	public float percent = 1f;

    public override void SetValue(float normalized) {
        this.percent = normalized;
        bar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, fullWidth * normalized);
    }

    private void OnValidate() {
        SetValue(percent);
    }
}
