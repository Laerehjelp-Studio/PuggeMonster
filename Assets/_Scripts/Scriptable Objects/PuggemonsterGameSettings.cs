using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Scriptable Objects/PuggeMonster Game Settings", fileName = "PuggeMonster Game Settings" )]
public class PuggeMonsterGameSettings: ScriptableObject
{
	[Header("General Mastery")]
	public float WhenIsMasteryAchieved = 5;
	[Header("Task Limits")]
	public float RecievePuggemonsterLimit = 10;
	public int QuestionSetSize = 4;
	public float QuestionSpamTimeLimitInMS = 800f;
	[Header( "Grades" )]
	public Grade BuildGrade;
	[Header("Debug")]
	public bool DeveloperMode = true;
	
	[Header( "UX Audio Events" )]
	public AudioEvent ButtonClickSound;
	public AudioEvent CorrectAnswerSound;
	public AudioEvent WrongAnswerSound;
	public AudioEvent PuggeMonsterAppearedSound;
}
