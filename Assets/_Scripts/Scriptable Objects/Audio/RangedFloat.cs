using System;

/* Source: 
 *		Audio ScriptableObjects are inspired by https://www.youtube.com/watch?v=6vmRwLYWNRo , 
 *		as well as https://github.com/richard-fine/scriptable-object-demo/tree/main/Assets/ScriptableObject/Audio */
[Serializable]
public struct RangedFloat
{
	public float minValue; 
	public float maxValue;

	public RangedFloat(float minValue, float maxValue) {
		this.minValue = minValue;
		this.maxValue = maxValue;
	}
}
