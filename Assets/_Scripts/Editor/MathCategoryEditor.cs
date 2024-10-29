using System;
using UnityEditor;
using UnityEngine;

[CustomEditor( typeof( MathCategory ) )]
public class MathCategoryEditor : Editor {
	private SerializedProperty decimals, ones, tens, hundreds, thousands;
	private SerializedProperty decimalMastery, oneMastery, tensMastery, hundredMastery, thousandsMastery;
	private const int TimelineStart = 0;
	private const int TimelineEnd = 1984;
	private MathCategory mathCategory;
	private float dragOffset;
	private bool isDragging;
	private bool isResizingStart;
	private bool isResizingEnd;
	private GUIStyle fontStyle;
	private void OnEnable () {
		mathCategory = (MathCategory)target;

		decimals = serializedObject.FindProperty( "Decimals" );
		ones = serializedObject.FindProperty( "Ones" );
		tens = serializedObject.FindProperty( "Tens" );
		hundreds = serializedObject.FindProperty( "Hundreds" );
		thousands = serializedObject.FindProperty( "Thousands" );

		decimalMastery = serializedObject.FindProperty( "DecimalMastery" );
		oneMastery = serializedObject.FindProperty( "OneMastery" );
		tensMastery = serializedObject.FindProperty( "TensMastery" );
		hundredMastery = serializedObject.FindProperty( "HundredMastery" );
		thousandsMastery = serializedObject.FindProperty( "ThousandsMastery" );
	}

	public override void OnInspectorGUI () {
		serializedObject.Update();
		fontStyle = new GUIStyle( GUI.skin.label ) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, wordWrap = true };
		//DrawCheckBoxes();

		// Increase the height of the timeline window to fit all bars
		Rect timelineRect = GUILayoutUtility.GetRect( 0, 6 * (30 + 20), GUILayout.ExpandWidth( true ) );
		DrawTimeline( timelineRect );

