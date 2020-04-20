using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIHealthBar : MonoBehaviour
{
    [System.Serializable]
    public class Portion
    {
        public string name;
        public Color color;
        [SerializeField]
        private float value;
        public RectTransform rTrans;
        public Image image;
        public Tooltip tooltip;
        [SerializeField]
        public string tooltipFormat = "Portion: {0}";
        public int sortOrder;
        
        public float Value
        {
            get { return value; }
            set {
                this.value = value;
                if (tooltip != null) {
                    tooltip.text = string.Format(tooltipFormat, 
                        Mathf.Floor(this.value / 0.5f) * 0.5f
                    );
                }
            }
        }
    }

    public enum DisplayType
    {
        Proportionate,
        CumulativePercents,
        IndividualPercents
    }

    public enum Pivot
    {
        Top, Bottom, Left, Right
    }

    public enum Orientation
    {
        Vertical, Horizontal
    }

    public RectTransform healthBar;
    public List<Portion> portions;
    public Orientation orientation;
    public Pivot alignment;
    public DisplayType arrangement;
    [SerializeField] private float maxValue = 0;
    [SerializeField] private bool splitIndividuals = true;
    [SerializeField] private bool forceKeepIndex = false;
    [SerializeField] private bool update = false;
    
    public float MaxValue
    {
        get {
            float t = PortionValueTotal(),
                m = PortionValueMax();

            switch (arrangement)
            {
                case DisplayType.Proportionate:
                    return t;

                case DisplayType.CumulativePercents:
                    if (maxValue < t) return t;
                    else return maxValue;

                case DisplayType.IndividualPercents:
                    if (maxValue < m) return m;
                    else return maxValue;

                default:
                    return maxValue;
            }
            //if (arrangement == DisplayType.Proportionate || maxValue < PortionValueTotal())
            //    return PortionValueTotal();
            //else return maxValue;
        }
        set { maxValue = value; }
    }

    public int PortionCount
    {
        get
        {
            if (portions == null) return -1;
            else return portions.Count;
        }
    }

    protected float PortionValueTotal() {
        float v = 0;
        for (int c = 0; c < PortionCount; c++)
            v += portions[c].Value;
        return v;
    }

    protected float PortionValueMax() {
        float v = float.MinValue;
        for (int c = 0; c < PortionCount; c++) {
            if(portions[c].Value > v)
                v = portions[c].Value;
        }
        return v;
    }

    protected Vector2 GetPivot(Pivot a)
    {
        switch (a)
        {
            case Pivot.Top:
                return new Vector2(0.5f, 1f);
            case Pivot.Bottom:
                return new Vector2(0.5f, 0f);
            case Pivot.Left:
                return new Vector2(0f, 0.5f);
            case Pivot.Right:
            default:
                return new Vector2(0.5f, 0.5f);
        }
    }

    protected Vector2 GetAnchor(Vector2 original, float val, Orientation o)
    {
        switch (o)
        {
            case Orientation.Vertical:
                original.y = val;
                break;
            case Orientation.Horizontal:
                original.x = val;
                break;
        }

        return original;
    }

    public void SetListIndexByIndex()
    {
        portions.Sort(delegate (Portion x, Portion y) {
            return x.sortOrder.CompareTo(y.sortOrder);
        });

        for (int c = portions.Count - 1; c >= 0; c--) {
            portions[c].sortOrder = c;
        }
    }

    protected void SetSibIndexByValue()
    {
        List<Portion> ports = new List<Portion>(portions);
        ports.Sort(delegate (Portion x, Portion y) {
            return x.Value.CompareTo(y.Value);
        });

        for (int c = ports.Count-1; c >= 0; c--) {
            if (ports[c].rTrans) ports[c].rTrans.SetAsLastSibling();
        }
    }
    
    protected void SetPortionSibIndex() {
        for (int c = 0; c < portions.Count; c++) {
            if (portions[c].rTrans) portions[c].rTrans.SetAsLastSibling();
        }
    }

    protected void SetPivot(Portion p, Pivot a) {
        if (p == null) return;
        p.rTrans.pivot = GetPivot(a);
    }

    protected void SetAnchors(Portion p, float startPercent, float endPercent, Pivot alignment) {
        SetAnchors(p, Vector2.zero, Vector2.one, startPercent, endPercent, alignment);
    }
    protected void SetAnchors(Portion p, Vector2 minAnchor, Vector2 maxAnchor,
        float startPercent, float endPercent, Pivot alignment)
    {
        if (p == null) return;
        startPercent = Mathf.Clamp01(startPercent);
        endPercent = Mathf.Clamp01(endPercent);

        switch (alignment)
        {
            case Pivot.Top:
            case Pivot.Right:
                float s = 1f - endPercent;
                float e = 1f - startPercent;

                startPercent = s;
                endPercent = e;
                break;
        }

        p.rTrans.anchorMin = GetAnchor(minAnchor, startPercent, orientation);
        p.rTrans.anchorMax = GetAnchor(maxAnchor, endPercent, orientation);

        //p.rTrans.sizeDelta = Vector2.zero;
        //p.rTrans.anchoredPosition = Vector2.zero;
    }

    void OnValidate()
    {
        if (update)
        {
            SetListIndexByIndex();

            for (int c = 0; c < portions.Count; c++)
            {
                portions[c].Value = Mathf.Max(portions[c].Value, 0f);
                if (portions[c].image) portions[c].image.color = portions[c].color;
            }

            UpdatePortions();
        }
    }

    public void UpdatePortions()
    {
        if (PortionCount <= 0) return;

        float min = (arrangement == DisplayType.IndividualPercents ? 1f : 0f);
        float max = 0f;

        for (int c = 0; c < PortionCount; c++)
        {
            SetPivot(portions[c], alignment);

            switch (arrangement)
            {
                case DisplayType.IndividualPercents:
                    max = portions[c].Value / MaxValue;
                    if (splitIndividuals) {
                        Vector2 minAnchor = Vector2.one * (float)c * (1f / PortionCount);
                        Vector2 maxAnchor = Vector2.one * (float)(c + 1) * (1f / PortionCount);

                        SetAnchors(portions[c], minAnchor, maxAnchor, 0f, max, alignment);
                    }
                    else {
                        SetAnchors(portions[c], 0f, max, alignment);
                    }
                    if (max < min) min = max;
                    break;

                default:
                    min = max;
                    max = min + (portions[c].Value / MaxValue);
                    SetAnchors(portions[c], min, max, alignment);
                    break;
            }
        }

        if (arrangement == DisplayType.IndividualPercents && !splitIndividuals && !forceKeepIndex)
            SetSibIndexByValue();
        else SetPortionSibIndex();
    }

    public string[] GetPortionNames()
    {
        string[] names = new string[PortionCount];
        for (int c = 0; c < names.Length; c++) {
            names[c] = portions[c].name;
        }
        return names;
    }

    public Portion GetPortion(string name)
    {
        Portion p = portions.Find(x =>
            x.name == name
        );

        return p;
    }

    public bool LerpValue(string name, float value, float time)
    {
        Portion p = GetPortion(name);
        if(p == null) {
            Debug.LogWarning("Invalid healthbar key: " + name, gameObject);
            return false;
        }
        LerpingHealthBar lerper = GetComponent<LerpingHealthBar>();
        if (lerper == null) {
            Debug.LogWarning("No LerpHealthbarScript!", gameObject);
            return false;
        }

        lerper.Lerp(value, name, time);
        return true;
    }

    public void SetValue(string name, float value)
    {
        Portion p = portions.Find(x => x.name == name );

        if (p != null) {
            p.Value = value;
            UpdatePortions();
        }
    }

    public float GetValue(string name) {
        Portion p = GetPortion(name);
        if (p == null) return 0f;
        return p.Value;
    }

    //public void SetValue(Portion p, float value) {
    //    if (p != null && portions != null && portions.Contains(p)) {
    //        p.value = value;
    //        UpdatePortions();
    //    }
    //}

    public void SetColor(string name, Color color)
    {
        Portion p = portions.Find(x =>
            x.name == name
        );

        if (p != null)
        {
            p.color = color;
            UpdatePortions();
        }
    }

    public void SetPortions(params Portion[] portions) {
        this.portions = new List<Portion>(portions);
        UpdatePortions();
    }
}
