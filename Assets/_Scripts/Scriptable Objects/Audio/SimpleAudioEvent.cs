using UnityEngine;


/* Source: 
 *		Audio ScriptableObjects are inspired by https://www.youtube.com/watch?v=6vmRwLYWNRo , 
 *		as well as https://github.com/richard-fine/scriptable-object-demo/tree/main/Assets/ScriptableObject/Audio */

[CreateAssetMenu(menuName="Audio Events/Simple")]
public class SimpleAudioEvent : AudioEvent
{
	public AudioClip[ ] _clips;

	[MinMaxRange(0, 1)]
	public RangedFloat _volume = new RangedFloat(0.4f, 0.6f);

	[MinMaxRange(0, 2)]
	public RangedFloat _pitch = new RangedFloat(0.8f, 1.1f);

	public override void Play(AudioSource source )
	{
		if ( _clips.Length == 0 ) {
			return;
		}
		source.clip = _clips[Random.Range(0, _clips.Length)];
		source.volume = Random.Range(_volume.minValue, _volume.maxValue);
		source.pitch = Random.Range(_pitch.minValue, _pitch.maxValue);
		source.Play();
	}
	public override void PlayOneShot(AudioSource source )
	{
		if ( _clips.Length == 0 ) {
			return;
		}
		source.clip = _clips[Random.Range(0, _clips.Length)];
		source.volume = Random.Range(_volume.minValue, _volume.maxValue);
		source.pitch = Random.Range(_pitch.minValue, _pitch.maxValue);
		source.PlayOneShot(source.clip);
	}
}
