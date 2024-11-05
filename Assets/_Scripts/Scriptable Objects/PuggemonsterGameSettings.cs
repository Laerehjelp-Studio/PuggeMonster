using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/PuggeMonster Game Settings", fileName = "PuggeMonster Game Settings" )]
public class PuggeMonsterGameSettings: ScriptableObject
{
	[Header("General Mastery")]
	public float WhenIsMasteryAchieved = 5;
	[Header("Task Limits")]
	public float RecievePuggemonsterLimit = 10;
	public int QuestionSetSize = 4;
	[Header( "Grades" )]
	public Grade BuildGrade;
}
