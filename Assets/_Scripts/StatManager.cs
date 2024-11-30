
using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Random = UnityEngine.Random;

static public class StatManager {
	private static int _generalWordMasteryMaxValue;
	private static int _generalMathMasteryMaxValue;
	private static OperatorStore _operatorStore;
	private static List<string> _generalMathMasteryList = new();
	
	private static List<string> _generalWordMasteredList = new();
	private static List<string> _generalWordDifficultyList = new();
	private static Dictionary<string, float> _generalWordMasteryScores = new();
	
	
	private static List<string> _generalLetterPictureMasteredList = new();
	private static List<string> _generalLetterPictureDifficultyList = new();
	private static Dictionary<string, float> _generalLetterPictureMasteryScores = new();
	
	private static List<string> _generalLetterSoundMasteredList = new();
	private static List<string> _generalLetterSoundDifficultyList = new();
	private static Dictionary<string, float> _generalLetterSoundMasteryScores = new(){{"A",0f},{"B",0f},{"C",0f},{"D",0f},{"E",0f},{"F",0f},{"G",0f},{"H",0f},{"I",0f},{"J",0f},{"K",0f},{"L",0f},{"M",0f},{"N",0f},{"O",0f},{"P",0f},{"Q",0f},{"R",0f},{"S",0f},{"T",0f},{"U",0f},{"V",0f},{"W",0f},{"X",0f},{"Y",0f},{"Z",0f},{"Æ",0f},{"Ø",0f},{"Å",0f}};
	public static Action OnDatabaseUpdate { get; set; } = delegate { };
	public static Action OnMathGeneralMastery { get; set; } = delegate { };

	public static int GeneralMathMathMasteryMaxValue {
		get {
			if (_generalMathMasteryMaxValue == 0) {
				CountMaxGeneralMathMasteryValue(_operatorStore.Addition.OneStats, _operatorStore.Addition.TensStats);
			}

			return _generalMathMasteryMaxValue;
		}
	}

	public static int GeneralWordMasteryMaxValue {
		get {
			if (_generalWordMasteryMaxValue == 0) {
				_generalWordMasteryMaxValue = WordQuestionLibrary.GetMaxWordCount;
			}

			return _generalWordMasteryMaxValue;
		}
	}

	private static bool initialized;

#region Initialization

