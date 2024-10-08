using UnityEngine;
using AYellowpaper.SerializedCollections;

public class MonsterIndexLibrary : MonoBehaviour
{
    public static MonsterIndexLibrary Instance { get; private set; }
    private void Awake()
    {
		if (Instance == default) {
			Instance = this;
		}
	}

    [SerializedDictionary("Index", "PuggeMonAsset")]
    public SerializedDictionary<int, PuggeMonster> monsterDictionary = new();
    public PuggeMonster GetMonsterFromIndex(int indexVal)
    {
        if (MonsterIndexLibrary.Instance.monsterDictionary.ContainsKey(indexVal))
        {
            return MonsterIndexLibrary.Instance.monsterDictionary[indexVal];
        }
        Debug.LogWarning("Trying to access a monster that does not exist!");

        return null;
    }
}
