using System.Collections.Generic;
using UnityEngine;

public class LetterSoundQuestionLibrary : MonoBehaviour {
	public static LetterSoundQuestionLibrary Instance { get; private set; }

	private Dictionary<AudioClip, char> _letterSoundTaskLibrary = new();
	[SerializeField] private List<AudioClip> _clipList = new();

	public static int GetMaxWordCount => LetterSoundQuestionLibrary.Instance._letterSoundTaskLibrary.Count;

	public static List<string> GetLetterList {
		get {
			List<string> tempLetterList = new();

			if (LetterSoundQuestionLibrary.Instance != null && LetterSoundQuestionLibrary.Instance._letterSoundTaskLibrary.Count > 0) {
				
				foreach ( char var in LetterSoundQuestionLibrary.Instance._letterSoundTaskLibrary.Values ) {
					tempLetterList.Add( var.ToString() );
				}
			} else if (WordQuestionLibrary.GetMaxWordCount > 0 ) {
				foreach ( string word in WordQuestionLibrary.GetWordList ) {
					string letter = word.Substring( 0, 1 );

					if (!tempLetterList.Contains( letter )) {
						tempLetterList.Add( letter );
					}
				}
			}

			return tempLetterList;
		}
	}

	private void Awake() {
		if ( Instance == default ) {
			Instance = this;

			// grab the sprites from the list and generate a dictionary with the names stripped from the sprite names.
			for ( int i = 0; i < _clipList.Count; i++ ) {
				string temp = _clipList[i].name;
				temp = temp.Replace( "SD_", "" );
				_letterSoundTaskLibrary.Add( _clipList[i], temp.ToCharArray()[0] );
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
		var.Character = _letterSoundTaskLibrary[var.Sound];

		return var;
	}

	public CharSoundQuestionPair GetCharAndSound( List<char> charList ) {
		if ( charList.Count == 0 ) {
			Debug.Log( $"Something is terribly wrong and I was unable to generate a WQPair: {charList.Count}" );
			//return new CharSoundQuestionPair();
		}
		
		CharSoundQuestionPair charQuestionPair = new CharSoundQuestionPair();
		char selectedCharacter = charList[Random.Range( 0, charList.Count )];
		charQuestionPair.Sound = GetSoundFromValue( selectedCharacter );
		charQuestionPair.Character = selectedCharacter;

		return charQuestionPair;
	}

	public static char GetCorrectChar ( AudioClip key ) {
		if ( LetterSoundQuestionLibrary.Instance._letterSoundTaskLibrary.TryGetValue( key, out var word ) ) {
			return word;
		}

		Debug.LogWarning( "Trying to access an AudioClip that does not exist!" );

		return default;
	}
	
	public static AudioClip GetSoundFromValue( char key ) {
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

		Debug.LogWarning( "Trying to access a Sprite that does not exist!" );

		return null;
	}

	public char GetInCorrectWord( AudioClip[] blockedSprite ) {
		List<AudioClip> tempSpriteList = new();

		foreach ( AudioClip item in _clipList ) {
			tempSpriteList.Add( item );
		}

		foreach ( AudioClip item in blockedSprite ) {
			tempSpriteList.Remove( item );
		}

		if ( tempSpriteList.Count > 0 ) {
			return _letterSoundTaskLibrary[tempSpriteList[Random.Range( 0, tempSpriteList.Count )]];
		}

		return default;
	}

	public static string GetInCorrectWord( string blockedCharacter ) {
		if (GetLetterList.Count == 0) {
			return default;
		}
		
		string tempCharacter = GetLetterList[Random.Range( 0, GetLetterList.Count )];
		
		if ( tempCharacter == blockedCharacter ) {
			return GetInCorrectWord(blockedCharacter);
		}

		return tempCharacter;
	}
}

public struct CharSoundQuestionPair {
	public AudioClip Sound;
	public char Character;
}
