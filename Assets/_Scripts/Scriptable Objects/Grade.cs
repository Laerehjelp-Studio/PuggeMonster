using UnityEngine;
[CreateAssetMenu( menuName = "Scriptable Objects/Grade", fileName = "Grade 001" )]
public class Grade : ScriptableObject {
	public string Name;
	public Subject[] Subjects;
}
