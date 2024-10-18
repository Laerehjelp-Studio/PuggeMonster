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
	public static MathTask GenerateMathQuestion ( string Difficulty ) {
		//public float<> Components; // Array with 2 numbers
		//public string Operator; // + - * or /
		//public float Correct; // The correct answer.
		//public float<> Incorrect; // Incorrect options.

		MathTask task = new();

		switch (Difficulty) {
			case "e": {
				//   Easy difficulty question
				task.Components = new();
				task.Incorrect = new();
				task.Components.Add( Random.Range( 0, 10 ) );
				task.Components.Add( Random.Range( 0, 10 ) );
				task.difficultyLevelStringValue = "Easy";

				string op = "+";
				task.Operator = op;
				float temp = task.Components[ 0 ] + task.Components[ 1 ];
				task.Correct = temp;

				task.Incorrect.Add( GetIncorrect( temp, task.Incorrect, 3 ) );
				task.Incorrect.Add( GetIncorrect( temp, task.Incorrect, 3 ) );
			}
			break;
			case "m"://   Medium difficulty question
				{
				task.Components = new();
				task.Incorrect = new();
				task.Components.Add( Random.Range( 10, 31 ) );
				task.Components.Add( Random.Range( 10, 31 ) );
				task.difficultyLevelStringValue = "Medium";
				string op = "+";
				task.Operator = op;
				float temp = task.Components[ 0 ] + task.Components[ 1 ];
				task.Correct = temp;

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
				string op = "+";
				task.Operator = op;
				float temp = task.Components[ 0 ] + task.Components[ 1 ];
				task.Correct = temp;

				task = AddIncorrectAnswers( task );
			}
			break;
		}
		return task;
	}
		return task;
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

		if (lastDigitInOption1 + lastDigitInOption2 > 9) {
			task.Incorrect.Add( GetIncorrect( temp, task.Incorrect, 3 ) );
			task.Incorrect.Add( GetIncorrect( temp, task.Incorrect, 3 ) );
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
	private static float GetIncorrect (float correct, List<float> Incorrect, int range = 5) {
		if (Incorrect == default) {
			Incorrect = new();
		}

		int modifier =  Random.Range( -1 * range, range );
		
		modifier = (modifier == 0) ? modifier + 1 : modifier;

		float currentIncorrect = modifier + correct;

		if (currentIncorrect == correct || Incorrect.Contains( currentIncorrect ) || currentIncorrect < 0) {
			return GetIncorrect( correct, Incorrect, range );
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
