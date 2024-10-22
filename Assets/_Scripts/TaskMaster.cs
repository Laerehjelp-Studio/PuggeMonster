using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class TaskMaster : MonoBehaviour {
	
	private GameModeType _gameMode;

	[SerializeField] private PuggeMonsterRewardAnimationBehaviour rewardAnimationScript;

	private List<MathTask> _mathTasks = new();
	private List<LetterTask> _letterTasks = new();
	private List<WordTask> _wordTasks = new();

	private StudentPerformance _currentStudentPerformance = new ( );
	[SerializeField] private int _maxTasks = 4;
	private int _currentTaskIndex = 0;
	private int _numberOfAnswers = 0;
	private float _currentScore = 0;

	private void Awake () {
		//_gameMode = GameManager.Instance.GameMode;
		_currentStudentPerformance.Initialize( _maxTasks );
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
				char[] difficultySet = new char[ _maxTasks ];
				difficultySet = GetDifficultySet(GameModeType.Math);
				MathCode mathCode = new MathCode();
				if (GameManager.Instance.MathCode.Operator != default) {
					mathCode = GameManager.Instance.MathCode;
				}
				for (int i = 0; i < _maxTasks; i++) {
					// first implementation, Will be replaced when a difficulty system has been created.
					MathTask task = new MathTask();
					task.Components = new();
					task.Incorrect = new();
					task.difficultyLetter = difficultySet[i];
					task.difficultySet = difficultySet;

					
					//_mathTasks.Add( MathGenerator.GenerateMathQuestion( task.difficultyLetter.ToString() ) ); 
					_mathTasks.Add( MathGenerator.GenerateMathQuestion( mathCode, task ) );
				}
				break;
			case GameModeType.Words:
				_wordTasks.Clear();

				for (int i = 0; i < _maxTasks; i++)
				{
					// first implementation, Will be replaced when a difficulty system has been created.

					_wordTasks.Add(WordGenerator.GenerateWordQuestion());
				}
				Debug.Log($"WordTasks: {_wordTasks.Count}, first Task: {_wordTasks[0].Correct}");
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
		if (_wordTasks.Count > 0) {
			WordTask wordTask = _wordTasks[ 0 ];
			SwapQuestion( wordTask );
		}
	}
	/// <summary>
	/// Generates a set of characters denoting difficulty. uses _maxTasks to decide how many. Introduces a little bit of randomness.
	/// </summary>
	/// <param name="math"></param>
	/// <returns></returns>
	private char[] GetDifficultySet ( GameModeType math ) {
		char[] difficultySet = new char[_maxTasks];
		float currentDifficultyModifier = 0f;
		
		for (int i = 0; i < _maxTasks; i++) {
			// If it is the first or last difficulty-letter, make let's check based on the _difficultyStudentPerformance.PerformanceAverage randomising between hard and medium
			if (i == 0 || i == _maxTasks - 1) {
				// if _difficultyStudentPerformance.PerformanceAverage is above the changing currentDifficulty let's do random between hard and medium
				if (_currentStudentPerformance.Average > currentDifficultyModifier) { 
					difficultySet[i] = (Random.Range( 0, 3 ) == 0) ? 'h' : 'm';
				} else { // If PerformanceAverage is not above currentDifficultyModifier then we will randomize 60/30 between easy and medium question.
					difficultySet[ i] = (Random.Range( 0, 3 ) == 0) ? 'm' : 'e';
				}
			} else {
				difficultySet[ i ] = 'e';
			}
			// Change currentDifficultyModifier depending on the newly added difficulty letter.
			switch (difficultySet[ i ]) {
				case 'h':
					currentDifficultyModifier += 2;
					break;
				case 'm':
					currentDifficultyModifier += 1;
					break;
				case 'e':
					currentDifficultyModifier -= 1;
					break;
			}
		}
		Debug.Log($"Average: {_currentStudentPerformance.Average} Sum: {_currentStudentPerformance.Sum}, {difficultySet[ 0 ]} {difficultySet[ 1 ]} {difficultySet[ 2 ]} {difficultySet[ 3 ]}, {currentDifficultyModifier}" );
		return difficultySet;
	}

	public string GetDifficultyLetter () {
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

		//Debug.Log($"Selected answer = {mathValue}, Corrrect Answer: {mathTask.Correct}, points = {points} / {1f * (1f / _numberOfAnswers)}, Answer Number: {_numberOfAnswers}");
		_currentStudentPerformance.Push( points );

		if (mathTask.Correct == mathValue) {
			StatManager.RegisterAnswer(mathTask, mathValue, points );
			
			_currentScore = _currentScore + points;
			
			if (_currentScore >= _maxTasks) {
				_currentScore = 0;
				int temp = Random.Range(0, PlayerStats.Instance.puggemonsterList.Length);
				PlayerStats.Instance.AddPuggeMonster(temp);
				rewardAnimationScript.PlayRewardAnimation(temp);
				RefreshTasks(GameModeType.Math);
			}

			GameManager.UIManager.SetExpBar( _currentScore );
			NextQuestion( mathTask);
		} else {
			StatManager.RegisterAnswer( mathTask, mathValue, -1 * points );
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
		//_currentDifficulty.AverageDifficulty.Push(points);

		//StatManager.RegisterAnswer(mathTask, points);

		if (wordTask.Correct == buttonInputValue)
		{
			_currentScore = _currentScore + points;

			if (_currentScore >= _maxTasks)
			{
				_currentScore = 0;
				int temp = Random.Range(0, PlayerStats.Instance.puggemonsterList.Length);
				PlayerStats.Instance.AddPuggeMonster(temp);
				rewardAnimationScript.PlayRewardAnimation(temp);

				RefreshTasks(GameModeType.Words);
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
public class StudentPerformance {
	private List<float> _performanceRegister = new(); // the average score for the set of four generated questions
	private int _maxSize = 2;
	public float Average { 
		get {
			return (Sum == 0 && Sum == _performanceRegister.Count) ? 0: Sum / _performanceRegister.Count; 
		} 
	}
	public float Sum {
		get {
			float _sum = 0;
			foreach (float i in _performanceRegister) {
				_sum += i;
			}
			return _sum;
		} 
	}

	public void Push (float item) {
		_performanceRegister.Add(item);

		if (_performanceRegister.Count > _maxSize) {
			_performanceRegister.Remove( _performanceRegister[0] );
			return; 
		}
	}

	public void Initialize ( int maxSize ) {
		_maxSize = maxSize;
	}
}

public struct MathTask {
	public List<float> Components; // Array with 2 numbers
	public string Operator; // + - * or /
	public float Correct; // The correct answer.
	public List<float> Incorrect; // Incorrect options.
	public Sprite NumberSprite;
	public string difficultyLevelStringValue;
	public char difficultyLetter;
	public char[] difficultySet;
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
