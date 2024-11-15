using System.Collections;
using UnityEngine;
using UnityEditor;


/* Source: 
 *		Audio ScriptableObjects are inspired by https://www.youtube.com/watch?v=6vmRwLYWNRo , 
 *		as well as https://github.com/richard-fine/scriptable-object-demo/tree/main/Assets/ScriptableObject/Audio */

[CustomPropertyDrawer(typeof(RangedFloat), true)]
public class RangedFloatDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		label = EditorGUI.BeginProperty(position, label, property);
		position = EditorGUI.PrefixLabel(position, label);

		SerializedProperty _minProp = property.FindPropertyRelative("minValue");
		SerializedProperty _maxProp = property.FindPropertyRelative("maxValue");

		float _minValue = _minProp.floatValue;
		float _maxValue = _maxProp.floatValue;

		float _rangedMin = 0;
		float _rangedMax = 1;

		var _ranges = ( MinMaxRangeAttribute[ ] )fieldInfo.GetCustomAttributes(typeof(MinMaxRangeAttribute), true);
		if (_ranges.Length > 0 )
		{
			_rangedMin = _ranges[0].Min;
			_rangedMax = _ranges[0].Max;
		}

		const float _rangeBoundsLabelWidth = 40f;

		var _rangeBoundsLabel1Rect = new Rect(position);
		_rangeBoundsLabel1Rect.width = _rangeBoundsLabelWidth;
		GUI.Label(_rangeBoundsLabel1Rect, new GUIContent(_minValue.ToString("F2")));
		position.xMin += _rangeBoundsLabelWidth;

		var _rangeBoundsLabel2Rect = new Rect(position);
		_rangeBoundsLabel2Rect.xMin = _rangeBoundsLabel2Rect.xMax - _rangeBoundsLabelWidth;
		GUI.Label(_rangeBoundsLabel2Rect, new GUIContent(_maxValue.ToString("F2")));
		position.xMax -= _rangeBoundsLabelWidth;

		EditorGUI.BeginChangeCheck();
		EditorGUI.MinMaxSlider(position, ref _minValue, ref _maxValue, _rangedMin, _rangedMax);
		if ( EditorGUI.EndChangeCheck() )
		{
			_minProp.floatValue = _minValue;
			_maxProp.floatValue = _maxValue;
		}

		EditorGUI.EndProperty();
	}
}
