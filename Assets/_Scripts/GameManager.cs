using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	[SerializeField] GameObject _mainMenu;

	public static GameManager Instance { get; private set; }

	private void Awake () {
		Instance = this;
		DontDestroyOnLoad( Instance );
	}

	public void GalleryLoader ( string sceneName ) {
		SceneLoader( sceneName, true );
		_mainMenu.SetActive( false );
	}

	public void MenuLoader (string sceneName) {
		SceneLoader( sceneName );
	}
	public void UnloadGallery () {
		if (_mainMenu == default) {
			Debug.LogWarning("_mainMenu is not defined! Unable to leave Gallery.");
			return;
		}

		_mainMenu.SetActive( true );
		UnloadScene( "GalleryScene" );
	}

	public static void SceneLoader(string sceneName, bool additive = false) {
		SceneManager.LoadScene(sceneName, additive ? LoadSceneMode.Additive : LoadSceneMode.Single );
	}

	public static void UnloadScene ( string sceneName ) {
		SceneManager.UnloadSceneAsync( sceneName );
	}
}
