using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Snail_GeneralMastery : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        slider.maxValue = StatManager.GeneralMasteryMaxValue; // the total ammount of mastery value
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        slider.value = StatManager.GeneralMathMastery;
    }
}
