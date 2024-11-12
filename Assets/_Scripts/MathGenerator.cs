using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class MathGenerator
{
	/// <summary>
	/// Generates questions based on Difficulty String.
	/// </summary>
	/// <param name="Difficulty"></param>
	/// <param name="task"></param>
	/// <returns></returns>
	
	 public static MathTask GenerateMathQuestion ( string Difficulty, MathTask task = new()) {
		//public float<> Components; // Array with 2 numbers
		//public string Operator; // + - * or /
		//public float Correct; // The correct answer.
		//public float<> Incorrect; // Incorrect options.
		
		if (task.Operator == default) {
			task.Operator = "+";
		}

		switch (Difficulty) {
			case "e": {
				//   Easy difficulty question
				task.Components = new();
				task.Incorrect = new();
				task.Components.Add( Random.Range( 0, 10 ) );
				task.Components.Add( Random.Range( 0, 10 ) );
				task.difficultyLevelStringValue = "Easy";


				task.Correct = GetMathResult( task );

				task.Incorrect.Add( GetIncorrectWhenOutOfBounds( task.Correct, task.Incorrect, 3, task.Operator ) );
				task.Incorrect.Add( GetIncorrectWhenOutOfBounds( task.Correct, task.Incorrect, 3, task.Operator ) );
			}
			break;
			case "m"://   Medium difficulty question
				{
				task.Components = new();
				task.Incorrect = new();
				task.Components.Add( Random.Range( 10, 31 ) );
				task.Components.Add( Random.Range( 10, 31 ) );
				task.difficultyLevelStringValue = "Medium";

				task.Correct = GetMathResult( task );

				task = AddIncorrectAnswers( task );
			}
			break;
			case "h"://   Hard difficulty question
				{
				task.Components = new();
				task.Incorrect = new();
				task.Components.Add( Random.Range( 20, 101 ) );
				task.Components.Add( Random.Range( 30, 101 ) );
				task.difficultyLevelStringValue = "Hard";

				task.Correct = GetMathResult( task );

				task = AddIncorrectAnswers( task );
			}
			break;
		}
		return task;
	}
	
	/// <summary>
	/// Generates math questions based on MathCode
	/// </summary>
	/// <param name="task"></param>
	/// <param name="mathCode"></param>
	/// <returns></returns>
	public static MathTask GenerateMathQuestion ( MathTask task, MathCode mathCode) {

		if (mathCode.Operator == default && task.Operator == default) {
			task.Operator = GetGeneralMasteryBasedOperator( GameManager.SelectedGrade, StatManager.GeneralMathMastery );
		} else if (mathCode.Operator != default) {
			task.Operator = mathCode.Operator;
		}

		// If Override-code AppDecides or mathCode.IsEmpty we get questions based on student performance.
		if (mathCode.AppDecides || mathCode.IsEmpty ) {
			return GenerateMathQuestionFromStudentPerformance( task, mathCode );
		}

		task.difficultyLevelStringValue = "Kode";

		task.Components.Add( Random.Range( mathCode.Lower, mathCode.Upper ) );
		if (task.Operator == "/") {
			task.Components.Add( GetRandomRangeNotZero(mathCode.Lower, mathCode.Upper) );
		} else {
			task.Components.Add( Random.Range( mathCode.Lower, mathCode.Upper ) );
		}
		task.Correct = GetMathResult( task );

		task = AddIncorrectAnswers( task );

		return task;
	}

	private static float GetRandomRangeNotZero(int mathCodeLower, int mathCodeUpper) {
		int component = Random.Range(mathCodeLower, mathCodeUpper);
		
		if (component == 0) {
			return (float)GetRandomRangeNotZero(mathCodeLower, mathCodeUpper);
		}
		
		return (float)component;
	}

	/// <summary>
	/// Returns an operator based on which are unlocked by General Mastery Score
	/// </summary>
	/// <returns></returns>
	private static string GetGeneralMasteryBasedOperator ( Grade grade, int generalMathMastery ) {
		if (grade == null) {
			Debug.LogError( "No grade selected for build in Game Settings." );
			return null;
		}

		Subject subject = SelectSubject( Subject.Subjects.Math, grade.Subjects );

		if (subject == null) {
			return null;
		}

		MathCategory category = subject.SelectCategoryByGMChance( generalMathMastery );

		if (category == null) {
			return null;
		}

		return category.Operator;
	}

	/// <summary>
	/// Returns float result of math question.
	/// </summary>
	/// <param name="task"></param>
	/// <returns></returns>
	private static float GetMathResult ( MathTask task ) {
		float temp = default;

		switch (task.Operator) {
			case "+":
				temp = task.Components[ 0 ] + task.Components[ 1 ];
				break;
			case "-":
				temp = task.Components[ 0 ] - task.Components[ 1 ];
				break;
			case "*":
				temp = task.Components[ 0 ] * task.Components[ 1 ];
				break;
			case "/":
			case ":":
				temp = task.Components[ 0 ] / task.Components[ 1 ];
				break;
		}

		return temp;
	}

	/// <summary>
	/// This generates questions based on the student's previous performance.
	/// </summary>
	/// <param name="task"></param>
	/// <param name="mathCode"></param>
	/// <returns></returns>
	public static MathTask GenerateMathQuestionFromStudentPerformance (  MathTask task, MathCode mathCode  = new() ) {
		if (task.difficultyLetter == default) {
			task.difficultyLetter = 'e';
		}

		UpdateTaskBasedOnGeneralMasteryUnlock(ref task, StatManager.GeneralMathMastery, GameManager.SelectedGrade );

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

	private static void UpdateTaskBasedOnGeneralMasteryUnlock ( ref MathTask task, int generalMathMastery, Grade grade ) {
		if (grade == null) {
			Debug.LogError( "No grade selected for build in Game Settings." );
			return;
		}

		Subject subject = SelectSubject( Subject.Subjects.Math, grade.Subjects );

		if (subject == null) {
			return;
		}

		MathCategory category = subject.SelectCategoryByGMChance( generalMathMastery );
		//Debug.LogWarning($"Subject: {subject.Name}, Category: {category.Name}, Mastered: {StatManager.GeneralMathMastery}");

		if (category == null) {
			return;
		}

		task.Operator = category.Operator;

		int placementNumberInt = category.SelectGMChancePlacementNumber( generalMathMastery );

		MathDifficultyList mathDifficultyLists = StatManager.GetDifficultyLists( task.Operator, task.difficultyLetter.ToString() );

		// If GetDifficultyLists does not produce a properly formed list, run the oldest (outdated) GenerateMathQuestion function.
		if (mathDifficultyLists.One == default ||
			mathDifficultyLists.Tens == default ||
			mathDifficultyLists.Hundreds == default ||
			mathDifficultyLists.Thousands == default) {

			Debug.LogError( "difficultyLists not properly formed." );
			GenerateMathQuestion( task.difficultyLetter.ToString(), task );
			return;
		}

		string firstComponent = "", secondComponent = "";

		// TODO: Implement decimals.

		GetComponentsFromGeneralMastery(task, placementNumberInt, mathDifficultyLists, ref firstComponent, ref secondComponent);

		//Debug.Log($"[UpdateTaskBasedOnGMUnlock]: {firstComponent}, placementNumberInt: {placementNumberInt}");

		task.Components.Add( float.Parse( firstComponent ) );
		task.Components.Add( float.Parse( secondComponent ) );

		task.Correct = GetMathResult( task );

		task = AddIncorrectAnswers( task );
	}

	private static void GetComponentsFromGeneralMastery(MathTask task, int placementNumberInt, MathDifficultyList mathDifficultyLists, ref string firstComponent, ref string secondComponent) {
		if (placementNumberInt >= 1) {
			GetComponentFromDifficultyList( task, mathDifficultyLists.One, ref firstComponent, ref secondComponent );
		}
		if (placementNumberInt >= 2) {
			GetComponentFromDifficultyList( task, mathDifficultyLists.Tens, ref firstComponent, ref secondComponent );
		}
		if (placementNumberInt >= 3) {
			GetComponentFromDifficultyList( task, mathDifficultyLists.Hundreds, ref firstComponent, ref secondComponent );
		}
		if (placementNumberInt >= 4) {
			GetComponentFromDifficultyList( task, mathDifficultyLists.Thousands, ref firstComponent, ref secondComponent );
		}

		if (task.Operator == "/" && int.TryParse(secondComponent, out int result) && result == 0) {
			firstComponent = "";
			secondComponent = "";
			GetComponentsFromGeneralMastery(task, placementNumberInt, mathDifficultyLists, ref firstComponent, ref secondComponent);
		}
	}

	/// <summary>
	/// Selects a subject
	/// </summary>
	/// <param name="subjectType"></param>
	/// <param name="subjects"></param>
	/// <returns></returns>
	private static Subject SelectSubject ( Subject.Subjects subjectType, Subject[] subjects ) {
		int subjectsLength = subjects.Length;
		if (subjects == Array.Empty<Subject>() || subjectsLength == 0) {
			Debug.LogError("No subjects added to Grade, please add at least one subject to Grade.");
			return null;
		}
		
		Subject tempSubject = ScriptableObject.CreateInstance<Subject>();
		
		foreach (Subject subject in subjects) {
			if (subject.SubjectType == subjectType) {
				tempSubject = subject;
				break;
			}
		}
		
		if (tempSubject != default) {
			return tempSubject;
		}

		Debug.LogError($"No valid match for subject type: {subjectType} was found.");
		return null;
	}

	/// <summary>
	/// Gets components from the difficulty list provided, and adds it to the front of the referred firstComponent, and secondComponent.
	/// </summary>
	/// <param name="task"></param>
	/// <param name="difficultyLists"></param>
	/// <param name="firstComponent"></param>
	/// <param name="secondComponent"></param>
	private static void GetComponentFromDifficultyList ( MathTask task, List<string> difficultyLists, ref string firstComponent, ref string secondComponent ) {
		string[] tempPair = difficultyLists[ Random.Range( 0, difficultyLists.Count ) ].Split( task.Operator );

		firstComponent = $"{tempPair[ 0 ]}{firstComponent}";
		secondComponent = $"{tempPair[ 1 ]}{secondComponent}";
	}

	/// <summary>
	/// Accepts and Returns a MathTask, the returned mathTask should have task.Incorrect added to it twice.
	/// </summary>
	/// <param name="task"></param>
	/// <returns></returns>
	private static MathTask AddIncorrectAnswers ( MathTask task ) {
		float temp = task.Correct;

		//int lastDigitInAnswer; 
		int lastDigitInOption1, lastDigitInOption2;

		string tempString;
		//tempString = "" + temp;
		//tempString = tempString[ tempString.Length - 1 ].ToString();
		//lastDigitInAnswer = Int32.Parse( tempString );

		tempString = "" + task.Components[ 0 ];
		tempString = tempString[ tempString.Length - 1 ].ToString();
		lastDigitInOption1 = Int32.Parse( tempString );

		tempString = "" + task.Components[ 1 ];
		tempString = tempString[ tempString.Length - 1 ].ToString();
		lastDigitInOption2 = Int32.Parse( tempString );

		if (lastDigitInOption1 + lastDigitInOption2 > 9  || temp < 20) { // TODO: Should probably also trigger when going below zero
			task.Incorrect.Add( GetIncorrectWhenOutOfBounds( temp, task.Incorrect, 3, task.Operator ) );
			task.Incorrect.Add( GetIncorrectWhenOutOfBounds( temp, task.Incorrect, 3, task.Operator ) );
		} else {
			int tempOptions = Random.Range( 0, 3 );
			switch (tempOptions) {
				case 0:
					task.Incorrect.Add( temp + 10 );
					task.Incorrect.Add( temp - 10 );
					break;
				case 1:
					task.Incorrect.Add( temp + 10 );
					task.Incorrect.Add( temp + 20 );
					break;
				case 2:
					task.Incorrect.Add( temp - 10 );
					task.Incorrect.Add( temp - 20 );
					break;
			}
		}
		return task;
	}

	/// <summary>
	/// Creates an incorrect answer using float correct, list float Incorrect, and int range.
	/// </summary>
	/// <param name="correct"></param>
	/// <param name="Incorrect"></param>
	/// <param name="range"></param>
	/// <param name="Operator"></param>
	/// <returns></returns>
	private static float GetIncorrectWhenOutOfBounds (float correct, List<float> Incorrect, int range = 5, string Operator = "") {
		Incorrect ??= new();

		int modifier =  Random.Range( -1 * range, range );

		if (modifier == 0) {
			modifier += 1;
		}
		
		float currentIncorrect = modifier + correct;

		if (Mathf.Approximately(currentIncorrect, correct) || Incorrect.Contains( currentIncorrect ) || currentIncorrect < 0 && Operator == "+") {
			return GetIncorrectWhenOutOfBounds( correct, Incorrect, range, Operator );
		}

		return currentIncorrect;
	}
}

public struct MathCode {
	public string Operator;
	public int Lower;
	public int Upper;
	public bool AppDecides;
	public bool IsEmpty { 
		get {
			return (Operator == default && Lower == default && Upper == default);
		} 
	}
}