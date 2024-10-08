using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
	[Header("Component References")]
	[SerializeField] private GameManager _gameManager;
	[SerializeField] private AudioSource _audioSourceGM;
	[Header("Music References")]
	[SerializeField] private AudioClip _mainMenuSceneMusak;
	[Range(0,1)] [SerializeField] private float _menuMusakVolume = 0.05f;
	[SerializeField] private AudioClip _gamePlaySceneMathMusak;
	[SerializeField] private AudioClip _gamePlaySceneWordsMusak;
	[SerializeField] private AudioClip _gamePlaySceneLettersMusak;
	[Range(0,1)] [SerializeField] private float _gamePlayMusakVolume = 0.06f;


	private void OnEnable () {
		if (_gameManager == default) {
			_gameManager = GameManager.Instance;
			if (_gameManager == default) {
				Debug.Log( "[MusicManager] GameManager.Instance is null." );
				return;
			}
		}
		_gameManager.OnGameModeUpdate += SwapMusicOnGameModeChange;
	}
	private void OnDisable () {
		if (_gameManager == default) {
			_gameManager = GameManager.Instance;
			if (_gameManager == default) {
				Debug.Log( "[MusicManager] GameManager.Instance is null." );
				return;
			}
		}
		_gameManager.OnGameModeUpdate -= SwapMusicOnGameModeChange;
	}

	private void SwapMusicOnGameModeChange ( GameModeType gameMode ) {
		switch (gameMode) {
			case GameModeType.Words:
				_audioSourceGM.clip = _gamePlaySceneWordsMusak;
				_audioSourceGM.volume = _gamePlayMusakVolume;
				break;
			case GameModeType.Math:
				_audioSourceGM.clip = _gamePlaySceneMathMusak;
				_audioSourceGM.volume = _gamePlayMusakVolume;
				break;
			case GameModeType.Letters:
				_audioSourceGM.clip = _gamePlaySceneLettersMusak;
				_audioSourceGM.volume = _gamePlayMusakVolume;
				break;
			default:
			case GameModeType.None:
				_audioSourceGM.volume = _menuMusakVolume;
				_audioSourceGM.clip= _mainMenuSceneMusak;
				break;
		}
		_audioSourceGM.Play();
		if (_audioSourceGM.clip == null) {
			_audioSourceGM.clip = _mainMenuSceneMusak;
		}
	}
}
