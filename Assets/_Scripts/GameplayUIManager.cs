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
	[SerializeField] private Image _questionSprite;
	[SerializeField] private TMP_Text _questionText;
	[SerializeField] private Button _questionSoundPlayButton;

	[Header("Experience Bar")]
	[SerializeField] private Slider _expBar;

	private Dictionary<Button, TMP_Text> _buttonRegistry = new();
	private List<int> _tempPlacementList = new();
	[SerializeField] private TMP_Text difficultyLevelText;
	[SerializeField] private TMP_Text difficultySetText;

	private void Awake () {
		if (_answerButtons.Count == 0) {
			Debug.LogError( $"{this.name} is missing answerButton-references." );
		}

		foreach (Button button in _answerButtons) {
			TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
			_buttonRegistry.Add( button, buttonText );
		}
		GameManager.Instance.RegisterManager( this );

		// _expBar.maxValue = GameManager.RecievePuggemonsterLimit; // Changed this 
		_expBar.maxValue = 1f; // to this


        // auto max size for 135
    }

	private void OnEnable () {
		GameManager.Instance.RegisterManager( this );
	}

	private void OnDisable () {
		// Empty argument de-registers the current UiManager
		GameManager.Instance.UnRegisterManager( this );
	}
	public void MathQuestion ( MathTask task ) {
		// Reset 
		_tempPlacementList.Clear();
		_questionText.enabled = true;
		_questionSprite.enabled = false;
		_questionSoundPlayButton.gameObject.SetActive(false);

		if (RandomizeButtonPlacement(out var _correctPlacement)) {
			return;
		}

		_questionText.text = $"{task.Components[0]} {task.Operator} {task.Components[ 1 ]}";
		SetDifficultyString(task.DifficultyLevelStringValue, task.DifficultySet);

		_buttonRegistry[ _answerButtons[ _correctPlacement ] ].text = $"{task.Correct}";
		_answerButtons[ _correctPlacement ].onClick.AddListener(() => {
			AnswerButtonClick( task.Correct, task, _correctPlacement );
		} );

		foreach (float incorrectValue in task.Incorrect) {
			int _incorrectPlacement = GetRandomAnswerPlacement();

			if (_incorrectPlacement == -1) {
				return;
			}

			_buttonRegistry[ _answerButtons[ _incorrectPlacement ] ].text = $"{incorrectValue}";
			_answerButtons[ _incorrectPlacement ].onClick.AddListener(() => {
				AnswerButtonClick( incorrectValue, task, _incorrectPlacement );
			} );
		}
	}

	private bool RandomizeButtonPlacement(out int _correctPlacement) {
		foreach ( Button button in _answerButtons) {
			button.interactable = true;
			button.onClick.RemoveAllListeners();
			_tempPlacementList.Add( _answerButtons.IndexOf( button ) );
		}

		_correctPlacement = GetRandomAnswerPlacement();
		if (_correctPlacement == -1) {
			return true;
		}

		return false;
	}

	private void SetDifficultyString(string difficultyLevelStringValue, char[] difficultySetArray) {
		if (!GameManager.IsGamelabBuild) {
			return;
		}
		difficultyLevelText.text = difficultyLevelStringValue;

		string difficultySet = "";
		foreach (char item in difficultySetArray) {
			difficultySet += $"[{item}] ";
		}
		difficultySetText.text = difficultySet;
	}

	public void LetterQuestion( LetterTask task ) {
		// Reset 
		_tempPlacementList.Clear();
		_questionText.enabled = false;
		switch (task.Mode) {
			case GameModeType.LetterPicture:
				_questionSprite.enabled = true;
				_questionSoundPlayButton.gameObject.SetActive( false );
				break;
			case GameModeType.Letters:
				_questionSoundPlayButton.gameObject.SetActive(true);
				_questionSprite.enabled = false;
				_questionSoundPlayButton.onClick.AddListener( () => {
					GameManager.PlayLetterSound(task.LetterSound);
				} );
				break;
		}
		difficultyLevelText.text = "";

		if ( RandomizeButtonPlacement( out var correctPlacement ) ) {
			return;
		}

		_questionSprite.sprite = task.TaskSprite;

		/*
			public Sprite WordSprite;
			public string Correct;
			public string[] Incorrect;
		 */

		_buttonRegistry[_answerButtons[correctPlacement]].text = $"{task.Correct}";
		SetDifficultyString( task.DifficultyLevelStringValue, task.DifficultySet );

		_answerButtons[correctPlacement].onClick.AddListener( () => {
			AnswerButtonClick( task.Correct, task, correctPlacement );
		} );

		foreach ( string incorrectValue in task.Incorrect ) {
			int _incorrectPlacement = GetRandomAnswerPlacement();

			if ( _incorrectPlacement == -1 ) {
				return;
			}

			_buttonRegistry[_answerButtons[_incorrectPlacement]].text = $"{incorrectValue}";
			_answerButtons[_incorrectPlacement].onClick.AddListener( () => {
				AnswerButtonClick( incorrectValue, task, _incorrectPlacement );
			} );
		}
	}

	public void WordQuestion(WordTask task)
	{
		// Reset 
		_tempPlacementList.Clear();
		_questionText.enabled = false;
		_questionSprite.enabled = true;
		difficultyLevelText.text = "";

		if (RandomizeButtonPlacement(out var _correctPlacement)) {
			return;
		}

		_questionSprite.sprite = task.TaskSprite;

		/*
			public Sprite WordSprite;
			public string Correct;
			public string[] Incorrect;
		 */

		_buttonRegistry[_answerButtons[_correctPlacement]].text = $"{task.Correct}";
		SetDifficultyString(task.DifficultyLevelStringValue, task.DifficultySet);
		
		_answerButtons[_correctPlacement].onClick.AddListener(() => {
			AnswerButtonClick(task.Correct, task, _correctPlacement);
		});

		foreach (string incorrectValue in task.Incorrect)
		{
			int _incorrectPlacement = GetRandomAnswerPlacement();

			if (_incorrectPlacement == -1)
			{
				return;
			}

			_buttonRegistry[_answerButtons[_incorrectPlacement]].text = $"{incorrectValue}";
			_answerButtons[_incorrectPlacement].onClick.AddListener(() => {
				AnswerButtonClick(incorrectValue, task, _incorrectPlacement);
			});
		}
	}
	
	private void AnswerButtonClick ( string valuePicked, LetterTask letterTask, int correctPlacement ) {
		_answerButtons[ correctPlacement ].interactable = false;
		LetterButtonClicked( valuePicked, letterTask );
	}
	
	private void AnswerButtonClick ( float valuePicked, MathTask mathTask, int correctPlacement ) {
		_answerButtons[ correctPlacement ].interactable = false;
		MathButtonClicked( valuePicked, mathTask );
	}

	private void AnswerButtonClick(string valuePicked, WordTask wordTask, int correctPlacement)
	{
		_answerButtons[correctPlacement].interactable = false;
		WordButtonClicked(valuePicked, wordTask);
	}

	private void MathButtonClicked ( float mathValue, MathTask mathTask ) {
		GameManager.TaskMaster.RegisterAnswer( mathTask, mathValue );
	}

	private void WordButtonClicked(string buttonValue, WordTask wordTask)
	{
		GameManager.TaskMaster.RegisterAnswer(wordTask, buttonValue);
	}
	private void LetterButtonClicked(string buttonValue, LetterTask letterTask)
	{
		GameManager.TaskMaster.RegisterAnswer(letterTask, buttonValue);
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


