using System.Collections.Generic;
using UnityEngine;

public static class LetterGenerator {
	public static LetterTask GenerateLetterQuestion () {
		LetterTask task = new();
		WordPictureQuestionPair wp = WordQuestionLibrary.Instance.GetWordAndSprite();
		task.Correct = wp.Word.Substring(0,1);
		task.TaskSprite = wp.Picture;

		Sprite[] tempSpriteArray = new Sprite[2];
		tempSpriteArray[0] = wp.Picture;

		task.Incorrect = new();
		task.Incorrect.Add(WordQuestionLibrary.Instance.GetInCorrectWord(tempSpriteArray).Substring(0,1));
		tempSpriteArray[1] = WordQuestionLibrary.GetSpriteFromValue(task.Incorrect[0]);
		task.Incorrect.Add(WordQuestionLibrary.Instance.GetInCorrectWord(tempSpriteArray).Substring(0,1));
		return task;
	}
	
	public static LetterTask GenerateWordQuestionBasedOnPerformance(ref LetterTask task) {
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
	private static void UpdateTaskBasedOnGeneralMasteryUnlock(ref LetterTask task, float generalWordMastery, Grade selectedGrade) {
		List<string> tempList = StatManager.GetLetterDifficultyList(task.DifficultyLetter, LetterMode.Picture );
		
		WordPictureQuestionPair wp = WordQuestionLibrary.Instance.GetWordAndSprite();
		task.Correct = wp.Word.Substring(0,1);
		task.LetterSound = LetterSoundQuestionLibrary.GetSoundFromValue(task.Correct);
		task.TaskSprite = wp.Picture;
		task.StorageKey = wp.Word;

		task.Incorrect = new();
		List<string> blocklist = new();
		blocklist.Add(task.StorageKey.Substring(0, 1));
		
		task.Incorrect.Add( LetterSoundQuestionLibrary.GetInCorrectLetter( blocklist ));
		if (task.Incorrect[0] != null) {
			blocklist.Add(task.Incorrect[0].Substring(0, 1));
		}

		task.Incorrect.Add(LetterSoundQuestionLibrary.GetInCorrectLetter( blocklist ) );
	}
}