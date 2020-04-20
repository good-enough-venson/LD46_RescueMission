using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class TooltipText : MonoBehaviour 
{
	[System.Serializable]
	public class Tip
	{
		public int priority;
		public string text;
		public float exitTime;
		public bool hasExitTime { get { return exitTime > 0f; } }
		public Tip() {  }
		public Tip(string m, int p){
			text = m; priority = p; exitTime = 0f;
		}
		public Tip(string m, int p, float t){
			text = m; priority = p; exitTime = t;
		}
	}
	
	protected static UnityEngine.UI.Text uiText;
	protected static List<Tip> tips;
	
	///<sumary>Returns either the last tip in tips, or null if there are none. Creates a new tip if there are none.</sumary>
	static Tip lastTip { 
		get { 
			if(tipCount > 0) return tips[tipCount-1];
			else return null; 
		}
		set {
			if(tipCount <= 0) {
				tips = new List<Tip>(1);
				tips.Add(value);
			}
			else {
				tips[tipCount-1] = value;
			}
		}
	}
	static int tipCount { get { 
		if(tips != null) return tips.Count; 
		else return -1; 
	} }
	
	public static bool AddTooltip(string message, int priority, float duration) {
		Tip newTip = new Tip(message, priority, Time.time + duration);
		return AddTooltip(newTip);
	}
	public static bool AddTooltip(Tip newTip)
	{
		if(uiText == null) {
			Debug.LogError("There must be a GameObject with a TooltipText Script!");
			return false;
		}
		//We want to replace the current tooltip if the new tip priority is
		// greater than or equal to that of the current tip.
		//If the priority is less, we will add it, but keep the current one for later.
		if(tipCount <= 0 || newTip.priority >= lastTip.priority) lastTip = newTip;
		else tips.Add(newTip);
		
		uiText.gameObject.SetActive(true);
		uiText.text = lastTip.text;
		
		return true;
	}
	public static void RemoveTooltip(Tip message)
	{
        if (uiText == null) return;
		uiText.text = "";
		if(tipCount > 0 && message != null) {
			tips.Remove(message);
		}
		if(tipCount > 0) {
			uiText.text = lastTip.text;
		}
	}
	
	public UnityEngine.UI.Text textObj;
    public Tip defaultToolTip;

    public void OnEnable() {
        if (!textObj) Debug.LogError("Please supply a UI Text Object!", gameObject);
        else uiText = textObj;

        if (defaultToolTip != null && !string.IsNullOrEmpty(defaultToolTip.text))
            AddTooltip(defaultToolTip);
    }
	
	public void Update() {
		for (int c = 0; c < tipCount; c++) {
			if(tips[c].hasExitTime && tips[c].exitTime < Time.time)
				RemoveTooltip(tips[c]); 
		}
	}
}