	/// <summary>
	/// Initialize all dictionaries, and lists we are using for storage.
	/// </summary>
	public static void Initialize(float defaultMasteryScore = 0f) {
		if (initialized) {
			//Debug.LogWarning($"[StatManager.Initialize] Initializing GeneralWordMastery, Initializing General Letter Picture Difficulty List].");
			if (_generalWordMasteryScores.Count != WordQuestionLibrary.GetWordList.Count) {
				InitializeGeneralWordMastery();
			}

			if (_generalLetterPictureMasteryScores == null) {
				_generalLetterPictureMasteryScores = new();
			}
			if (_generalLetterPictureMasteryScores.Count == 0 || _generalLetterPictureMasteryScores.Count != WordQuestionLibrary.GetWordList.Count) {
				InitializeGeneralLetterPictureMastery();
			}
			if (_generalLetterSoundMasteryScores == null) {
				_generalLetterSoundMasteryScores = new();
			}
			if (_generalLetterSoundMasteryScores.Count == 0 || _generalLetterSoundMasteryScores.Count != LetterSoundQuestionLibrary.GetLetterList.Count) {
				InitializeGeneralLetterSoundMastery();
			}
			
			return;
		}

		Debug.Log("Initializing StatManager");
		Dictionary<string, float> _initDictAddition = new();
		List<string> _initListAddition = new();
		Dictionary<string, float> _initDictSubtraction = new();
		List<string> _initListSubtraction = new();
		Dictionary<string, float> _initDictMultiplication = new();
		List<string> _initListMultiplication = new();
		Dictionary<string, float> _initDictDivision = new();
		List<string> _initListDivision = new();

		for (int first = 0; first < 10; first++) {
			for (int second = 0; second < 10; second++) {
				string _mathAddition = $"{first}+{second}";
				_initDictAddition.TryAdd(_mathAddition, 0f);

				if (!_initListAddition.Contains(_mathAddition)) {
					_initListAddition.Add(_mathAddition);
				}

				string _mathSubtraction = $"{first}-{second}";
				_initDictSubtraction.TryAdd(_mathSubtraction, 0f);

				if (!_initListSubtraction.Contains(_mathSubtraction)) {
					_initListSubtraction.Add(_mathSubtraction);
				}

				string _mathMultiplication = $"{first}*{second}";
				_initDictMultiplication.TryAdd(_mathMultiplication, 0f);

				if (!_initListMultiplication.Contains(_mathMultiplication)) {
					_initListMultiplication.Add(_mathMultiplication);
				}

				string _mathDivision = $"{first}/{second}";
				_initDictDivision.TryAdd(_mathDivision, 0f);

				if (!_initListDivision.Contains(_mathDivision)) {
					_initListDivision.Add(_mathDivision);
				}
			}
		}

		Dictionary<string, float> _initDictZeroRemovedAddition = RemoveZeroOperatorZero(_initDictAddition, "+");
		_operatorStore.Addition.DecimalStats = new(_initDictZeroRemovedAddition);
		_operatorStore.Addition.OneStats = new(_initDictAddition);
		_operatorStore.Addition.TensStats = new(_initDictZeroRemovedAddition);
		_operatorStore.Addition.HundredsStats = new(_initDictZeroRemovedAddition);
		_operatorStore.Addition.ThousandsStats = new(_initDictZeroRemovedAddition);

		List<string> _initListZeroRemovedAddition = RemoveZeroOperatorZero(_initListAddition, "+");
		_operatorStore.Addition.DecimalDifficultySorted = new(ShuffleList(_initListZeroRemovedAddition));
		_operatorStore.Addition.OneDifficultySorted = new(ShuffleList(_initListAddition));
		_operatorStore.Addition.TensDifficultySorted = new(ShuffleList(_initListZeroRemovedAddition));
		_operatorStore.Addition.HundredsDifficultySorted = new(ShuffleList(_initListZeroRemovedAddition));
		_operatorStore.Addition.ThousandsDifficultySorted = new(ShuffleList(_initListZeroRemovedAddition));

		Dictionary<string, float> _initDictZeroRemovedSubtraction = RemoveZeroOperatorZero(_initDictSubtraction, "-");
		_operatorStore.Subtraction.DecimalStats = new(_initDictZeroRemovedSubtraction);
		_operatorStore.Subtraction.OneStats = new(_initDictSubtraction);
		_operatorStore.Subtraction.TensStats = new(_initDictZeroRemovedSubtraction);
		_operatorStore.Subtraction.HundredsStats = new(_initDictZeroRemovedSubtraction);
		_operatorStore.Subtraction.ThousandsStats = new(_initDictZeroRemovedSubtraction);

		List<string> _initListZeroRemovedSubtraction = RemoveZeroOperatorZero(_initListSubtraction, "+");
		_operatorStore.Subtraction.DecimalDifficultySorted = new(ShuffleList(_initListZeroRemovedSubtraction));
		_operatorStore.Subtraction.OneDifficultySorted = new(ShuffleList(_initListSubtraction));
		_operatorStore.Subtraction.TensDifficultySorted = new(ShuffleList(_initListZeroRemovedSubtraction));
		_operatorStore.Subtraction.HundredsDifficultySorted = new(ShuffleList(_initListZeroRemovedSubtraction));
		_operatorStore.Subtraction.ThousandsDifficultySorted = new(ShuffleList(_initListZeroRemovedSubtraction));

		Dictionary<string, float> _initDictZeroRemovedDivision = RemoveZeroOperatorZero(_initDictDivision, "/");
		_operatorStore.Division.DecimalStats = new(_initDictZeroRemovedDivision);
		_operatorStore.Division.OneStats = new(_initDictDivision);
		_operatorStore.Division.TensStats = new(_initDictZeroRemovedDivision);
		_operatorStore.Division.HundredsStats = new(_initDictZeroRemovedDivision);
		_operatorStore.Division.ThousandsStats = new(_initDictZeroRemovedDivision);

		List<string> _initListZeroRemovedDivision = RemoveZeroOperatorZero(_initListDivision, "/");
		_operatorStore.Division.DecimalDifficultySorted = new(ShuffleList(_initListZeroRemovedDivision));
		_operatorStore.Division.OneDifficultySorted = new(ShuffleList(_initListDivision));
		_operatorStore.Division.TensDifficultySorted = new(ShuffleList(_initListZeroRemovedDivision));
		_operatorStore.Division.HundredsDifficultySorted = new(ShuffleList(_initListZeroRemovedDivision));
		_operatorStore.Division.ThousandsDifficultySorted = new(ShuffleList(_initListZeroRemovedDivision));

		Dictionary<string, float> _initDictZeroRemovedMultiplication = RemoveZeroOperatorZero(_initDictMultiplication, "*");
		_operatorStore.Multiplication.DecimalStats = new(_initDictZeroRemovedMultiplication);
		_operatorStore.Multiplication.OneStats = new(_initDictMultiplication);
		_operatorStore.Multiplication.TensStats = new(_initDictZeroRemovedMultiplication);
		_operatorStore.Multiplication.HundredsStats = new(_initDictZeroRemovedMultiplication);
		_operatorStore.Multiplication.ThousandsStats = new(_initDictZeroRemovedMultiplication);

		List<string> _initListZeroRemovedMultiplication = RemoveZeroOperatorZero(_initListMultiplication, "*");
		_operatorStore.Multiplication.DecimalDifficultySorted = new(ShuffleList(_initListZeroRemovedMultiplication));
		_operatorStore.Multiplication.OneDifficultySorted = new(ShuffleList(_initListMultiplication));
		_operatorStore.Multiplication.TensDifficultySorted = new(ShuffleList(_initListZeroRemovedMultiplication));
		_operatorStore.Multiplication.HundredsDifficultySorted = new(ShuffleList(_initListZeroRemovedMultiplication));
		_operatorStore.Multiplication.ThousandsDifficultySorted = new(ShuffleList(_initListZeroRemovedMultiplication));

		initialized = true;

		CountMaxGeneralMathMasteryValue(_initDictAddition, _initDictZeroRemovedAddition);

		InitializeGeneralWordMastery();
		InitializeGeneralLetterPictureMastery();
		InitializeGeneralLetterSoundMastery();
	}

	private static void InitializeGeneralWordMastery() {
		_generalWordDifficultyList = ShuffleList(WordQuestionLibrary.GetWordList);

		foreach (string keyString in WordQuestionLibrary.GetWordList) {
			if (!_generalWordMasteryScores.ContainsKey(keyString)) {
				_generalWordMasteryScores.Add(keyString, 0f);
			}
		}
	}

	private static void InitializeGeneralLetterPictureMastery() {
		_generalLetterPictureDifficultyList = ShuffleList(WordQuestionLibrary.GetWordList);

		foreach (string keyString in _generalLetterPictureDifficultyList) {
			if (!_generalLetterPictureMasteryScores.ContainsKey(keyString)) {
				_generalLetterPictureMasteryScores.Add(keyString, 0f);
			}
		}
	}
	private static void InitializeGeneralLetterSoundMastery() {
		_generalLetterSoundDifficultyList = ShuffleList(LetterSoundQuestionLibrary.GetLetterList);
		
		foreach (string keyString in _generalLetterSoundDifficultyList) {
			if (!_generalLetterSoundMasteryScores.ContainsKey(keyString)) {
				_generalLetterSoundMasteryScores.Add(keyString, 0f);
			}
		}
	}

	//
	
	private static void CountMaxGeneralMathMasteryValue(Dictionary<string, float> initDictAddition, Dictionary<string, float> initDictZeroRemovedAddition) {
		if (!initialized) {
			return;
		}

		_generalMathMasteryMaxValue = (initDictAddition.Count * 4);
		_generalMathMasteryMaxValue += (initDictZeroRemovedAddition.Count * 4 * 4);
	}

	/// <summary>
	/// Attach StatManager Events.
	/// </summary>
	public static void AttachEvents() {
		GameManager.OnGameSave += SaveGame;
		GameManager.OnGameLoad += LoadGame;
		GameManager.OnClearSaveGame += ClearSaveGame;
	}

