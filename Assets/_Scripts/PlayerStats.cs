using UnityEngine;
using Newtonsoft.Json;
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

	private void OnEnable() {
		GameManager.OnGameSave += SavePuggeMonster;
		GameManager.OnGameLoad += LoadPuggeMonster;
		GameManager.OnClearSaveGame += ClearPuggeMonster;
	}

	private void OnDisable() {
		GameManager.OnGameSave -= SavePuggeMonster;
		GameManager.OnGameLoad -= LoadPuggeMonster;
		GameManager.OnClearSaveGame -= ClearPuggeMonster;
	}

	private void ClearPuggeMonster() {
		if (PlayerPrefs.HasKey("PuggemonArray")) {
			PlayerPrefs.DeleteKey("PuggemonArray");
		}
	}

	private void LoadPuggeMonster() {
		if (PlayerPrefs.HasKey("PuggemonArray")) {
			string json = PlayerPrefs.GetString("PuggemonArray");
			PuggemonArray = JsonConvert.DeserializeObject<int[]>(json);
			Debug.Log("Saved PuggeMonsters loaded successfully.");
		}
	}

	private void SavePuggeMonster() {
		PlayerPrefs.SetString("PuggemonArray", JsonConvert.SerializeObject(PuggemonArray));
	}

	public void AddPuggeMonster(int monsterIndex) {
		PuggemonArray[monsterIndex]++;
		GameManager.SaveGame();
	}
}
