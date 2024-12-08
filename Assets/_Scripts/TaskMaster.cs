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
	private List<WordTask> _wordTasks = new();
	private List<LetterTask> _letterTasks = new();

	public StudentPerformance CurrentStudentPerformance = new();
	private int _currentTaskIndex;
	private int _numberOfAnswers;
	private float _currentScore;
	private int _maxTasks = 4;
	private float _receivePuggemonScoreLimit;
	private float _lastGenerationTime;
	private float _lastAnswerTime;
	private float _spamTimeLimit;

	private void Awake() {
		_maxTasks = GameManager.QuestionSetSize;
		_spamTimeLimit = GameManager.QuestionSpamTimeLimitInMS;
		_receivePuggemonScoreLimit = GameManager.RecievePuggemonsterLimit;

		//_gameMode = GameManager.Instance.GameMode;
		CurrentStudentPerformance.Initialize( _maxTasks );
		GameManager.Instance.RegisterManager( this );
	}

	private void OnEnable() {
		// Register TaskMaster enabling GameManager.TaskMaster-syntax.
		GameManager.Instance.RegisterManager( this );
		//GameManager.Instance.OnGameModeUpdate += UpdateGameMode;
		GameManager.Instance.OnSceneLoad += RefreshTasks;
	}

	private void OnDisable() {
		// Empty argument de-registers the current TaskMaster
		GameManager.Instance.UnRegisterManager( this );

		//GameManager.Instance.OnGameModeUpdate -= UpdateGameMode;
		GameManager.Instance.OnSceneLoad -= RefreshTasks;
	}

	public void RefreshTasks( GameModeType gameMode ) {
		char[] difficultySet = new char[_maxTasks];

		switch ( gameMode ) {
			case GameModeType.Math:
				_mathTasks.Clear();
				difficultySet = GetDifficultySet( GameModeType.Math );

				// if there exists a mathCode, we're going to ship it.
				MathCode mathCode = new();
				mathCode.AppDecides = false;

				// If MathCode is stored in the GameManager Instance, replace our default instance.
				if ( GameManager.Instance.MathCode.Operator != default ) {
					mathCode = GameManager.Instance.MathCode;
				}

				for ( int i = 0; i < _maxTasks; i++ ) {
					// first implementation, Will be replaced when a difficulty system has been created.
					MathTask task = new MathTask();
					task.Components = new();
					task.Incorrect = new();
					task.DifficultyLetter = difficultySet[i];
					task.DifficultySet = difficultySet;

					//_mathTasks.Add( MathGenerator.GenerateMathQuestion( task.difficultyLetter.ToString() ) ); 
					_mathTasks.Add( MathGenerator.GenerateMathQuestion( task, mathCode ) );
				}

				_lastGenerationTime = Time.realtimeSinceStartup * 1000;

				break;
			case GameModeType.Words:
				_wordTasks.Clear();
				difficultySet = GetDifficultySet( GameModeType.Words );
				

				for ( int i = 0; i < _maxTasks; i++ ) {
					// first implementation, Will be replaced when a difficulty system has been created.
					WordTask task = new WordTask();

					task.DifficultyLetter = difficultySet[i];
					task.DifficultySet = difficultySet;
					WordGenerator.GenerateWordQuestionBasedOnPerformance( ref task );

					_wordTasks.Add( task );
				}

				//Debug.Log( $"WordTasks: {_wordTasks.Count}, first Task: {_wordTasks[0].Correct}" );
				_lastGenerationTime = Time.realtimeSinceStartup * 1000;

				break;
			case GameModeType.Letters:
				_letterTasks.Clear();
				difficultySet = GetDifficultySet( GameModeType.Letters );
				
				LetterCode letterCode = new();
				
				if (!GameManager.Instance.LetterCode.IsEmpty) {
					letterCode = GameManager.Instance.LetterCode;
				}
				
				for ( int i = 0; i < _maxTasks; i++ ) {
					// first implementation, Will be replaced when a difficulty system has been created.
					LetterTask task = new LetterTask();
					task.Mode = GameModeType.Letters;
					
					task.DifficultyLetter = difficultySet[i];
					task.DifficultySet = difficultySet;
					task = LetterGenerator.GenerateQuestionBasedOnPerformance( ref task, letterCode );
					
					_letterTasks.Add( task );
				}

				_lastGenerationTime = Time.realtimeSinceStartup * 1000;

				break;
			case GameModeType.LetterPicture:
				_letterTasks.Clear();
				difficultySet = GetDifficultySet( GameModeType.LetterPicture );

				for ( int i = 0; i < _maxTasks; i++ ) {
					// first implementation, Will be replaced when a difficulty system has been created.
					LetterTask task = new LetterTask();
					task.Mode = GameModeType.LetterPicture;
					task = LetterGenerator.GenerateQuestionBasedOnPerformance( ref task );

					task.DifficultyLetter = difficultySet[i];
					task.DifficultySet = difficultySet;

					_letterTasks.Add( task );
				}

				_lastGenerationTime = Time.realtimeSinceStartup * 1000;

				break;
			case GameModeType.None:
				break;
		}

		if ( _mathTasks.Count > 0 ) {
			MathTask mathTask = _mathTasks[0];
			SwapQuestion( mathTask );
		}

		if ( _wordTasks.Count > 0 ) {
			WordTask wordTask = _wordTasks[0];
			SwapQuestion( wordTask );
		}

		if ( _letterTasks.Count > 0 ) {
			LetterTask letterTask = _letterTasks[0];
			SwapQuestion( letterTask );
		}
	}

	/// <summary>
	/// Generates a set of characters denoting difficulty. uses _maxTasks to decide how many. Introduces a little bit of randomness.
	/// </summary>
	/// <param name="gameMode"></param>
	/// <returns></returns>
	private char[] GetDifficultySet( GameModeType gameMode ) {
		char[] difficultySet = new char[_maxTasks];
		float currentDifficultyModifier = 0f;

		for ( int i = 0; i < _maxTasks; i++ ) {
			if ( CurrentStudentPerformance.Sum > currentDifficultyModifier ) {
				// Chance distribution range: h m m 
				difficultySet[i] = ( Random.Range( 0, 3 ) == 0 ) ? 'h' : 'm';
			} else { // If PerformanceAverage is not above currentDifficultyModifier then we will randomize 60/30 between easy and medium question.
				// Chance distribution range: m e e
				difficultySet[i] = ( Random.Range( 0, 3 ) == 0 ) ? 'm' : 'e';
			}

			// Change currentDifficultyModifier depending on the newly added difficulty letter.
			switch ( difficultySet[i] ) {
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

	private bool DetectSpammer( float lastAnswerTime ) {
		float currentAnswerTime = ( Time.realtimeSinceStartup * 1000 ) - lastAnswerTime;

		return currentAnswerTime < _spamTimeLimit;
	}

#region Word Tasks Section

	private void NextQuestion( WordTask wordTask ) {
		_currentTaskIndex = _wordTasks.IndexOf( wordTask );
		_numberOfAnswers = 0;

		if ( _currentTaskIndex + 1 < _wordTasks.Count ) {
			_currentTaskIndex = _currentTaskIndex + 1;
		} else {
			_currentTaskIndex = 0;
			RefreshTasks( GameModeType.Words );

			return;
		}

		WordTask newWordTask = _wordTasks[_currentTaskIndex];

		SwapQuestion( newWordTask );
	}

	public void RegisterAnswer( WordTask wordTask, string buttonInputValue ) {
		_numberOfAnswers++;

		float points = 1f * ( 1f / _numberOfAnswers );

		if ( points < 0.4 ) {
			points = 0;
		}

		bool spamDetected = DetectSpammer( _lastAnswerTime );

		//Debug.Log($"Correct answer = {buttonInputValue}, points = {points} / {1f * (1f / _numberOfAnswers)}, Answer Number: {_numberOfAnswers}");
		Debug.Log( $"timeSinceLastResponse: {( Time.realtimeSinceStartup * 1000 ) - _lastAnswerTime}, IsSpamming: {spamDetected}" );
		if ( !spamDetected ) {
			if ( wordTask.Correct == buttonInputValue ) {
				_currentScore = _currentScore + points;

				if ( _currentScore >= _receivePuggemonScoreLimit ) // Was comparing to _maxTasks, wich is the ammount of tasks to generate. Changed to score limit
				{
					_currentScore = 0;
					int temp = PlayerStats.GetNewPuggeMonsterIndex;
					// Adding the puggemonster to your library now happens when you click the puggemon in the rewardAnimationScript.
					rewardAnimationScript.PlayRewardAnimation( temp );
				} else {
					// Play the CorrectAnswer sound if we are not also receiving a new Puggemon. This prevents a clash between the two audios.
					GameManager.CorrectAnswer();
				}

				CurrentStudentPerformance.Push( points );
				StatManager.RegisterAnswer( wordTask, buttonInputValue, points );

				GameManager.UIManager.SetExpBar( _currentScore / _receivePuggemonScoreLimit );

				NextQuestion( wordTask );
			} else {
				GameManager.WrongAnswer();
				CurrentStudentPerformance.Push( -1 * points );
				StatManager.RegisterAnswer( wordTask, buttonInputValue, -1 * points );
			}
		} else if ( wordTask.Correct == buttonInputValue ) {
			GameManager.CorrectAnswer();
			NextQuestion( wordTask );
		} else {
			GameManager.WrongAnswer();
		}

		_lastAnswerTime = Time.realtimeSinceStartup * 1000;
	}

	private void SwapQuestion( WordTask newWordTask ) {
		GameManager.UIManager.WordQuestion( newWordTask );
	}

# endregion

#region Math Task Section

	public void RegisterAnswer( MathTask mathTask, float mathValue ) {
		_numberOfAnswers++;

		float points = 1f * ( 1f / _numberOfAnswers );

		if ( points < 0.4 ) {
			points = 0;
		}

		bool spamDetected = DetectSpammer( _lastAnswerTime );

		//Debug.Log($"Selected answer = {mathValue}, Correct Answer: {mathTask.Correct}, points = {points} / {1f * (1f / _numberOfAnswers)}, Answer Number: {_numberOfAnswers}");
		Debug.Log( $"timeSinceLastResponse: {( Time.realtimeSinceStartup * 1000 ) - _lastAnswerTime}, IsSpamming: {spamDetected}" );

		// Floating point comparison should use Mathf.Approximately
		if ( !spamDetected ) {
			if ( Mathf.Approximately( mathTask.Correct, mathValue ) ) {
				StatManager.RegisterAnswer( mathTask, mathValue, points );
				_currentScore += points;
				CurrentStudentPerformance.Push( points );

				if ( _currentScore >= _receivePuggemonScoreLimit ) {
					_currentScore = 0;
					int temp = PlayerStats.GetNewPuggeMonsterIndex;
					// Adding the puggemonster to your library now happens when you click the puggemon in the rewardAnimationScript.
					rewardAnimationScript.PlayRewardAnimation( temp );
				} else {
					// Play the CorrectAnswer sound if we are not also receiving a new Puggemon. This prevents a clash between the two audios.
					GameManager.CorrectAnswer();
				}

				GameManager.UIManager.SetExpBar( _currentScore / _receivePuggemonScoreLimit );

				NextQuestion( mathTask );
			} else {
				GameManager.WrongAnswer();
				StatManager.RegisterAnswer( mathTask, mathValue, -1 * points );
				CurrentStudentPerformance.Push( points * -1 );
			}
		} else if ( Mathf.Approximately( mathTask.Correct, mathValue ) ) {
			GameManager.CorrectAnswer();
			NextQuestion( mathTask );
		} else {
			GameManager.WrongAnswer();
		}

		_lastAnswerTime = Time.realtimeSinceStartup * 1000;
	}

	private void NextQuestion( MathTask mathTask ) {
		_currentTaskIndex = _mathTasks.IndexOf( mathTask );
		_numberOfAnswers = 0;

		if ( _currentTaskIndex + 1 < _mathTasks.Count ) {
			_currentTaskIndex = _currentTaskIndex + 1;
		} else {
			_currentTaskIndex = 0;
			RefreshTasks( GameModeType.Math );

			return;
		}

		MathTask newMathTask = _mathTasks[_currentTaskIndex];

		SwapQuestion( newMathTask );
	}

	private void SwapQuestion( MathTask newMathTask ) {
		GameManager.UIManager.MathQuestion( newMathTask );
	}

#endregion

#region Letter Task Section

	public void RegisterAnswer( LetterTask newLetterTask, string buttonInputValue ) {
		_numberOfAnswers++;

		float points = 1f * ( 1f / _numberOfAnswers );

		if ( points < 0.4 ) {
			points = 0;
		}

		bool spamDetected = DetectSpammer( _lastAnswerTime );

		//Debug.Log($"Correct answer = {buttonInputValue}, points = {points} / {1f * (1f / _numberOfAnswers)}, Answer Number: {_numberOfAnswers}");
		Debug.Log( $"timeSinceLastResponse: {( Time.realtimeSinceStartup * 1000 ) - _lastAnswerTime}, IsSpamming: {spamDetected}" );
		if ( !spamDetected ) {
			if ( newLetterTask.Correct == buttonInputValue ) {
				_currentScore = _currentScore + points;

				if ( _currentScore >= _receivePuggemonScoreLimit ) { // Was comparing to _maxTasks, wich is the ammount of tasks to generate. Changed to score limit
					_currentScore = 0;
					int temp = PlayerStats.GetNewPuggeMonsterIndex;
					// Adding the puggemonster to your library now happens when you click the puggemon in the rewardAnimationScript.
					rewardAnimationScript.PlayRewardAnimation( temp );
				} else {
					// Play the CorrectAnswer sound if we are not also receiving a new Puggemon. This prevents a clash between the two audios.
					GameManager.CorrectAnswer();
				}

				CurrentStudentPerformance.Push( points );
				StatManager.RegisterAnswer( newLetterTask, buttonInputValue, points );

				GameManager.UIManager.SetExpBar( _currentScore / _receivePuggemonScoreLimit );

				NextQuestion( newLetterTask );
			} else {
				GameManager.WrongAnswer();
				CurrentStudentPerformance.Push( -1 * points );
				StatManager.RegisterAnswer( newLetterTask, buttonInputValue, -1 * points );
			}
		} else if ( newLetterTask.Correct == buttonInputValue ) {
			GameManager.CorrectAnswer();
			NextQuestion( newLetterTask );
		} else {
			GameManager.WrongAnswer();
		}

		_lastAnswerTime = Time.realtimeSinceStartup * 1000;
	}

	private void NextQuestion( LetterTask letterTask ) {
		_currentTaskIndex = _letterTasks.IndexOf( letterTask );
		_numberOfAnswers = 0;

		if ( _currentTaskIndex + 1 < _letterTasks.Count ) {
			_currentTaskIndex = _currentTaskIndex + 1;
		} else {
			_currentTaskIndex = 0;
			
			RefreshTasks( letterTask.Mode );

			return;
		}

		LetterTask newWordTask = _letterTasks[_currentTaskIndex];

		SwapQuestion( newWordTask );
	}
	
	private void SwapQuestion( LetterTask newLetterTask ) {
		GameManager.UIManager.LetterQuestion( newLetterTask );
	}

#endregion
}

/// <summary>
/// Helper to maintain average difficulty across _maxTasks in order to properly ascertain the difficulty curve it should adopt.
/// </summary>
public class StudentPerformance {
	private List<float> _performanceRegister = new(); // the average score for the set of four generated questions
	private int _maxSize = GameManager.QuestionSetSize * 2;
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
	public Sprite TaskSprite;
	public string DifficultyLevelStringValue;
	public char DifficultyLetter;
	public char[] DifficultySet;

	public bool Equals(MathTask other) {
		return Equals(Components, other.Components) && Operator == other.Operator && Correct.Equals(other.Correct) && Equals(Incorrect, other.Incorrect) && Equals(TaskSprite, other.TaskSprite) && DifficultyLevelStringValue == other.DifficultyLevelStringValue && DifficultyLetter == other.DifficultyLetter && Equals(DifficultySet, other.DifficultySet);
	}

	public override bool Equals(object obj) {
		return obj is MathTask other && Equals(other);
	}

	public override int GetHashCode() {
		return HashCode.Combine(Components, Operator, Correct, Incorrect, TaskSprite, DifficultyLevelStringValue, DifficultyLetter, DifficultySet);
	}
}
public struct LetterTask : IEquatable<LetterTask> {
	public SimpleAudioEvent LetterSound;
	public Sprite TaskSprite;
	public string Correct;
	public GameModeType Mode;
	public List<string> Incorrect;
	public string DifficultyLevelStringValue;
	public char DifficultyLetter;
	public char[] DifficultySet;
	public string StorageKey;

	public bool Equals( LetterTask other ) {
		return Equals( LetterSound, other.LetterSound ) && Equals( TaskSprite, other.TaskSprite ) && Correct == other.Correct && Mode == other.Mode && Equals( Incorrect, other.Incorrect ) && DifficultyLevelStringValue == other.DifficultyLevelStringValue && DifficultyLetter == other.DifficultyLetter && Equals( DifficultySet, other.DifficultySet ) && StorageKey == other.StorageKey;
	}

	public override bool Equals( object obj ) {
		return obj is LetterTask other && Equals( other );
	}

	public override int GetHashCode() {
		return HashCode.Combine( LetterSound, TaskSprite, Correct, (int)Mode, Incorrect, DifficultyLevelStringValue, DifficultyLetter, DifficultySet );
	}
}
public struct WordTask : IEquatable<WordTask> {
	public Sprite TaskSprite;
	public string Correct;
	public List<string> Incorrect;
	public string DifficultyLevelStringValue;
	public char DifficultyLetter;
	public char[] DifficultySet;

	public bool Equals(WordTask other) {
		return Equals(TaskSprite, other.TaskSprite) && Correct == other.Correct && Equals(Incorrect, other.Incorrect) && DifficultyLevelStringValue == other.DifficultyLevelStringValue && DifficultyLetter == other.DifficultyLetter && Equals(DifficultySet, other.DifficultySet);
	}

	public override bool Equals(object obj) {
		return obj is WordTask other && Equals(other);
	}

	public override int GetHashCode() {
		return HashCode.Combine(TaskSprite, Correct, Incorrect, DifficultyLevelStringValue, DifficultyLetter, DifficultySet);
	}
}

public enum LetterMode {
	Picture,
	Sound
	
}