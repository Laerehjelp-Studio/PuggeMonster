using System.Collections.Generic;
using Random = UnityEngine.Random;

public static class LetterGenerator {
	public static LetterTask GenerateQuestionBasedOnPerformance(ref LetterTask task) {
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
		List<string> letterDifficultyList = StatManager.GetLetterDifficultyList(task.DifficultyLetter, LetterMode.Sound);
		
		if (task.Mode == GameModeType.LetterPicture) {
			WordPictureQuestionPair wp = WordQuestionLibrary.Instance.GetWordAndSprite();
			task.Correct = wp.Word.Substring(0,1);
			task.TaskSprite = wp.Picture;
			task.StorageKey = wp.Word;
		}

		if (task.Mode == GameModeType.Letters) {
			task.Correct = letterDifficultyList[Random.Range(0, letterDifficultyList.Count)];
		}
		
		task.LetterSound = LetterSoundQuestionLibrary.GetSoundFromValue(task.Correct);

		task.Incorrect = new();
		List<string> blocklist = new();
		
		if (task.Mode == GameModeType.LetterPicture) {
			blocklist.Add(task.StorageKey.Substring(0, 1));
		
			task.Incorrect.Add( LetterSoundQuestionLibrary.GetInCorrectLetter(LetterSoundQuestionLibrary.GetWordList, blocklist ));
			if (task.Incorrect[0] != null) {
				blocklist.Add(task.Incorrect[0].Substring(0, 1));
			}

			task.Incorrect.Add(LetterSoundQuestionLibrary.GetInCorrectLetter(LetterSoundQuestionLibrary.GetWordList, blocklist ) );
		}
		
		if (task.Mode == GameModeType.Letters) {
			blocklist.Add(task.Correct);
		
			task.Incorrect.Add( LetterSoundQuestionLibrary.GetInCorrectLetter(letterDifficultyList, blocklist ));
			if (task.Incorrect[0] != null) {
				blocklist.Add(task.Incorrect[0]);
			}

			task.Incorrect.Add(LetterSoundQuestionLibrary.GetInCorrectLetter(letterDifficultyList, blocklist ) );
		}
	}
}