	public static void DetachEvents() {
		GameManager.OnGameSave -= SaveGame;
		GameManager.OnGameLoad -= LoadGame;
		GameManager.OnClearSaveGame -= ClearSaveGame;
	}

	// Save the _operatorStore to PlayerPrefs
	private static void SaveGame() {
		// Math Mastery
		string json = JsonConvert.SerializeObject(_operatorStore);
		PlayerPrefs.SetString("OperatorStore", json);
		json = JsonConvert.SerializeObject(_generalMathMasteryList);
		PlayerPrefs.SetString("GeneralMathMasteryList", json);

		// Word mastery
		json = JsonConvert.SerializeObject(_generalWordMasteryScores);
		PlayerPrefs.SetString("GeneralWordMasteryScores", json);
		json = JsonConvert.SerializeObject(_generalWordMasteredList);
		PlayerPrefs.SetString("GeneralWordMasteryList", json);

		// Letter Picture Mastery
		json = JsonConvert.SerializeObject(_generalLetterPictureMasteryScores);
		PlayerPrefs.SetString("GeneralLetterPictureMasteryScores", json);
		json = JsonConvert.SerializeObject(_generalLetterPictureMasteredList);
		PlayerPrefs.SetString("GeneralLetterPictureMastered", json);
		
		// Letter Sound Mastery
		json = JsonConvert.SerializeObject(_generalLetterSoundMasteryScores);
		PlayerPrefs.SetString("GeneralLetterSoundMasteryScores", json);
		json = JsonConvert.SerializeObject(_generalLetterSoundMasteredList);
		PlayerPrefs.SetString("GeneralLetterSoundMastered", json);

		Debug.Log("Game saved successfully.");
	}

	// Load the _operatorStore from PlayerPrefs
	private static void LoadGame() {
		if (PlayerPrefs.HasKey("OperatorStore")) {
			initialized = true;
			string json = PlayerPrefs.GetString("OperatorStore");
			_operatorStore = JsonConvert.DeserializeObject<OperatorStore>(json);
			Debug.Log("OperatorStore loaded successfully.");

		} else {
			Debug.LogWarning("No saved game data found.");
		}

		if (PlayerPrefs.HasKey("GeneralMathMasteryList")) {
			string json = PlayerPrefs.GetString("GeneralMathMasteryList");
			_generalMathMasteryList = JsonConvert.DeserializeObject<List<string>>(json);
			Debug.Log("MathMastery loaded successfully.");
		}

		if (PlayerPrefs.HasKey("GeneralWordMasteryList")) {
			string json = PlayerPrefs.GetString("GeneralWordMasteryList");
			_generalWordMasteredList = JsonConvert.DeserializeObject<List<string>>(json);
			Debug.Log("Mastered Words loaded successfully.");
		}

		if (PlayerPrefs.HasKey("GeneralWordMasteryScores")) {
			string json = PlayerPrefs.GetString("GeneralWordMasteryScores");
			_generalWordMasteryScores = JsonConvert.DeserializeObject<Dictionary<string, float>>(json);
			_generalWordDifficultyList = ReorderByFloats(_generalWordDifficultyList, _generalWordMasteryScores);
			Debug.Log("Word Mastery Scores loaded successfully.");
		}

		if (PlayerPrefs.HasKey("GeneralLetterPictureMasteryScores")) {
			string json = PlayerPrefs.GetString("GeneralLetterPictureMasteryScores");
			_generalLetterPictureMasteryScores = JsonConvert.DeserializeObject<Dictionary<string, float>>(json);
			_generalLetterPictureDifficultyList = ReorderByFloats(_generalLetterPictureDifficultyList, _generalLetterPictureMasteryScores);
		}

		if (PlayerPrefs.HasKey("GeneralLetterPictureMasteredList")) {
			string json = PlayerPrefs.GetString("GeneralLetterPictureMasteredList");
			_generalLetterPictureMasteredList = JsonConvert.DeserializeObject<List<string>>(json);
		}
		
		if (PlayerPrefs.HasKey("GeneralLetterSoundMasteryScores")) {
			string json = PlayerPrefs.GetString("GeneralLetterSoundMasteryScores");
			_generalLetterSoundMasteryScores = JsonConvert.DeserializeObject<Dictionary<string, float>>(json);
			_generalLetterSoundDifficultyList = ReorderByFloats(_generalLetterSoundDifficultyList, _generalLetterSoundMasteryScores);
		}

		if (PlayerPrefs.HasKey("GeneralLetterSoundMastered")) {
			string json = PlayerPrefs.GetString("GeneralLetterSoundMastered");
			_generalLetterSoundMasteredList = JsonConvert.DeserializeObject<List<string>>(json);
		}
	}

	private static List<string> ReorderByFloats(List<string> list, Dictionary<string, float> dict, string currentKey = default) {

		if (list == null && dict == null || dict == null) {
			return null;
		}

		if (list == null || list.Count == 0 && dict.Count != 0) {
			list = new();
			foreach (string item in dict.Keys) {
				list.Add(item);
			}
		}

		string[] tempArray = list.ToArray();
		int currentIndex = default;
		int previousIndex = default;
		int nextIndex = default;
		float currentFloat = default;
		float previousFloat = default;
		float nextFloat = default;

		if (currentKey != default) {
			currentIndex = Array.IndexOf(tempArray, currentKey);
			currentFloat = dict[tempArray[currentIndex]];
			previousIndex = (currentIndex - 1 >= 0) ? currentIndex - 1 : 0;
			previousFloat = dict[tempArray[previousIndex]];
			nextIndex = (currentIndex + 1 < tempArray.Length) ? currentIndex + 1 : tempArray.Length - 1;
			nextFloat = dict[tempArray[nextIndex]];
		}

		List<string> result = new List<string>();

		if (currentIndex == default) {
			foreach (string entry in list) {
				currentFloat = dict[entry];
				currentIndex = Array.IndexOf(tempArray, entry);
				previousIndex = (currentIndex - 1 >= 0) ? currentIndex - 1 : 0;
				previousFloat = dict[tempArray[previousIndex]];
				nextIndex = (currentIndex + 1 < tempArray.Length) ? currentIndex + 1 : tempArray.Length - 1;
				nextFloat = dict[tempArray[nextIndex]];

				SortDirectionByFloatValue(dict, ref tempArray, currentIndex, currentFloat, previousIndex, previousFloat, nextIndex, nextFloat);
			}
		} else {
			SortDirectionByFloatValue(dict, ref tempArray, currentIndex, currentFloat, previousIndex, previousFloat, nextIndex, nextFloat);
		}

		list.Clear();

		foreach (string key in tempArray) {
			list.Add(key);
		}

		return list;
	}

