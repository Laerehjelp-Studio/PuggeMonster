using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppleColorBasedOnStruggle : MonoBehaviour
{
    [Header("Color variations")]
    [SerializeField] private Color Doing_Well_Color;
    [SerializeField] private Color Doing_Default_Color;
    [SerializeField] private Color Doing_Poorly_Color;

    [Header("Components")]
    [SerializeField] private Image appleColorImage;

    private float previousPerformance = 0;
    private int currentPerformanceInInt;
    private bool changingColor = false;

    private TaskMaster taskmaster;

    private void Start()
    {
        appleColorImage.color = Doing_Default_Color;
        taskmaster = GameManager.TaskMaster;
    }

    void Update()
    {
        //Debug.Log("sum: " + taskmaster.CurrentStudentPerformance.Sum);
        if(taskmaster == null)
        {
            return;
        }
        if(taskmaster.CurrentStudentPerformance.Sum > 1)
        {
            currentPerformanceInInt = 1;
        }
        else if (taskmaster.CurrentStudentPerformance.Sum < -1)
        {
            currentPerformanceInInt = -1;
        }
        else
        {
            currentPerformanceInInt = 0;
        }

        if (previousPerformance != currentPerformanceInInt && !changingColor)
        {
            changingColor = true;
            previousPerformance = currentPerformanceInInt;
        }

        if (changingColor)
        {
            if (currentPerformanceInInt == 0)
            {
                appleColorImage.color = Color.Lerp(appleColorImage.color, Doing_Default_Color, Time.deltaTime * 0.5f);
                if(appleColorImage.color == Doing_Default_Color)
                {
                    changingColor = false;
                }
            }

            if (currentPerformanceInInt == 1)
            {
                appleColorImage.color = Color.Lerp(appleColorImage.color, Doing_Well_Color, Time.deltaTime * 0.5f);
                if (appleColorImage.color == Doing_Well_Color)
                {
                    changingColor = false;
                }
            }

            if (currentPerformanceInInt == -1)
            {
                appleColorImage.color = Color.Lerp(appleColorImage.color, Doing_Poorly_Color, Time.deltaTime * 0.5f);
                if (appleColorImage.color == Doing_Poorly_Color)
                {
                    changingColor = false;
                }
            }
        }
    }
}
