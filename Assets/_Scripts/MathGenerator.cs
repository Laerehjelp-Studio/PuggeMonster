using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class MathGenerator
{
	// Fake numbers
	static private int generalMastery = 0;
	/// <summary>
	/// Generates questions based on Difficulty String.
	/// </summary>
	/// <param name="Difficulty"></param>
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

				task.Incorrect.Add( GetIncorrectWhenOutOfBounds( task.Correct, task.Incorrect, 3 ) );
				task.Incorrect.Add( GetIncorrectWhenOutOfBounds( task.Correct, task.Incorrect, 3 ) );
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
				;

				task = AddIncorrectAnswers( task );
			}
			break;
		}
		return task;
	}
	/// <summary>
	/// Generates math questions based on MathCode
	/// </summary>
	/// <param name="mathCode"></param>
	/// <returns></returns>
	public static MathTask GenerateMathQuestion ( MathCode mathCode ) {
		MathTask task = new();

		task.Components = new();
		task.Incorrect = new();

		task.Operator = mathCode.Operator;

		if (mathCode.AppDecides) {
			return GenerateMathQuestionFromStudentPerformance( task, mathCode );
		}

		task.difficultyLevelStringValue = "Kode";

		task.Components.Add( Random.Range( mathCode.Lower, mathCode.Upper ) );
		task.Components.Add( Random.Range( mathCode.Lower, mathCode.Upper ) );

		task.Correct = GetMathResult( task );

		task = AddIncorrectAnswers( task );

		return task;
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
	/// <returns></returns>
	public static MathTask GenerateMathQuestionFromStudentPerformance (  MathTask task, MathCode mathCode  = new() ) {
		if (task.Operator == default) {
			task.Operator = "+";
		}
		if (task.difficultyLetter == default) {
			task.difficultyLetter = 'e';
		}

		DifficultyList difficultyLists = StatManager.GetDifficultyLists( task.Operator, task.difficultyLetter.ToString() );

		// If GetDifficultyLists does not produce a properly formed list, run the oldest GenerateMathQuestion function.
		if (difficultyLists.One == default ||
			difficultyLists.Tens == default ||
			difficultyLists.Hundreds == default ||
			difficultyLists.Thousands == default) {

			Debug.LogError( "difficultyLists not properly formed." );
			return GenerateMathQuestion( task.difficultyLetter.ToString(), task );
		}

		// Get a difficulty adjusted pair of One's
		string firstComponent = "", secondComponent = "";

		switch (task.difficultyLetter) {
			case 'e':
				GetComponentFromDifficultyList( task, difficultyLists.One, ref firstComponent, ref secondComponent );

				task.Components.Add( float.Parse(firstComponent) );
				task.Components.Add( float.Parse(secondComponent) );

				task.Correct = GetMathResult( task );

				task.difficultyLevelStringValue = "Easy";

				task.Incorrect.Add( GetIncorrectWhenOutOfBounds( task.Correct, task.Incorrect, 3 ) );
				task.Incorrect.Add( GetIncorrectWhenOutOfBounds( task.Correct, task.Incorrect, 3 ) );
				break;
			case 'm':
				GetComponentFromDifficultyList( task, difficultyLists.One, ref firstComponent, ref secondComponent );
				GetComponentFromDifficultyList( task, difficultyLists.Tens, ref firstComponent, ref secondComponent );

				task.Components.Add( float.Parse( firstComponent ) );
				task.Components.Add( float.Parse( secondComponent ) );

				task.Correct = GetMathResult( task );

				task = AddIncorrectAnswers( task );
				task.difficultyLevelStringValue = "Medium";
				break;
			case 'h':
				GetComponentFromDifficultyList( task, difficultyLists.One, ref firstComponent, ref secondComponent );
				GetComponentFromDifficultyList( task, difficultyLists.Tens, ref firstComponent, ref secondComponent );
				GetComponentFromDifficultyList( task, difficultyLists.Hundreds, ref firstComponent, ref secondComponent );

				task.Components.Add( float.Parse( firstComponent ) );
				task.Components.Add( float.Parse( secondComponent ) );

				task.Correct = GetMathResult( task );

				task = AddIncorrectAnswers( task );
				task.difficultyLevelStringValue = "Hard";
				break;
		}

		return task;
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
	/// <param name="temp"></param>
	/// <param name="lastDigitInOption1"></param>
	/// <param name="lastDigitInOption2"></param>
	/// <returns></returns>
	private static MathTask AddIncorrectAnswers ( MathTask task ) {
		float temp = task.Correct;

		int lastDigitInAnswer, lastDigitInOption1, lastDigitInOption2;


		string tempString = "" + temp;
		tempString = tempString[ tempString.Length - 1 ].ToString();
		lastDigitInAnswer = Int32.Parse( tempString );

		tempString = "" + task.Components[ 0 ];
		tempString = tempString[ tempString.Length - 1 ].ToString();
		lastDigitInOption1 = Int32.Parse( tempString );

		tempString = "" + task.Components[ 1 ];
		tempString = tempString[ tempString.Length - 1 ].ToString();
		lastDigitInOption2 = Int32.Parse( tempString );

		if (lastDigitInOption1 + lastDigitInOption2 > 9  || temp < 20) { // TODO: Should probably also trigger when going below zero
			task.Incorrect.Add( GetIncorrectWhenOutOfBounds( temp, task.Incorrect, 3 ) );
			task.Incorrect.Add( GetIncorrectWhenOutOfBounds( temp, task.Incorrect, 3 ) );
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
	/// <returns></returns>
	private static float GetIncorrectWhenOutOfBounds (float correct, List<float> Incorrect, int range = 5) {
		if (Incorrect == default) {
			Incorrect = new();
		}

		int modifier =  Random.Range( -1 * range, range );
		
		modifier = (modifier == 0) ? modifier + 1 : modifier;

		float currentIncorrect = modifier + correct;

		if (currentIncorrect == correct || Incorrect.Contains( currentIncorrect ) || currentIncorrect < 0) {
			return GetIncorrectWhenOutOfBounds( correct, Incorrect, range );
		}

		return modifier + correct;
	}

	/// <summary>
	/// difficulty 1, 2 or 3. 1=easy 2=medium 3=hard
	/// </summary>
	/// <param name="diff"></param>
	private static void FindDifficulty(int diff)
	{

	}
}

public struct MathCode {
	public string Operator;
	public int Lower;
	public int Upper;
	public bool AppDecides;
}