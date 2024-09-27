using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	[SerializeField] GameObject _mainMenu;

	public static GameManager Instance { get; private set; }
	public static TaskMaster TaskMaster { get; private set; }
	public static GameplayUIManager UIManager { get; private set; }
	public static GalleryManager GalleryManager { get; private set; }

	public delegate void VoidDelegateGameMode ( GameModeType gameMode);

	public VoidDelegateGameMode OnGameModeUpdate;
	public VoidDelegateGameMode OnSceneLoad;

	public DeviceScale DeviceScaler;

	public float GetDeviceWidth {  get; }

	private GameModeType _gameMode;

	private void Awake () {
		Instance = this;
		DontDestroyOnLoad( Instance );
		SceneManager.sceneLoaded += NewSceneLoaded;

		if (Application.platform == RuntimePlatform.WebGLPlayer) {
			DeviceScaler = DeviceScale.WebGL;
		}
		
		ResizeByScale( DeviceScaler );
	}



	public float[] GetDeviceBasedRectSize() {
		float[] result = new float[3];

		switch (DeviceScaler) {
			case DeviceScale.WebGL: 
				result[ 0 ] = 960f; // Width
				result[ 1 ] = 600f; // Height
				result[ 2 ] = 0.4f; // Main Menu Scale
			break;
			case DeviceScale.iPad7:
				result[ 0 ] = 2048f; // Width
				result[ 1 ] = 1536f; // Height
				result[ 2 ] = 1f; // Main Menu Scale
				break;
		}
		return result;
	}

	private void ResizeByScale ( DeviceScale deviceScaler ) {
		float[] size = GetDeviceBasedRectSize();

		// Main Menu Scene
		SetMainMenuCanvasSizes( size[ 0 ], size[ 1 ], size[2] );
	}

	private void SetMainMenuCanvasSizes ( float width, float height, float scale ) {
		// Set Canvas Scaler Resolution.
		GameObject _mainMenuCanvasGameObject = GameObject.Find( "MainMenuCanvas" );
		if (_mainMenuCanvasGameObject != null && _mainMenuCanvasGameObject.TryGetComponent(out CanvasScaler canvasScaler)) {
			canvasScaler.referenceResolution = new Vector2 ( width, height );
		}
		if (_mainMenuCanvasGameObject != null && _mainMenuCanvasGameObject.TryGetComponent( out RectTransform _mainMenuCanvas )) {
			SetRectTransform( _mainMenuCanvas, width, height );
		}
		GameObject _mainMenuGameObject = GameObject.Find( "MainMenu" );
		if (_mainMenuGameObject != null && _mainMenuGameObject.TryGetComponent(out RectTransform _mainMenuRectTransform)) {
			_mainMenuRectTransform.localScale = new Vector3 ( scale , scale , scale );
		}


		// Set PanningTransform's Size.
		float modifier = (DeviceScaler == DeviceScale.WebGL ) ? 1.2f: 1f;
		GameObject _panningTransformGameObject = GameObject.Find( "PanningTransform" );

		if (_panningTransformGameObject != null && _panningTransformGameObject.TryGetComponent( out RectTransform _panningTransformRect )) {
			float[] pos = { -width * 0.5f, _panningTransformRect.anchoredPosition.y, 11f };
			float[] size = { width * 2 , height};

			SetRectTransform( _panningTransformRect, pos, size );
		}


		

		//GameObject _mainMenuCanvasBackgroundGameObject = GameObject.Find( "BackGroundPLACEHOLDER" );

		//if (_mainMenuCanvasBackgroundGameObject != null && _mainMenuCanvasBackgroundGameObject.TryGetComponent( out RectTransform _mainMenuCanvasBackground )) {
		//	SetRectTransform( _mainMenuCanvasBackground, width * 2 * modifier, height );
		//}
	}
	/// <summary>
	/// Sets a RectTransform's settings.
	/// </summary>
	/// <param name="resizableCanvas"></param>
	/// <param name="pos"></param>
	/// <param name="size"></param>
	private void SetRectTransform ( RectTransform resizableCanvas, float[] pos, float[] size ) {
		Debug.Log( $"Canvas: {resizableCanvas.name}, Scaler: {DeviceScaler}, X: {pos[0]}, Y: {pos[ 1 ]}, Width:{size[0]}, Height: {size[ 1 ]}" );

		if (resizableCanvas == null || pos == null || size == null) {
			return;
		}

		resizableCanvas.sizeDelta = new Vector2( size[ 0 ], size[ 1 ] );
		resizableCanvas.anchoredPosition = new Vector3( pos[ 0 ], pos[ 1 ], 0f );

		//resizableCanvas.rect.Set( pos[ 0], pos[ 1 ], size[ 0 ], size[ 1 ] );
	}
	/// <summary>
	/// Sets a RectTransform's settings.
	/// </summary>
	/// <param name="resizableCanvas"></param>
	/// <param name="width"></param>
	/// <param name="height"></param>
	private void SetRectTransform ( RectTransform resizableCanvas, float width, float height ) {
		Debug.Log($"Canvas: {resizableCanvas.name}, Scaler: {DeviceScaler}, Width:{width}, Height: {height}");

		if (resizableCanvas == null) {
			return;
		}
		
		//resizableCanvas.sizeDelta = new Vector2( width, height );

		resizableCanvas.SetSizeWithCurrentAnchors( RectTransform.Axis.Horizontal, width );
		resizableCanvas.SetSizeWithCurrentAnchors( RectTransform.Axis.Vertical, height );
	}

	private void OnDisable () {
		SceneManager.sceneLoaded -= NewSceneLoaded;
	}

	private void NewSceneLoaded ( Scene arg0, LoadSceneMode arg1 ) {
		ResizeByScale(DeviceScaler);
		OnSceneLoad?.Invoke( _gameMode );
	}

#region Registering/deregistering Managers
	public void RegisterManager (TaskMaster taskMaster = default) {
		TaskMaster = taskMaster;
	}

	public void RegisterManager ( GameplayUIManager uiManager = default ) {
		UIManager = uiManager;
	}

	public void RegisterManager ( GalleryManager galleryManager = default ) {
		GalleryManager = galleryManager;
	}
	
	public void UnRegisterManager ( TaskMaster taskMaster ) {
		TaskMaster = default;
	}

	public void UnRegisterManager ( GameplayUIManager uiManager) {
		UIManager = default;
	}

	public void UnRegisterManager ( GalleryManager galleryManager ) {
		GalleryManager = default;
	}
	#endregion

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


public enum DeviceScale {
	None,
	WebGL,
	iPad7
}