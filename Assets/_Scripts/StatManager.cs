
using System;
using System.Collections.Generic;

static public class StatManager {
	private static OperatorStore operatorStore = new();

	/// <summary>
	/// Initialize all dictionaries, and lists we are using for storage.
	/// </summary>
	public static void Initialize () {
		Dictionary<string, float> _initDictAddition = new();
		List<string> _initListAddition = new();
		Dictionary<string, float> _initDictSubtraction = new();
		List<string> _initListSubtraction = new();
		Dictionary<string, float> _initDictMultiplication = new();
		List<string> _initListMultiplication = new();
		Dictionary<string, float> _initDictDivision = new();
		List<string> _initListDivision = new();

		for (int first = 0; first < 9; first++) {
			for (int second = 0; second < 9; second++) {
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


		operatorStore.Addition.DecimalStats = new( _initDictAddition );
		operatorStore.Addition.OneStats = new( _initDictAddition );
		operatorStore.Addition.TensStats = new( _initDictAddition );
		operatorStore.Addition.HundredsStats = new( _initDictAddition );
		operatorStore.Addition.ThousandsStats = new( _initDictAddition );
		operatorStore.Addition.DecimalDifficultySorted = new( _initListAddition );
		operatorStore.Addition.OneDifficultySorted = new( _initListAddition );
		operatorStore.Addition.TensDifficultySorted = new( _initListAddition );
		operatorStore.Addition.HundredsDifficultySorted = new( _initListAddition );
		operatorStore.Addition.ThousandsDifficultySorted = new( _initListAddition );
		operatorStore.Subtraction.DecimalStats = new( _initDictSubtraction );
		operatorStore.Subtraction.OneStats = new( _initDictSubtraction );
		operatorStore.Subtraction.TensStats = new( _initDictSubtraction );
		operatorStore.Subtraction.HundredsStats = new( _initDictSubtraction );
		operatorStore.Subtraction.ThousandsStats = new( _initDictSubtraction );
		operatorStore.Subtraction.DecimalDifficultySorted = new( _initListSubtraction );
		operatorStore.Subtraction.OneDifficultySorted = new( _initListSubtraction );
		operatorStore.Subtraction.TensDifficultySorted = new( _initListSubtraction );
		operatorStore.Subtraction.HundredsDifficultySorted = new( _initListSubtraction );
		operatorStore.Subtraction.ThousandsDifficultySorted = new( _initListSubtraction );
		operatorStore.Division.DecimalStats = new( _initDictDivision );
		operatorStore.Division.OneStats = new( _initDictDivision );
		operatorStore.Division.TensStats = new( _initDictDivision );
		operatorStore.Division.HundredsStats = new( _initDictDivision );
		operatorStore.Division.ThousandsStats = new( _initDictDivision );
		operatorStore.Division.DecimalDifficultySorted = new( _initListDivision );
		operatorStore.Division.OneDifficultySorted = new( _initListDivision );
		operatorStore.Division.TensDifficultySorted = new( _initListDivision );
		operatorStore.Division.HundredsDifficultySorted = new( _initListDivision );
		operatorStore.Division.ThousandsDifficultySorted = new( _initListDivision );
		operatorStore.Multiplication.DecimalStats = new( _initDictMultiplication );
		operatorStore.Multiplication.OneStats = new( _initDictMultiplication );
		operatorStore.Multiplication.TensStats = new( _initDictMultiplication );
		operatorStore.Multiplication.HundredsStats = new( _initDictMultiplication );
		operatorStore.Multiplication.ThousandsStats = new( _initDictMultiplication );
		operatorStore.Multiplication.DecimalDifficultySorted = new( _initListMultiplication );
		operatorStore.Multiplication.OneDifficultySorted = new( _initListMultiplication );
		operatorStore.Multiplication.TensDifficultySorted = new( _initListMultiplication );
		operatorStore.Multiplication.HundredsDifficultySorted = new( _initListMultiplication );
		operatorStore.Multiplication.ThousandsDifficultySorted = new( _initListMultiplication );
	}

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

		string thousandsPair = GetPair( mathTask, 4, firstComponent, secondComponent );
		string hundredPair = GetPair( mathTask, 3, firstComponent, secondComponent );
		string tennerPair = GetPair( mathTask, 2, firstComponent, secondComponent );
		string onerPair = GetPair( mathTask, 1, firstComponent, secondComponent );

		string decimalPair = GetDecimalPair( mathTask, firstComponent, secondComponent );

		string mathPiece = $"{mathTask.Components[ 0 ]}{mathTask.Operator}{mathTask.Components[ 1 ]}";

		switch (mathTask.Operator) {
			case "+":
				UpdateAdditiveDatabase( mathTask.Operator, points, decimalPair, onerPair, tennerPair, hundredPair, thousandsPair );
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
			operatorStore.Division.DecimalStats[ decimalPair ] += points;
			ReorderByFloats( operatorStore.Division.DecimalDifficultySorted, decimalPair, 0, taskOperator );
		}

		// If the onerPair does not exist, return early.
		if (onerPair == null) {
			return;
		}

		operatorStore.Division.OneStats[ onerPair ] += points;
		ReorderByFloats( operatorStore.Division.OneDifficultySorted, onerPair, 1, taskOperator );

		// If the tennerPair does not exist, return early.
		if (tennerPair == null) {
			return;
		}

		operatorStore.Division.TensStats[ tennerPair ] += points;
		ReorderByFloats( operatorStore.Division.TensDifficultySorted, tennerPair, 2, taskOperator );

		// If the hundredPair does not exist, return early.
		if (hundredPair == null) {
			return;
		}

		operatorStore.Division.HundredsStats[ hundredPair ] += points;
		ReorderByFloats( operatorStore.Division.HundredsDifficultySorted, hundredPair, 3, taskOperator );

		// If the thousandsPair does not exist, return early.
		if (thousandsPair == null) {
			return;
		}

		operatorStore.Division.ThousandsStats[ thousandsPair ] += points;
		ReorderByFloats( operatorStore.Division.ThousandsDifficultySorted, thousandsPair, 4, taskOperator );
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
			operatorStore.Multiplication.DecimalStats[ decimalPair ] += points;
			ReorderByFloats( operatorStore.Multiplication.DecimalDifficultySorted, decimalPair, 0, taskOperator );
		}

		// If the onerPair does not exist, return early.
		if (onerPair == null) {
			return;
		}
		
		operatorStore.Multiplication.OneStats[ onerPair ] += points;
		ReorderByFloats( operatorStore.Multiplication.OneDifficultySorted, onerPair, 1, taskOperator );

		// If the tennerPair does not exist, return early.
		if (tennerPair == null) {
			return;
		}
		
		operatorStore.Multiplication.TensStats[ tennerPair ] += points;
		ReorderByFloats( operatorStore.Multiplication.TensDifficultySorted, tennerPair, 2, taskOperator );

		// If the hundredPair does not exist, return early.
		if (hundredPair == null) {
			return;
		}
		
		operatorStore.Multiplication.HundredsStats[ hundredPair ] += points;
		ReorderByFloats( operatorStore.Multiplication.HundredsDifficultySorted, hundredPair, 3, taskOperator );

		// If the thousandsPair does not exist, return early.
		if (thousandsPair == null) {
			return;
		}

		operatorStore.Multiplication.ThousandsStats[ thousandsPair ] += points;
		ReorderByFloats( operatorStore.Multiplication.ThousandsDifficultySorted, thousandsPair, 4, taskOperator );
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
			operatorStore.Subtraction.DecimalStats[ decimalPair ] += points;
			ReorderByFloats( operatorStore.Subtraction.DecimalDifficultySorted, decimalPair, 0, taskOperator );
		}