	private static void SortDirectionByFloatValue(Dictionary<string, float> dict, ref string[] tempArray, int currentIndex, float currentFloat, int previousIndex, float previousFloat, int nextIndex, float nextFloat) {
		if (currentFloat > previousFloat) {
			while (currentFloat >= previousFloat) {
				// Deconstructive Swap
				(tempArray[previousIndex], tempArray[currentIndex]) = (tempArray[currentIndex], tempArray[previousIndex]);
				currentIndex = previousIndex;

				if (currentIndex == 0) {
					break;
				} // We've moved the item to the top of the list.

				previousIndex = (currentIndex - 1 >= 0) ? currentIndex - 1 : 0;
				previousFloat = dict[tempArray[previousIndex]];
			}
		}
		else if (currentFloat < nextFloat) {
			while (currentFloat <= nextFloat) {

				// Deconstructive Swap
				(tempArray[nextIndex], tempArray[currentIndex]) = (tempArray[currentIndex], tempArray[nextIndex]);
				currentIndex = nextIndex;

				if (currentIndex == tempArray.Length - 1) {
					break;
				} // We've moved the item to the top of the list.

				nextIndex = (currentIndex + 1 < tempArray.Length) ? currentIndex + 1 : 0;
				nextFloat = dict[tempArray[nextIndex]];
			}
		}
	}

	private static void ClearSaveGame() {
		if (PlayerPrefs.HasKey("OperatorStore")) {
			PlayerPrefs.DeleteKey("OperatorStore");
		}

		if (PlayerPrefs.HasKey("GeneralMathMasteryList")) {
			PlayerPrefs.DeleteKey("GeneralMathMasteryList");
		}

		if (PlayerPrefs.HasKey("GeneralWordMasteryList")) {
			PlayerPrefs.DeleteKey("GeneralWordMasteryList");
		}

		if (PlayerPrefs.HasKey("GeneralWordMasteryScores")) {
			PlayerPrefs.DeleteKey("GeneralWordMasteryScores");
		}

		if (PlayerPrefs.HasKey("GeneralLetterPictureMasteryScores")) {
			PlayerPrefs.DeleteKey("GeneralLetterPictureMasteryScores");
		}
		if (PlayerPrefs.HasKey("GeneralLetterPictureMasteredList")) {
			PlayerPrefs.DeleteKey("GeneralLetterPictureMasteredList");
		}

		if (PlayerPrefs.HasKey("GeneralLetterSoundMasteryScores")) {
			PlayerPrefs.DeleteKey("GeneralLetterSoundMasteryScores");
		}

		if (PlayerPrefs.HasKey("GeneralLetterSoundMastered")) {
			PlayerPrefs.DeleteKey("GeneralLetterSoundMastered");
		}
	}

	/// <summary>
	/// Used to shuffle the sorted by difficulty-lists upon initialization.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="list"></param>
	/// <returns></returns>
	private static List<T> ShuffleList<T>(List<T> list) {
		// Fisher-Yates shuffle algorithm using Unity's Random.Range()
		for (int i = list.Count - 1; i > 0; i--) {
			int j = Random.Range(0, i + 1);

			// Swap elements
			(list[i], list[j]) = (list[j], list[i]);
		}

		return list;
	}

	/// <summary>
	/// Removes ZeroOperatorZero (0+0 etc) from dictionaries.
	/// </summary>
	/// <param name="keyValuePairs"></param>
	/// <param name="Operator"></param>
	/// <returns></returns>
	private static Dictionary<string, float> RemoveZeroOperatorZero(Dictionary<string, float> keyValuePairs, string Operator) {
		keyValuePairs = new(keyValuePairs);
		keyValuePairs.Remove($"0{Operator}0");
		return keyValuePairs;
	}

	/// <summary>
	/// Removes ZeroOperatorZero (0+0 etc) from lists.
	/// </summary>
	/// <param name="sortedList"></param>
	/// <param name="Operator"></param>
	/// <returns></returns>
	private static List<string> RemoveZeroOperatorZero(List<string> sortedList, string Operator) {
		sortedList = new(sortedList);
		sortedList.Remove($"0{Operator}0");
		return sortedList;
	}

#endregion

#region GeneralMastery

	public static int GeneralMathMastery {
		get { return _generalMathMasteryList.Count; }
	}

	public static int GeneralWordMastery {
		get { return _generalWordMasteredList.Count; }
	}

#endregion

#region Math Statistics Storage Management

	/// <summary>
	/// Registers mastery score for each of the component pairs.
	/// </summary>
	/// <param name="mathTask"></param>
	/// <param name="selectedValue"></param>
	/// <param name="points"></param>
	public static void RegisterAnswer(MathTask mathTask, float selectedValue, float points) {
		/* Function Plan:
		*	1. Separate Math task into answer-pairs (decimals, ones, tens, hundreds, thousands)
		*	2. Register points boost/decrease.
		*/

		string firstComponent = $"{mathTask.Components[0]}";
		string secondComponent = $"{mathTask.Components[1]}";

		string thousandsPair = GetPair(mathTask.Operator, 4, firstComponent, secondComponent);
		string hundredPair = GetPair(mathTask.Operator, 3, firstComponent, secondComponent);
		string tennerPair = GetPair(mathTask.Operator, 2, firstComponent, secondComponent);
		string onerPair = GetPair(mathTask.Operator, 1, firstComponent, secondComponent);

		string decimalPair = GetDecimalPair(mathTask, firstComponent, secondComponent);

		switch (mathTask.Operator) {
			case "+":
				UpdateGeneralizedDatabase(mathTask.Operator, points, decimalPair, onerPair, tennerPair, hundredPair, thousandsPair, _operatorStore.Addition);
				OnDatabaseUpdate?.Invoke();
				break;
			case "-":
				UpdateGeneralizedDatabase(mathTask.Operator, points, decimalPair, onerPair, tennerPair, hundredPair, thousandsPair, _operatorStore.Subtraction);
				OnDatabaseUpdate?.Invoke();
				break;
			case "*":
				UpdateGeneralizedDatabase(mathTask.Operator, points, decimalPair, onerPair, tennerPair, hundredPair, thousandsPair, _operatorStore.Multiplication);
				OnDatabaseUpdate?.Invoke();
				break;
			case "/":
			case ":":
				UpdateGeneralizedDatabase(mathTask.Operator, points, decimalPair, onerPair, tennerPair, hundredPair, thousandsPair, _operatorStore.Division);
				OnDatabaseUpdate?.Invoke();
				break;
		}
	}

