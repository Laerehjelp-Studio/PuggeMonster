using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	[SerializeField] GameObject _mainMenu;

	public static GameManager Instance { get; private set; }
	public static TaskMaster TaskMaster { get; private set; }
	public static StatManager StatManager { get; private set; }
	public static GameplayUIManager UIManager { get; private set; }

	public delegate void VoidDelegateGameMode ( GameModeType gameMode);

	public VoidDelegateGameMode OnGameModeUpdate;
	public VoidDelegateGameMode OnSceneLoad;

	private GameModeType _gameMode;

	private void Awake () {
		Instance = this;
		DontDestroyOnLoad( Instance );
		SceneManager.sceneLoaded += NewSceneLoaded;
	}

	private void NewSceneLoaded ( Scene arg0, LoadSceneMode arg1 ) {
		OnSceneLoad?.Invoke( _gameMode );
	}

	public void RegisterTaskMaster (TaskMaster taskMaster = default) {
		TaskMaster = taskMaster;
	}

	public void RegisterUIManager ( GameplayUIManager uiManager = default ) {
		UIManager = uiManager;
	}

	public void RegisterStatManager ( StatManager statManager = default ) {
		StatManager = statManager;
	}

	public void GalleryLoader ( string sceneName ) {
		SceneLoader( sceneName, true );
		_mainMenu.SetActive( false );
	}

	public void MenuLoader ( string sceneName ) {
		SceneLoader( sceneName );
	}
	public void UnloadGallery () {
		if (_mainMenu == default) {
			Debug.LogWarning( "_mainMenu is not defined! Unable to leave Gallery." );
			return;
		}

		_mainMenu.SetActive( true );
		UnloadScene( "GalleryScene" );
	}

	public static void SceneLoader ( string sceneName, bool additive = false ) {
		SceneManager.LoadScene( sceneName, additive ? LoadSceneMode.Additive : LoadSceneMode.Single );
	}

	public static void UnloadScene ( string sceneName ) {
		SceneManager.UnloadSceneAsync( sceneName );
	}
	/// <summary>
	/// This is used with buttons
	/// </summary>
	/// <param name="gameMode"></param>
	public void SetGameMode ( string gameMode ) {
		switch (gameMode) {
			case "letters":
			case "Letters":
				GameMode = GameModeType.Letters;
				break;
			case "words":
			case "Words":
				GameMode = GameModeType.Words;
				break;
			case "math":
			case "Math":
				GameMode = GameModeType.Math;
				break;
		}
	}
	public GameModeType GameMode { get { return _gameMode; } set { _gameMode = value; OnGameModeUpdate?.Invoke( _gameMode ); } }
}

public enum GameModeType {
	None,
	Math,
	Letters,
	Words
}