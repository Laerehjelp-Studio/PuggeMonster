using System.Collections;
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
                    task.Components[0] = Random.Range(0, 10);
                    task.Components[1] = Random.Range(0, 10);

                    string op = "+";
                    task.Operator = op;
                    float temp = task.Components[0] + task.Components[1];
                    task.Correct = temp;

                    if (temp <= 0)
                    {
                        task.Incorrect[0] = temp + Random.Range(1, 3);
                        task.Incorrect[1] = temp + Random.Range(1, 3);
                        if (task.Incorrect[0] == task.Incorrect[1])
                        {
                            task.Incorrect[1] += 1;
                        }
                    }
                    else
                    {
                        task.Incorrect[0] = temp + Random.Range(-2, 3) + 1;
                        task.Incorrect[1] = temp + Random.Range(-2, 3) + 1;
                        if (task.Incorrect[0] == task.Incorrect[1])
                        {
                            task.Incorrect[1] += 1;
                            if (task.Incorrect[1] == temp)
                            {
                                task.Incorrect[1] += 1;
                            }
                        }
                    }
                }


                break;
            case "m"://   Medium difficulty question
                {
                    task.Components[0] = Random.Range(0, 101);
                    task.Components[1] = Random.Range(10, 101);

                    string op = "+";
                    task.Operator = op;
                    float temp = task.Components[0] + task.Components[1];
                    task.Correct = temp;

                    if (temp <= 0)
                    {
                        task.Incorrect[0] = temp + Random.Range(2, 10);
                        task.Incorrect[1] = temp + Random.Range(2, 10);
                        if (task.Incorrect[0] == task.Incorrect[1])
                        {
                            task.Incorrect[1] += 3;
                        }
                    }
                    else
                    {
                        task.Incorrect[0] = temp + Random.Range(-2, 10) + 1;
                        task.Incorrect[1] = temp + Random.Range(-2, 10) + 1;
                        if (task.Incorrect[0] == task.Incorrect[1])
                        {
                            task.Incorrect[1] += 1;
                            if (task.Incorrect[1] == temp)
                            {
                                task.Incorrect[1] += 1;
                            }
                        }
                    }
                }
                break;
            case "h"://   Hard difficulty question
                {
                    task.Components[0] = Random.Range(7000, 40000);
                    task.Components[1] = Random.Range(-7000, -300);

                    string op = "*";
                    task.Operator = op;
                    float temp = task.Components[0] * task.Components[1];
                    task.Correct = temp;

                    task.Incorrect[0] = temp + Random.Range(1, 3);
                    task.Incorrect[1] = temp + Random.Range(1, 3);
                    if (task.Incorrect[0] == task.Incorrect[1])
                    {
                        task.Incorrect[1] += 1;
                    }
                }
                break;
        }

        // FindDifficulty() this returns...  sprite, operator, 
        // 
        
        return task;
    }

    /// <summary>
    /// difficulty 1, 2 or 3. 1=easy 2=medium 3=hard
    /// </summary>
    /// <param name="diff"></param>
    private static void FindDifficulty(int diff)
    {

    }

}
