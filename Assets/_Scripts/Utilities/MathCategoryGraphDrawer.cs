using UnityEditor;
using UnityEngine;

public static class MathCategoryGraphDrawer
{
	/*
	private const int TimelineStart = 0;
	private const int TimelineEnd = 1984;
	private static bool _initialized;

	private static SerializedProperty decimals, ones, tens, hundreds, thousands;
	private static SerializedProperty decimalMastery, oneMastery, tensMastery, hundredMastery, thousandsMastery;
	private static MathCategory mathCategory;
	private static bool isDragging;
	private static bool isResizingStart;
	private static bool isResizingEnd;
	private static GUIStyle fontStyle;
	private static SerializedObject _serializedObject;
	private static MathCategory _mathCategory;
	
	public static void Initialise(MathCategory target, SerializedObject serializedObject) {
		if (_initialized) {
			return;
		}
		_serializedObject = serializedObject;
		_mathCategory = target;

		decimals = _serializedObject.FindProperty( "Decimals" );
		ones = _serializedObject.FindProperty( "Ones" );
		tens = _serializedObject.FindProperty( "Tens" );
		hundreds = _serializedObject.FindProperty( "Hundreds" );
		thousands = _serializedObject.FindProperty( "Thousands" );

		decimalMastery = _serializedObject.FindProperty( "DecimalMastery" );
		oneMastery = _serializedObject.FindProperty( "OneMastery" );
		tensMastery = _serializedObject.FindProperty( "TensMastery" );
		hundredMastery = _serializedObject.FindProperty( "HundredMastery" );
		thousandsMastery = _serializedObject.FindProperty( "ThousandsMastery" );
		_initialized = true;
	}
	
	public static void DrawGraph (SerializedObject serializedObject) {
		if (_serializedObject == null && serializedObject != null) {
			_serializedObject = serializedObject;
		}
		
		if (_serializedObject == default) {
			Debug.LogError("Missing _serializedObject: Please run 'MathCategoryGraphDrawer.Initialise' before using 'MathGraphDrawer.DrawGraph'.");
			return;
		}
		
		_serializedObject.Update();
		fontStyle = new GUIStyle( GUI.skin.label ) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, wordWrap = true };
		DrawCheckBoxes();

		// Increase the height of the timeline window to fit all bars
		Rect timelineRect = GUILayoutUtility.GetRect( 0, 6 * (30 + 20), GUILayout.ExpandWidth( true ) );
		DrawTimeline( timelineRect );

		_serializedObject.ApplyModifiedProperties();
	}
	
	private static void DrawCheckBoxes () {
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

	private static void DrawWideEmptySpace ( float width ) {
		EditorGUILayout.BeginVertical( GUILayout.Width( width ) );

		EditorGUILayout.EndVertical();
	}

	private static void DisplayCheckbox ( string label, SerializedProperty property, float width ) {
		EditorGUILayout.BeginVertical( GUILayout.Width( width ) );
		EditorGUILayout.LabelField( label, EditorStyles.boldLabel, GUILayout.Width( width ) );
		property.boolValue = EditorGUILayout.Toggle( property.boolValue, GUILayout.Width( width ), GUILayout.Height( width / 2 ));
		EditorGUILayout.EndVertical();
	}
	private static void DrawTimeline ( Rect rect ) {
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

		if (decimals.boolValue) {
			yPos = DrawMasteryBar( decimalMastery, yPos, rect, barHeight );
		}
		if (ones.boolValue) {
			yPos = DrawMasteryBar( oneMastery, yPos + spacing, rect, barHeight );
		}
		if (tens.boolValue){
			yPos = DrawMasteryBar(tensMastery, yPos + spacing, rect, barHeight );
		}
		if (hundreds.boolValue) {
			yPos = DrawMasteryBar(hundredMastery, yPos + spacing, rect, barHeight );
		}
		if (thousands.boolValue) {
			yPos = DrawMasteryBar(thousandsMastery, yPos + spacing, rect, barHeight );
		}
	}
	
	private static void DrawMainCategoryBar ( Rect rect ) {
		int min = Mathf.Clamp( Mathf.Min( decimalMastery.FindPropertyRelative( "CategoryStart" ).intValue,
			oneMastery.FindPropertyRelative( "CategoryStart" ).intValue,
			tensMastery.FindPropertyRelative( "CategoryStart" ).intValue,
			hundredMastery.FindPropertyRelative( "CategoryStart" ).intValue,
			thousandsMastery.FindPropertyRelative( "CategoryStart" ).intValue ),0,1984);
		int max = Mathf.Clamp( Mathf.Max( decimalMastery.FindPropertyRelative( "CategoryEnd" ).intValue,
			oneMastery.FindPropertyRelative( "CategoryEnd" ).intValue,
			tensMastery.FindPropertyRelative( "CategoryEnd" ).intValue,
			hundredMastery.FindPropertyRelative( "CategoryEnd" ).intValue,
			thousandsMastery.FindPropertyRelative( "CategoryEnd" ).intValue ),0,1984);
		
		_mathCategory.CategoryStart = min;
		_mathCategory.CategoryEnd = max;

		float startX = Mathf.Lerp( rect.x, rect.xMax, ((float)min - TimelineStart) / (TimelineEnd - TimelineStart) );
		float endX = Mathf.Lerp( rect.x, rect.xMax, ((float)max - TimelineStart) / (TimelineEnd - TimelineStart) );
		Rect mathRect = new Rect( startX, rect.y + 10, endX - startX, 20 );
		EditorGUI.DrawRect( mathRect, new Color( 0.3f, 0.5f, 0.3f, 1f ) );
		EditorGUI.LabelField( mathRect, _mathCategory.Name, fontStyle );
	}
	
	private static float DrawMasteryBar(SerializedProperty masteryProperty, float yPos, Rect timelineRect, float barHeight)
	{
		float start = Mathf.Clamp( masteryProperty.FindPropertyRelative( "CategoryStart" ).intValue, TimelineStart, TimelineEnd );
		float end = Mathf.Clamp( masteryProperty.FindPropertyRelative( "CategoryEnd" ).intValue, TimelineStart, TimelineEnd );

		float barStartX = Mathf.Lerp( timelineRect.x, timelineRect.xMax, (start - TimelineStart) / (TimelineEnd - TimelineStart) );
		float barEndX = Mathf.Lerp( timelineRect.x, timelineRect.xMax, (end - TimelineStart) / (TimelineEnd - TimelineStart) );
		Rect barRect = new Rect( barStartX, yPos, barEndX - barStartX, barHeight );

		EditorGUI.DrawRect( barRect, new Color(0.3f,0.3f,0.3f,1f) );
		EditorGUI.LabelField( barRect, masteryProperty.displayName.Split(" " )[0], fontStyle );

		HandleDragAndResize( barRect, masteryProperty.FindPropertyRelative( "CategoryStart" ), masteryProperty.FindPropertyRelative( "CategoryEnd" ), timelineRect );
		return yPos + barHeight + 5;
	}
	private static void HandleDragAndResize(Rect barRect, SerializedProperty startProperty, SerializedProperty endProperty, Rect timelineRect) {
	    Event e = Event.current;
	    int controlID = GUIUtility.GetControlID(FocusType.Passive);
	    int edgeOffset = 10;
	    
	    bool isHoveringLeft = e.mousePosition.x > barRect.x - edgeOffset && e.mousePosition.x < barRect.x + edgeOffset;
	    bool isHoveringRight = e.mousePosition.x > barRect.xMax - edgeOffset && e.mousePosition.x < barRect.xMax + edgeOffset;
	    
	    if (e.type == EventType.MouseDown && barRect.Contains(e.mousePosition)) {
	        isDragging = !isHoveringLeft && !isHoveringRight;
	        isResizingStart = isHoveringLeft;
	        isResizingEnd = isHoveringRight;

	        if (isResizingStart || isResizingEnd || isDragging) {
	            GUIUtility.hotControl = controlID;
	            e.Use();
	        }
	    } else if (e.type == EventType.MouseDrag && GUIUtility.hotControl == controlID) {
	        int delta = (int)((e.delta.x / timelineRect.width) * (TimelineEnd - TimelineStart));

	        if (isDragging) {
	            int barWidth = endProperty.intValue - startProperty.intValue;
	            startProperty.intValue = Mathf.Clamp(startProperty.intValue + delta, TimelineStart, TimelineEnd - barWidth);
	            endProperty.intValue = startProperty.intValue + barWidth;
	        } else if (isResizingStart) {
	            startProperty.intValue = Mathf.Clamp(startProperty.intValue + delta, TimelineStart, endProperty.intValue);
	        } else if (isResizingEnd) {
	            endProperty.intValue = Mathf.Clamp(endProperty.intValue + delta, startProperty.intValue, TimelineEnd);
	        }

	        // Apply the changes to the serialized object to reflect the update
	        startProperty.serializedObject.ApplyModifiedProperties();
	        endProperty.serializedObject.ApplyModifiedProperties();

	        e.Use();
	    } else if (e.type == EventType.MouseUp && GUIUtility.hotControl == controlID) {
	        isDragging = isResizingStart = isResizingEnd = false;
	        GUIUtility.hotControl = 0;
	        e.Use();
	    }

	    // Update cursor for edge-hover resize
	    if (isHoveringLeft || isHoveringRight) {
	        EditorGUIUtility.AddCursorRect(barRect, MouseCursor.ResizeHorizontal);
	    }

	    // Debugging: log to check positions and actions
	    //Debug.Log($"Updated startProperty: {startProperty.intValue}, endProperty: {endProperty.intValue}");
	}
*/
}