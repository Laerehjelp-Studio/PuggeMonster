using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GalleryDisplayManager : MonoBehaviour
{
    [SerializeField] private GalleryPageComponents leftPageData, rightPageData;
    [SerializeField] private Sprite blankImage;

    List<int> unlockedMonsers = new();
    List<int> lockedMonsers = new();

    private Vector3 mouseStartClickPos, mouseStopClickPos;
    private int pageNumber = 0, maxPageNumber;

    void Start()
    {
        // Get the amount of avaliable puggemonsters and set the amount of pages needed to display all of them.
        maxPageNumber = PlayerStats.Instance.PuggemonArray.Length;
        // if (maxPageNumber % 2 == 1) // odd number
        // {
        //     maxPageNumber++;
        // }
        maxPageNumber -= 2;
        unlockedMonsers.Clear();
        lockedMonsers.Clear();

        for (int i = 0; i < PlayerStats.Instance.PuggemonArray.Length; i++)
        {
            if (PlayerStats.Instance.PuggemonArray[i] > 0) // above 0 means it is unlocked, and also how many you have
            {
                unlockedMonsers.Add(i); // add their index value to the list
            }
            else
            {
                lockedMonsers.Add(i); // add their index value to the list
            }
        }
        LoadPage();
    }

    void Update()
    {
        DetectPageFlipMouseInput();
    }

    void DetectPageFlipMouseInput()
    {
        // Detect when first input is recieved
        if (Input.GetMouseButtonDown(0))
        {
            mouseStartClickPos = Input.mousePosition;
        }

        // Detect when input is let go
        if(Input.GetMouseButtonUp(0))
        {
            mouseStopClickPos = Input.mousePosition;

            // Check if the mouse movement distance qualifies as a swipe
            if(Vector3.Distance(mouseStartClickPos, mouseStopClickPos) < 100)
            {
                return;
            }

            // Detect the direction of the swipe and swap page accordingly
            if(mouseStartClickPos.x > mouseStopClickPos.x) 
            {
                // Swipe towards RIGHT, aka increase the page number
                if(pageNumber < maxPageNumber)
                {
                    pageNumber += 2;
                }
            }
            else
            {
                // Swipe towards LEFT, aka decrease the page number
                if (pageNumber > 0)
                {
                    pageNumber -= 2;
                }
            }
            LoadPage();
        }
    }

    void LoadPage()
    {
        LoadLeftPage();
        LoadRightPage();
    }

    void LoadLeftPage()
    {
        // LEFT page
        if (unlockedMonsers.Count > pageNumber) // unlocked monsters
        {
            DisplayPage(leftPageData, unlockedMonsers, pageNumber,0);
            Debug.Log("Left page image set to unlocked monster");
            return;
        }
        else // locked monsters
        {
            DisplayPage(leftPageData, lockedMonsers, pageNumber - unlockedMonsers.Count,1);
            Debug.Log("left page image set to locked monster");
        }
    }

    void LoadRightPage()
    {
        // RIGHT page
        if (unlockedMonsers.Count > pageNumber + 1)
        {
            DisplayPage(rightPageData, unlockedMonsers, pageNumber + 1, 0);
            Debug.Log("Right page image set to unlocked monster");
            return;
        }
        if (lockedMonsers.Count > pageNumber + 1- unlockedMonsers.Count)
        {
            DisplayPage(rightPageData, lockedMonsers, pageNumber + 1 - unlockedMonsers.Count, 1);
            
            Debug.Log("Right page image set to Locked monster");
            return;
        }

        // no more puggemons, set page to be blank
        rightPageData.PuggemonImage.sprite = blankImage;
        rightPageData.PuggemonImage.color = new Color(1, 1, 1, 0); // set the alpha to ZERO to make the image blank.
        rightPageData.PuggemonButton.onClick.RemoveAllListeners();
        rightPageData.AmmountDisplayText.text = "";
        rightPageData.LoreText.text = "";
        rightPageData.NameOfPuggemon.text = "";
        Debug.Log("right page image set to blank");
    }

    private void DisplayPage(GalleryPageComponents page, List<int> monsterList, int index, int pictureIndex) {
        page.PuggemonImage.sprite = MonsterIndexLibrary.Instance.GetMonsterFromIndex(monsterList[index]).GetPicture(pictureIndex);
        page.PuggemonImage.color = new Color(255, 255, 255, 1);
        page.PuggemonButton.onClick.RemoveAllListeners();
        
        if (pictureIndex == 0) {
            page.PuggemonButton.interactable = true;
            page.PuggemonButton.onClick.AddListener(delegate { GameManager.PlayPuggemonCollectSound(index); });
        } else {
            page.PuggemonButton.interactable = false;
        }
        page.AmmountDisplayText.text = "" + PlayerStats.Instance.PuggemonArray[monsterList[index]];
        page.LoreText.text = MonsterIndexLibrary.Instance.GetMonsterFromIndex(monsterList[index]).Lore;
        page.NameOfPuggemon.text = MonsterIndexLibrary.Instance.GetMonsterFromIndex(monsterList[index]).Name;
    }

    public void UnloadGallery()
    {
        GameManager.Instance.UnloadGallery();
    }
}
