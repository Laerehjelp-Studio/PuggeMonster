
using System;
using System.Collections.Generic;
using UnityEngine.UIElements;
using static UnityEngine.ParticleSystem;

static public class StatManager {
	private static OperatorStore operatorStore = new();

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


	public static void RegisterAnswer ( MathTask mathTask, float selectedValue, float points ) {
		/* Function Plan: 
		 *	1. Separate Math task into answer-pairs (decimals, ones, tens, hundreds, thousands)
		 *	2. Register points boost/decrease.
		 *	3. Sort DifficultyLists.
		 */
		string decimalPair = null;
		string onerPair = null;
		string tennerPair = null;
		string hundredPair = null;
		string thousandsPair = null;

		string firstComponent = $"{mathTask.Components[ 0 ]}";
		string secondComponent = $"{mathTask.Components[ 1 ]}";

		thousandsPair = GetPair( mathTask, 4, firstComponent, secondComponent );
		hundredPair = GetPair( mathTask, 3, firstComponent, secondComponent );
		tennerPair = GetPair( mathTask, 2, firstComponent, secondComponent );
		onerPair = GetPair( mathTask, 1, firstComponent, secondComponent );

		decimalPair = GetDecimalPair( mathTask, firstComponent, secondComponent );

		string mathPiece = $"{mathTask.Components[ 0 ]}{mathTask.Operator}{mathTask.Components[ 1 ]}";

		switch (mathTask.Operator) {
			case "+":
				operatorStore.Addition.DecimalStats[decimalPair] += points;
				operatorStore.Addition.OneStats[ decimalPair ] += points;
				operatorStore.Addition.TensStats[ decimalPair ] += points;
				operatorStore.Addition.HundredsStats[ decimalPair ] += points;
				operatorStore.Addition.ThousandsStats[ thousandsPair ] += points;

				ReorderByFloats(operatorStore.Addition.DecimalDifficultySorted, decimalPair, 0, mathTask.Operator );
				ReorderByFloats(operatorStore.Addition.OneDifficultySorted, onerPair, 1, mathTask.Operator );
				ReorderByFloats(operatorStore.Addition.TensDifficultySorted, tennerPair, 2, mathTask.Operator );
				ReorderByFloats(operatorStore.Addition.HundredsDifficultySorted, hundredPair, 3, mathTask.Operator );
				ReorderByFloats(operatorStore.Addition.ThousandsDifficultySorted, thousandsPair, 4, mathTask.Operator );
				break;
			case "-":
				operatorStore.Subtraction.DecimalStats[ decimalPair ] += points;
				operatorStore.Subtraction.OneStats[ decimalPair ] += points;
				operatorStore.Subtraction.TensStats[ decimalPair ] += points;
				operatorStore.Subtraction.HundredsStats[ decimalPair ] += points;
				operatorStore.Subtraction.ThousandsStats[ thousandsPair ] += points;

				ReorderByFloats(operatorStore.Subtraction.DecimalDifficultySorted, decimalPair, 0, mathTask.Operator );
				ReorderByFloats(operatorStore.Subtraction.OneDifficultySorted, onerPair, 1, mathTask.Operator );
				ReorderByFloats(operatorStore.Subtraction.TensDifficultySorted, tennerPair, 2, mathTask.Operator );
				ReorderByFloats(operatorStore.Subtraction.HundredsDifficultySorted, hundredPair, 3, mathTask.Operator );
				ReorderByFloats( operatorStore.Subtraction.ThousandsDifficultySorted, thousandsPair, 4, mathTask.Operator );

				break;
			case "*":
				operatorStore.Multiplication.DecimalStats[ decimalPair ] += points;
				operatorStore.Multiplication.OneStats[ decimalPair ] += points;
				operatorStore.Multiplication.TensStats[ decimalPair ] += points;
				operatorStore.Multiplication.HundredsStats[ decimalPair ] += points;
				operatorStore.Multiplication.ThousandsStats[ thousandsPair ] += points;

				ReorderByFloats(operatorStore.Multiplication.DecimalDifficultySorted, decimalPair, 0, mathTask.Operator );
				ReorderByFloats( operatorStore.Multiplication.OneDifficultySorted, onerPair, 1, mathTask.Operator );
				ReorderByFloats( operatorStore.Multiplication.TensDifficultySorted, tennerPair, 2, mathTask.Operator );
				ReorderByFloats( operatorStore.Multiplication.HundredsDifficultySorted, hundredPair, 3, mathTask.Operator );
				ReorderByFloats( operatorStore.Multiplication.ThousandsDifficultySorted, thousandsPair, 4, mathTask.Operator );

				break;

			case "/":
			case ":":
				operatorStore.Division.DecimalStats[ decimalPair ] += points;
				operatorStore.Division.OneStats[ decimalPair ] += points;
				operatorStore.Division.TensStats[ decimalPair ] += points;
				operatorStore.Division.HundredsStats[ decimalPair ] += points;
				operatorStore.Division.ThousandsStats[ thousandsPair ] += points;

				ReorderByFloats( operatorStore.Division.DecimalDifficultySorted, decimalPair, 0, mathTask.Operator );
				ReorderByFloats( operatorStore.Division.OneDifficultySorted, onerPair,1, mathTask.Operator );
				ReorderByFloats( operatorStore.Division.TensDifficultySorted, tennerPair,2, mathTask.Operator );
				ReorderByFloats( operatorStore.Division.HundredsDifficultySorted, hundredPair,3, mathTask.Operator );
				ReorderByFloats( operatorStore.Division.ThousandsDifficultySorted, thousandsPair, 4, mathTask.Operator );

				break;
		}
	}

	private static void ReorderByFloats ( List<string> difficultySortedList, string componentPair, int decimalSpot, string operatorString ) {
		string[] difficultySortedArray = difficultySortedList.ToArray();

		int currentIndex = Array.IndexOf( difficultySortedArray, componentPair );
		float currentpairValue = GetFloat(componentPair, decimalSpot, operatorString );
		int lowerEntryIndex = (currentIndex - 1 >= 0) ? currentIndex - 1 : 0;
		int higherEntryIndex = (currentIndex + 1 <= difficultySortedArray.Length - 1) ? currentIndex - 1 : 0;
		float lowerEntryfloat = GetFloat(componentPair, decimalSpot, operatorString );
		float higherEntryFloat = GetFloat( componentPair, decimalSpot, operatorString );


	}

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
		return float.MinValue;
	}

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

