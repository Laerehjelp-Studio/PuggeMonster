using UnityEngine;

public abstract class Category : ScriptableObject {
	public string Name = "";
	public int CategoryStart = 0;
	public int CategoryEnd = 100;
	public string Result = ""; 
	public virtual bool SelectRandom (float generalMasteryChance) {
		return (Random.Range( CategoryStart, CategoryEnd ) < generalMasteryChance);
	}
}
