using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu( menuName = "Scriptable Objects/Subject", fileName = "G001 Subject" )]
public class Subject : ScriptableObject {
	public string Name = "";
	public Subjects SubjectType;
	[SerializeField] private MathCategory[] MathCategories;


	public virtual MathCategory SelectCategoryByGMChance ( int generalMasteryChance ) {
		int MathCategoryLength = MathCategories.Length;
		
		if (MathCategoryLength == 0) {
			Debug.LogError("Unable to select MathCategory - MathCategory[] empty.");
			return null;
		}
		if (MathCategoryLength > 1) {
			MathCategory tempMathCategory = ScriptableObject.CreateInstance<MathCategory>(  );
			bool foundCategory = false;
			foreach (MathCategory item in MathCategories) {
				if (generalMasteryChance < item.CategoryStart && item.CategoryEnd < generalMasteryChance) {
					Debug.LogError("generalMasteryChance outside parameters.");
					continue;
				}

				if (item.SelectRandom(generalMasteryChance)) {
					//Debug.Log($"Random Category Selected: {item.Name}");
					tempMathCategory = item;
					foundCategory = true;
					break;
				}
			}
			
			if (foundCategory) {
				//Debug.Log($"Selecting Category1: {tempMathCategory.Name}");
				return tempMathCategory;
			}
		}

		//Debug.Log($"Selecting Category2: {MathCategories[ 0 ].Name}, length: {MathCategoryLength}");
		return MathCategories[0];
	}
	public enum Subjects {
		None,
		Math,
		Letters,
		Words
	}
}
