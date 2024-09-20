using UnityEngine;

/// <summary>
/// 
/// </summary>
public class TaskMaster : MonoBehaviour {
	/* */
}

public struct MathTask {
	public float[] Components; // Array with 2 numbers
	public string Operator; // + - * or /
	public float Correct; // The correct answer.
	public float[] Incorrect; // Incorrect options.
	public Sprite NumberSprite;
}