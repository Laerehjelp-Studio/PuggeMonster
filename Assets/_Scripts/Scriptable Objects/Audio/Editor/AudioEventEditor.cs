using System.Collections;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;


/* Source: 
 *		Audio ScriptableObjects are inspired by https://www.youtube.com/watch?v=6vmRwLYWNRo , 
 *		as well as https://github.com/richard-fine/scriptable-object-demo/tree/main/Assets/ScriptableObject/Audio */
[CustomEditor(typeof(AudioEvent), true)]
public class AudioEventEditor : Editor
{
	private AudioSource _previewer;
	private bool _createdNewPreviewer = false;

	public void OnEnable()
	{
		SceneManager.sceneLoaded += InitialisePreviewAudioSource;
		InitialisePreviewAudioSource(SceneManager.GetActiveScene(), LoadSceneMode.Single);
	}

	private void InitialisePreviewAudioSource(Scene scene, LoadSceneMode mode)
	{
		GameObject previewerGO = GameObject.FindGameObjectWithTag("AudioPreviewer");
		if (previewerGO != null) {
			_previewer = previewerGO.GetComponent<AudioSource>();
		}
		if ( _previewer == null )
		{
			_createdNewPreviewer = true;
			_previewer = EditorUtility.CreateGameObjectWithHideFlags("Audio Preview", HideFlags.DontSaveInBuild, typeof(AudioSource)).GetComponent<AudioSource>();
		}

		_previewer.tag = "AudioPreviewer";
	}

	public void OnDestroy() {
		if (_createdNewPreviewer && _previewer != null) {
			DestroyImmediate(_previewer.gameObject);
		}

		SceneManager.sceneLoaded -= InitialisePreviewAudioSource;
		if (_previewer != null) {
			_previewer.clip = null;
			_previewer.volume = 1;
			_previewer.pitch = 1;
		}
	}

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		EditorGUI.BeginDisabledGroup(serializedObject.isEditingMultipleObjects);
		if (GUILayout.Button("Preview"))
		{
			( ( AudioEvent )target ).Play(_previewer);
		}
		EditorGUI.EndDisabledGroup();
	}
}
