using UnityEngine;
using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using static UnityEditor.Progress;

public class WordQuestionLibrary : MonoBehaviour
{
    public static WordQuestionLibrary Instance { get; private set; }

    private Dictionary<Sprite, string> wordTaskLibrary = new();
    [SerializeField] private List<Sprite> spriteList = new();

    private List<string> incorrectWordList = new();

    private void Awake()
    {
        Instance = this;

        // grab the sprites from the list and generate a dictionary with the names stripped from the sprite names.
        for (int i = 0; i < spriteList.Count; i++)
        {
            string temp = spriteList[i].name;
            temp = temp.Replace("WD_", "");
            wordTaskLibrary.Add(spriteList[i], temp);
            incorrectWordList.Add(temp);
        }
    }


    public WordQuestionPair GetWordAndSprite()
    {
        WordQuestionPair var = new WordQuestionPair();
        var.Picture = spriteList[Random.Range(0, spriteList.Count)];
        var.Word = wordTaskLibrary[var.Picture];
        return var;
    }

    public static string GetCorrectWord(Sprite key)
    {
        if (WordQuestionLibrary.Instance.wordTaskLibrary.ContainsKey(key))
        {
            return WordQuestionLibrary.Instance.wordTaskLibrary[key];
        }
        Debug.LogWarning("Trying to access a Sprite that does not exist!");
        return null;
    }


    public static Sprite GetSpriteFromValue(string key)
    {
        if (WordQuestionLibrary.Instance.wordTaskLibrary.ContainsValue(key))
        {
            foreach (var item in WordQuestionLibrary.Instance.wordTaskLibrary)
            {
                if (item.Value == key)
                {

                    return item.Key;
                }
            }
        }
        Debug.LogWarning("Trying to access a Sprite that does not exist!");
        return null;
    }

    public string GetInCorrectWord(Sprite[] blockedSprite)
    {
        List<Sprite> tempSpriteList = spriteList;

        foreach (Sprite item in blockedSprite)
        {
            tempSpriteList.Remove(item);
        }
        if(tempSpriteList.Count > 0)
        {
            return wordTaskLibrary[tempSpriteList[Random.Range(0, tempSpriteList.Count)]];
        }
        return null;
    }
}
public struct WordQuestionPair
{
    public string Word;
    public Sprite Picture;
}
