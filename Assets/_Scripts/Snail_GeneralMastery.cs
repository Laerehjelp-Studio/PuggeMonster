using System;
using UnityEngine;
using UnityEngine.UI;

public class Snail_GeneralMastery : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    private void Awake() {
        GameManager.OnSceneLoad += OnSceneLoaded;
    }

    private void Start() {
        SetMaxValueAndValue();
    }

    private void SetMaxValueAndValue() {
        slider.maxValue = StatManager.GeneralMasteryMaxValue; // the total ammount of mastery value
        slider.value = StatManager.GeneralMathMastery;
    }

    private void OnSceneLoaded(GameModeType gamemode) {
        SetMaxValueAndValue();
    }

    private void OnDisable()
    {
        GameManager.OnSceneLoad -= OnSceneLoaded;
    }
}
