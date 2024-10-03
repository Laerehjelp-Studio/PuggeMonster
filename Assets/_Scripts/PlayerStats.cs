using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    public int[] PuggemonArray;
    public SO_PuggeMonsterRegistry puggemonsterList;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        PuggemonArray = new int[puggemonsterList.GetAllPuggeMonsters.Count];
        Debug.Log("The list of monsters contains: " + PuggemonArray.Length + " amount of monsters");
    }
}
