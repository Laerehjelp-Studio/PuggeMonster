using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackBarsFadeInController : MonoBehaviour
{
    private float animationDuration = 1f;
    [SerializeField] private GameObject fadeBarsObj;
    [SerializeField] private GameObject fadeBackgroundObj;
    [SerializeField] private float StopScaleY = 0.55f;
    [SerializeField] private float StartScaleY = 5f;

    [Header("Testing Buttons")]
    [SerializeField] private bool testTheAnimation = false;
    [SerializeField] private bool removeTheBars = false;

    private Image backgoundImage;
    private Image blackBarsImage;
    private float _doneAnimatingAt;
    public bool _doneAnimating = false;
    private bool animating = false;

    private float barsScaleStepSize;

    private Color startColor = new Color(255,255,255,0);
    private Color stopColor = new Color(255,255,255,1);


    void Start()
    {
        // Start the timer coroutine
        barsScaleStepSize = (StartScaleY - StopScaleY) / animationDuration;
    }

    public void StartAnimatingBlackBars()
    {
        OnStartAnimation();
        _doneAnimatingAt = Time.realtimeSinceStartup + animationDuration;
        _doneAnimating = false;
    }

    public void RemoveBlackBars()
    {
        if(backgoundImage == null || blackBarsImage == null)
        {
            Debug.LogWarning("Missing reference in " + transform.name + "'s script");
            return;
        }
        fadeBarsObj.SetActive(false);
        fadeBackgroundObj.SetActive(false);
        fadeBarsObj.transform.localScale = new Vector3(
                fadeBarsObj.transform.localScale.x,
                StartScaleY,
                fadeBarsObj.transform.localScale.z);
        backgoundImage.color = startColor;
        blackBarsImage.color = startColor;
    }


    void OnStartAnimation()
    {
        fadeBarsObj.SetActive(true);
        fadeBackgroundObj.SetActive(true);
        blackBarsImage = fadeBarsObj.GetComponent<Image>();
        backgoundImage = fadeBackgroundObj.GetComponent<Image>();
        backgoundImage.color = startColor;
        blackBarsImage.color = startColor;
    }

    private void Update()
    {
        if(removeTheBars)
        {
            RemoveBlackBars();
            removeTheBars = false;
        }
        if (testTheAnimation)
        {
            testTheAnimation = false;
            _doneAnimating = false;
            RemoveBlackBars();
            StartAnimatingBlackBars();
        }

        if (_doneAnimating)
        {
            return;
        }
        
        if (Time.realtimeSinceStartup < _doneAnimatingAt)
        {
            FadeInAnimation(animationDuration - (_doneAnimatingAt - Time.realtimeSinceStartup));
        }
        else
        {
            OnStopAnimation();
        }
    }


    void FadeInAnimation(float elapsedTime)
    {
        if(fadeBarsObj.transform.localScale.y >= StopScaleY)
        {
            fadeBarsObj.transform.localScale = new Vector3(
                fadeBarsObj.transform.localScale.x, 
                StartScaleY - (barsScaleStepSize * elapsedTime), 
                fadeBarsObj.transform.localScale.z);
        }
        if(blackBarsImage.color.a <= 1)
        {
            blackBarsImage.color = Color.Lerp(startColor, stopColor, elapsedTime / animationDuration);
        }
        if (backgoundImage.color.a <= 1)
        {
            backgoundImage.color = Color.Lerp(startColor,stopColor, elapsedTime / animationDuration);
        }
    }

    void OnStopAnimation()
    {
        _doneAnimating = true;
        fadeBarsObj.transform.localScale = new Vector3(
                fadeBarsObj.transform.localScale.x,
                StopScaleY,
                fadeBarsObj.transform.localScale.z);
    }
}
