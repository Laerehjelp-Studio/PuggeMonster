using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TeacherStatDisplayController : MonoBehaviour {
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

	public void SetStats(string Operator) {
		MathDifficultyList easyDifficulty = StatManager.GetDifficultyLists(Operator,"e");
		//MathDifficultyList mediumDifficulty = StatManager.GetDifficultyLists(Operator,"m");
		MathDifficultyList hardDifficulty = StatManager.GetDifficultyLists(Operator,"h");
		//MathDifficultyList additionEasy = StatManager.GetDifficultyLists("+","e");
		//MathDifficultyList additionMedium = StatManager.GetDifficultyLists("+","m");
		//MathDifficultyList additionHard = StatManager.GetDifficultyLists("+","h");
		//MathDifficultyList subtractionEasy = StatManager.GetDifficultyLists("-","e");
		//MathDifficultyList subtractionMedium = StatManager.GetDifficultyLists("-","m");
		//MathDifficultyList subtractionHard = StatManager.GetDifficultyLists("-","h");
		//MathDifficultyList multiplicationEasy = StatManager.GetDifficultyLists("*","e");
		//MathDifficultyList multiplicationMedium = StatManager.GetDifficultyLists("*","m");
		//MathDifficultyList multiplicationHard = StatManager.GetDifficultyLists("*","h");
		//MathDifficultyList divisionEasy = StatManager.GetDifficultyLists("/","e");
		//MathDifficultyList divisionMedium = StatManager.GetDifficultyLists("/","m");
		//MathDifficultyList divisionHard = StatManager.GetDifficultyLists("/","h");

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

	private void SetOperatorText(List<string> difficultyList, TMP_Text textField) {
		string difficultyString = "";
		int entries = 0;
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
