using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodeMenuController : MonoBehaviour {
	[SerializeField] private Image firstInput, secondInput, ThirdInput;
	private List<int> buttonInputs = new();

	private bool validCode = true;
	private MathCode mathCode = new();
	private LetterCode letterCode = new();

	public void LoadSceneBasedOnCode() {
		if (buttonInputs.Count < 1) {
			return;
		}

		switch (buttonInputs[0]) {
			case 1: // math selected
				MathCase();
				break;

			case 2: // Number friends (numbers that add up to 10, 100, 1000 etc...)
				NumberFriendsCase();
				break;

			case 3: // letters
				LettersCase();
				break;

			case 4: // WORDS!
				WordCase();
				break;
			case 5: // NOTHING YET
			case 6: // NOTHING YET
			case 7: // NOTHING YET
			case 8: // NOTHING YET
			case 9: // NOTHING YET
				validCode = false;
				ClearCodeInput();
				break;
		}
	}

	void ClearCodeInput() {
		string firstInputString = (buttonInputs.Count == 1) ? buttonInputs[0].ToString(): "";
		string secondInputString = (buttonInputs.Count == 2) ? buttonInputs[1].ToString() : "";
		string thirdInputString = (buttonInputs.Count == 3) ? buttonInputs[2].ToString() : "";
		Debug.Log($"Code is {firstInputString}{secondInputString}{thirdInputString}");
		buttonInputs.Clear();
		firstInput.color = new Color(0, 0, 0, 0);
		secondInput.color = new Color(0, 0, 0, 0);
		ThirdInput.color = new Color(0, 0, 0, 0);
	}

	void NumberFriendsCase() {
		validCode = false;
		ClearCodeInput();
	}


	void MathCase() {
		mathCode = new();
		
		if (buttonInputs.Count < 2) {
			return;
		}
		
		switch (buttonInputs[1]) {
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
				ClearCodeInput();
				break;
		}

		if (buttonInputs.Count < 3) {
			return;
		}
		switch (buttonInputs[2]) {
			case 1: // numbers from 0-10
				mathCode.Lower = 0;
				mathCode.Upper = 10;
				mathCode.AppDecides = false;
				validCode = true;
				break;

			case 2: // numbers from 10-100
				mathCode.Lower = 10;
				mathCode.Upper = 100;
				mathCode.AppDecides = false;
				validCode = true;
				break;

			case 3: // numbers from 100-1000
				mathCode.Lower = 100;
				mathCode.Upper = 1000;
				mathCode.AppDecides = false;
				validCode = true;
				break;

			case 4: // numbers from 1000-10000
				mathCode.Lower = 1000;
				mathCode.Upper = 10000;
				mathCode.AppDecides = false;
				validCode = true;
				break;

			case 5: // numbers from 0-30
				mathCode.Lower = 0;
				mathCode.Upper = 30;
				mathCode.AppDecides = false;
				validCode = true;
				break;

			case 6: // numbers from 0-50
				mathCode.Lower = 0;
				mathCode.Upper = 50;
				mathCode.AppDecides = false;
				validCode = true;
				break;

			case 7: // numbers from 0-100
				mathCode.Lower = 0;
				mathCode.Upper = 100;
				mathCode.AppDecides = false;
				validCode = true;
				break;

			case 8: // numbers from 0-1000
				mathCode.Lower = 0;
				mathCode.Upper = 1000;
				mathCode.AppDecides = false;
				validCode = true;
				break;

			case 9: // The app decides.. this is default math mode!
				mathCode.AppDecides = true;
				validCode = true;
				break;
		}

		GameManager.Instance.GameMode = GameModeType.Math;
		GameManager.Instance.MathCode = mathCode;
		ClearCodeInput();

		if (validCode) {
			GameManager.Instance.MenuLoader("GamePlayScene");
		}
	}

	void LettersCase() {
		if (buttonInputs.Count < 2) {
			return;
		}
		
		switch (buttonInputs[1]) {
			case 1: // Letters Picture .
				validCode = true;
				GameManager.Instance.GameMode = GameModeType.LetterPicture;
				GameManager.Instance.MenuLoader("GamePlayScene");
				break;

			case 2: // Letters Sound
				
				validCode = false;
				GameManager.Instance.GameMode = GameModeType.Letters;
				break;
			case 3: // NOTHING YET
			case 4: // NOTHING YET 
			case 5: // NOTHING YET
			case 6: // NOTHING YET
			case 7: // NOTHING YET
			case 8: // NOTHING YET
			case 9: // NOTHING YET
				validCode = false;
				ClearCodeInput();
				break;
		}

		if (buttonInputs.Count < 3 || GameManager.Instance.GameMode != GameModeType.Letters) {
			return;
		}

		letterCode.AppDecides = false;
		
		switch (buttonInputs[2]) {
			case 1: // ABCDEF
				validCode = true;
				letterCode.AllowedLetters = new() {
					"A",
					"B",
					"C",
					"D",
					"E",
					"F"
				};
				
				break;

			case 2: // GHKIJKL
				validCode = true;
				letterCode.AllowedLetters = new() {
					"G",
					"H",
					"I",
					"J",
					"K",
					"L"
				};
				break;

			case 3: // MNOPQR
				validCode = true;
				letterCode.AllowedLetters = new() {
					"M",
					"N",
					"O",
					"P",
					"Q",
					"R"
				}; 
				break;

			case 4: // STUVWX
				validCode = true;
				letterCode.AllowedLetters = new() {
					"S",
					"T",
					"U",
					"V",
					"X",
				}; 
				break;

			case 5: // YZÆØÅ
				validCode = true;
				letterCode.AllowedLetters = new() {
					"Y",
					"Z",
					"Æ",
					"Ø",
					"Å"
				}; 
				break;

			case 6: // ABCDEFGHIJKLMNO
				validCode = true;
				letterCode.AllowedLetters = new() {
					"A",
					"B",
					"C",
					"D",
					"E",
					"F",
					"G",
					"H",
					"I",
					"J",
					"K",
					"L",
					"M",
					"N",
					"O"
				}; 
				break;

			case 7: // PQRSTUVWXYZÆØÅ
				validCode = true;
				letterCode.AllowedLetters = new(){
					"P",
					"Q",
					"R",
					"S",
					"T",
					"U",
					"V",
					"W",
					"X",
					"Y",
					"Z",
					"Æ",
					"Ø",
					"Å"
				}; 
				break;

			case 8: // ABCDEFGHIJKLMNOPQRSTUVWXYZÆØÅ
				validCode = true;
				letterCode.AllowedLetters = new() {
					"A",
					"B",
					"C",
					"D",
					"E",
					"F",
					"G",
					"H",
					"I",
					"J",
					"K",
					"L",
					"M",
					"N",
					"O",
					"P",
					"Q",
					"R",
					"S",
					"T",
					"U",
					"V",
					"W",
					"X",
					"Y",
					"Z",
					"Æ",
					"Ø",
					"Å"
				};  
				break;

			case 9: // 
				validCode = true;
				letterCode.AppDecides = true;
				break;
		}
		
		
		if (validCode) {
			GameManager.Instance.LetterCode = letterCode;
			GameManager.Instance.MenuLoader("GamePlayScene");
		}
		ClearCodeInput();
	}


	private void WordCase() {
		validCode = true;
		GameManager.Instance.GameMode = GameModeType.Words;
		
		/*if (buttonInputs.Count < 2) {
			return;
		}
		
		switch (buttonInputs[1]) {
			case 1: // Word MODE
				validCode = true;
				GameManager.Instance.GameMode = GameModeType.Words;
				break;

			case 2: // NOTHING YET
			case 3: // NOTHING YET
			case 4: // NOTHING YET 
			case 5: // NOTHING YET
			case 6: // NOTHING YET
			case 7: // NOTHING YET
			case 8: // NOTHING YET
			case 9: // NOTHING YET
				validCode = false;
				break;
		}*/

		/*if (buttonInputs.Count < 3) {
			return;
		}
		switch (buttonInputs[2]) {
			case 1: // 
				break;

			case 2: // 
				break;

			case 3: // 
				break;

			case 4: // 
				break;

			case 5: // 
				break;

			case 6: // 
				break;

			case 7: // 
				break;

			case 8: // 
				break;

			case 9: // 
				break;
		}*/
		
		
		ClearCodeInput();
		if (validCode) {
			GameManager.Instance.MenuLoader("GamePlayScene");
		}
	}
	public void GetInputFromButton(GameObject buttonObject) {
		if (buttonInputs.Count > 2) {
			return;
		}
		
		if (buttonInputs.Count == 0) {
			firstInput.sprite = buttonObject.GetComponent<Image>().sprite;
			firstInput.color = new Color(255, 255, 255, 255);
		}
		
		if (buttonInputs.Count == 1) {
			secondInput.sprite = buttonObject.GetComponent<Image>().sprite;
			secondInput.color = new Color(255, 255, 255, 255);
		}

		if (buttonInputs.Count == 2) {
			ThirdInput.sprite = buttonObject.GetComponent<Image>().sprite;
			ThirdInput.color = new Color(255, 255, 255, 255);
		}

		string temp = buttonObject.name.Substring(7, 1);
		buttonInputs.Add(int.Parse(temp));
	}
}