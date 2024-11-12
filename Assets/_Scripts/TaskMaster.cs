using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// 
/// </summary>
public class TaskMaster : MonoBehaviour {
	
	private GameModeType _gameMode;

	[SerializeField] private PuggeMonsterRewardAnimationBehaviour rewardAnimationScript;

	private List<MathTask> _mathTasks = new();
	private List<LetterTask> _letterTasks = new();
	private List<WordTask> _wordTasks = new();

	public StudentPerformance CurrentStudentPerformance = new ( );
	private int _currentTaskIndex;
	private int _numberOfAnswers;
	private float _currentScore;
	private int _maxTasks = 4;
	private float _receivePuggemonScoreLimit;
	private float _lastGenerationTime;
	private float _lastAnswerTime;
	private float _spamTimeLimit;
	private int _lastPuggeMonsterAwarded;

	private void Awake () {
		_maxTasks = GameManager.QuestionSetSize;
		_spamTimeLimit = GameManager.QuestionSpamTimeLimitInMS;
		_receivePuggemonScoreLimit = GameManager.RecievePuggemonsterLimit;

		//_gameMode = GameManager.Instance.GameMode;
		CurrentStudentPerformance.Initialize( _maxTasks );
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
		char[] difficultySet = new char[ _maxTasks ];
		switch (gameMode) {
			case GameModeType.Math:
				_mathTasks.Clear();
				difficultySet = GetDifficultySet(GameModeType.Math);
				
				// if there exists a mathCode, we're going to ship it.
				MathCode mathCode = new();
				mathCode.AppDecides = false;

				// If MathCode is stored in the GameManager Instance, replace our default instance.
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
					_mathTasks.Add( MathGenerator.GenerateMathQuestion( task, mathCode ) );
				}
				
				_lastGenerationTime = Time.realtimeSinceStartup * 1000;
				break;
			case GameModeType.Words:
				_wordTasks.Clear();
				difficultySet = GetDifficultySet(GameModeType.Words);
				
				for (int i = 0; i < _maxTasks; i++)
				{
					// first implementation, Will be replaced when a difficulty system has been created.
					WordTask task = new WordTask();
					
					task.difficultyLetter = difficultySet[i];
					task.difficultySet = difficultySet;
					WordGenerator.GenerateWordQuestionBasedOnPerformance(ref task);
					
					_wordTasks.Add(task);
					
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
	/// <param name="gameMode"></param>
	/// <returns></returns>
	private char[] GetDifficultySet ( GameModeType gameMode ) {
		char[] difficultySet = new char[_maxTasks];
		float currentDifficultyModifier = 0f;
		
		for (int i = 0; i < _maxTasks; i++) {
			if (CurrentStudentPerformance.Sum > currentDifficultyModifier) { 
				// Chance distribution range: h m m 
				difficultySet[i] = (Random.Range( 0, 3 ) == 0) ? 'h' : 'm';
			} else { // If PerformanceAverage is not above currentDifficultyModifier then we will randomize 60/30 between easy and medium question.
				// Chance distribution range: m e e
				difficultySet[ i] = (Random.Range( 0, 3 ) == 0) ? 'm' : 'e';
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
		//Debug.Log($"Average: {_currentStudentPerformance.Average} Sum: {_currentStudentPerformance.Sum}, {difficultySet[ 0 ]} {difficultySet[ 1 ]} {difficultySet[ 2 ]} {difficultySet[ 3 ]}, {currentDifficultyModifier}" );
		return difficultySet;
	}
	
	public void RegisterAnswer ( MathTask mathTask, float mathValue ) {
		_numberOfAnswers++;

		float points = 1f * (1f / _numberOfAnswers);

		if (points < 0.4) {
			points = 0;
		}
		
		bool cheatDetected = DetectCheater(_lastAnswerTime);
		
		//Debug.Log($"Selected answer = {mathValue}, Correct Answer: {mathTask.Correct}, points = {points} / {1f * (1f / _numberOfAnswers)}, Answer Number: {_numberOfAnswers}");
		Debug.Log($"timeSinceLastResponse: {(Time.realtimeSinceStartup * 1000) - _lastAnswerTime }, IsCheating: {cheatDetected}");
		
		// Floating point comparison should use Mathf.Approximately
		if (!cheatDetected) {
			if (Mathf.Approximately(mathTask.Correct, mathValue)) {
				StatManager.RegisterAnswer(mathTask, mathValue, points );
				_currentScore +=  points;
				CurrentStudentPerformance.Push( points );
				
				if (_currentScore >= _receivePuggemonScoreLimit) {
					_currentScore = 0;
					int temp = GetPuggeMonsterIndex(_lastPuggeMonsterAwarded);
					PlayerStats.Instance.AddPuggeMonster(temp);
					_lastPuggeMonsterAwarded = temp;
					rewardAnimationScript.PlayRewardAnimation(temp);
				}

				GameManager.UIManager.SetExpBar( _currentScore / _receivePuggemonScoreLimit);
				
				NextQuestion( mathTask);
			} else {
				StatManager.RegisterAnswer( mathTask, mathValue, -1 * points );
				CurrentStudentPerformance.Push( points * -1 );
			}
		} else if (Mathf.Approximately(mathTask.Correct, mathValue)) {
			NextQuestion( mathTask);
		}
		_lastAnswerTime = Time.realtimeSinceStartup * 1000;
	}

	private bool DetectCheater(float lastAnswerTime) {
		float currentAnswerTime = (Time.realtimeSinceStartup * 1000) - lastAnswerTime;
		return currentAnswerTime < _spamTimeLimit;
	}

	private int GetPuggeMonsterIndex(int notThisPuggeMonster) {
		int newPuggeMonsterIndex = Random.Range(0, PlayerStats.Instance.puggemonsterList.Length);
		
		if (notThisPuggeMonster == newPuggeMonsterIndex) {
			GetPuggeMonsterIndex(notThisPuggeMonster);
		}
		
		return newPuggeMonsterIndex;
	}
	
	public void RegisterAnswer(WordTask wordTask, string buttonInputValue) {
		_numberOfAnswers++;

		float points = 1f * (1f / _numberOfAnswers);

		if (points < 0.4) {
			points = 0;
		}
		
		bool cheatDetected = DetectCheater(_lastAnswerTime);
		
		//Debug.Log($"Correct answer = {buttonInputValue}, points = {points} / {1f * (1f / _numberOfAnswers)}, Answer Number: {_numberOfAnswers}");

		if (!cheatDetected) {
			if (wordTask.Correct == buttonInputValue) {
				_currentScore = _currentScore + points;

				if (_currentScore >= _receivePuggemonScoreLimit) // Was comparing to _maxTasks, wich is the ammount of tasks to generate. Changed to score limit
				{
					_currentScore = 0;
					int temp = Random.Range(0, PlayerStats.Instance.puggemonsterList.Length);
					PlayerStats.Instance.AddPuggeMonster(temp);
					rewardAnimationScript.PlayRewardAnimation(temp);
				}

				CurrentStudentPerformance.Push(points);
				StatManager.RegisterAnswer(wordTask, buttonInputValue, points);
				
				GameManager.UIManager.SetExpBar(_currentScore / _receivePuggemonScoreLimit);
				NextQuestion(wordTask);
			} else {
				CurrentStudentPerformance.Push(-1 * points);
				StatManager.RegisterAnswer(wordTask, buttonInputValue, -1 * points);
			}
		} else if (wordTask.Correct == buttonInputValue) {
			NextQuestion(wordTask);
		}
		_lastAnswerTime = Time.realtimeSinceStartup * 1000;
	}

	private void NextQuestion (MathTask mathTask) {
		_currentTaskIndex = _mathTasks.IndexOf( mathTask );
		_numberOfAnswers = 0;

		if (_currentTaskIndex + 1 < _mathTasks.Count) {
			_currentTaskIndex = _currentTaskIndex + 1;
		} else {
			_currentTaskIndex = 0;
			RefreshTasks(GameModeType.Math);
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
			RefreshTasks(GameModeType.Words);
			return;
		}
		WordTask newWordTask = _wordTasks[_currentTaskIndex];

		SwapQuestion(newWordTask);
	}

	private void SwapQuestion ( MathTask newMathTask ) {
		GameManager.UIManager.MathQuestion( newMathTask );
	}

	private void SwapQuestion(WordTask newWordTask)
	{
		GameManager.UIManager.WordQuestion( newWordTask );
	}
}

/// <summary>
/// Helper to maintain average difficulty across _maxTasks in order to properly ascertain the difficulty curve it should adopt.
/// </summary>
public class StudentPerformance {
	private List<float> _performanceRegister = new(); // the average score for the set of four generated questions
	private int _maxSize = GameManager.QuestionSetSize;
	public float Average => (Sum == 0 && Mathf.Approximately(Sum, _performanceRegister.Count)) ? 0 : Sum / _performanceRegister.Count;

	public float Sum {
		get {
			float _sum = 0;
			foreach (float i in _performanceRegister) {
				_sum += i;
			}

			return _sum;
		}
	}

	public void Push(float item) {
		_performanceRegister.Add(item);

		if (_performanceRegister.Count > _maxSize) {
			_performanceRegister.Remove(_performanceRegister[0]);
		}
	}

	public void Initialize(int maxSize) {
		_maxSize = maxSize;
	}
}

public struct MathTask : IEquatable<MathTask> {
	public List<float> Components; // Array with 2 numbers
	public string Operator; // + - * or /
	public float Correct; // The correct answer.
	public List<float> Incorrect; // Incorrect options.
	public Sprite NumberSprite;
	public string difficultyLevelStringValue;
	public char difficultyLetter;
	public char[] difficultySet;

	public bool Equals(MathTask other) {
		return Equals(Components, other.Components) && Operator == other.Operator && Correct.Equals(other.Correct) && Equals(Incorrect, other.Incorrect) && Equals(NumberSprite, other.NumberSprite) && difficultyLevelStringValue == other.difficultyLevelStringValue && difficultyLetter == other.difficultyLetter && Equals(difficultySet, other.difficultySet);
	}

	public override bool Equals(object obj) {
		return obj is MathTask other && Equals(other);
	}

	public override int GetHashCode() {
		return HashCode.Combine(Components, Operator, Correct, Incorrect, NumberSprite, difficultyLevelStringValue, difficultyLetter, difficultySet);
	}
}
public struct LetterTask {
	public AudioClip LetterSound;
	public string Correct;
	public string[] Incorrect;
}

public struct WordTask : IEquatable<WordTask> {
	public Sprite WordSprite;
	public string Correct;
	public List<string> Incorrect;
	public string difficultyLevelStringValue;
	public char difficultyLetter;
	public char[] difficultySet;

	public bool Equals(WordTask other) {
		return Equals(WordSprite, other.WordSprite) && Correct == other.Correct && Equals(Incorrect, other.Incorrect) && difficultyLevelStringValue == other.difficultyLevelStringValue && difficultyLetter == other.difficultyLetter && Equals(difficultySet, other.difficultySet);
	}

	public override bool Equals(object obj) {
		return obj is WordTask other && Equals(other);
	}

	public override int GetHashCode() {
		return HashCode.Combine(WordSprite, Correct, Incorrect, difficultyLevelStringValue, difficultyLetter, difficultySet);
	}
}
