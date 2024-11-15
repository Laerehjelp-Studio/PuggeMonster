using System;
using UnityEngine;
using Random = UnityEngine.Random;

/* Source: 
 *		Audio ScriptableObjects are inspired by https://www.youtube.com/watch?v=6vmRwLYWNRo , 
 *		as well as https://github.com/richard-fine/scriptable-object-demo/tree/main/Assets/ScriptableObject/Audio */

[CreateAssetMenu(menuName = "Audio Events/Composite")]
public class CompositeAudioEvent : AudioEvent
{
	[Serializable]
	public struct CompositeEntry
	{
		public AudioEvent Event;
		public float Weight;
	}

	public CompositeEntry[] Entries;

	public override void Play(AudioSource source)
	{
		float totalWeight = 0;

		for(int i= 0; i<Entries.Length; ++i )
			totalWeight += Entries[i].Weight;

		float _picked = Random.Range( 0, totalWeight );
		for(int i= 0;i < Entries.Length; ++i )
		{
			if ( _picked > Entries[i].Weight )
			{
				_picked -= Entries[i].Weight;
				continue;
			}

			Entries[i].Event.PlayOneShot(source);
			return;
		}
	}
	public override void PlayOneShot(AudioSource source)
	{
		float totalWeight = 0;

		for(int i= 0; i<Entries.Length; ++i )
			totalWeight += Entries[i].Weight;

		float _picked = Random.Range( 0, totalWeight );
		for(int i= 0;i < Entries.Length; ++i )
		{
			if ( _picked > Entries[i].Weight )
			{
				_picked -= Entries[i].Weight;
				continue;
			}

			Entries[i].Event.PlayOneShot(source);
			return;
		}
	}
}
