using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomPropertyDrawer(typeof(LabelAttribute))]
public class LabelAttributeDrawer : PropertyDrawer 
{
	private int labelHeight = 0;

	private LabelAttribute _attributeValue = null;
	private LabelAttribute attributeValue
	{
		get
		{
			if (_attributeValue == null)
			{
				_attributeValue = (LabelAttribute) attribute;
			}
			return _attributeValue;
		}
	}

	public override float GetPropertyHeight (SerializedProperty prop, GUIContent label)
	{
		int preSpace = attributeValue.preSpace;
		int postSpace = attributeValue.postSpace;
		int labelSpace = (string.IsNullOrEmpty (attributeValue.labelText) ? 
			0 : (int)base.GetPropertyHeight (prop, new GUIContent (attributeValue.labelText)));

		labelHeight = preSpace + labelSpace + postSpace;

		float propertyHeight = base.GetPropertyHeight (prop, label);

		return propertyHeight + labelHeight;
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		//labelHeight = (int) base.GetPropertyHeight (property, new GUIContent (attributeValue.labelText));

		Rect labelPos = new Rect (position.position.x, position.position.y + attributeValue.preSpace, position.width, labelHeight);
		Rect propertyPos = new Rect (position.position.x, position.position.y + labelHeight, position.width, position.height - labelHeight);

		EditorGUI.LabelField (labelPos, attributeValue.labelText, EditorStyles.boldLabel);

		if(attributeValue.paramAsLabel) {
			labelPos = new Rect (propertyPos.x + (propertyPos.width * (3f/8f)), propertyPos.y, propertyPos.width * (5f/8f), propertyPos.height);
			propertyPos = new Rect (propertyPos.position, new Vector2 (propertyPos.width * (3f/8f), propertyPos.height));

			EditorGUI.LabelField (propertyPos, property.displayName, EditorStyles.label);
			EditorGUI.LabelField (labelPos, GetParamString (property), EditorStyles.label);
		}
		else EditorGUI.PropertyField (propertyPos, property);
	}

	string GetParamString (SerializedProperty property)
	{
		string paramString = "";

		SerializedPropertyType propertyType = property.propertyType;

		switch(propertyType){
		case SerializedPropertyType.Boolean:
			paramString = property.boolValue.ToString();
			break;

		case SerializedPropertyType.Float:
			paramString = property.floatValue.ToString();
			break;

		case SerializedPropertyType.Integer:
			paramString = property.intValue.ToString();
			break;

		case SerializedPropertyType.String:
			paramString = property.stringValue.ToString();
			break;

		default :
			paramString = "Type \'" + property.type + "\' is not supported.";
			break;
		}
		return paramString;
	}
}

//[CustomPropertyDrawer(typeof(IndentAttribute))]
//public class IndentDrawer : PropertyDrawer
//{
//	private IndentAttribute _attributeValue = null;
//	private IndentAttribute attributeValue
//	{
//		get
//		{
//			if (_attributeValue == null)
//			{
//				_attributeValue = (IndentAttribute) attribute;
//			}
//			return _attributeValue;
//		}
//	}
//
//	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
//		EditorGUI.indentLevel += Mathf.Max(attributeValue.increment, 0);
//		EditorGUI.PropertyField(position, property);
//	}
//}
