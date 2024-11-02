using System;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu( menuName = "Scriptable Objects/Math Category", fileName = "G001 Math ...." )]
public class MathCategory : ScriptableObject {
	public string Name = "";
	public int CategoryStart;
	public int CategoryEnd = 100;
	public string Operator = ""; 

	[Header( "Enable Placements" )]
	[SerializeField] bool Decimals;
	[SerializeField] bool Ones;
	[SerializeField] bool Tens;
	[SerializeField] bool Hundreds;
	[SerializeField] bool Thousands;

	[Header( "Placement Number Configuration" )]
	[SerializeField] PlacementNumber DecimalMastery;
	[SerializeField] PlacementNumber OneMastery;
	[SerializeField] PlacementNumber TensMastery;
	[SerializeField] PlacementNumber HundredMastery;
	[SerializeField] PlacementNumber ThousandsMastery;

	private void Awake () {
		DecimalMastery = NewPlacementNumber( DecimalMastery, "Desimaler", -1 );
		OneMastery = NewPlacementNumber( OneMastery, "Enere", 1 );
		TensMastery = NewPlacementNumber( TensMastery, "Tiere", 2 );
		HundredMastery = NewPlacementNumber( HundredMastery, "Hundretalls", 3 );
		ThousandsMastery = NewPlacementNumber( ThousandsMastery, "Tusentalls", 4 );
	}
	
	/// <summary>
	/// Randomly returns true based on General Mastery score.
	/// </summary>
	/// <param name="generalMasteryChance"></param>
	/// <returns></returns>
	public bool SelectRandom ( float generalMasteryChance ) {
		if (generalMasteryChance < CategoryStart || generalMasteryChance > CategoryEnd)
		{
			return false;
		}
		
		// Calculate normalized position in range [0, 1]
		float normalizedPosition = (generalMasteryChance - CategoryStart) / (CategoryEnd - CategoryStart);

		// Calculate probability using exponential decay
		float probability = Mathf.Lerp( 1f, 0f, Mathf.Pow( normalizedPosition, 2 ) ); // Adjust the exponent for steeper/gradual drop-off

		return (Random.value < probability);
	}

	private PlacementNumber NewPlacementNumber ( PlacementNumber placementNumber = new(), string _name = "", int result = 0 ) {
		placementNumber.Name = (placementNumber.Name != default) ? placementNumber.Name : _name;
		placementNumber.CategoryStart = (placementNumber.CategoryStart != default) ? placementNumber.CategoryStart : 0;
		placementNumber.CategoryEnd = (placementNumber.CategoryEnd != default) ? placementNumber.CategoryEnd : 100;
		placementNumber.Result = (placementNumber.Result != default) ? placementNumber.Result : $"{result}";
		return placementNumber;
	}

	public int SelectGMChancePlacementNumber ( int generalMasteryChance ) {
		float _maxChance = 0.9f;
		
		if (Decimals && SelectGMChancePN( generalMasteryChance, DecimalMastery, _maxChance )) {
			return int.Parse( DecimalMastery.Result );
		}
		if (Ones && SelectGMChancePN( generalMasteryChance, OneMastery, _maxChance )) {
			return int.Parse( OneMastery.Result );
		}
		if (Tens && SelectGMChancePN( generalMasteryChance, TensMastery, _maxChance )) {
			return int.Parse( TensMastery.Result );
		}
		if (Hundreds && SelectGMChancePN( generalMasteryChance, HundredMastery, _maxChance )) {
			return int.Parse( HundredMastery.Result );
		}
		if (Thousands && SelectGMChancePN( generalMasteryChance, ThousandsMastery, _maxChance )) {
			return int.Parse( ThousandsMastery.Result) ;
		}
		return int.Parse( OneMastery.Result );
	}
	/// <summary>
	/// Selects PlacementNumber based on GM-Chance.
	/// </summary>
	/// <param name="generalMasteryChance"></param>
	/// <param name="placementNumber"></param>
	/// <param name="maxChance"></param>
	/// <returns></returns>
	public bool SelectGMChancePN(int generalMasteryChance, PlacementNumber placementNumber, float maxChance = 0.9f)
	{
		// Check if generalMasteryChance is outside the range
		if (generalMasteryChance < placementNumber.CategoryStart || generalMasteryChance > placementNumber.CategoryEnd)
		{
			return false;
		}

		// Calculate normalized position in range [0, 1]
		float normalizedPosition = (float)(generalMasteryChance - placementNumber.CategoryStart) / (placementNumber.CategoryEnd - placementNumber.CategoryStart);

		// Define maxChance at CategoryStart and decrease it towards CategoryEnd
		float probability = Mathf.Lerp(maxChance, 0f, normalizedPosition * normalizedPosition); // Exponential decay for sharper drop-off

		// Determine whether to return true or false based on the calculated probability
		return Random.value < probability;
	}
}
[Serializable]
public struct PlacementNumber {
	public string Name;
	public int CategoryStart;
	public int CategoryEnd;
	public string Result;
}

