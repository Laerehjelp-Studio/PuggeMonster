using UnityEngine;

public class ExitMenuButton : MonoBehaviour {
	void Awake () {
		if (Application.platform == RuntimePlatform.WebGLPlayer) {
			gameObject.SetActive( false );
		}
	}

	public void QuitApplication () {
		Application.Quit();
	}
}
