using UnityEditor;
using UnityEngine;

[CustomEditor( typeof( MathCategory ) )]
public class MathCategoryEditor : Editor {
	private SerializedProperty decimals, ones, tens, hundreds, thousands;
	private SerializedProperty decimalMastery, oneMastery, tensMastery, hundredMastery, thousandsMastery;
	private const int TimelineStart = 0;
	private const int TimelineEnd = 1984;
	private MathCategory mathCategory;
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
		DrawCheckBoxes();

		// Increase the height of the timeline window to fit all bars
		Rect timelineRect = GUILayoutUtility.GetRect( 0, 6 * (30 + 20), GUILayout.ExpandWidth( true ) );
		DrawTimeline( timelineRect );
		
		// Attempt at updating to propagate the settings graph up the chain of command.
		//MathCategoryGraphDrawer.Initialise( mathCategory, serializedObject );
		//MathCategoryGraphDrawer.DrawGraph( serializedObject);
		
		DrawDefaultInspector();
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

		if (decimals.boolValue) {
			yPos = DrawMasteryBar( decimalMastery, yPos, rect, barHeight );
		}
		if (ones.boolValue) {
			yPos = DrawMasteryBar( oneMastery, yPos + spacing, rect, barHeight );
		}
		if (tens.boolValue){
			yPos = DrawMasteryBar( tensMastery, yPos + spacing, rect, barHeight );
		}
		if (hundreds.boolValue) {
			yPos = DrawMasteryBar( hundredMastery, yPos + spacing, rect, barHeight );
		}
		if (thousands.boolValue) {
			yPos = DrawMasteryBar( thousandsMastery, yPos + spacing, rect, barHeight );
		}
	}

	private void DrawMainCategoryBar ( Rect rect ) {
		int min = Mathf.Clamp( MaxMin("min"),0,1984);
		int max = Mathf.Clamp( MaxMin("max"),0,1984);
		
		mathCategory.CategoryStart = min;
		mathCategory.CategoryEnd = max;

		float startX = Mathf.Lerp( rect.x, rect.xMax, ((float)min - TimelineStart) / (TimelineEnd - TimelineStart) );
		float endX = Mathf.Lerp( rect.x, rect.xMax, ((float)max - TimelineStart) / (TimelineEnd - TimelineStart) );
		Rect mathRect = new Rect( startX, rect.y + 10, endX - startX, 20 );
		EditorGUI.DrawRect( mathRect, new Color( 0.3f, 0.5f, 0.3f, 1f ) );
		EditorGUI.LabelField( mathRect, mathCategory.Name, fontStyle );
	}

	private int MaxMin(string mode = "min") {
		int value = -1;

		switch (mode) {
			case "min":
				value = int.MaxValue;
				if (decimals.boolValue) {
					value = Mathf.Min(decimalMastery.FindPropertyRelative("CategoryStart").intValue, value);
				}

				if (ones.boolValue) {
					value = Mathf.Min(oneMastery.FindPropertyRelative("CategoryStart").intValue, value);
				}

				if (tens.boolValue) {
					value = Mathf.Min(tensMastery.FindPropertyRelative("CategoryStart").intValue, value);
				}

				if (hundreds.boolValue) {
					value = Mathf.Min(hundredMastery.FindPropertyRelative("CategoryStart").intValue, value);
				}

				if (thousands.boolValue) {
					value = Mathf.Min(thousandsMastery.FindPropertyRelative("CategoryStart").intValue, value);
				}

				break;
			case "max":
				value = int.MinValue;
				if (decimals.boolValue) {
					value = Mathf.Max(decimalMastery.FindPropertyRelative("CategoryEnd").intValue, value);
				}

				if (ones.boolValue) {
					value = Mathf.Max(oneMastery.FindPropertyRelative("CategoryEnd").intValue, value);
				}

				if (tens.boolValue) {
					value = Mathf.Max(tensMastery.FindPropertyRelative("CategoryEnd").intValue, value);
				}

				if (hundreds.boolValue) {
					value = Mathf.Max(hundredMastery.FindPropertyRelative("CategoryEnd").intValue, value);
				}

				if (thousands.boolValue) {
					value = Mathf.Max(thousandsMastery.FindPropertyRelative("CategoryEnd").intValue, value);
				}
				break;
		}
		return value;
	}

	private float DrawMasteryBar ( SerializedProperty masteryProperty, float yPos, Rect timelineRect, float barHeight ) {
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

	private void HandleDragAndResize(Rect barRect, SerializedProperty startProperty, SerializedProperty endProperty, Rect timelineRect) {
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
}