	/// <summary>
	/// Updates one of four categories by: Registering points, sorting the difficulty lists connected to the registered points.
	/// </summary>
	/// <param name="taskOperator"></param>
	/// <param name="points"></param>
	/// <param name="decimalPair"></param>
	/// <param name="onerPair"></param>
	/// <param name="tennerPair"></param>
	/// <param name="hundredPair"></param>
	/// <param name="thousandsPair"></param>
	/// <param name="currentOperator"></param>
	private static void UpdateGeneralizedDatabase(string taskOperator, float points, string decimalPair, string onerPair, string tennerPair, string hundredPair, string thousandsPair, Operator currentOperator) {

		if (decimalPair != null && taskOperator != null && decimalPair != $"0{taskOperator}0") {
			currentOperator.DecimalStats[decimalPair] += points;

			AddMathGMIfMastered(PairZeroFill(decimalPair, -1, taskOperator), currentOperator.DecimalStats[decimalPair]);

			currentOperator.DecimalDifficultySorted = ReorderByFloats(currentOperator.DecimalDifficultySorted, decimalPair, 0, taskOperator);
		}

		// If the onerPair does not exist, return early.
		if (onerPair == null) {
			Debug.LogError("OnerPair is null;");
			return;
		}

		currentOperator.OneStats[onerPair] += points;

		AddMathGMIfMastered(PairZeroFill(onerPair, 0, taskOperator), currentOperator.OneStats[onerPair]);

		currentOperator.OneDifficultySorted = ReorderByFloats(currentOperator.OneDifficultySorted, onerPair, 1, taskOperator);

		// If the tennerPair does not exist, return early.
		if (tennerPair == null || taskOperator != null && tennerPair == $"0{taskOperator}0") {
			//UnityEngine.Debug.LogError( "TennerPair is null;" );
			return;
		}

		currentOperator.TensStats[tennerPair] += points;

		AddMathGMIfMastered(PairZeroFill(tennerPair, 1, taskOperator), currentOperator.TensStats[tennerPair]);

		currentOperator.TensDifficultySorted = ReorderByFloats(currentOperator.TensDifficultySorted, tennerPair, 2, taskOperator);

		// If the hundredPair does not exist, return early.
		if (hundredPair == null || taskOperator != null && hundredPair == $"0{taskOperator}0") {
			return;
		}

		currentOperator.HundredsStats[hundredPair] += points;

		AddMathGMIfMastered(PairZeroFill(hundredPair, 2, taskOperator), currentOperator.HundredsStats[hundredPair]);

		currentOperator.HundredsDifficultySorted = ReorderByFloats(currentOperator.HundredsDifficultySorted, hundredPair, 3, taskOperator);

		// If the thousandsPair does not exist, return early.
		if (thousandsPair == null || taskOperator != null && thousandsPair == $"0{taskOperator}0") {
			return;
		}

		currentOperator.ThousandsStats[thousandsPair] += points;

		AddMathGMIfMastered(PairZeroFill(thousandsPair, 3, taskOperator), currentOperator.ThousandsStats[thousandsPair]);

		currentOperator.ThousandsDifficultySorted = ReorderByFloats(currentOperator.ThousandsDifficultySorted, thousandsPair, 4, taskOperator);
	}
	
	/// <summary>
	/// Adds entry to math mastery if it beats mastery level.
	/// </summary>
	/// <param name="entry"></param>
	/// <param name="value"></param>
	private static void AddMathGMIfMastered(string entry, float value) {
		if (value > GameManager.WhenIsMasteryAchieved && !_generalMathMasteryList.Contains(entry)) {
			_generalMathMasteryList.Add(entry);
			OnMathGeneralMastery?.Invoke();
		}
	}
	
	/// <summary>
	/// Adds zeros to the end of and in front of your pairs.
	/// </summary>
	/// <param name="numberPair"></param>
	/// <param name="numeric"></param>
	/// <param name="taskOperator"></param>
	/// <returns></returns>
	private static string PairZeroFill(string numberPair, int numeric, string taskOperator) {
		string before = "";
		string after = "";
		if (numeric > 0) {
			for (int i = 0; i < numeric; i++) {
				after += "0";
			}
		} else if (numeric < 0) {
			before += "0.";
		}

		string firstNumber = numberPair.Split(taskOperator)[0];
		string secondNumber = numberPair.Split(taskOperator)[1];
		firstNumber = $"{before}{firstNumber}{after}";
		secondNumber = $"{before}{secondNumber}{after}";

		return $"{firstNumber}{taskOperator}{secondNumber}";
	}

