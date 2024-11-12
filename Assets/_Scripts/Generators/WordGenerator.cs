using System.Collections.Generic;
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

	public static WordTask GenerateWordQuestionBasedOnPerformance(ref WordTask task) {
		if (task.difficultyLetter == default) {
			task.difficultyLetter = 'e';
		}

		UpdateTaskBasedOnGeneralMasteryUnlock(ref task, StatManager.GeneralWordMastery, GameManager.SelectedGrade );

		switch (task.difficultyLetter) {
			case 'e':
				task.difficultyLevelStringValue = "Easy";
				break;
			case 'm':
				task.difficultyLevelStringValue = "Medium";
				break;
			case 'h':
				task.difficultyLevelStringValue = "Hard";
				break;
		}

		return task;
	}

	private static void UpdateTaskBasedOnGeneralMasteryUnlock(ref WordTask task, float generalWordMastery, Grade selectedGrade) {
		
		
		List<string> tempList = StatManager.GetWordDifficultyList(task.difficultyLetter);
		
		WordQuestionPair wp = WordQuestionLibrary.Instance.GetWordAndSprite(tempList);
		task.Correct = wp.Word;
		task.WordSprite = wp.Picture;

		Sprite[] tempSpriteArray = new Sprite[2];
		tempSpriteArray[0] = wp.Picture;

		task.Incorrect = new();
		task.Incorrect.Add(WordQuestionLibrary.Instance.GetInCorrectWord(tempSpriteArray));
		tempSpriteArray[1] = WordQuestionLibrary.GetSpriteFromValue(task.Incorrect[0]);
		task.Incorrect.Add(WordQuestionLibrary.Instance.GetInCorrectWord(tempSpriteArray));
	}
}