		DrawDefaultInspector();
		//DrawDefaultInspector();
		serializedObject.ApplyModifiedProperties();
	}

	private void DrawCheckBoxes () {
		EditorGUILayout.BeginHorizontal( GUILayout.ExpandWidth( true ) );
		float width = ((EditorGUIUtility.currentViewWidth - 20) / 5);
		DrawWideEmptySpace( 10f );
		DisplayCheckbox( "Decimal", decimals, width );
		DisplayCheckbox( "Ones", ones, width );
		DisplayCheckbox( "Tens", tens, width );
		DisplayCheckbox( "Hundreds", hundreds, width );
		DisplayCheckbox( "Thousands", thousands, width );
		DrawWideEmptySpace(10f);
		EditorGUILayout.EndHorizontal();
	}

	private void DrawWideEmptySpace ( float width ) {
		EditorGUILayout.BeginVertical( GUILayout.Width( width ) );

		EditorGUILayout.EndVertical();
	}

	private void DisplayCheckbox ( string label, SerializedProperty property, float width ) {
		EditorGUILayout.BeginVertical( GUILayout.Width( width ) );
		EditorGUILayout.LabelField( label, EditorStyles.boldLabel, GUILayout.Width( width ) );
		property.boolValue = EditorGUILayout.Toggle( property.boolValue, GUILayout.Width( width ), GUILayout.Height( width / 2 ));
		EditorGUILayout.EndVertical();
	}

	private void DrawTimeline ( Rect rect ) {
		EditorGUI.DrawRect( rect, Color.gray );

		for (int i = 0; i <= TimelineEnd; i += 496) {
			float xPos = Mathf.Lerp( rect.x, rect.xMax, (float)(i - TimelineStart) / (TimelineEnd - TimelineStart) );
			EditorGUI.LabelField( new Rect( xPos - 20, rect.y - 20, 40, 20 ), i.ToString(), EditorStyles.centeredGreyMiniLabel );
			Handles.DrawLine( new Vector2( xPos, rect.y ), new Vector2( xPos, rect.yMax ) );
		}

		DrawMainCategoryBar( rect );

		float barHeight = 30f;  // Height of bars
		float spacing = 10f;  // Additional spacing between rows
		float yPos = rect.y + 30;

		if (decimals.boolValue)
			yPos = DrawMasteryBar( decimalMastery, yPos, rect, barHeight );
		if (ones.boolValue)
			yPos = DrawMasteryBar( oneMastery, yPos + spacing, rect, barHeight );
		if (tens.boolValue)
			yPos = DrawMasteryBar( tensMastery, yPos + spacing, rect, barHeight );
		if (hundreds.boolValue)
			yPos = DrawMasteryBar( hundredMastery, yPos + spacing, rect, barHeight );
		if (thousands.boolValue)
			yPos = DrawMasteryBar( thousandsMastery, yPos + spacing, rect, barHeight );
	}

	private void DrawMainCategoryBar ( Rect rect ) {
		float min = Mathf.Min( decimalMastery.FindPropertyRelative( "CategoryStart" ).floatValue,
							   oneMastery.FindPropertyRelative( "CategoryStart" ).floatValue,
							   tensMastery.FindPropertyRelative( "CategoryStart" ).floatValue,
							   hundredMastery.FindPropertyRelative( "CategoryStart" ).floatValue,
							   thousandsMastery.FindPropertyRelative( "CategoryStart" ).floatValue );
		float max = Mathf.Max( decimalMastery.FindPropertyRelative( "CategoryEnd" ).floatValue,
							   oneMastery.FindPropertyRelative( "CategoryEnd" ).floatValue,
							   tensMastery.FindPropertyRelative( "CategoryEnd" ).floatValue,
							   hundredMastery.FindPropertyRelative( "CategoryEnd" ).floatValue,
							   thousandsMastery.FindPropertyRelative( "CategoryEnd" ).floatValue );

		float startX = Mathf.Lerp( rect.x, rect.xMax, (min - TimelineStart) / (TimelineEnd - TimelineStart) );
		float endX = Mathf.Lerp( rect.x, rect.xMax, (max - TimelineStart) / (TimelineEnd - TimelineStart) );
		Rect mathRect = new Rect( startX, rect.y + 10, endX - startX, 20 );
		EditorGUI.DrawRect( mathRect, new Color( 0.6f, 0.6f, 1f, 0.5f ) );
		EditorGUI.LabelField( mathRect, mathCategory.Name, fontStyle );
	}

	private float DrawMasteryBar ( SerializedProperty masteryProperty, float yPos, Rect timelineRect, float barHeight ) {
		float start = Mathf.Clamp( masteryProperty.FindPropertyRelative( "CategoryStart" ).floatValue, TimelineStart, TimelineEnd );
		float end = Mathf.Clamp( masteryProperty.FindPropertyRelative( "CategoryEnd" ).floatValue, TimelineStart, TimelineEnd );

		float barStartX = Mathf.Lerp( timelineRect.x, timelineRect.xMax, (start - TimelineStart) / (TimelineEnd - TimelineStart) );
		float barEndX = Mathf.Lerp( timelineRect.x, timelineRect.xMax, (end - TimelineStart) / (TimelineEnd - TimelineStart) );
		Rect barRect = new Rect( barStartX, yPos, barEndX - barStartX, barHeight );

		EditorGUI.DrawRect( barRect, new Color(0.3f,0.3f,0.3f,1f) );
		EditorGUI.LabelField( barRect, masteryProperty.displayName, fontStyle );

		//HandleDragAndResize( barRect, masteryProperty.FindPropertyRelative( "CategoryStart" ), masteryProperty.FindPropertyRelative( "CategoryEnd" ), timelineRect );

		return yPos + barHeight + 5;
	}

	private void HandleDragAndResize ( Rect barRect, SerializedProperty startProperty, SerializedProperty endProperty, Rect timelineRect ) {
		Event e = Event.current;
		bool isHoveringLeft = e.mousePosition.x >= barRect.x && e.mousePosition.x <= barRect.x + 5;
		bool isHoveringRight = e.mousePosition.x >= barRect.xMax - 5 && e.mousePosition.x <= barRect.xMax;

		if (e.type == EventType.MouseDown && barRect.Contains( e.mousePosition )) {
			dragOffset = e.mousePosition.x;
			isDragging = !isHoveringLeft && !isHoveringRight;
			isResizingStart = isHoveringLeft;
			isResizingEnd = isHoveringRight;

			if (isResizingStart || isResizingEnd || isDragging) {
				GUIUtility.hotControl = GUIUtility.GetControlID( FocusType.Passive );
				e.Use();
			}
		} else if (e.type == EventType.MouseDrag && GUIUtility.hotControl == GUIUtility.GetControlID( FocusType.Passive )) {
			float delta = (e.delta.x / timelineRect.width) * (TimelineEnd - TimelineStart);

			if (isDragging) {
				float barWidth = endProperty.floatValue - startProperty.floatValue;
				startProperty.floatValue = Mathf.Clamp( startProperty.floatValue + delta, TimelineStart, TimelineEnd - barWidth );
				endProperty.floatValue = startProperty.floatValue + barWidth;
			} else if (isResizingStart) {
				startProperty.floatValue = Mathf.Clamp( startProperty.floatValue + delta, TimelineStart, endProperty.floatValue );
			} else if (isResizingEnd) {
				endProperty.floatValue = Mathf.Clamp( endProperty.floatValue + delta, startProperty.floatValue, TimelineEnd );
			}

			e.Use();
		} else if (e.type == EventType.MouseUp && GUIUtility.hotControl == GUIUtility.GetControlID( FocusType.Passive )) {
			isDragging = isResizingStart = isResizingEnd = false;
			GUIUtility.hotControl = 0;
			e.Use();
		}

		// Change cursor if hovering over left or right edge for resizing
		if (isHoveringLeft || isHoveringRight) {
			EditorGUIUtility.AddCursorRect( barRect, MouseCursor.ResizeHorizontal );
		}
	}
}