	/// <summary>
	/// Reorder each category's difficulty list depending on float values registered.
	/// </summary>
	/// <param name="difficultySortedList"></param>
	/// <param name="componentPair"></param>
	/// <param name="decimalSpot"></param>
	/// <param name="operatorString"></param>
	private static List<string> ReorderByFloats(List<string> difficultySortedList, string componentPair, int decimalSpot, string operatorString) {
		string[] difficultySortedArray = difficultySortedList.ToArray();

		int currentIndex = Array.IndexOf(difficultySortedArray, componentPair);
		float currentMathPairScore = GetMathPairScore(componentPair, decimalSpot, operatorString);
		int lowerEntryIndex = (currentIndex - 1 >= 0) ? currentIndex - 1 : 0;
		int higherEntryIndex = (currentIndex + 1 < difficultySortedArray.Length) ? currentIndex + 1 : 0;
		float lowerEntryMathPairScore = GetMathPairScore(difficultySortedArray[lowerEntryIndex], decimalSpot, operatorString);
		float higherEntryMathPairScore = GetMathPairScore(difficultySortedArray[higherEntryIndex], decimalSpot, operatorString);

		if (lowerEntryMathPairScore < currentMathPairScore) {
			while (lowerEntryMathPairScore < currentMathPairScore) {

				string tempKey = difficultySortedArray[lowerEntryIndex];
				difficultySortedArray[lowerEntryIndex] = componentPair;
				difficultySortedArray[currentIndex] = tempKey;
				currentIndex = lowerEntryIndex;

				if (currentIndex == 0) {
					break;
				} // We've moved the item to the top of the list.

				lowerEntryIndex = (currentIndex - 1 >= 0) ? currentIndex - 1 : 0;
				lowerEntryMathPairScore = GetMathPairScore(difficultySortedArray[lowerEntryIndex], decimalSpot, operatorString);
			}
		}
		else if (higherEntryMathPairScore > currentMathPairScore) {
			while (higherEntryMathPairScore > currentMathPairScore) {

				string tempKey = difficultySortedArray[higherEntryIndex];
				difficultySortedArray[higherEntryIndex] = componentPair;
				difficultySortedArray[currentIndex] = tempKey;
				currentIndex = higherEntryIndex;

				if (currentIndex == difficultySortedArray.Length - 1) {
					break;
				} // We've moved the item to the top of the list.

				higherEntryIndex = (currentIndex + 1 < difficultySortedArray.Length) ? currentIndex + 1 : 0;
				higherEntryMathPairScore = GetMathPairScore(difficultySortedArray[higherEntryIndex], decimalSpot, operatorString);
			}
		}

		difficultySortedList.Clear();

		foreach (string key in difficultySortedArray) {
			difficultySortedList.Add(key);
		}

		return difficultySortedList;
	}

	/// <summary>
	/// Gets the correct float from our operatorStore.
	/// </summary>
	/// <param name="componentPair"></param>
	/// <param name="decimalSpot"></param>
	/// <param name="operatorString"></param>
	/// <returns></returns>
	private static float GetMathPairScore(string componentPair, int decimalSpot, string operatorString) {

		switch (operatorString) {
			case "+":
				switch (decimalSpot) {
					case 0:
						if (componentPair == "0+0") {
							return 0f;
						}

						return _operatorStore.Addition.DecimalStats[componentPair];
					case 1:
						return _operatorStore.Addition.OneStats[componentPair];
					case 2:
						if (componentPair == "0+0") {
							return 0f;
						}

						return _operatorStore.Addition.TensStats[componentPair];
					case 3:
						if (componentPair == "0+0") {
							return 0f;
						}

						return _operatorStore.Addition.HundredsStats[componentPair];
					case 4:
						if (componentPair == "0+0") {
							return 0f;
						}

						return _operatorStore.Addition.ThousandsStats[componentPair];
				}

				break;
			case "-":
				switch (decimalSpot) {
					case 0:
						if (componentPair == "0-0") {
							return 0f;
						}

						return _operatorStore.Subtraction.DecimalStats[componentPair];
					case 1:
						return _operatorStore.Subtraction.OneStats[componentPair];
					case 2:
						if (componentPair == "0-0") {
							return 0f;
						}

						return _operatorStore.Subtraction.TensStats[componentPair];
					case 3:
						if (componentPair == "0-0") {
							return 0f;
						}

						return _operatorStore.Subtraction.HundredsStats[componentPair];
					case 4:
						if (componentPair == "0-0") {
							return 0f;
						}

						return _operatorStore.Subtraction.ThousandsStats[componentPair];
				}

				break;
			case "*":
				switch (decimalSpot) {
					case 0:
						if (componentPair == "0*0") {
							return 0f;
						}

						return _operatorStore.Multiplication.DecimalStats[componentPair];
					case 1:
						return _operatorStore.Multiplication.OneStats[componentPair];
					case 2:
						if (componentPair == "0*0") {
							return 0f;
						}

						return _operatorStore.Multiplication.TensStats[componentPair];
					case 3:
						if (componentPair == "0*0") {
							return 0f;
						}

						return _operatorStore.Multiplication.HundredsStats[componentPair];
					case 4:
						if (componentPair == "0*0") {
							return 0f;
						}

						return _operatorStore.Multiplication.ThousandsStats[componentPair];
				}

				break;
			case "/":
			case ":":
				switch (decimalSpot) {
					case 0:
						if (componentPair == "0/0") {
							return 0f;
						}

						return _operatorStore.Division.DecimalStats[componentPair];
					case 1:
						return _operatorStore.Division.OneStats[componentPair];
					case 2:
						if (componentPair == "0/0") {
							return 0f;
						}

						return _operatorStore.Division.TensStats[componentPair];
					case 3:
						if (componentPair == "0/0") {
							return 0f;
						}

						return _operatorStore.Division.HundredsStats[componentPair];
					case 4:
						if (componentPair == "0/0") {
							return 0f;
						}

						return _operatorStore.Division.ThousandsStats[componentPair];
				}

				break;
		}

		return 0f;
	}

	public static List<string> GetNonZeroMathFloatList(List<string> floatList,int decimalSpot, string operatorString) {
		List<string> nonZeroList = new();
		foreach (string key in floatList) {
			if (!Mathf.Approximately(GetMathPairScore(key, decimalSpot, operatorString), 0)) {
				nonZeroList.Add(key);
			}
		}
		return nonZeroList;
	}
	
	/// <summary>
	/// This function extracts a decimal-pair from the components.
	/// </summary>
	/// <param name="mathTask"></param>
	/// <param name="firstComponent"></param>
	/// <param name="secondComponent"></param>
	/// <returns></returns>
	private static string GetDecimalPair(MathTask mathTask, string firstComponent, string secondComponent) {
		if (firstComponent.IndexOf(",", StringComparison.Ordinal) != -1 ||
			firstComponent.IndexOf(".", StringComparison.Ordinal) != -1 ||
			secondComponent.IndexOf(",", StringComparison.Ordinal) != -1 ||
			secondComponent.IndexOf(".", StringComparison.Ordinal) != -1) {
			string first = "0";
			string second = "0";
			if (firstComponent.IndexOf(",", StringComparison.Ordinal) != -1) {
				first = firstComponent.Split(",")[1];
			}
			else if (firstComponent.IndexOf(".", StringComparison.Ordinal) != -1) {
				first = firstComponent.Split(".")[1];
			}

			if (secondComponent.IndexOf(",", StringComparison.Ordinal) != -1) {
				second = secondComponent.Split(",")[1];
			}
			else if (secondComponent.IndexOf(".", StringComparison.Ordinal) != -1) {
				second = secondComponent.Split(".")[1];
			}

			return $"{first}{mathTask.Operator}{second}";
		}

		return null;
	}

