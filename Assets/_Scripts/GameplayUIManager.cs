using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUIManager : MonoBehaviour {
	// UI Specific
	[SerializeField] private Button[] _answerButtons;
	[SerializeField] private Image _questionBackground;
	[SerializeField] private TMP_Text _questionText;
	private Dictionary<Button, TMP_Text> _buttonRegistry = new();
	private List<int> _randomPlacementList = new();
	private List<int> _tempPlacementList = new();

	private void OnEnable () {
		GameManager.Instance.RegisterUIManager(this);
	}
	private void OnDisable () {
		// Empty argument de-registers the current UiManager
		GameManager.Instance.RegisterUIManager();
	}

	private void Start () {
		_buttonRegistry.Clear();
		if (_answerButtons.Length == 0) {
			Debug.LogError( $"{this.name} is missing answerButton-references." );
		}
		int buttonCount = 0;
		foreach (Button button in _answerButtons) {
			TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
			_buttonRegistry.Add( button, buttonText );
			_randomPlacementList.Add( buttonCount );
			buttonCount++;
		}

	}

	public void MathQuestion ( MathTask mathTask ) {
		// Reset 
		_tempPlacementList.Clear();
		foreach ( Button button in _answerButtons) {
			button.enabled = true;
		}

		int _correctPlacement = GetRandomAnswerPlacement();
		_buttonRegistry[ _answerButtons[ _correctPlacement ] ].text = $"{mathTask.Correct}";
		_answerButtons[ _correctPlacement ].onClick.AddListener(() => {
			MathButtonClicked(mathTask.Correct, mathTask );
			_answerButtons[ _correctPlacement ].enabled = false;

		} );

		foreach (float incorrectValue in mathTask.Incorrect) {
			int incorrectPlacement = GetRandomAnswerPlacement();

			_buttonRegistry[ _answerButtons[ incorrectPlacement ] ].text = $"{incorrectValue}";
			_answerButtons[ incorrectPlacement ].onClick.AddListener(() => {
				MathButtonClicked( incorrectValue, mathTask );
				_answerButtons[ incorrectPlacement ].enabled = false;
			} );
		}
	}

	private void MathButtonClicked ( float mathValue, MathTask mathTask ) {
		GameManager.TaskMaster.RegisterAnswer( mathTask, mathValue );
	}

	private int GetRandomAnswerPlacement ( ) {
		if (_tempPlacementList.Count == 0) {
			_tempPlacementList = new List<int>(_randomPlacementList);
		}

		int potentialIndex = Random.Range( 0, _tempPlacementList.Count );
		int potentialAnswer = _tempPlacementList[ potentialIndex ];
		_tempPlacementList.RemoveAt( potentialIndex );
		
		return potentialAnswer;
	}

}


