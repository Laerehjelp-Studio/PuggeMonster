
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public static class MathGenerator
{

    // Fake numbers
    static private int generalMastery = 0;
    public static MathTask GenerateMathQuestion(string Difficulty)
    {
        //public float[] Components; // Array with 2 numbers
        //public string Operator; // + - * or /
        //public float Correct; // The correct answer.
        //public float[] Incorrect; // Incorrect options.

        MathTask task = new();

        switch (Difficulty)
        {
            case "e":
                {
                    //   Easy difficulty question
                    task.Components = new();
                    task.Incorrect = new();
                    task.Components.Add(Random.Range(0, 10));
                    task.Components.Add(Random.Range(0, 10));

                    string op = "+";
                    task.Operator = op;
                    float temp = task.Components[0] + task.Components[1];
                    task.Correct = temp;

					task.Incorrect.Add( GetIncorrect( temp, task.Incorrect, 10 ) );
					task.Incorrect.Add( GetIncorrect( temp, task.Incorrect, 10 ) );
				}


                break;
            case "m"://   Medium difficulty question
                {
                    task.Components = new();
                    task.Incorrect = new();
                    task.Components.Add(Random.Range(0, 31));
                    task.Components.Add(Random.Range(10, 31));

                    string op = "+";
                    task.Operator = op;
                    float temp = task.Components[0] + task.Components[1];
                    task.Correct = temp;


                    task.Incorrect.Add( GetIncorrect( temp, task.Incorrect, 10 ) );
                    task.Incorrect.Add( GetIncorrect( temp, task.Incorrect, 10 ) );
                }
                break;
            case "h"://   Hard difficulty question
                {
                    task.Components = new();
                    task.Incorrect = new();
                    task.Components.Add(Random.Range(0, 101));
                    task.Components.Add(Random.Range(10, 101));

                    string op = "+";
                    task.Operator = op;
                    float temp = task.Components[0] + task.Components[1];
                    task.Correct = temp;

					task.Incorrect.Add( GetIncorrect( temp, task.Incorrect, 10 ) );
					task.Incorrect.Add( GetIncorrect( temp, task.Incorrect, 10 ) );
				}
                break;
        }

        // FindDifficulty() this returns...  sprite, operator, 
        // 
        
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
