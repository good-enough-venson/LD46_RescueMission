using UnityEngine;

//[System.Serializable]
public class Lerpable
{
    public string name;
    public string message;
    public float startTime;
    public float duration;
    public float endTime;
    public AnimationCurve curve;

    public float percent {
        get { return (Time.time - startTime) / (endTime - startTime); }
    }
    public virtual bool lerping {
        get { return Time.time < endTime; }
    }

    public virtual void Update() { }
    public virtual void SetToEnd() { }

    public override string ToString() {
        return string.Format("Lerpable: {0} Starting: {1} Ending: {2} Duration: {3} Calls: {4}",
            name, startTime, endTime, duration, message);
    }
}