using System.Collections.Generic;
using System.Drawing;
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
	private bool _galleryLoaded = false;

	private void Awake () {
		Instance = this;
		DontDestroyOnLoad( Instance );
		SceneManager.sceneLoaded += NewSceneLoaded;

		if (Application.platform == RuntimePlatform.WebGLPlayer) {
			DeviceScaler = DeviceScale.WebGL;
		}
		
		ResizeByScale( DeviceScaler );
	}



	public float[] GetDeviceBasedRectSizeAndScale() {
		float[] result = new float[ 4 ];

		switch (DeviceScaler) {
			case DeviceScale.iPad7:
				result = new float[ ]{
					2048f, // Width
					1536f, // Height
					0.86f, // Main Menu Scale X
					1f, // Main Menu Scale Y
				};
				// Left Menu Scale  Resolution Ratio: 2.13, 2.56
				break;

			case DeviceScale.WebGL:
			default:
				result = new float[]{
					960f, // Width
					600f, // Height
					0.4f, // Main Menu Scale X
					0.4f, // Main Menu Scale Y
				};
				break;
		}
		return result;
	}

	private void ResizeByScale ( DeviceScale deviceScaler ) {
		float[] sizeAndScale = GetDeviceBasedRectSizeAndScale();
		Vector3 scale = new Vector3( sizeAndScale[ 2 ] , sizeAndScale[ 3 ] , sizeAndScale[ 2 ] );

		// Main Menu Scene
		if (!_galleryLoaded) // the main menu scaler got mad when the gallery is loaded, because the gallery function disables the mainMenu Object
		{
            SetMainMenuCanvasSizes( sizeAndScale[ 0 ], sizeAndScale[ 1 ], scale );
		}
	}

	private void SetMainMenuCanvasSizes ( float width, float height, Vector3 scale ) {
		// Find Master Main Menu Canvas. - It needs to be found since the reference breaks upon scene load.
		GameObject _mainMenuMasterCanvasGameObject = GameObject.Find( "Main Menu Master Canvas" );

		// Set reference Resolution, and Rect Transform size.
		if (_mainMenuMasterCanvasGameObject != null ) { 
			if (_mainMenuMasterCanvasGameObject.TryGetComponent(out CanvasScaler canvasScaler)) {
				canvasScaler.referenceResolution = new Vector2 ( width, height );
			}

			if (_mainMenuMasterCanvasGameObject.TryGetComponent( out RectTransform _mainMenuMasterCanvas )) {
				SetRectTransform( _mainMenuMasterCanvas, width, height );
			}
		}

		// Find Panned Main Menu Game Object. 
		Transform _mainMenuGameObject = _mainMenuMasterCanvasGameObject.transform.Find("Main Menu Panned Canvas");

        // Set scale for Panned Main Menu.
        if (_mainMenuGameObject != null && _mainMenuGameObject.TryGetComponent(out RectTransform _mainMenuRectTransform))
        {
            _mainMenuRectTransform.localScale = new Vector3(scale.x, scale.y, scale.z);
        }

        // Set PanningTransform's Size.
        float modifier = (DeviceScaler == DeviceScale.WebGL) ? 1.2f : 1f;
        Transform _panningTransformGameObject = _mainMenuMasterCanvasGameObject.transform.Find("PanningTransform");

        if (_panningTransformGameObject != null && _panningTransformGameObject.TryGetComponent(out RectTransform _panningTransformRect))
        {
            float[] pos = { -width * 0.5f, _panningTransformRect.anchoredPosition.y, 11f };
            float[] size = { width * 2, height };

            SetRectTransform(_panningTransformRect, pos, size, default);
        }
	}

	/// <summary>
	/// Sets a RectTransform's settings.
	/// </summary>
	/// <param name="resizableCanvas"></param>
	/// <param name="pos"></param>
	/// <param name="size"></param>
	private void SetRectTransform ( RectTransform resizableCanvas, float[] pos, float[] size, float[] scale ) {
		Debug.Log( $"Canvas: {resizableCanvas.name}, Scaler: {DeviceScaler}, X: {pos[0]}, Y: {pos[ 1 ]}, Width:{size[0]}, Height: {size[ 1 ]}" );

		if (resizableCanvas == null || pos == null || size == null) {
			return;
		}

		resizableCanvas.sizeDelta = new Vector2( size[ 0 ], size[ 1 ] );
		resizableCanvas.anchoredPosition = new Vector3( pos[ 0 ], pos[ 1 ], 0f );

		if (scale != default) {
			resizableCanvas.localScale = new Vector3( scale[ 1 ], scale[ 2 ], scale[ 1 ] );
		}
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
		_galleryLoaded = true;
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
		_galleryLoaded = false;
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