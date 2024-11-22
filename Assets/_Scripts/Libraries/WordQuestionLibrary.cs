using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class WordQuestionLibrary : MonoBehaviour {
	public static WordQuestionLibrary Instance { get; private set; }

	private Dictionary<Sprite, string> wordTaskLibrary = new();
	[SerializeField] private List<Sprite> spriteList = new();

	public static int GetMaxWordCount => WordQuestionLibrary.Instance.wordTaskLibrary.Count;


	private void Awake() {
		if ( Instance == default ) {
			Instance = this;

			// grab the sprites from the list and generate a dictionary with the names stripped from the sprite names.
			for ( int i = 0; i < spriteList.Count; i++ ) {
				string temp = spriteList[i].name;
				temp = temp.Replace( "WD_", "" );
				wordTaskLibrary.Add( spriteList[i], temp );
			}
		}
	}
	
	public static List<string> GetWordList {
		get {
			List<string> tempWordList = new();
			if (WordQuestionLibrary.Instance == default) {
				return tempWordList;
			}
			
			foreach ( string var in WordQuestionLibrary.Instance.wordTaskLibrary.Values ) {
				tempWordList.Add( var );
			}

			return tempWordList;
		}
	}
	
	public WordPictureQuestionPair GetWordAndSprite() {
		if ( spriteList.Count == 0 ) {
			Debug.Log( $"Something is terribly wrong and I was unable to generate a WQPair: {spriteList.Count}" );
			//return new WordQuestionPair();
		}

		WordPictureQuestionPair var = new WordPictureQuestionPair();
		var.Picture = spriteList[Random.Range( 0, spriteList.Count )];
		var.Word = wordTaskLibrary[var.Picture];

		return var;
	}

	public WordPictureQuestionPair GetWordAndSprite( List<string> wordList ) {
		if ( wordList.Count == 0 ) {
			Debug.Log( $"Something is terribly wrong and I was unable to generate a WQPair: {wordList.Count}" );
			//return new WordQuestionPair();
		}

		WordPictureQuestionPair wordPictureQuestionPair = new WordPictureQuestionPair();
		string selectedWord = wordList[Random.Range( 0, wordList.Count )];
		wordPictureQuestionPair.Picture = GetSpriteFromValue( selectedWord );
		wordPictureQuestionPair.Word = selectedWord;

		return wordPictureQuestionPair;
	}

	public static string GetCorrectWord( Sprite key ) {
		if ( WordQuestionLibrary.Instance.wordTaskLibrary.ContainsKey( key ) ) {
			return WordQuestionLibrary.Instance.wordTaskLibrary[key];
		}

		Debug.LogWarning( "Trying to access a Sprite that does not exist!" );

		return null;
	}


	public static Sprite GetSpriteFromValue( string key ) {
		if ( WordQuestionLibrary.Instance.wordTaskLibrary.ContainsValue( key ) ) {
			foreach ( var item in WordQuestionLibrary.Instance.wordTaskLibrary ) {
				if ( item.Value == key ) {
					return item.Key;
				}
			}
		}

		Debug.LogWarning( "Trying to access a Sprite that does not exist!" );

		return null;
	}

	public string GetInCorrectWord( Sprite[] blockedSprite ) {
		List<Sprite> tempSpriteList = new();

		foreach ( Sprite item in spriteList ) {
			tempSpriteList.Add( item );
		}

		foreach ( Sprite item in blockedSprite ) {
			tempSpriteList.Remove( item );
		}

		if ( tempSpriteList.Count > 0 ) {
			return wordTaskLibrary[tempSpriteList[Random.Range( 0, tempSpriteList.Count )]];
		}

		return null;
	}
}

public struct WordPictureQuestionPair {
	public string Word;
	public Sprite Picture;
}
