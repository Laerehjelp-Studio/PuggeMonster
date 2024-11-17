using System.Collections.Generic;
using UnityEngine;

public static class WordGenerator 
{
	public static WordTask GenerateWordQuestion()
	{
		WordTask task = new();
		
		WordPictureQuestionPair wp = WordQuestionLibrary.Instance.GetWordAndSprite();
		task.Correct = wp.Word;
		task.TaskSprite = wp.Picture;

		Sprite[] tempSpriteArray = new Sprite[2];
		tempSpriteArray[0] = wp.Picture;

        task.Incorrect = new();
		task.Incorrect.Add(WordQuestionLibrary.Instance.GetInCorrectWord(tempSpriteArray));
		tempSpriteArray[1] = WordQuestionLibrary.GetSpriteFromValue(task.Incorrect[0]);
        task.Incorrect.Add(WordQuestionLibrary.Instance.GetInCorrectWord(tempSpriteArray));
        return task;
	}

	public static WordTask GenerateWordQuestionBasedOnPerformance(ref WordTask task) {
		if (task.DifficultyLetter == default) {
			task.DifficultyLetter = 'e';
		}

		UpdateTaskBasedOnGeneralMasteryUnlock(ref task, StatManager.GeneralWordMastery, GameManager.SelectedGrade );

		switch (task.DifficultyLetter) {
			case 'e':
				task.DifficultyLevelStringValue = "Easy";
				break;
			case 'm':
				task.DifficultyLevelStringValue = "Medium";
				break;
			case 'h':
				task.DifficultyLevelStringValue = "Hard";
				break;
		}

		return task;
	}

	private static void UpdateTaskBasedOnGeneralMasteryUnlock(ref WordTask task, float generalWordMastery, Grade selectedGrade) {
		List<string> tempList = StatManager.GetWordDifficultyList(task.DifficultyLetter);
		
		WordPictureQuestionPair wp = WordQuestionLibrary.Instance.GetWordAndSprite(tempList);
		task.Correct = wp.Word;
		task.TaskSprite = wp.Picture;

		Sprite[] tempSpriteArray = new Sprite[2];
		tempSpriteArray[0] = wp.Picture;

		task.Incorrect = new();
		task.Incorrect.Add(WordQuestionLibrary.Instance.GetInCorrectWord(tempSpriteArray));
		tempSpriteArray[1] = WordQuestionLibrary.GetSpriteFromValue(task.Incorrect[0]);
		task.Incorrect.Add(WordQuestionLibrary.Instance.GetInCorrectWord(tempSpriteArray));
	}
}