using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GalleryScrolling : MonoBehaviour
{
    [SerializeField]
    private GameObject galleryImages;

    [SerializeField]
    private GameObject scrollBarObj;

    private float maxScrollDistance;
    private float startingXPos;
    private float inverseMaxValue;
    private float calculatedStartOffsetValue;

    private Vector3 mouseStartPos;
    private Vector3 menuPosOnClick;

    private void Start()
    {
        startingXPos = galleryImages.transform.localPosition.x;
        int a = PlayerStats.Instance.PuggemonArray.Length;
        if(a%2 !=0) // check if odd number, then round up to the closest even number
        {
            a++;
        }
        maxScrollDistance = startingXPos - (450f * ((a * 0.5f)-2));
        inverseMaxValue = 1 / (maxScrollDistance - startingXPos); // I divide here ONCE so that I can use the inverse for multiplication later.
        calculatedStartOffsetValue = startingXPos * inverseMaxValue;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseStartPos = Input.mousePosition;
            menuPosOnClick = galleryImages.transform.localPosition;
        }

        if (Input.GetMouseButton(0))
        {
            float mouseDelta = mouseStartPos.x - Input.mousePosition.x;
            float posCalculated = menuPosOnClick.x - mouseDelta * 2f;
            if (galleryImages.transform.localPosition.x <= startingXPos && galleryImages.transform.localPosition.x >= maxScrollDistance)
            {
                if(posCalculated < maxScrollDistance)
                {
                    posCalculated = maxScrollDistance;
                }
                if (posCalculated > startingXPos)
                {
                    posCalculated = startingXPos;
                }
                galleryImages.transform.localPosition = new Vector3(posCalculated, galleryImages.transform.localPosition.y, galleryImages.transform.localPosition.z);
            }
            scrollBarObj.GetComponent<Scrollbar>().value = Mathf.Abs((galleryImages.transform.localPosition.x) * inverseMaxValue) - calculatedStartOffsetValue;
        }
    }
}
