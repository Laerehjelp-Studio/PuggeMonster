using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class TaskMaster : MonoBehaviour {
	
	private GameModeType _gameMode;

	private List<MathTask> _mathTasks = new();
	private List<LetterTask> _letterTasks = new();
	private List<WordTask> _wordTasks = new();

	private MathDifficulty _currentDifficulty = new();
	[SerializeField] private int _maxTasks = 4;
	private int _currentTaskIndex = 0;
	private int _numberOfAnswers = 0;
	private float _currentScore = 0;

	private void Awake () {
		//_gameMode = GameManager.Instance.GameMode;
		_currentDifficulty.AverageDifficulty = new();
		GameManager.Instance.RegisterManager( this );
	}

	private void OnEnable () {
		// Register TaskMaster enabling GameManager.TaskMaster-syntax.
		GameManager.Instance.RegisterManager(this);
		//GameManager.Instance.OnGameModeUpdate += UpdateGameMode;
		GameManager.Instance.OnSceneLoad += RefreshTasks;
	}

	private void OnDisable () {
		// Empty argument de-registers the current TaskMaster
		GameManager.Instance.UnRegisterManager( this );
		//GameManager.Instance.OnGameModeUpdate -= UpdateGameMode;
		GameManager.Instance.OnSceneLoad -= RefreshTasks;
	}

	//private void UpdateGameMode (GameModeType gameMode ) {
	//	_gameMode = gameMode;
	//}

	public void RefreshTasks (GameModeType gameMode) {
		switch (gameMode) {
			case GameModeType.Math:
				_mathTasks.Clear();

				for (int i = 0; i < _maxTasks; i++) {
					// first implementation, Will be replaced when a difficulty system has been created.

					_mathTasks.Add( MathGenerator.GenerateMathQuestion( GetDifficultyLetter() ) ); // TODO: Update to actually reflect our GDD.
				}
				break;
			case GameModeType.Words:
				_wordTasks.Clear();

				for (int i = 0; i < _maxTasks; i++)
				{
					// first implementation, Will be replaced when a difficulty system has been created.

					_wordTasks.Add(WordGenerator.GenerateWordQuestion());
				}
				break;
			case GameModeType.Letters:
				_letterTasks.Clear();
				
				_letterTasks.Add( LetterGenerator.GenerateLetterQuestion() );
				break;
			case GameModeType.None:
				break;
		}
		if (_mathTasks.Count > 0 ) {
			MathTask mathTask = _mathTasks[0];
			SwapQuestion( mathTask );
		}
	}

	private string GetDifficultyLetter () {
		int _difficulty = Random.Range( 0, 3 );
		string _difficultyString = "";
        //return "h";

		switch (_difficulty) {
			case 0:
				_difficultyString = "h";
				break;
			case 1:
				_difficultyString = "m";
				break;
			case 2:
				_difficultyString = "e";
				break;
		}

		return _difficultyString;
	}

	public void RegisterAnswer ( MathTask mathTask, float mathValue ) {
		_numberOfAnswers++;

		float points = 1f * (1f / _numberOfAnswers);

		if (points < 0.4) {
			points = 0;
		}
		Debug.Log($"Correct answer = {mathValue}, points = {points} / {1f * (1f / _numberOfAnswers)}, Answer Number: {_numberOfAnswers}");
		_currentDifficulty.AverageDifficulty.Add( points );

		StatManager.RegisterAnswer(mathTask, points);

		if (mathTask.Correct == mathValue) {
			_currentScore = _currentScore + points;
			
			if (_currentScore >= _maxTasks) {
				_currentScore = 0;
				PlayerStats.Instance.AddPuggeMonster();
				RefreshTasks(GameModeType.Math);
			}

			GameManager.UIManager.SetExpBar( _currentScore );
			NextQuestion( mathTask);
		}
	}

    public void RegisterAnswer(WordTask wordTask, string buttonInputValue)
    {
        _numberOfAnswers++;

        float points = 1f * (1f / _numberOfAnswers);

        if (points < 0.4)
        {
            points = 0;
        }
        Debug.Log($"Correct answer = {buttonInputValue}, points = {points} / {1f * (1f / _numberOfAnswers)}, Answer Number: {_numberOfAnswers}");
        //_currentDifficulty.AverageDifficulty.Add(points);

        //StatManager.RegisterAnswer(mathTask, points);

        if (wordTask.Correct == buttonInputValue)
        {
            _currentScore = _currentScore + points;

            if (_currentScore >= _maxTasks)
            {
                _currentScore = 0;
                PuggeMonsterManager.AddPuggeMonster();
                RefreshTasks(GameModeType.Math);
            }

            GameManager.UIManager.SetExpBar(_currentScore);
            NextQuestion(wordTask);
        }
    }

    private void NextQuestion (MathTask mathTask) {
		_currentTaskIndex = _mathTasks.IndexOf(mathTask );
		_numberOfAnswers = 0;

		if (_currentTaskIndex + 1 < _mathTasks.Count) {
			_currentTaskIndex = _currentTaskIndex + 1;
		} else {
			_currentTaskIndex = 0;
			RefreshTasks(_gameMode);
			return;
		}
		MathTask newMathTask = _mathTasks[ _currentTaskIndex ];

		SwapQuestion( newMathTask );
	}

    private void NextQuestion(WordTask wordTask)
    {
        _currentTaskIndex = _wordTasks.IndexOf(wordTask);
        _numberOfAnswers = 0;

        if (_currentTaskIndex + 1 < _wordTasks.Count)
        {
            _currentTaskIndex = _currentTaskIndex + 1;
        }
        else
        {
            _currentTaskIndex = 0;
            RefreshTasks(_gameMode);
            return;
        }
        WordTask newWordTask = _wordTasks[_currentTaskIndex];

        SwapQuestion(newWordTask);
    }

    private void SwapQuestion ( MathTask newMathTask ) {
		GameManager.UIManager.MathQuestion(newMathTask );
	}

    private void SwapQuestion(WordTask newWordTask)
    {
        GameManager.UIManager.WordQuestion(newWordTask);
    }
}

/// <summary>
/// Helper to maintain average difficulty across _maxTasks in order to properly ascertain the difficulty curve it should adopt.
/// </summary>
public struct MathDifficulty {
	public List<float> AverageDifficulty; // the averedge score for the set of four generated questions
	public float CurrentDifficultyAverage { 
		get {
			float _sum = 0;
			foreach (float i in AverageDifficulty) {
				_sum += i;
			}
			return _sum / AverageDifficulty.Count; 
		} 
	}
}

public struct MathTask {
	public List<float> Components; // Array with 2 numbers
	public string Operator; // + - * or /
	public float Correct; // The correct answer.
	public List<float> Incorrect; // Incorrect options.
	public Sprite NumberSprite;
}
public struct LetterTask {
	public AudioClip LetterSound;
	public string Correct;
	public string[] Incorrect;
}
public struct WordTask {
	public Sprite WordSprite;
	public string Correct;
	public List<string> Incorrect ;
}
