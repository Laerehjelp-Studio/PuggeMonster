using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodeMenuController : MonoBehaviour
{
	[SerializeField] private Image firstInput, secondInput, ThirdInput;
	private List<int> buttonInputs = new();

	private bool validCode = true;
	private MathCode mathCode = new();

	public void LoadSceneBasedOnCode()
	{
		switch(buttonInputs[0])
		{
			case 1: // math selected
				MathCase();
				break;

			case 2: // Number friends (numbers that add up to 10, 100, 1000 etc...)
				NumberFriendsCase();
				break;

			case 3: // letters
				LettersCase();
				break;
			
			case 4: // NOTHING YET
				validCode = false;
				break;

			case 5: // NOTHING YET
				validCode = false;
				break;

			case 6: // NOTHING YET
				validCode = false;
				break;

			case 7: // NOTHING YET
				validCode = false;
				break;

			case 8: // NOTHING YET
				validCode = false;
				break;

			case 9: // NOTHING YET
				validCode = false;
				break;
		}
	}

	void MathCase()
	{
		mathCode = new();
		switch(buttonInputs[1])
		{
			case 1: // + Addition operator
				mathCode.Operator = "+";
				break;

			case 2: // - Subtraction operator
				mathCode.Operator = "-";
				break;

			case 3: // / Division operator
				mathCode.Operator = "/";
				break;

			case 4: // * Multiplication operator
				mathCode.Operator = "*";
				break;

			case 5: // NOTHING YET
			case 6: // NOTHING YET
			case 7: // NOTHING YET
			case 8: // NOTHING YET
			case 9: // NOTHING YET
				validCode = false;
				break;



		}

		switch (buttonInputs[2])
		{
			case 1: // numbers from 0-10
				mathCode.Lower = 0;
				mathCode.Upper = 10;
				mathCode.AppDecides = false;
				break;

			case 2: // numbers from 10-100
				mathCode.Lower = 10;
				mathCode.Upper = 100;
				mathCode.AppDecides = false;
				break;

			case 3: // numbers from 100-1000
				mathCode.Lower = 100;
				mathCode.Upper = 1000;
				mathCode.AppDecides = false;
				break;

			case 4: // numbers from 1000-10000
				mathCode.Lower = 1000;
				mathCode.Upper = 10000;
				mathCode.AppDecides = false;
				break;

			case 5: // numbers from 0-30
				mathCode.Lower = 0;
				mathCode.Upper = 30;
				mathCode.AppDecides = false;
				break;

			case 6: // numbers from 0-50
				mathCode.Lower = 0;
				mathCode.Upper = 50;
				mathCode.AppDecides = false;
				break;

			case 7: // numbers from 0-100
				mathCode.Lower = 0;
				mathCode.Upper = 100;
				mathCode.AppDecides = false;
				break;

			case 8: // numbers from 0-1000
				mathCode.Lower = 0;
				mathCode.Upper = 1000;
				mathCode.AppDecides = false;
				break;

			case 9: // The app decides.. this is default math mode!
				mathCode.AppDecides = true;
				break;
		}
		GameManager.Instance.GameMode = GameModeType.Math;
		CheckCodeAndRun();
	}

	void CheckCodeAndRun()
	{
		if(!validCode)
		{
			buttonInputs.Clear();
			firstInput.color = new Color(0, 0, 0, 0);
			secondInput.color = new Color(0, 0, 0, 0);
			ThirdInput.color = new Color(0, 0, 0, 0);
			Debug.Log("cleared the code");
			validCode = true;
		}
		else
		{
			Debug.Log("Code is" + buttonInputs[0] + buttonInputs[1] + buttonInputs[2]);
			buttonInputs.Clear();
			firstInput.color = new Color(0, 0, 0, 0);
			secondInput.color = new Color(0, 0, 0, 0);
			ThirdInput.color = new Color(0, 0, 0, 0);

			GameManager.Instance.MathCode = mathCode;
			GameManager.Instance.MenuLoader("GamePlayScene");
		}
	}

	void NumberFriendsCase()
	{

	}

	void LettersCase()
	{

	}


	public void getInputFromButton(GameObject buttonObject)
	{
		if(buttonInputs.Count > 2)
		{
			return;
		}
		else
		{
			if(buttonInputs.Count == 0)
			{
				firstInput.sprite = buttonObject.GetComponent<Image>().sprite;
				firstInput.color = new Color(255, 255, 255, 255);
			}
			if (buttonInputs.Count == 1)
			{
				secondInput.sprite = buttonObject.GetComponent<Image>().sprite;
				secondInput.color = new Color(255, 255, 255, 255);
			}
			if (buttonInputs.Count == 2)
			{
				ThirdInput.sprite = buttonObject.GetComponent<Image>().sprite;
				ThirdInput.color = new Color(255, 255, 255, 255);
			}

			string temp = buttonObject.name.Substring(7, 1);
			buttonInputs.Add(int.Parse(temp));
		}
	}
}
