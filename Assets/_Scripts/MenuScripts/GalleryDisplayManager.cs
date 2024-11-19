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
        if (maxPageNumber % 2 == 1) // odd number
        {
            maxPageNumber++;
        }
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
            leftPageData.PuggemonImage.sprite = MonsterIndexLibrary.Instance.GetMonsterFromIndex(unlockedMonsers[pageNumber]).GetPicture(0);
            leftPageData.PuggemonImage.color = new Color(255, 255, 255, 1);
            leftPageData.AmmountDisplayText.text = "" + PlayerStats.Instance.PuggemonArray[unlockedMonsers[pageNumber]];
            leftPageData.LoreText.text = MonsterIndexLibrary.Instance.GetMonsterFromIndex(unlockedMonsers[pageNumber]).Lore;
            leftPageData.NameOfPuggemon.text = MonsterIndexLibrary.Instance.GetMonsterFromIndex(unlockedMonsers[pageNumber]).Name;
            Debug.Log("Left page image set to unlocked monster");
            return;
        }
        else // locked monsters
        {
            leftPageData.PuggemonImage.sprite = MonsterIndexLibrary.Instance.GetMonsterFromIndex(lockedMonsers[pageNumber - unlockedMonsers.Count]).GetPicture(1);
            leftPageData.PuggemonImage.color = new Color(255, 255, 255, 1);
            leftPageData.AmmountDisplayText.text = "" + PlayerStats.Instance.PuggemonArray[lockedMonsers[pageNumber - unlockedMonsers.Count]];
            leftPageData.LoreText.text = MonsterIndexLibrary.Instance.GetMonsterFromIndex(lockedMonsers[pageNumber - unlockedMonsers.Count]).Lore;
            leftPageData.NameOfPuggemon.text = MonsterIndexLibrary.Instance.GetMonsterFromIndex(lockedMonsers[pageNumber - unlockedMonsers.Count]).Name;
            Debug.Log("left page image set to locked monster");
        }
    }

    void LoadRightPage()
    {
        // RIGHT page
        if (unlockedMonsers.Count > pageNumber + 1)
        {
            rightPageData.PuggemonImage.sprite = MonsterIndexLibrary.Instance.GetMonsterFromIndex(unlockedMonsers[pageNumber + 1]).GetPicture(0);
            rightPageData.PuggemonImage.color = new Color(255, 255, 255, 1);
            rightPageData.AmmountDisplayText.text = "" + PlayerStats.Instance.PuggemonArray[unlockedMonsers[pageNumber + 1]];
            rightPageData.LoreText.text = MonsterIndexLibrary.Instance.GetMonsterFromIndex(unlockedMonsers[pageNumber + 1]).Lore;
            rightPageData.NameOfPuggemon.text = MonsterIndexLibrary.Instance.GetMonsterFromIndex(unlockedMonsers[pageNumber + 1]).Name;
            Debug.Log("Right page image set to unlocked monster");
            return;
        }
        if (lockedMonsers.Count > pageNumber - unlockedMonsers.Count)
        {
            rightPageData.PuggemonImage.sprite = MonsterIndexLibrary.Instance.GetMonsterFromIndex(lockedMonsers[pageNumber + 1 - unlockedMonsers.Count]).GetPicture(1);
            rightPageData.PuggemonImage.color = new Color(255, 255, 255, 1);
            rightPageData.AmmountDisplayText.text = "" + PlayerStats.Instance.PuggemonArray[lockedMonsers[pageNumber + 1 - unlockedMonsers.Count]];
            rightPageData.LoreText.text = MonsterIndexLibrary.Instance.GetMonsterFromIndex(lockedMonsers[pageNumber + 1 - unlockedMonsers.Count]).Lore;
            rightPageData.NameOfPuggemon.text = MonsterIndexLibrary.Instance.GetMonsterFromIndex(lockedMonsers[pageNumber + 1 - unlockedMonsers.Count]).Name;
            Debug.Log("Right page image set to Locked monster");
            return;
        }

        // no more puggemons, set page to be blank
        rightPageData.PuggemonImage.sprite = blankImage;
        rightPageData.PuggemonImage.color = new Color(1, 1, 1, 0); // set the alpha to ZERO to make the image blank.
        rightPageData.AmmountDisplayText.text = "";
        rightPageData.LoreText.text = "";
        rightPageData.NameOfPuggemon.text = "";
        Debug.Log("right page image set to blank");
    }
    public void UnloadGallery()
    {
        GameManager.Instance.UnloadGallery();
    }
}
