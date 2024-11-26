using System.Collections.Generic;
using UnityEngine;

public class LetterSoundQuestionLibrary : MonoBehaviour {
	public static LetterSoundQuestionLibrary Instance { get; private set; }

	private Dictionary<SimpleAudioEvent, string> _letterPictureTaskLibrary = new();
	private Dictionary<SimpleAudioEvent, string> _letterSoundTaskLibrary = new();
	[SerializeField] private List<SimpleAudioEvent> _clipList = new();

	public static int GetMaxWordCount => LetterSoundQuestionLibrary.Instance._letterSoundTaskLibrary.Count;

	public static List<string> GetLetterList {
		get {
			List<string> tempLetterList = new();
			if (LetterSoundQuestionLibrary.Instance != null && LetterSoundQuestionLibrary.Instance._letterSoundTaskLibrary.Count > 0) {
				
				foreach ( string var in LetterSoundQuestionLibrary.Instance._letterSoundTaskLibrary.Values ) {
					tempLetterList.Add( var );
				}
			}

			return tempLetterList;
		}
	}
	public static List<string> GetWordList {
		get {
			List<string> tempWordList = new();

			if (LetterSoundQuestionLibrary.Instance != null && LetterSoundQuestionLibrary.Instance._letterPictureTaskLibrary.Count > 0) {
				foreach ( string var in LetterSoundQuestionLibrary.Instance._letterPictureTaskLibrary.Values ) {
					tempWordList.Add( var );
				}
			} else if (WordQuestionLibrary.GetMaxWordCount > 0 ) {
				foreach ( string word in WordQuestionLibrary.GetWordList ) {

					if (!tempWordList.Contains( word )) {
						tempWordList.Add( word );
					}
				}
			}

			return tempWordList;
		}
	}
	private void Awake() {
		if ( Instance == default ) {
			Instance = this;

			// grab the sprites from the list and generate a dictionary with the names stripped from the sprite names.
			for ( int i = 0; i < _clipList.Count; i++ ) {
				string temp = _clipList[i].name;
				temp = temp.Replace( "SD_", "" );
				_letterSoundTaskLibrary.Add( _clipList[i], temp );
			}
		}
	}
	
	public CharSoundQuestionPair GetCharAndSound() {
		if ( _clipList.Count == 0 ) {
			Debug.Log( $"Something is terribly wrong and I was unable to generate a WQPair: {_clipList.Count}" );
			//return new CharSoundQuestionPair();
		}
		
		CharSoundQuestionPair var = new CharSoundQuestionPair();
		var.Sound = _clipList[Random.Range( 0, _clipList.Count )];
		var.StorageKey = _letterSoundTaskLibrary[var.Sound];

		return var;
	}

	public CharSoundQuestionPair GetCharAndSound( List<string> charList ) {
		if ( charList.Count == 0 ) {
			Debug.Log( $"Something is terribly wrong and I was unable to generate a WQPair: {charList.Count}" );
			//return new CharSoundQuestionPair();
		}
		
		CharSoundQuestionPair charQuestionPair = new CharSoundQuestionPair();
		string selectedCharacter = charList[Random.Range( 0, charList.Count )];
		charQuestionPair.Sound = GetSoundFromValue( selectedCharacter );
		charQuestionPair.StorageKey = selectedCharacter;

		return charQuestionPair;
	}

	public static string GetCorrectChar ( SimpleAudioEvent key ) {
		if ( LetterSoundQuestionLibrary.Instance._letterSoundTaskLibrary.TryGetValue( key, out var word ) ) {
			return word;
		}

		Debug.LogWarning( "Trying to access an AudioClip that does not exist!" );

		return default;
	}
	
	public static SimpleAudioEvent GetSoundFromValue( string key ) {
		if (LetterSoundQuestionLibrary.Instance == default) {
			return null;
		}
		
		var tempList = LetterSoundQuestionLibrary.Instance._letterSoundTaskLibrary;
		
		if (tempList.Count > 0 && tempList.ContainsValue( key ) ) {
			foreach ( var item in tempList ) {
				if ( item.Value == key ) {
					return item.Key;
				}
			}
		}

		return null;
	}

	public string GetInCorrectLetter( SimpleAudioEvent[] blockedSprite ) {
		List<SimpleAudioEvent> tempSpriteList = new();

		foreach ( SimpleAudioEvent item in _clipList ) {
			tempSpriteList.Add( item );
		}

		foreach ( SimpleAudioEvent item in blockedSprite ) {
			tempSpriteList.Remove( item );
		}

		if ( tempSpriteList.Count > 0 ) {
			return _letterSoundTaskLibrary[tempSpriteList[Random.Range( 0, tempSpriteList.Count )]];
		}

		return default;
	}

	public static string GetInCorrectLetter( string blockedCharacter ) {
		if (GetLetterList.Count == 0) {
			return default;
		}
		
		string tempCharacter = GetLetterList[Random.Range( 0, GetLetterList.Count )].Substring(0,1);
		
		if ( tempCharacter == blockedCharacter ) {
			return GetInCorrectLetter(blockedCharacter);
		}

		return tempCharacter;
	}

	public static string GetInCorrectLetter( List<string> blockedWords ) {
		List<string> tempWordList = new();
		
		foreach ( var item in LetterSoundQuestionLibrary.GetWordList ) {
			string letter = item.Substring(0, 1);
			
			if (!blockedWords.Contains(letter)) {
				tempWordList.Add( letter );
			}
		}
		
		if ( tempWordList.Count > 0 ) {
			return tempWordList[Random.Range( 0, tempWordList.Count )];
		}

		return default;
	}
}

public struct CharSoundQuestionPair {
	public SimpleAudioEvent Sound;
	public string StorageKey;
}
