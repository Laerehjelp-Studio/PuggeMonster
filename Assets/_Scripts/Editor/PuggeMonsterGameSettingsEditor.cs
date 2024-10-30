using UnityEngine;
using UnityEditor;

[CustomEditor( typeof( PuggeMonsterGameSettings ),true )]
public class PuggeMonsterGameSettingsEditor : Editor {
	private int selectedGradeIndex = 0;
	private string[] gradeNames;
	PuggeMonsterGameSettings settings;
	private void OnEnable () {
		// Get the target object
		settings = (PuggeMonsterGameSettings)target;

		// Initialize grade names if grades are available
		if (settings.Grades != null && settings.Grades.Length > 0) {
			gradeNames = new string[ settings.Grades.Length ];
			for (int i = 0; i < settings.Grades.Length; i++) {
				gradeNames[ i ] = settings.Grades[ i ].name; // Assumes Grade class has a 'name' property
			}
		} else {
			gradeNames = new string[] { "No Grades Available" };
		}
	}

	public override void OnInspectorGUI () {
		// Draw the default inspector
		DrawDefaultInspector();

		// Dropdown for grades
		EditorGUILayout.LabelField( "Select Grade used in Build", EditorStyles.boldLabel );
		selectedGradeIndex = EditorGUILayout.Popup( selectedGradeIndex, gradeNames );

		// Optionally, add any additional logic here if you want to do something with the selected grade
		if (selectedGradeIndex < settings.Grades.Length) {  
			settings.BuildGrade = settings.Grades[ selectedGradeIndex ];
			serializedObject.ApplyModifiedProperties();
		}
		
	}
}
