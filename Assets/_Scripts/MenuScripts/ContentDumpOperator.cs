using TMPro;
using UnityEngine;

public class ContentDumpOperator : MonoBehaviour
{
	[SerializeField] private TMP_Text _heading;
	[SerializeField] private TeacherStatDisplayController _teacherStatDisplayController;
	public void OperatorDump(string mathOperator, string heading = "") {
		_heading.SetText(heading);

		_teacherStatDisplayController.SetMathStats(mathOperator);
		
		/*string _onerString = "<b>Ones</b> -> \n";
		foreach (var item in mathOperator.OneStats) {
			_onerString += $"	{item.Key}: {mathOperator.OneStats[ item.Key ]}\n";
		}
		_oners.SetText(_onerString);

		string _tennerString = "<b>Tenners</b> -> \n";
		foreach (var item in mathOperator.TensStats) {
			_tennerString += $"	{item.Key}: {item.Value}\n";
		}
		_tenners.SetText(_tennerString);
		
		string _hundredString = "<b>Hundreds</b> -> \n";    
		foreach (var item in mathOperator.HundredsStats) {
			_hundredString += $"	{item.Key}: {item.Value}\n";
		}
		_hundreds.SetText(_hundredString);

		string _thousandsString = "<b>Thousands</b> ->  \n";
		foreach (var item in mathOperator.ThousandsStats) {
			_thousandsString += $"	{item.Key}: {item.Value}\n";
		}
		_thousands.SetText( _thousandsString );*/
	}

	public void WordsDump(string heading = "") {
		_heading.SetText(heading);
		_teacherStatDisplayController.SetWordText("Bokstaver og Ord");
	}
}