	/// <summary>
	/// This function cets a pair of "x+y" dependant on 
	/// </summary>
	/// <param name="mathTaskOperator"></param>
	/// <param name="length"></param>
	/// <param name="firstComponent"></param>
	/// <param name="secondComponent"></param>
	/// <returns></returns>
	private static string GetPair(string mathTaskOperator, int length, string firstComponent, string secondComponent) {
		/* Function Plan:
		*		1. Check that length of first & Second Components
		*		2. get the correct positioned (if it exists) number, return paired numbers.
		*/
		string first = "0";
		string second = "0";

		if (firstComponent.Length >= length) {
			int firstComponentStart = firstComponent.Length - length;
			first = firstComponent.Substring(firstComponentStart, 1);
		}

		if (secondComponent.Length >= length) {
			int secondComponentStart = secondComponent.Length - length;
			second = secondComponent.Substring(secondComponentStart, 1);
		}

		return $"{first}{mathTaskOperator}{second}";
	}

	/// <summary>
	/// Returns the complete OperatorStore.
	/// </summary>
	public static OperatorStore GetStore => _operatorStore;

	/// <summary>
	/// Produces difficulty lists dependent on the Operator and Difficulty provided.
	/// </summary>
	/// <param name="Operator"></param>
	/// <param name="difficulty"></param>
	/// <returns></returns>
	public static MathDifficultyList GetDifficultyLists(string Operator, string difficulty) {
		if (Operator == default) {
			Debug.LogError($"Operator is: 'null'");
		}

		MathDifficultyList mathDifficultyLists = new() {
			//Decimal = new(),
			One = new(),
			Tens = new(),
			Hundreds = new(),
			Thousands = new()
		};

		Operator _tempOperator = default;
		switch (Operator) {
			case "+":
				_tempOperator = StatManager.GetStore.Addition;
				break;
			case "-":
				_tempOperator = StatManager.GetStore.Subtraction;
				break;
			case "*":
				_tempOperator = StatManager.GetStore.Multiplication;
				break;
			case "/":
			case ":":
				_tempOperator = StatManager.GetStore.Division;
				break;
		}

		if (_tempOperator.OneDifficultySorted == default ||
			_tempOperator.TensDifficultySorted == default ||
			_tempOperator.HundredsDifficultySorted == default ||
			_tempOperator.ThousandsDifficultySorted == default) {

			Debug.LogError($"_tempOperator not properly formed.");
			return new MathDifficultyList();
		}

		switch (difficulty) {
			case "e":
			case "E":
				//_difficultyLists.Decimal = _tempOperator.DecimalDifficultySorted.GetRange(0,20);
				mathDifficultyLists.One = _tempOperator.OneDifficultySorted.GetRange(0, 20);
				mathDifficultyLists.Tens = _tempOperator.TensDifficultySorted.GetRange(0, 20);
				mathDifficultyLists.Hundreds = _tempOperator.HundredsDifficultySorted.GetRange(0, 20);
				mathDifficultyLists.Thousands = _tempOperator.ThousandsDifficultySorted.GetRange(0, 20);
				break;
			case "m":
			case "M":
				//_difficultyLists.Decimal = _tempOperator.DecimalDifficultySorted.GetRange( 39, 20 );
				mathDifficultyLists.One = _tempOperator.OneDifficultySorted.GetRange(39, 20);
				mathDifficultyLists.Tens = _tempOperator.TensDifficultySorted.GetRange(39, 20);
				mathDifficultyLists.Hundreds = _tempOperator.HundredsDifficultySorted.GetRange(39, 20);
				mathDifficultyLists.Thousands = _tempOperator.ThousandsDifficultySorted.GetRange(39, 20);
				break;
			case "h":
			case "H":
				//_difficultyLists.Decimal = _tempOperator.DecimalDifficultySorted.GetRange( 63, 33 );
				mathDifficultyLists.One = _tempOperator.OneDifficultySorted.GetRange(_tempOperator.OneDifficultySorted.Count - 10, 10);
				mathDifficultyLists.Tens = _tempOperator.TensDifficultySorted.GetRange(_tempOperator.TensDifficultySorted.Count - 10, 10);
				mathDifficultyLists.Hundreds = _tempOperator.HundredsDifficultySorted.GetRange(_tempOperator.HundredsDifficultySorted.Count - 10, 10);
				mathDifficultyLists.Thousands = _tempOperator.ThousandsDifficultySorted.GetRange(_tempOperator.ThousandsDifficultySorted.Count - 10, 10);
				break;
		}

		return mathDifficultyLists;
	}
#endregion

#region Words Statistics Storage Management
	public static void RegisterAnswer(WordTask wordTask, string selectedValue, float points) {
		_generalWordMasteryScores[selectedValue] += points;

		AddWordGMIfMastered(selectedValue, _generalWordMasteryScores[selectedValue]);

		_generalWordDifficultyList = ReorderByFloats(_generalWordDifficultyList, _generalWordMasteryScores, selectedValue);
		
		//PrintSortedWordDifficultyList();
	}

	private static void PrintSortedWordDifficultyList() {
		string SortedWordMastery = "";
		foreach (string key in _generalWordDifficultyList) {
			SortedWordMastery += $"{key}: {_generalWordMasteryScores[key]}\n";
		}
		Debug.LogWarning(SortedWordMastery);
	}

	private static void AddWordGMIfMastered(string selectedValue, float wordMasteryScore) {
		if (!_generalWordMasteredList.Contains(selectedValue) && wordMasteryScore > GameManager.WhenIsMasteryAchieved) {
			_generalWordMasteredList.Add(selectedValue);
		}
	}
	public static Dictionary<string, float> GetWordMasteryScore { 
		get { 
			return _generalWordMasteryScores;
		}
	}
	
