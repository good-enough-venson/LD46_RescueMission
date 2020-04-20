using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
	public string text {
		get { if(tooltip != null) return tooltip.text; else return ""; }
		set { if(tooltip != null) tooltip.text = value; }
	}
	public int priority {
		get { if(tooltip != null) return tooltip.priority; else return -1; }
		set { if(tooltip != null) tooltip.priority = value; }
	}
	
	private bool m_tooltipDisplayed = false;
	//private RectTransform tooltipItem;
	//private Text tooltipText;
	//public string tooltipString;
	
	public TooltipText.Tip tooltip;
	public float delay = 0.5f;
	//private Vector3 m_tooltipOffset;
	public void OnPointerEnter(PointerEventData eventData) {
		if(!this.enabled || string.IsNullOrEmpty(text)) return;
		Invoke("TurnOnTooltip", delay);
	}
	public void OnPointerExit(PointerEventData eventData)
	{
		if(!this.enabled) return;
		
		if(m_tooltipDisplayed) {
			TurnOffTooltip();	
		}
		CancelInvoke("TurnOnTooltip");
	}
	public void OnPointerDown(PointerEventData eventData){
		if(!this.enabled) return;
		
		CancelInvoke("TurnOnTooltip");
	}
	//void Start()
	//{
		//GameObject obj = GameObject.FindGameObjectWithTag("Tooltip");
		//tooltipItem = obj.GetComponent<RectTransform>();
		//if(!tooltipItem){
		//	Debug.LogWarning("There should be a RectTransform with a Text Component tagged \"Tooltip\"!", this.gameObject);
		//	this.enabled = false;
		//	return;
		//}
		
		//Offset the tooltip above the target GameObject
		//m_tooltipOffset = new Vector3(0, tooltipItem.sizeDelta.y , 0);
		//Deactivate the tooltip so that it is only shown when you want it to
		//tooltipItem.gameObject.SetActive(m_tooltipDisplayed);
		//Invoke("TurnOffTooltip", 0.1f);
		//tooltipText = tooltipItem.GetComponent<Text>();
		//if(!tooltipText) tooltipText = tooltipItem.GetComponentInChildren<Text>();
	//}
	
	void OnDisable()
	{
		if(IsInvoking("TurnOnTooltip")){
			CancelInvoke("TurnOnTooltip");
		}
		
		if(m_tooltipDisplayed) {
			TurnOffTooltip();
		}
	}
	
	void TurnOffTooltip()
	{
		m_tooltipDisplayed = false;
		TooltipText.RemoveTooltip(tooltip);
		//tooltipItem.gameObject.SetActive(m_tooltipDisplayed);
	}
	
	void TurnOnTooltip()
	{	
		m_tooltipDisplayed = true;
		TooltipText.AddTooltip(tooltip);
		//if(tooltipText != null) tooltipText.text = t;
		//tooltipItem.gameObject.SetActive(m_tooltipDisplayed);
		//m_tooltipOffset = new Vector3(0, tooltipItem.sizeDelta.y , 0);
		//tooltipItem.transform.position = transform.position + m_tooltipOffset;
		//tooltipItem.position = transform.position + Vector3.up * 30f;
	}
}