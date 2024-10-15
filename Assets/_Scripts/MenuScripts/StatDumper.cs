using System;
using TMPro;
using UnityEngine;

public class StatDumper : MonoBehaviour
{
	[SerializeField] private TMP_Text _textField;

	private void Awake () {
		StatManager.OnDatabaseUpdate += UpdateInterface;
	}

	private void UpdateInterface () {
		OperatorStore operatorStore = StatManager.GetStore;
		string contentString = "Database Content: \n";
		contentString += "		Addition: \n";
		contentString += "			Ones- \n";
		foreach (var item in operatorStore.Addition.OneStats) {
			contentString += $"				Entry: {item.Key}, Mastery: {item.Value}\n";
		}
		contentString += "			Tenners- \n";
		foreach (var item in operatorStore.Addition.TensStats) {
			contentString += $"				Entry: {item.Key}, Mastery: {item.Value}\n";
		}
		contentString += "			Hundreds- \n";
		foreach (var item in operatorStore.Addition.HundredsStats) {
			contentString += $"				Entry: {item.Key}, Mastery: {item.Value}\n";
		}
		contentString += "			Thousands- \n";
		foreach (var item in operatorStore.Addition.ThousandsStats) {
			contentString += $"				Entry: {item.Key}, Mastery: {item.Value}\n";
		}
		_textField.SetText( contentString );
	}
	private void OnEnable () {
		UpdateInterface();
	}
}
