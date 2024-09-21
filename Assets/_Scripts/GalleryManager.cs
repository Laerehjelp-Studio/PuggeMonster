using UnityEngine;

public class GalleryManager : MonoBehaviour {
	private void Awake () {
		GameManager.Instance.RegisterManager( this );
	}

	public void UnloadGallery () {
		GameManager.Instance.UnloadGallery();
	}
}
