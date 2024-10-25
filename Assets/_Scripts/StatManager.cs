
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

static public class StatManager {
	[SerializeField] private static OperatorStore _operatorStore = new();
	[SerializeField] private static List<string> _generalMasteryList = new();
	public static Action OnDatabaseUpdate { get; set; } = delegate { };

	private static bool initialized = false;


	#region Initialization
	/// <summary>
	/// Initialize all dictionaries, and lists we are using for storage.
	/// </summary>
	public static void Initialize ( float defaultMasteryScore = 0f ) {
		if (initialized) { return; }
		Debug.Log( "Initializing StatManager" );
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
				if (!_initDictAddition.ContainsKey( _mathAddition )) {
					_initDictAddition.Add( _mathAddition, 0f );
				}
				if (!_initListAddition.Contains( _mathAddition )) {
					_initListAddition.Add( _mathAddition );
				}
				string _mathSubtraction = $"{first}-{second}";
				if (!_initDictSubtraction.ContainsKey( _mathSubtraction )) {
					_initDictSubtraction.Add( _mathSubtraction, 0f );
				}
				if (!_initListSubtraction.Contains( _mathSubtraction )) {
					_initListSubtraction.Add( _mathSubtraction );
				}
				string _mathMultiplication = $"{first}*{second}";
				if (!_initDictMultiplication.ContainsKey( _mathMultiplication )) {
					_initDictMultiplication.Add( _mathMultiplication, 0f );
				}
				if (!_initListMultiplication.Contains( _mathMultiplication )) {
					_initListMultiplication.Add( _mathMultiplication );
				}
				string _mathDivision = $"{first}/{second}";
				if (!_initDictDivision.ContainsKey( _mathDivision )) {
					_initDictDivision.Add( _mathDivision, 0f );
				}
				if (!_initListDivision.Contains( _mathDivision )) {
					_initListDivision.Add( _mathDivision );
				}
			}
		}


		Dictionary<string, float> _initDictZeroRemovedAddition = RemoveZeroOperatorZero( _initDictAddition, "+" );
		_operatorStore.Addition.DecimalStats = new( _initDictZeroRemovedAddition );
		_operatorStore.Addition.OneStats = new( _initDictAddition );
		_operatorStore.Addition.TensStats = new( _initDictZeroRemovedAddition );
		_operatorStore.Addition.HundredsStats = new( _initDictZeroRemovedAddition );
		_operatorStore.Addition.ThousandsStats = new( _initDictZeroRemovedAddition );

		List<string> _initListZeroRemovedAddition = RemoveZeroOperatorZero( _initListAddition, "+" );
		_operatorStore.Addition.DecimalDifficultySorted = new( ShuffleList( _initListZeroRemovedAddition ) );
		_operatorStore.Addition.OneDifficultySorted = new( ShuffleList( _initListAddition ) );
		_operatorStore.Addition.TensDifficultySorted = new( ShuffleList( _initListZeroRemovedAddition ) );
		_operatorStore.Addition.HundredsDifficultySorted = new( ShuffleList( _initListZeroRemovedAddition ) );
		_operatorStore.Addition.ThousandsDifficultySorted = new( ShuffleList( _initListZeroRemovedAddition ) );

		Dictionary<string, float> _initDictZeroRemovedSubtraction = RemoveZeroOperatorZero( _initDictSubtraction, "-" );
		_operatorStore.Subtraction.DecimalStats = new( _initDictZeroRemovedSubtraction );
		_operatorStore.Subtraction.OneStats = new( _initDictSubtraction );
		_operatorStore.Subtraction.TensStats = new( _initDictZeroRemovedSubtraction );
		_operatorStore.Subtraction.HundredsStats = new( _initDictZeroRemovedSubtraction );
		_operatorStore.Subtraction.ThousandsStats = new( _initDictZeroRemovedSubtraction );

		List<string> _initListZeroRemovedSubtraction = RemoveZeroOperatorZero( _initListSubtraction, "+" );
		_operatorStore.Subtraction.DecimalDifficultySorted = new( ShuffleList( _initListZeroRemovedSubtraction ) );
		_operatorStore.Subtraction.OneDifficultySorted = new( ShuffleList( _initListSubtraction ) );
		_operatorStore.Subtraction.TensDifficultySorted = new( ShuffleList( _initListZeroRemovedSubtraction ) );
		_operatorStore.Subtraction.HundredsDifficultySorted = new( ShuffleList( _initListZeroRemovedSubtraction ) );
		_operatorStore.Subtraction.ThousandsDifficultySorted = new( ShuffleList( _initListZeroRemovedSubtraction ) );

		Dictionary<string, float> _initDictZeroRemovedDivision = RemoveZeroOperatorZero( _initDictDivision, "/" );
		_operatorStore.Division.DecimalStats = new( _initDictZeroRemovedDivision );
		_operatorStore.Division.OneStats = new( _initDictDivision );
		_operatorStore.Division.TensStats = new( _initDictZeroRemovedDivision );
		_operatorStore.Division.HundredsStats = new( _initDictZeroRemovedDivision );
		_operatorStore.Division.ThousandsStats = new( _initDictZeroRemovedDivision );

		List<string> _initListZeroRemovedDivision = RemoveZeroOperatorZero( _initListDivision, "/" );
		_operatorStore.Division.DecimalDifficultySorted = new( ShuffleList( _initListZeroRemovedDivision ) );
		_operatorStore.Division.OneDifficultySorted = new( ShuffleList( _initListDivision ) );
		_operatorStore.Division.TensDifficultySorted = new( ShuffleList( _initListZeroRemovedDivision ) );
		_operatorStore.Division.HundredsDifficultySorted = new( ShuffleList( _initListZeroRemovedDivision ) );
		_operatorStore.Division.ThousandsDifficultySorted = new( ShuffleList( _initListZeroRemovedDivision ) );

		Dictionary<string, float> _initDictZeroRemovedMultiplication = RemoveZeroOperatorZero( _initDictMultiplication, "*" );
		_operatorStore.Multiplication.DecimalStats = new( _initDictZeroRemovedMultiplication );
		_operatorStore.Multiplication.OneStats = new( _initDictMultiplication );
		_operatorStore.Multiplication.TensStats = new( _initDictZeroRemovedMultiplication );
		_operatorStore.Multiplication.HundredsStats = new( _initDictZeroRemovedMultiplication );
		_operatorStore.Multiplication.ThousandsStats = new( _initDictZeroRemovedMultiplication );

		List<string> _initListZeroRemovedMultiplication = RemoveZeroOperatorZero( _initListMultiplication, "*" );
		_operatorStore.Multiplication.DecimalDifficultySorted = new( ShuffleList( _initListZeroRemovedMultiplication ) );
		_operatorStore.Multiplication.OneDifficultySorted = new( ShuffleList( _initListMultiplication ) );
		_operatorStore.Multiplication.TensDifficultySorted = new( ShuffleList( _initListZeroRemovedMultiplication ) );
		_operatorStore.Multiplication.HundredsDifficultySorted = new( ShuffleList( _initListZeroRemovedMultiplication ) );
		_operatorStore.Multiplication.ThousandsDifficultySorted = new( ShuffleList( _initListZeroRemovedMultiplication ) );

		initialized = true;
	}

	/// <summary>
	/// Used to shuffle the sorted by difficulty-lists upon initialization.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="list"></param>
	/// <returns></returns>
	public static List<T> ShuffleList<T> ( List<T> list ) {
		// Fisher-Yates shuffle algorithm using Unity's Random.Range()
		for (int i = list.Count - 1; i > 0; i--) {
			int j = Random.Range( 0, i + 1 );

			// Swap elements
			T temp = list[ i ];
			list[ i ] = list[ j ];
			list[ j ] = temp;
		}

		return list;
	}

	/// <summary>
	/// Removes ZeroOperatorZero (0+0 etc) from dictionaries.
	/// </summary>
	/// <param name="keyValuePairs"></param>
	/// <param name="Operator"></param>
	/// <returns></returns>
	private static Dictionary<string, float> RemoveZeroOperatorZero ( Dictionary<string, float> keyValuePairs, string Operator ) {
		keyValuePairs = new( keyValuePairs );
		keyValuePairs.Remove( $"0{Operator}0" );
		return keyValuePairs;
	}
	/// <summary>
	/// Removes ZeroOperatorZero (0+0 etc) from lists.
	/// </summary>
	/// <param name="sortedList"></param>
	/// <param name="Operator"></param>
	/// <returns></returns>
	private static List<string> RemoveZeroOperatorZero ( List<string> sortedList, string Operator ) {
		sortedList = new( sortedList );
		sortedList.Remove( $"0{Operator}0" );
		return sortedList;
	}



	#endregion

	#region GeneralMastery
	public static int GeneralMastery {
		get {
			return _generalMasteryList.Count;
		}
	}

	private static void AddGMIfMastered (string entry, float value) {
		if (GameManager.Instance.TaskMasteredLevel && !_generalMasteryList.Contains(entry)) {
			_generalMasteryList.Add( entry );
		}
	}

	#endregion

	#region Statistics Storage Management

	/// <summary>
	/// Registers mastery score for each of the component pairs.
	/// </summary>
	/// <param name="mathTask"></param>
	/// <param name="selectedValue"></param>
	/// <param name="points"></param>
	public static void RegisterAnswer ( MathTask mathTask, float selectedValue, float points ) {
		/* Function Plan: 
		 *	1. Separate Math task into answer-pairs (decimals, ones, tens, hundreds, thousands)
		 *	2. Register points boost/decrease.
		 */

		string firstComponent = $"{mathTask.Components[ 0 ]}";
		string secondComponent = $"{mathTask.Components[ 1 ]}";

		string thousandsPair = GetPair( mathTask.Operator, 4, firstComponent, secondComponent );
		string hundredPair = GetPair( mathTask.Operator, 3, firstComponent, secondComponent );
		string tennerPair = GetPair( mathTask.Operator, 2, firstComponent, secondComponent );
		string onerPair = GetPair( mathTask.Operator, 1, firstComponent, secondComponent );

		string decimalPair = GetDecimalPair( mathTask, firstComponent, secondComponent );

		string mathPiece = $"{mathTask.Components[ 0 ]}{mathTask.Operator}{mathTask.Components[ 1 ]}";

		switch (mathTask.Operator) {
			case "+":
				UpdateAdditiveDatabase( mathTask.Operator, points, decimalPair, onerPair, tennerPair, hundredPair, thousandsPair );
				OnDatabaseUpdate?.Invoke();
				break;
			case "-":
				UpdateSubtractionDatabase( mathTask.Operator, points, decimalPair, onerPair, tennerPair, hundredPair, thousandsPair );
				break;
			case "*":
				UpdateMultiplicationDatabase( mathTask.Operator, points, decimalPair, onerPair, tennerPair, hundredPair, thousandsPair );
				break;
			case "/":
			case ":":
				UpdateDivisionDatabase( mathTask.Operator, points, decimalPair, onerPair, tennerPair, hundredPair, thousandsPair );
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
	private static void UpdateDivisionDatabase ( string taskOperator, float points, string decimalPair, string onerPair, string tennerPair, string hundredPair, string thousandsPair ) {
		if (decimalPair != null) {
			_operatorStore.Division.DecimalStats[ decimalPair ] += points;
			ReorderByFloats( _operatorStore.Division.DecimalDifficultySorted, decimalPair, 0, taskOperator );
		}

		// If the onerPair does not exist, return early.
		if (onerPair == null) {
			return;
		}

		_operatorStore.Division.OneStats[ onerPair ] += points;
		ReorderByFloats( _operatorStore.Division.OneDifficultySorted, onerPair, 1, taskOperator );

		// If the tennerPair does not exist, return early.
		if (tennerPair == null) {
			return;
		}

		_operatorStore.Division.TensStats[ tennerPair ] += points;
		ReorderByFloats( _operatorStore.Division.TensDifficultySorted, tennerPair, 2, taskOperator );

		// If the hundredPair does not exist, return early.
		if (hundredPair == null) {
			return;
		}

		_operatorStore.Division.HundredsStats[ hundredPair ] += points;
		ReorderByFloats( _operatorStore.Division.HundredsDifficultySorted, hundredPair, 3, taskOperator );

		// If the thousandsPair does not exist, return early.
		if (thousandsPair == null) {
			return;
		}

		_operatorStore.Division.ThousandsStats[ thousandsPair ] += points;
		ReorderByFloats( _operatorStore.Division.ThousandsDifficultySorted, thousandsPair, 4, taskOperator );
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
	private static void UpdateMultiplicationDatabase ( string taskOperator, float points, string decimalPair, string onerPair, string tennerPair, string hundredPair, string thousandsPair ) {
		if (decimalPair != null) {
			_operatorStore.Multiplication.DecimalStats[ decimalPair ] += points;
			ReorderByFloats( _operatorStore.Multiplication.DecimalDifficultySorted, decimalPair, 0, taskOperator );
		}

		// If the onerPair does not exist, return early.
		if (onerPair == null) {
			return;
		}
		
		_operatorStore.Multiplication.OneStats[ onerPair ] += points;
		ReorderByFloats( _operatorStore.Multiplication.OneDifficultySorted, onerPair, 1, taskOperator );

		// If the tennerPair does not exist, return early.
		if (tennerPair == null) {
			return;
		}
		
		_operatorStore.Multiplication.TensStats[ tennerPair ] += points;
		ReorderByFloats( _operatorStore.Multiplication.TensDifficultySorted, tennerPair, 2, taskOperator );

		// If the hundredPair does not exist, return early.
		if (hundredPair == null) {
			return;
		}
		
		_operatorStore.Multiplication.HundredsStats[ hundredPair ] += points;
		ReorderByFloats( _operatorStore.Multiplication.HundredsDifficultySorted, hundredPair, 3, taskOperator );

		// If the thousandsPair does not exist, return early.
		if (thousandsPair == null) {
			return;
		}

		_operatorStore.Multiplication.ThousandsStats[ thousandsPair ] += points;
		ReorderByFloats( _operatorStore.Multiplication.ThousandsDifficultySorted, thousandsPair, 4, taskOperator );
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
	private static void UpdateSubtractionDatabase ( string taskOperator, float points, string decimalPair, string onerPair, string tennerPair, string hundredPair, string thousandsPair ) {
		if (decimalPair != null) {
			_operatorStore.Subtraction.DecimalStats[ decimalPair ] += points;
			ReorderByFloats( _operatorStore.Subtraction.DecimalDifficultySorted, decimalPair, 0, taskOperator );
		}

		// If the onerPair does not exist, return early.
		if (onerPair == null) {
			return;
		}
		
		_operatorStore.Subtraction.OneStats[ onerPair ] += points;
		ReorderByFloats( _operatorStore.Subtraction.OneDifficultySorted, onerPair, 1, taskOperator );

		// If the tennerPair does not exist, return early.
		if (tennerPair == null) {
			return;
		}
		
		_operatorStore.Subtraction.TensStats[ tennerPair ] += points;
		ReorderByFloats( _operatorStore.Subtraction.TensDifficultySorted, tennerPair, 2, taskOperator );

		// If the hundredPair does not exist, return early.
		if (hundredPair == null) {
			return;
		}
		
		_operatorStore.Subtraction.HundredsStats[ hundredPair ] += points;
		ReorderByFloats( _operatorStore.Subtraction.HundredsDifficultySorted, hundredPair, 3, taskOperator );

		// If the thousandsPair does not exist, return early.
		if (thousandsPair == null) {
			return;
		}

		_operatorStore.Subtraction.ThousandsStats[ thousandsPair ] += points;
		ReorderByFloats( _operatorStore.Subtraction.ThousandsDifficultySorted, thousandsPair, 4, taskOperator );
	}

	/// <summary>
	/// Updates one of four categories by: Registering points, sorting the difficulty lists connected to the registered points.
	/// </summary>
	/// <param name="mathTask"></param>
	/// <param name="points"></param>
	/// <param name="decimalPair"></param>
	/// <param name="onerPair"></param>
	/// <param name="tennerPair"></param>
	/// <param name="hundredPair"></param>
	/// <param name="thousandsPair"></param>
	private static void UpdateAdditiveDatabase ( string mathTask, float points, string decimalPair, string onerPair, string tennerPair, string hundredPair, string thousandsPair ) {
		
		if (decimalPair != null) {
			_operatorStore.Addition.DecimalStats[ decimalPair ] += points;
			ReorderByFloats( _operatorStore.Addition.DecimalDifficultySorted, decimalPair, 0, mathTask );
		}

		// If the onerPair does not exist, return early.
		if (onerPair == null) {
			UnityEngine.Debug.LogError("OnerPair is null;");
			return;
		}

		_operatorStore.Addition.OneStats[ onerPair ] += points;
		ReorderByFloats( _operatorStore.Addition.OneDifficultySorted, onerPair, 1, mathTask );
		
		// If the tennerPair does not exist, return early.
		if (tennerPair == null || tennerPair == "0+0") {
			//UnityEngine.Debug.LogError( "TennerPair is null;" );
			return;
		}

		_operatorStore.Addition.TensStats[ tennerPair ] += points;
		ReorderByFloats( _operatorStore.Addition.TensDifficultySorted, tennerPair, 2, mathTask );

		// If the hundredPair does not exist, return early.
		if (hundredPair == null || hundredPair == "0+0") {
			return;
		}

		_operatorStore.Addition.HundredsStats[ hundredPair ] += points;
		ReorderByFloats( _operatorStore.Addition.HundredsDifficultySorted, hundredPair, 3, mathTask );

		// If the thousandsPair does not exist, return early.
		if (thousandsPair == null || thousandsPair == "0+0") {
			return;
		}

		_operatorStore.Addition.ThousandsStats[ thousandsPair ] += points;
		ReorderByFloats( _operatorStore.Addition.ThousandsDifficultySorted, thousandsPair, 4, mathTask );
	}
	
	/// <summary>
	/// Reorder each category's difficulty list depending on float values registered.
	/// </summary>
	/// <param name="difficultySortedList"></param>
	/// <param name="componentPair"></param>
	/// <param name="decimalSpot"></param>
	/// <param name="operatorString"></param>


	private static void ReorderByFloats ( List<string> difficultySortedList, string componentPair, int decimalSpot, string operatorString ) {
		string[] difficultySortedArray = difficultySortedList.ToArray();

		int currentIndex = Array.IndexOf( difficultySortedArray, componentPair );
		float currentPairValue = GetFloat( componentPair, decimalSpot, operatorString );
		int lowerEntryIndex = (currentIndex - 1 >= 0) ? currentIndex - 1 : 0;
		int higherEntryIndex = (currentIndex + 1 < difficultySortedArray.Length) ? currentIndex - 1 : 0;
		float lowerEntryFloat = GetFloat( componentPair, decimalSpot, operatorString );
		float higherEntryFloat = GetFloat( componentPair, decimalSpot, operatorString );

		if (lowerEntryFloat < currentPairValue) {
			while (lowerEntryFloat < currentPairValue) {
				
				string oldValue = difficultySortedArray[ lowerEntryIndex ];
				difficultySortedArray[ lowerEntryIndex ] = componentPair;
				difficultySortedArray[ currentIndex ] = oldValue;
				currentIndex = lowerEntryIndex;

				if (currentIndex == 0) { break; } // We've moved the item to the top of the list.

				currentPairValue = GetFloat( componentPair, decimalSpot, operatorString );
				lowerEntryIndex = (currentIndex - 1 >= 0) ? currentIndex - 1 : 0;
				lowerEntryFloat = GetFloat( componentPair, decimalSpot, operatorString );

			}
		} else if (higherEntryFloat > currentPairValue) {
			while (higherEntryFloat > currentPairValue) {

				string oldValue = difficultySortedArray[ higherEntryIndex ];
				difficultySortedArray[ higherEntryIndex ] = componentPair;
				difficultySortedArray[ currentIndex ] = oldValue;
				currentIndex = higherEntryIndex;

				if (currentIndex == difficultySortedArray.Length - 1) { break; } // We've moved the item to the top of the list.

				currentPairValue = GetFloat( componentPair, decimalSpot, operatorString );
				higherEntryIndex = (currentIndex + 1 < difficultySortedArray.Length) ? currentIndex - 1 : 0;
				higherEntryFloat = GetFloat( componentPair, decimalSpot, operatorString );
			}
		}
		difficultySortedList.Clear();

		foreach ( string pairValue in difficultySortedArray ) {
			difficultySortedList.Add( pairValue );
		}
	}
	
	/// <summary>
	/// Gets the correct float from our operatorStore.
	/// </summary>
	/// <param name="componentPair"></param>
	/// <param name="decimalSpot"></param>
	/// <param name="operatorString"></param>
	/// <returns></returns>
	private static float GetFloat ( string componentPair, int decimalSpot, string operatorString ) {
		switch (operatorString) {
			case "+":
				switch (decimalSpot) {
					case 0:
						return _operatorStore.Addition.DecimalStats[ componentPair ];
					case 1:
						return _operatorStore.Addition.OneStats[ componentPair ];
					case 2:
						return _operatorStore.Addition.TensStats[ componentPair ];
					case 3:
						return _operatorStore.Addition.HundredsStats[ componentPair ];
					case 4:
						return _operatorStore.Addition.ThousandsStats[ componentPair ];
				}
				break;
			case "-":
				switch (decimalSpot) {
					case 0:
						return _operatorStore.Subtraction.DecimalStats[ componentPair ];
					case 1:
						return _operatorStore.Subtraction.OneStats[ componentPair ];
					case 2:
						return _operatorStore.Subtraction.TensStats[ componentPair ];
					case 3:
						return _operatorStore.Subtraction.HundredsStats[ componentPair ];
					case 4:
						return _operatorStore.Subtraction.ThousandsStats[ componentPair ];
				}
				break;
			case "*":
				switch (decimalSpot) {
					case 0:
						return _operatorStore.Multiplication.DecimalStats[ componentPair ];
					case 1:
						return _operatorStore.Multiplication.OneStats[ componentPair ];
					case 2:
						return _operatorStore.Multiplication.TensStats[ componentPair ];
					case 3:
						return _operatorStore.Multiplication.HundredsStats[ componentPair ];
					case 4:
						return _operatorStore.Multiplication.ThousandsStats[ componentPair ];
				}
				break;
			case "/":
			case ":":
				switch (decimalSpot) {
					case 0:
						return _operatorStore.Division.DecimalStats[ componentPair ];
					case 1:
						return _operatorStore.Division.OneStats[ componentPair ];
					case 2:
						return _operatorStore.Division.TensStats[ componentPair ];
					case 3:
						return _operatorStore.Division.HundredsStats[ componentPair ];
					case 4:
						return _operatorStore.Division.ThousandsStats[ componentPair ];
				}
				break;
		}
		return 0f;
	}
	
	/// <summary>
	/// This function extracts a decimal-pair from the components.
	/// </summary>
	/// <param name="mathTask"></param>
	/// <param name="firstComponent"></param>
	/// <param name="secondComponent"></param>
	/// <returns></returns>
	private static string GetDecimalPair ( MathTask mathTask, string firstComponent, string secondComponent ) {
		if (firstComponent.IndexOf(",") != -1 || firstComponent.IndexOf( "." ) != -1 || secondComponent.IndexOf( "," ) != -1 || secondComponent.IndexOf( "." ) != -1) {
			string first = "0";
			string second = "0";
			if (firstComponent.IndexOf( "," ) != -1) {
				first = firstComponent.Split( "," )[ 1 ];
			} else if (firstComponent.IndexOf( "." ) != -1) {
				first = firstComponent.Split( "." )[ 1 ];
			}

			if (secondComponent.IndexOf( "," ) != -1) {
				second = secondComponent.Split( "," )[ 1 ];
			} else if (secondComponent.IndexOf( "." ) != -1) {
				second = secondComponent.Split( "." )[ 1 ];
			}

			return $"{first}{mathTask.Operator}{second}";
		}
		return null;
	}
	
	/// <summary>
	/// This function cets a pair of "x+y" dependant on 
	/// </summary>
	/// <param name="mathTask"></param>
	/// <param name="length"></param>
	/// <param name="firstComponent"></param>
	/// <param name="secondComponent"></param>
	/// <returns></returns>
	private static string GetPair ( string mathTaskOperator, int length, string firstComponent, string secondComponent ) {
		/* Function Plan:
		 *		1. Check that length of first & Second Components
		 *		2. get the correct positioned (if it exists) number, return paired numbers.
		 */
		string first = "0";
		string second = "0";

		if (firstComponent.Length >= length) {
			int firstComponentStart = firstComponent.Length - length;
			first = firstComponent.Substring( firstComponentStart, 1 );
		}
		if (secondComponent.Length >= length) {
			int secondComponentStart = secondComponent.Length - length;
			second = secondComponent.Substring( secondComponentStart, 1 );
		}	
		
		return $"{first}{mathTaskOperator}{second}";
	}
	/// <summary>
	/// Returns the complete OperatorStore.
	/// </summary>
	public static OperatorStore GetStore {  get { return _operatorStore; } }
	/// <summary>
	/// Produces difficulty lists dependant on the Operator and Difficulty provided.
	/// </summary>
	/// <param name="Operator"></param>
	/// <param name="difficulty"></param>
	/// <returns></returns>
	public static DifficultyList GetDifficultyLists (string Operator, string difficulty) {
		if (Operator == default) {
			Debug.LogError($"Operator is: '{Operator}'");
		}
		DifficultyList _difficultyLists = new() {
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

		if ( _tempOperator.OneDifficultySorted == default || 
			_tempOperator.TensDifficultySorted == default || 
			_tempOperator.HundredsDifficultySorted == default ||
			_tempOperator.ThousandsDifficultySorted == default) {

			Debug.LogError( $"_tempOperator not properly formed." );
			return new DifficultyList();
		}
		
		switch (difficulty) {
			case "e":
			case "E":
				//_difficultyLists.Decimal = _tempOperator.DecimalDifficultySorted.GetRange(0,20);
				_difficultyLists.One = _tempOperator.OneDifficultySorted.GetRange(0, 20);
				_difficultyLists.Tens = _tempOperator.TensDifficultySorted.GetRange(0, 20);
				_difficultyLists.Hundreds = _tempOperator.HundredsDifficultySorted.GetRange(0, 20);
				_difficultyLists.Thousands = _tempOperator.ThousandsDifficultySorted.GetRange(0, 20);
				break;
			case "m":
			case "M":
				//_difficultyLists.Decimal = _tempOperator.DecimalDifficultySorted.GetRange( 39, 20 );
				_difficultyLists.One = _tempOperator.OneDifficultySorted.GetRange( 39, 20 );
				_difficultyLists.Tens = _tempOperator.TensDifficultySorted.GetRange( 39, 20 );
				_difficultyLists.Hundreds = _tempOperator.HundredsDifficultySorted.GetRange( 39, 20 );
				_difficultyLists.Thousands = _tempOperator.ThousandsDifficultySorted.GetRange( 39, 20 );
				break;
			case "h":
			case "H":
				//_difficultyLists.Decimal = _tempOperator.DecimalDifficultySorted.GetRange( 63, 33 );
				_difficultyLists.One = _tempOperator.OneDifficultySorted.GetRange( _tempOperator.OneDifficultySorted.Count - 10, 10 );
				_difficultyLists.Tens = _tempOperator.TensDifficultySorted.GetRange( _tempOperator.TensDifficultySorted.Count - 10, 10 );
				_difficultyLists.Hundreds = _tempOperator.HundredsDifficultySorted.GetRange( _tempOperator.HundredsDifficultySorted.Count - 10, 10 );
				_difficultyLists.Thousands = _tempOperator.ThousandsDifficultySorted.GetRange( _tempOperator.ThousandsDifficultySorted.Count - 10, 10 );
				break;
		}

		return _difficultyLists;
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
public struct DifficultyList {
	//public List<string> Decimal;
	public List<string> One;
	public List<string> Tens;
	public List<string> Hundreds;
	public List<string> Thousands;
}

