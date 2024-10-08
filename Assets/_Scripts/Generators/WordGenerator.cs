using AYellowpaper.SerializedCollections;
using UnityEngine;

public static class WordGenerator 
{
	public static WordTask GenerateWordQuestion()
	{
		WordTask task = new();
		WordQuestionPair wp = WordQuestionLibrary.Instance.GetWordAndSprite();
		task.Correct = wp.Word;
		task.WordSprite = wp.Picture;

		Sprite[] tempSpriteArray = new Sprite[2];
		tempSpriteArray[0] = wp.Picture;



        task.Incorrect = new();
		task.Incorrect.Add(WordQuestionLibrary.Instance.GetInCorrectWord(tempSpriteArray));
		tempSpriteArray[1] = WordQuestionLibrary.GetSpriteFromValue(task.Incorrect[0]);
        task.Incorrect.Add(WordQuestionLibrary.Instance.GetInCorrectWord(tempSpriteArray));
        return task;
	}
}