		// If the onerPair does not exist, return early.
		if (onerPair == null) {
			return;
		}
		
		operatorStore.Subtraction.OneStats[ onerPair ] += points;
		ReorderByFloats( operatorStore.Subtraction.OneDifficultySorted, onerPair, 1, taskOperator );

		// If the tennerPair does not exist, return early.
		if (tennerPair == null) {
			return;
		}
		
		operatorStore.Subtraction.TensStats[ tennerPair ] += points;
		ReorderByFloats( operatorStore.Subtraction.TensDifficultySorted, tennerPair, 2, taskOperator );

		// If the hundredPair does not exist, return early.
		if (hundredPair == null) {
			return;
		}
		
		operatorStore.Subtraction.HundredsStats[ hundredPair ] += points;
		ReorderByFloats( operatorStore.Subtraction.HundredsDifficultySorted, hundredPair, 3, taskOperator );

		// If the thousandsPair does not exist, return early.
		if (thousandsPair == null) {
			return;
		}

		operatorStore.Subtraction.ThousandsStats[ thousandsPair ] += points;
		ReorderByFloats( operatorStore.Subtraction.ThousandsDifficultySorted, thousandsPair, 4, taskOperator );
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
			operatorStore.Addition.DecimalStats[ decimalPair ] += points;
			ReorderByFloats( operatorStore.Addition.DecimalDifficultySorted, decimalPair, 0, mathTask );
		}

		// If the onerPair does not exist, return early.
		if (onerPair == null) {
			//UnityEngine.Debug.LogError("OnerPair is null;");
			return;
		}

		//UnityEngine.Debug.Log(UnityEngine.JsonUtility.ToJson( operatorStore.Addition.OneStats ) );
		operatorStore.Addition.OneStats[ onerPair ] += points;
		ReorderByFloats( operatorStore.Addition.OneDifficultySorted, onerPair, 1, mathTask );
		

		// If the tennerPair does not exist, return early.
		if (tennerPair == null) {
			//UnityEngine.Debug.LogError( "TennerPair is null;" );
			return;
		}

		operatorStore.Addition.TensStats[ tennerPair ] += points;
		ReorderByFloats( operatorStore.Addition.TensDifficultySorted, tennerPair, 2, mathTask );

		// If the hundredPair does not exist, return early.
		if (hundredPair == null) {
			return;
		}

		operatorStore.Addition.HundredsStats[ hundredPair ] += points;
		ReorderByFloats( operatorStore.Addition.HundredsDifficultySorted, hundredPair, 3, mathTask );

		// If the thousandsPair does not exist, return early.
		if (thousandsPair == null) {
			return;
		}

		operatorStore.Addition.ThousandsStats[ thousandsPair ] += points;
		ReorderByFloats( operatorStore.Addition.ThousandsDifficultySorted, thousandsPair, 4, mathTask );
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
						return operatorStore.Addition.DecimalStats[ componentPair ];
					case 1:
						return operatorStore.Addition.OneStats[ componentPair ];
					case 2:
						return operatorStore.Addition.TensStats[ componentPair ];
					case 3:
						return operatorStore.Addition.HundredsStats[ componentPair ];
					case 4:
						return operatorStore.Addition.ThousandsStats[ componentPair ];
				}
				break;
			case "-":
				switch (decimalSpot) {
					case 0:
						return operatorStore.Subtraction.DecimalStats[ componentPair ];
					case 1:
						return operatorStore.Subtraction.OneStats[ componentPair ];
					case 2:
						return operatorStore.Subtraction.TensStats[ componentPair ];
					case 3:
						return operatorStore.Subtraction.HundredsStats[ componentPair ];
					case 4:
						return operatorStore.Subtraction.ThousandsStats[ componentPair ];
				}
				break;
			case "*":
				switch (decimalSpot) {
					case 0:
						return operatorStore.Multiplication.DecimalStats[ componentPair ];
					case 1:
						return operatorStore.Multiplication.OneStats[ componentPair ];
					case 2:
						return operatorStore.Multiplication.TensStats[ componentPair ];
					case 3:
						return operatorStore.Multiplication.HundredsStats[ componentPair ];
					case 4:
						return operatorStore.Multiplication.ThousandsStats[ componentPair ];
				}
				break;
			case "/":
			case ":":
				switch (decimalSpot) {
					case 0:
						return operatorStore.Division.DecimalStats[ componentPair ];
					case 1:
						return operatorStore.Division.OneStats[ componentPair ];
					case 2:
						return operatorStore.Division.TensStats[ componentPair ];
					case 3:
						return operatorStore.Division.HundredsStats[ componentPair ];
					case 4:
						return operatorStore.Division.ThousandsStats[ componentPair ];
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
	private static string GetPair ( MathTask mathTask, int length, string firstComponent, string secondComponent ) {
		if (firstComponent.Length == length || secondComponent.Length == length) {
			string first = (firstComponent.Length == length) ? firstComponent.Substring( 0, 1 ) : "0";
			string second = (secondComponent.Length == length) ? secondComponent.Substring( 0, 1 ) : "0";
			
			return $"{first}{mathTask.Operator}{second}";
		}

		return null;
	}
}

public struct OperatorStore {
	public Operator Addition; 
	public Operator Subtraction; 
	public Operator Multiplication; 
	public Operator Division;
}
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

