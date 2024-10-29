using UnityEngine;

public abstract class Category : ScriptableObject {
	public string Name = "";
	public float CategoryStart = 0f;
	public float CategoryEnd = 100f;
	public string Result = ""; 
	public virtual bool SelectRandom (float progressChance) {
		return ((CategoryEnd - Random.Range( CategoryStart, CategoryEnd )) < progressChance);
	}
}
