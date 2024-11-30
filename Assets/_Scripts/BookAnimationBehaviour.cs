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
    private bool inGallery = false;

    private void Awake()
    {
        spriteImage = GetComponent<SpriteRenderer>();
        spriteImage.enabled = false;
    }

    public void StartBookAnimation()
    {
        if(!playAnimation)
        {
            playAnimation = true;
        }
    }

    private void OnEnable() {
        if (inGallery) {
            StartCoroutine(PlayAnimationThing());
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
        int index = (!inGallery) ? 0: (imageFramesList.Count - 1);
        spriteImage.sprite = imageFramesList[index];

        Sprite spriteTarget = (!inGallery) ? imageFramesList[imageFramesList.Count - 1]: imageFramesList[0];
        while (spriteImage.sprite != spriteTarget)
        {
            index = (!inGallery) ? index + 1: index - 1;
            spriteImage.sprite = imageFramesList[index];
            yield return new WaitForSecondsRealtime(calculatedWaitTime);
        }

        if (inGallery) {
            inGallery = false;
            spriteImage.sprite = imageFramesList[0];
            BookButton.SetActive(true);
            BookWhiteOut.SetActive(true);
            floatingScript.enabled = true;
            spriteImage.enabled = false;
        } else {
            gameManager.GalleryLoader("GalleryScene");
            inGallery = true;
        }
        StopCoroutine(PlayAnimationThing());
        Debug.Log("animation finished");
    }
}
