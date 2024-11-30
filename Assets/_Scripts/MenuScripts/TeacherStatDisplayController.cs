using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TeacherStatDisplayController : MonoBehaviour {
	[Header("Header References")]
	[SerializeField] private TMP_Text _decimalHeader;
	[SerializeField] private TMP_Text _onesHeader;
	[SerializeField] private TMP_Text _tensHeader;
	[SerializeField] private TMP_Text _hundredsHeader;
	[SerializeField] private TMP_Text _thousandsHeader;
	
	[Header("Content References")]
	[SerializeField] private TMP_Text _decimalEasy;
	[SerializeField] private TMP_Text _decimalMedium;
	[SerializeField] private TMP_Text _decimalHard;
	[SerializeField] private TMP_Text _onesEasy;
	[SerializeField] private TMP_Text _onesMedium;
	[SerializeField] private TMP_Text _onesHard;
	[SerializeField] private TMP_Text _tensEasy;
	[SerializeField] private TMP_Text _tensMedium;
	[SerializeField] private TMP_Text _tensHard;
	[SerializeField] private TMP_Text _hundredsEasy;
	[SerializeField] private TMP_Text _hundredsMedium;
	[SerializeField] private TMP_Text _hundredsHard;
	[SerializeField] private TMP_Text _thousandsEasy;
	[SerializeField] private TMP_Text _thousandsMedium;
	[SerializeField] private TMP_Text _thousandsHard;

	public void SetMathStats(string Operator) {
		MathDifficultyList easyDifficulty = StatManager.GetDifficultyLists(Operator,"e");
		MathDifficultyList hardDifficulty = StatManager.GetDifficultyLists(Operator,"h");

		ZeroUpdateMathStats(ref easyDifficulty, Operator);
		ZeroUpdateMathStats(ref hardDifficulty,Operator);
		//SetOperatorText(easyDifficulty.Decimal, _decimalEasy);
		SetOperatorText(easyDifficulty.One, _onesEasy);
		SetOperatorText(easyDifficulty.Tens, _tensEasy);
		SetOperatorText(easyDifficulty.Hundreds, _hundredsEasy);
		SetOperatorText(easyDifficulty.Thousands, _thousandsEasy);
		
		//SetOperatorText(mediumDifficulty.Decimal, _decimalMedium);
		//SetOperatorText(mediumDifficulty.One, _onesMedium);
		//SetOperatorText(mediumDifficulty.Tens, _tensMedium);
		//SetOperatorText(mediumDifficulty.Hundreds, _hundredsMedium);
		//SetOperatorText(mediumDifficulty.Thousands, _thousandsMedium);
		
		//SetOperatorText(hardDifficulty.Decimal, _decimalHard);
		SetOperatorText(hardDifficulty.One, _onesHard);
		SetOperatorText(hardDifficulty.Tens, _tensHard);
		SetOperatorText(hardDifficulty.Hundreds, _hundredsHard);
		SetOperatorText(hardDifficulty.Thousands, _thousandsHard);
		
	}

	private void ZeroUpdateMathStats(ref MathDifficultyList mathDifficultyLists, string Operator) {
		mathDifficultyLists.One = StatManager.GetNonZeroMathFloatList(mathDifficultyLists.One, 1, Operator);
		mathDifficultyLists.Tens = StatManager.GetNonZeroMathFloatList(mathDifficultyLists.Tens, 2, Operator);
		mathDifficultyLists.Hundreds = StatManager.GetNonZeroMathFloatList(mathDifficultyLists.Hundreds, 3, Operator);
		mathDifficultyLists.Thousands = StatManager.GetNonZeroMathFloatList(mathDifficultyLists.Thousands, 4, Operator);
	}

	public void SetWordText( string heading) {
		List<string> easyDifficulty = StatManager.GetWordDifficultyList('e');
		easyDifficulty = StatManager.GetNonZeroWordFloatList(easyDifficulty, 'e');
		List<string> hardDifficulty = StatManager.GetWordDifficultyList('h');
		hardDifficulty = StatManager.GetNonZeroWordFloatList(hardDifficulty, 'h');
		_onesHeader.text = "Bilde Ord";
		SetOperatorText(easyDifficulty, _onesEasy);
		SetOperatorText(hardDifficulty, _onesHard);
		
		easyDifficulty = StatManager.GetLetterDifficultyList('e', LetterMode.Picture);
		easyDifficulty = StatManager.GetNonZeroLetterFloatList(easyDifficulty, LetterMode.Picture, 'e');
		hardDifficulty = StatManager.GetLetterDifficultyList('h', LetterMode.Picture);
		hardDifficulty = StatManager.GetNonZeroLetterFloatList(hardDifficulty, LetterMode.Picture, 'h');
		_tensHeader.text = "Bilde Bokstav";
		SetOperatorText(easyDifficulty, _tensEasy);
		SetOperatorText(hardDifficulty, _tensHard);

		easyDifficulty = StatManager.GetLetterDifficultyList('e', LetterMode.Sound);
		easyDifficulty = StatManager.GetNonZeroLetterFloatList(easyDifficulty, LetterMode.Sound, 'e');
		hardDifficulty = StatManager.GetLetterDifficultyList('h', LetterMode.Sound);
		hardDifficulty = StatManager.GetNonZeroLetterFloatList(hardDifficulty, LetterMode.Sound, 'h');
		_hundredsHeader.text = "Lyd Bokstav";
		SetOperatorText(easyDifficulty, _hundredsEasy);
		SetOperatorText(hardDifficulty, _hundredsHard);
		
		_thousandsHeader.transform.parent.gameObject.SetActive(false);
	}
	
	private void SetOperatorText(List<string> difficultyList, TMP_Text textField) {
		string difficultyString = "";
		int entries = 0;
		if (difficultyList.Count == 0) {
			textField.text = "Ukjent.";
			return;
		}
		foreach (var difficultyKey in difficultyList) {
			entries++;
			if (entries >= 10) {
				continue;
			}
			
			difficultyString += $"{difficultyKey}\n";
		}
		textField.text = difficultyString;
	}
}
