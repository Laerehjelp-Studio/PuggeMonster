using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	
	public static GameManager Instance { get; private set; }

	private void Awake () {
		Instance = this;
		DontDestroyOnLoad( Instance );
	}

	public void GalleryLoader ( string sceneName ) {
		SceneLoader( sceneName, true );
	}

	public void MenuLoader (string sceneName) {
		SceneLoader( sceneName );
	}
	public void UnloadGallery () {
		UnloadScene( "GalleryScene" );
	}

	public static void SceneLoader(string sceneName, bool additive = false) {
		SceneManager.LoadScene(sceneName, additive ? LoadSceneMode.Additive : LoadSceneMode.Single );
	}

	public static void UnloadScene ( string sceneName ) {
		SceneManager.UnloadSceneAsync( sceneName );
	}
}
