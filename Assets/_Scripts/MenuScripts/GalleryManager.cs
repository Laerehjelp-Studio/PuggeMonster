using System.Collections;
using UnityEngine;

public class GalleryManager : MonoBehaviour {

	[SerializeField] private GameObject monsterImageGrid;

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
            if (PlayerStats.Instance.PuggemonArray[i] > 0)
            {
                //MonsterIndexLibrary.Instance.GetMonsterFromIndex(i);
            }
        }
    }
}
