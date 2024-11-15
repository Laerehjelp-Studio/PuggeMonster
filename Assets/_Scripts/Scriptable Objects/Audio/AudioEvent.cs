using UnityEngine;

/* Source: 
 *		Audio ScriptableObjects are inspired by https://www.youtube.com/watch?v=6vmRwLYWNRo , 
 *		as well as https://github.com/richard-fine/scriptable-object-demo/tree/main/Assets/ScriptableObject/Audio */
public abstract class AudioEvent : ScriptableObject
{
	public abstract void Play(AudioSource source);
}