	/// <summary>
	/// Produces difficulty lists dependent on the Operator and Difficulty provided.
	/// </summary>
	/// <param name="difficulty"></param>
	/// <returns></returns>
	public static List<string> GetWordDifficultyList( char difficulty ) {
		List <string> _difficultyList = new();

		List<string> _tempList = WordQuestionLibrary.GetWordList;
		int _partSize = (int)(_tempList.Count / 5);
		//Debug.Log($"Getting word mastery scores for difficulty {difficulty}, part size {_partSize}, and {_tempList.Count}");
		switch (difficulty) {
			case 'e':
			case 'E':
				_difficultyList = _generalWordDifficultyList.GetRange(0, _partSize);
				break;
			case 'm':
			case 'M':
				_difficultyList = _generalWordDifficultyList.GetRange(_partSize*2, _partSize);
				break;
			case 'h':
			case 'H':
				_difficultyList = _generalWordDifficultyList.GetRange(_partSize*4, _partSize);
				break;
		}

		return _difficultyList;
	}
	
	public static List<string> GetNonZeroWordFloatList(List<string> floatList) {
		List<string> nonZeroList = new();
		foreach (string key in floatList) {
			if (_generalWordMasteryScores.ContainsKey(key) && !Mathf.Approximately(_generalWordMasteryScores[key], 0)) {
				nonZeroList.Add(key);
			}
		}
		return nonZeroList;
	}
#endregion

#region Letter Statistics Storage Management
	public static void RegisterAnswer(LetterTask task, string selectedValue, float points) {

		switch (task.Mode) {
			case GameModeType.LetterPicture:
				_generalLetterPictureMasteryScores[task.StorageKey] += points;

				AddLetterPictureGMIfMastered(task.StorageKey, _generalLetterPictureMasteryScores[task.StorageKey]);

				_generalLetterPictureDifficultyList = ReorderByFloats(_generalLetterPictureDifficultyList, _generalLetterPictureMasteryScores, task.StorageKey);

				break;
			case GameModeType.Letters: {
				string storageLetter = task.StorageKey.Substring(0, 1);
				if (_generalLetterSoundMasteryScores.ContainsKey(storageLetter)) {
					_generalLetterSoundMasteryScores[storageLetter] += points;
					
					AddLetterSoundGMIfMastered(storageLetter, _generalLetterSoundMasteryScores[storageLetter]);

					_generalLetterSoundDifficultyList = ReorderByFloats(_generalLetterSoundDifficultyList, _generalLetterSoundMasteryScores, storageLetter);
				} else {
					Debug.LogError("[StatManager.RegisterAnswer] There are no sound mastery scores!]");
				}
				
			} break;
		}
		
		//PrintSortedWordDifficultyList();
	}

	private static void AddLetterPictureGMIfMastered(string taskStorageKey, float generalLetterPictureMasteryScore) {
		if (!_generalLetterPictureMasteredList.Contains(taskStorageKey) && generalLetterPictureMasteryScore > GameManager.WhenIsMasteryAchieved) {
			_generalLetterPictureMasteredList.Add(taskStorageKey);
		}
	}

	private static void AddLetterSoundGMIfMastered(string storageLetter, float generalLetterSoundMasteryScore) {
		if (!_generalLetterSoundMasteredList.Contains(storageLetter) && generalLetterSoundMasteryScore > GameManager.WhenIsMasteryAchieved) {
			_generalLetterSoundMasteredList.Add(storageLetter);
		}
	}

	public static List<string> GetNonZeroLetterFloatList(List<string> floatList, LetterMode letterMode) {
		List<string> nonZeroList = new();
		foreach (string key in floatList) {
			if (letterMode == LetterMode.Sound && _generalLetterSoundMasteryScores.ContainsKey(key) && !Mathf.Approximately(_generalLetterSoundMasteryScores[key], 0)) {
				nonZeroList.Add(key);
			}
			if (letterMode == LetterMode.Picture && _generalLetterPictureMasteryScores.ContainsKey(key) && !Mathf.Approximately(_generalLetterPictureMasteryScores[key], 0)) {
				nonZeroList.Add(key);
			}
		}
		return nonZeroList;
	}
	
	public static List<string> GetLetterDifficultyList( char taskDifficultyLetter, LetterMode mode ) {
		List <string> difficultyList = new();
		List <string> keyList = new();
		List <string> tempSortedList = new();
		switch ( mode ) {
			case LetterMode.Picture:
				keyList = WordQuestionLibrary.GetWordList;
				tempSortedList = _generalWordDifficultyList;
				break;
			case LetterMode.Sound:
				keyList = LetterSoundQuestionLibrary.GetLetterList;
				tempSortedList = _generalLetterSoundDifficultyList;
				break;
		}
		
		int partSize = (int)(keyList.Count / 5);
		//Debug.Log($"Getting word mastery scores for difficulty {taskDifficultyLetter}, part size {partSize}, and {tempSortedList.Count}");
		
		switch (taskDifficultyLetter) {
			case 'e':
			case 'E':
				difficultyList = tempSortedList.GetRange(0, partSize);
				break;
			case 'm':
			case 'M':
				difficultyList = tempSortedList.GetRange(partSize*2, partSize);
				break;
			case 'h':
			case 'H':
				difficultyList = tempSortedList.GetRange(partSize*4, partSize);
				break;
		}
		
		
		return difficultyList;
	}
#endregion
}

[Serializable]
public struct OperatorStore {
	public Operator Addition; 
	public Operator Subtraction; 
	public Operator Multiplication; 
	public Operator Division;
}
[Serializable]
public struct Operator {
	public Dictionary<string, float> DecimalStats;
	public List<string> DecimalDifficultySorted;
	public Dictionary<string, float> OneStats;
	public List<string> OneDifficultySorted;
	public Dictionary<string, float> TensStats;
	public List<string> TensDifficultySorted;
	public Dictionary<string, float> HundredsStats;
	public List<string> HundredsDifficultySorted;
	public Dictionary<string, float> ThousandsStats;
	public List<string> ThousandsDifficultySorted;
}

[Serializable]
public struct MathDifficultyList {
	//public List<string> Decimal;
	public List<string> One;
	public List<string> Tens;
	public List<string> Hundreds;
	public List<string> Thousands;
}
