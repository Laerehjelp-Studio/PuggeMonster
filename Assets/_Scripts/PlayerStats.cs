using UnityEngine;

public class PlayerStats : MonoBehaviour
{
	public static PlayerStats Instance { get; private set; }

	public int[] PuggemonArray;
	public SO_PuggeMonsterRegistry puggemonsterList;

	private void Awake()
	{
		if (Instance == default) {
			PuggemonArray = new int[ puggemonsterList.GetAllPuggeMonsters.Count ];
			Debug.Log( "The list of monsters contains: " + PuggemonArray.Length + " amount of monsters" );
		
			Instance = this;
		}
	}

	public void AddPuggeMonster(int monsterIndex) {
		PuggemonArray[monsterIndex]++;
		GameManager.SaveGame();
	}
}
