using System;
using UnityEngine;
using UnityEngine.UI;

public class Snail_GeneralMastery : MonoBehaviour
{
    [SerializeField]
    private Slider slider;


    private void Start() {
        GameManager.Instance.OnSceneLoad += OnSceneLoaded;
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
        GameManager.Instance.OnSceneLoad -= OnSceneLoaded;
    }
}
