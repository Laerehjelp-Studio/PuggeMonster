using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookAnimationBehaviour : MonoBehaviour
{
    [SerializeField] private List<Sprite> imageFramesList = new List<Sprite>();

    [SerializeField] private bool playAnimation = false;
    [SerializeField] private float framesPerSec = 30;
    [SerializeField] private GameObject BookButton, BookWhiteOut;
    [SerializeField] private GalleryFloat floatingScript;
    [SerializeField] private GameManager gameManager;
    private float calculatedWaitTime;
    private SpriteRenderer spriteImage;

    private void Awake()
    {
        spriteImage = GetComponent<SpriteRenderer>();
        spriteImage.enabled = false;
    }

    private void OnEnable() {
        
        spriteImage.sprite = imageFramesList[0];
    }

    public void StartBookAnimation()
    {
        if(!playAnimation)
        {
            playAnimation = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(playAnimation)
        {
            spriteImage.enabled = true;
            floatingScript.enabled = false;
            BookButton.SetActive(false);
            BookWhiteOut.SetActive(false);
            StartCoroutine(PlayAnimationThing());
            playAnimation = false;
        }
    }

    IEnumerator PlayAnimationThing()
    {
        calculatedWaitTime = 1 / framesPerSec;
        int index = 0;
        spriteImage.sprite = imageFramesList[index];

        while (spriteImage.sprite != imageFramesList[imageFramesList.Count - 1])
        {
            spriteImage.sprite = imageFramesList[index];
            index++;
            yield return new WaitForSecondsRealtime(calculatedWaitTime);
        }
        gameManager.GalleryLoader("GalleryScene");
        BookButton.SetActive(true);
        BookWhiteOut.SetActive(true);
        floatingScript.enabled = true;
        spriteImage.enabled = false;
        StopCoroutine(PlayAnimationThing());
        Debug.Log("animation finished");
    }
}
