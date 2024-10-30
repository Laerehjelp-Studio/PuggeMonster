using System;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu( menuName = "Scriptable Objects/Math Category", fileName = "G001 Math ...." )]
public class MathCategory : ScriptableObject {
	public string Name = "";
	public int CategoryStart = 0;
	public int CategoryEnd = 100;
	public string Operator = ""; 
	public virtual bool SelectRandom ( float generalMasteryChance ) {
		// Calculate normalized position in range [0, 1]
		float normalizedPosition = (float)(generalMasteryChance - CategoryStart) / (CategoryEnd - CategoryStart);

		// Calculate probability using exponential decay
		float probability = Mathf.Lerp( 1f, 0f, Mathf.Pow( normalizedPosition, 2 ) ); // Adjust the exponent for steeper/gradual dropoff

		return (Random.value < probability);
	}

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

	private PlacementNumber NewPlacementNumber ( PlacementNumber placementNumber = new(), string name = "", int result = 0 ) {
		placementNumber.Name = (placementNumber.Name != default) ? placementNumber.Name : name;
		placementNumber.CategoryStart = (placementNumber.CategoryStart != default) ? placementNumber.CategoryStart : 0;
		placementNumber.CategoryEnd = (placementNumber.CategoryEnd != default) ? placementNumber.CategoryEnd : 100;
		placementNumber.Result = (placementNumber.Result != default) ? placementNumber.Result : $"{result}";
		return placementNumber;
	}

	public int SelectGMChancePlacementNumber ( int generalMasteryChance ) {
		if (Thousands && SelectGMChancePN( generalMasteryChance, ThousandsMastery )) {
			return int.Parse( ThousandsMastery.Result) ;
		}
		if (Hundreds && SelectGMChancePN( generalMasteryChance, HundredMastery )) {
			return int.Parse( HundredMastery.Result );
		}
		if (Tens && SelectGMChancePN( generalMasteryChance, TensMastery )) {
			return int.Parse( TensMastery.Result );
		}
		if (Ones && SelectGMChancePN( generalMasteryChance, OneMastery )) {
			return int.Parse( OneMastery.Result );
		}
		if (Decimals && SelectGMChancePN( generalMasteryChance, DecimalMastery )) {
			return int.Parse( DecimalMastery.Result );
		}
		return int.Parse( OneMastery.Result );
	}

	public virtual bool SelectGMChancePN ( int generalMasteryChance, PlacementNumber placementNumber ) {
		// Calculate normalized position in range [0, 1]
		float normalizedPosition = (float)(generalMasteryChance - placementNumber.CategoryStart) / (placementNumber.CategoryEnd - placementNumber.CategoryStart);

		// Calculate probability using exponential decay
		float probability = Mathf.Lerp( 1f, 0f, Mathf.Pow( normalizedPosition, 2 ) ); // Adjust the exponent for steeper/gradual dropoff

		//return (Random.value < probability);
		return (Random.Range( placementNumber.CategoryStart , placementNumber.CategoryEnd+1 ) < generalMasteryChance );

	}
}
[Serializable]
public struct PlacementNumber {
	public string Name;
	public int CategoryStart;
	public int CategoryEnd;
	public string Result;
}

