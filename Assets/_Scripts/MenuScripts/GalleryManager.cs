using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GalleryManager : MonoBehaviour {

	[SerializeField] private GameObject monsterImageGrid;
    [SerializeField] private GameObject monsterPrefab;

	private void Awake () {
		GameManager.Instance.RegisterManager( this );
	}

	public void UnloadGallery () {
		GameManager.Instance.UnloadGallery();
	}

    private void Start()
    {
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
        }
    }
}
