using UnityEditor;

[CustomEditor(typeof(PuggeMonsterGameSettings), true)]
public class PuggeMonsterGameSettingsEditor : Editor
{
    private int selectedGradeIndex = 0;
    private string[] gradeNames;
    PuggeMonsterGameSettings settings;
    private SerializedProperty buildGradeProp;

    private void OnEnable()
    {
        // Get the target object
        settings = (PuggeMonsterGameSettings)target;

        // Reference the SerializedProperty for BuildGrade
        buildGradeProp = serializedObject.FindProperty("_buildGrade");

        // Initialize grade names if grades are available
        if (settings.Grades != null && settings.Grades.Length > 0)
        {
            gradeNames = new string[settings.Grades.Length];
            for (int i = 0; i < settings.Grades.Length; i++)
            {
                gradeNames[i] = settings.Grades[i].name; // Assumes Grade class has a 'name' property
            }

            // Set selectedGradeIndex based on the current BuildGrade
            selectedGradeIndex = System.Array.IndexOf(settings.Grades, settings.BuildGrade);
            if (selectedGradeIndex < 0) selectedGradeIndex = 0; // Default to the first grade if not found
        }
        else
        {
            gradeNames = new string[] { "No Grades Available" };
        }
    }

    public override void OnInspectorGUI()
    {
        // Draw the default inspector
        DrawDefaultInspector();

        // Dropdown for grades
        EditorGUILayout.LabelField("Select Grade used in Build", EditorStyles.boldLabel);
        int newSelectedGradeIndex = EditorGUILayout.Popup(selectedGradeIndex, gradeNames);

        // Update the BuildGrade if a new selection is made
        if (newSelectedGradeIndex != selectedGradeIndex)
        {
            selectedGradeIndex = newSelectedGradeIndex;
            buildGradeProp.objectReferenceValue = settings.Grades[selectedGradeIndex];
            
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(settings); // Mark the ScriptableObject as dirty to save the change
        }
    }
}