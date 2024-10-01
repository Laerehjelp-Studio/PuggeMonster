using UnityEngine;
using UnityEngine.Rendering;

public class MonsterIndexLibrary : MonoBehaviour
{
    public static MonsterIndexLibrary Instance { get; private set; }

    [SerializedDictionary("Index", "PuggeMonAsset")]
    public static SerializedDictionary<int, PuggeMonster> monsterDictionary = new();
    public static PuggeMonster GetMonsterFromIndex(int indexVal)
    {
        if (monsterDictionary.ContainsKey(indexVal))
        {
            return monsterDictionary[indexVal];
        }
        Debug.LogWarning("Trying to access a monster that does not exist!");
        return null;
    }
}
