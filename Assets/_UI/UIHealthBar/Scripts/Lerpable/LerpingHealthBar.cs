using System.Collections.Generic;

public class LerpingHealthBar : LerpingScript
{
    private UIHealthBar m_healthBar;
    protected UIHealthBar healthBar
    {
        get {
            if (m_healthBar == null)
                m_healthBar = GetComponent<UIHealthBar>();
            return m_healthBar;
        }
        set {
            m_healthBar = value;
        }
    }

    public void Lerp(float to, string tag, float lerpTime, string lerpEndCallback = null)
    {
        if (curve == null || curve.length < 1) return;
        if (lerpables == null) lerpables = new List<Lerpable>();

        LerpableUIHealthBar lerpable = lerpables.Find(l => l.name == tag) as LerpableUIHealthBar;
        if (lerpable == null) {
            lerpable = new LerpableUIHealthBar(healthBar, tag, to, curve, lerpTime);
            lerpables.Add(lerpable);
            StartCoroutine(Lerperate(lerpable));
        }
        else {
            lerpable.to = to;
        }
    }
}
