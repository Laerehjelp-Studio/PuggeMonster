using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUIManager : MonoBehaviour {
	// UI Specific
	[Header( "Answers" )]
	[SerializeField] private List<Button> _answerButtons = new();

	[Header("Questions")]
	[SerializeField] private Image _questionBackground;
	[SerializeField] private TMP_Text _questionText;

	[Header("Experience Bar")]
	[SerializeField] private Slider _expBar;

	private Dictionary<Button, TMP_Text> _buttonRegistry = new();
	private List<int> _tempPlacementList = new();

	private void Awake () {
		if (_answerButtons.Count == 0) {
			Debug.LogError( $"{this.name} is missing answerButton-references." );
		}

		foreach (Button button in _answerButtons) {
			TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
			_buttonRegistry.Add( button, buttonText );
		}
		GameManager.Instance.RegisterManager( this );
	}

	private void OnEnable () {
		GameManager.Instance.RegisterManager( this );
	}

	private void OnDisable () {
		// Empty argument de-registers the current UiManager
		GameManager.Instance.UnRegisterManager( this );
	}
	public void MathQuestion ( MathTask mathTask ) {
		// Reset 
		_tempPlacementList.Clear();
		foreach ( Button button in _answerButtons) {
			button.interactable = true;
			button.onClick.RemoveAllListeners();
			_tempPlacementList.Add( _answerButtons.IndexOf( button ) );
		}

		int _correctPlacement = GetRandomAnswerPlacement();
		if (_correctPlacement == -1) {
			return;
		}

		_questionText.text = $"{mathTask.Components[0]} {mathTask.Operator} {mathTask.Components[ 1 ]}";
		
		_buttonRegistry[ _answerButtons[ _correctPlacement ] ].text = $"{mathTask.Correct}";
		_answerButtons[ _correctPlacement ].onClick.AddListener(() => {
			AnswerButtonClick( mathTask.Correct, mathTask, _correctPlacement );
		} );

		foreach (float incorrectValue in mathTask.Incorrect) {
			int _incorrectPlacement = GetRandomAnswerPlacement();

			if (_incorrectPlacement == -1) {
				return;
			}

			_buttonRegistry[ _answerButtons[ _incorrectPlacement ] ].text = $"{incorrectValue}";
			_answerButtons[ _incorrectPlacement ].onClick.AddListener(() => {
				AnswerButtonClick( incorrectValue, mathTask, _incorrectPlacement );
			} );
		}
	}

	private void AnswerButtonClick ( float valuePicked, MathTask mathTask, int _correctPlacement ) {
		_answerButtons[ _correctPlacement ].interactable = false;
		MathButtonClicked( valuePicked, mathTask );
	}

	private void MathButtonClicked ( float mathValue, MathTask mathTask ) {
		GameManager.TaskMaster.RegisterAnswer( mathTask, mathValue );
	}

	private int GetRandomAnswerPlacement ( ) {
		if (_tempPlacementList.Count == 0) {
			Debug.LogWarning( "_tempPlacementList is empty. This will not work." );
			return -1;
		}

		int potentialIndex = Random.Range( 0, _tempPlacementList.Count );
		int potentialAnswer = _tempPlacementList[ potentialIndex ];
		_tempPlacementList.RemoveAt( potentialIndex );
		
		return potentialAnswer;
	}

	public void SetExpBar(float value = 0) {
		_expBar.value = value;
	}

	public void ResetUI () {
		SetExpBar();
	}
}


