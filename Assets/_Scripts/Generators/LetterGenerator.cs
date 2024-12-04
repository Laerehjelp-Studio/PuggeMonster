using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public static class LetterGenerator {
	public static LetterTask GenerateQuestionBasedOnPerformance(ref LetterTask task, LetterCode letterCode = new()) {
		if (task.DifficultyLetter == default) {
			task.DifficultyLetter = 'm';
		}
		
		UpdateTaskBasedOnGeneralMasteryUnlock(ref task, StatManager.GeneralWordMastery, GameManager.SelectedGrade, letterCode );

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
	private static void UpdateTaskBasedOnGeneralMasteryUnlock(ref LetterTask task, float generalWordMastery, Grade selectedGrade, LetterCode letterCode) {
		List<string> letterDifficultyList = StatManager.GetLetterDifficultyList(task.DifficultyLetter, LetterMode.Sound);
		List<string> allowedLetters = new();
		
		
		if (task.Mode == GameModeType.LetterPicture) {
			WordPictureQuestionPair wp = WordQuestionLibrary.Instance.GetWordAndSprite();
			task.Correct = wp.Word.Substring(0,1);
			task.TaskSprite = wp.Picture;
			task.StorageKey = wp.Word;
		}

		if (task.Mode == GameModeType.Letters) {
			if (letterCode.IsEmpty) {
				task.Correct = letterDifficultyList[Random.Range(0, letterDifficultyList.Count)];
			} else {
				allowedLetters = letterCode.AllowedLetters;
				task.Correct = allowedLetters[Random.Range(0, allowedLetters.Count)];
			}
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

			if (letterCode.IsEmpty) {
				task.Incorrect.Add(LetterSoundQuestionLibrary.GetInCorrectLetter(letterDifficultyList, blocklist));
				
				if (task.Incorrect[0] != null) {
					blocklist.Add(task.Incorrect[0]);
				}
				
				task.Incorrect.Add(LetterSoundQuestionLibrary.GetInCorrectLetter(letterDifficultyList, blocklist));
			} else {
				task.Incorrect.Add(LetterSoundQuestionLibrary.GetInCorrectLetter(allowedLetters, blocklist));
				
				if (task.Incorrect[0] != null) {
					blocklist.Add(task.Incorrect[0]);
				}

				task.Incorrect.Add(LetterSoundQuestionLibrary.GetInCorrectLetter(allowedLetters, blocklist));
			}
		}
	}
}

public struct LetterCode {
	public List<string> AllowedLetters;
	public bool AppDecides;

	public bool IsEmpty {
		get {
			return AllowedLetters == default || AllowedLetters.Count == 0;
		}
	}
}