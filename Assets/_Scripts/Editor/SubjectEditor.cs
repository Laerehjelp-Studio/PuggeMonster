using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Subject))]
public class SubjectEditor : Editor {
	private SerializedProperty mathCategories;

	private void OnEnable() {
		mathCategories = serializedObject.FindProperty("MathCategories"); // Assuming array is named "mathCategories"
	}

	public override void OnInspectorGUI() {
		serializedObject.Update();

		// Define a rect to contain the graph
		// Rect timelineRect = GUILayoutUtility.GetRect(0, mathCategories.arraySize * (30 + 20), GUILayout.ExpandWidth(true));
		// for (int i = 0; i < mathCategories.arraySize; i++) {
		// 	MathCategory mathCategory = mathCategories.GetArrayElementAtIndex(i).objectReferenceValue as MathCategory;
		// 	SerializedObject _serializedObject = mathCategories.GetArrayElementAtIndex(i).serializedObject;
		// 	MathCategoryGraphDrawer.Initialise(mathCategory, serializedObject);
		// 	MathCategoryGraphDrawer.DrawGraph();
		// }
		DrawDefaultInspector();

		serializedObject.ApplyModifiedProperties();
	}
}
