using System;
using UnityEngine;
[CreateAssetMenu( menuName = "Scriptable Objects/Math Category", fileName = "G001 Math ...." )]
public class MathCategory : Category
{
	[Header("Display on Timeline")]
	[SerializeField] bool Decimals;
	[SerializeField] bool Ones;
	[SerializeField] bool Tens;
	[SerializeField] bool Hundreds;
	[SerializeField] bool Thousands;

	[Header("Placement Number Configuration")]
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
		ThousandsMastery = NewPlacementNumber( ThousandsMastery, "Tusentalls", 4);
	}

	private PlacementNumber NewPlacementNumber ( PlacementNumber placementNumber = new(), string name = "", int result = 0) {
		placementNumber.Name = (placementNumber.Name != default) ? placementNumber.Name : name;
		placementNumber.CategoryStart = (placementNumber.CategoryStart != default) ? placementNumber.CategoryStart : 0f;
		placementNumber.CategoryEnd = (placementNumber.CategoryEnd != default) ? placementNumber.CategoryEnd : 100f;
		placementNumber.Result = (placementNumber.Result != default) ? placementNumber.Result : $"{result}";
		return placementNumber;
	}
}
[Serializable]
public struct PlacementNumber {
	public string Name;
	public float CategoryStart;
	public float CategoryEnd;
	public string Result;
}

