using UnityEngine;

[CreateAssetMenu( menuName = "Scriptable Objects/Subject", fileName = "G001 Subject" )]
public class Subject : ScriptableObject {
	public string Name = "";
	public int GeneralMastery = 0;
	public Subjects SubjectType;
	[SerializeField] private MathCategory[] MathCategory;

	private void Awake () {
		switch (SubjectType) {
			case Subjects.Math:
				StatManager.OnMathGeneralMastery += UpdateMathGeneralMastery;
				break;
			case Subjects.Letters:
				break;
			case Subjects.Words:
				break;
			case Subjects.None:
			default:
				break;
		}
	}

	private void UpdateMathGeneralMastery () {
		GeneralMastery = StatManager.GeneralMathMastery;
	}

	public enum Subjects {
		None,
		Math,
		Letters,
		Words
	}
}
