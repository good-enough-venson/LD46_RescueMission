using UnityEngine;
using System.Collections;

public class LabelAttribute : PropertyAttribute 
{
	public readonly int preSpace;
	public readonly string labelText;
	public readonly int postSpace;
	public readonly bool paramAsLabel;

	public LabelAttribute(string text, bool paramAsLabel = false)
	{
		this.preSpace = 0;
		this.postSpace = 0;
		this.labelText = text;
		this.paramAsLabel = paramAsLabel;
	}
	public LabelAttribute(int preSpace, string text, int postSpace, bool paramAsLabel = false){
		this.preSpace = preSpace;
		this.postSpace = postSpace;
		this.labelText = text;
		this.paramAsLabel = paramAsLabel;
	}
	public LabelAttribute(int preSpace, string text, bool paramAsLabel = false){
		this.preSpace = preSpace;
		this.postSpace = 0;
		this.labelText = text;
		this.paramAsLabel = paramAsLabel;
	}
	public LabelAttribute(string text, int postSpace, bool paramAsLabel = false){
		this.preSpace = 0;
		this.postSpace = postSpace;
		this.labelText = text;
		this.paramAsLabel = paramAsLabel;
	}
	public LabelAttribute(int space, bool paramAsLabel = false)
	{
		this.preSpace = space;
		this.postSpace = 0;
		this.labelText = null;
		this.paramAsLabel = paramAsLabel;
	}
	public LabelAttribute(bool paramAsLabel)
	{
		this.preSpace = 0;
		this.postSpace = 0;
		this.labelText = null;
		this.paramAsLabel = paramAsLabel;
	}
}

//public class IndentAttribute : PropertyAttribute
//{
//	public readonly int increment;
//
//	public IndentAttribute(int i){
//		increment = i;
//	}
//}
