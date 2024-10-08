using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GalleryManager : MonoBehaviour {

	[SerializeField] private GameObject monsterImageGrid;
	[SerializeField] private GameObject monsterPrefab;

	List<int> unlockedMonsers = new();
	List<int> lockedMonsers = new();

	public void UnloadGallery () {
		GameManager.Instance.UnloadGallery();
	}

	private void Start() {
		unlockedMonsers.Clear();
		lockedMonsers.Clear();

		Debug.Log( $"UpdatePuggemonList: {PlayerStats.Instance.PuggemonArray.Length}" );

		for (int i = 0; i < PlayerStats.Instance.PuggemonArray.Length; i++) {
			if (PlayerStats.Instance.PuggemonArray[ i ] > 0) // above 0 means it is unlocked, and also how many you have
			{
				unlockedMonsers.Add( i ); // add their index value to the list
			} else {
				lockedMonsers.Add( i ); // add their index value to the list
			}
		}

		for (int i = 0; i < unlockedMonsers.Count; i++) // display the unlocked monsters
		{
			GameObject Go = Instantiate( monsterPrefab, monsterImageGrid.transform );
			Go.GetComponentInChildren<Image>().sprite = MonsterIndexLibrary.Instance.GetMonsterFromIndex( unlockedMonsers[ i ] ).GetPicture( 0 );
		}

		Debug.Log( $"RenderGallery - Locked:{lockedMonsers.Count}, Unlocked: {unlockedMonsers.Count}" );
		for (int i = 0; i < lockedMonsers.Count; i++) // then display the locked monsters after
		{
			GameObject Go = Instantiate( monsterPrefab, monsterImageGrid.transform );

			PuggeMonster puggeMonster = MonsterIndexLibrary.Instance.GetMonsterFromIndex( lockedMonsers[ i ] );
			if (puggeMonster) {
				Go.GetComponentInChildren<Image>().sprite = puggeMonster.GetPicture( 1 );
			}
		}

		/* // this will make the monsters apear, but not in order of unlocked or not
		for (int i = 0; i < PlayerStats.Instance.PuggemonArray.Length; i++)
		{
			GameObject Go = Instantiate(monsterPrefab, monsterImageGrid.transform);
			if (PlayerStats.Instance.PuggemonArray[i] > 0) // above 0 means it is unlocked, and also how many you have
			{
				Go.GetComponentInChildren<Image>().sprite = MonsterIndexLibrary.GetMonsterFromIndex(i).GetPicture(0); // 0 is the default picture
			}
			else
			{
				Go.GetComponentInChildren<Image>().sprite = MonsterIndexLibrary.GetMonsterFromIndex(i).GetPicture(1); // 1 is the White silouette
			}
		}*/
	}
}
