using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayButtonBehaviour : MonoBehaviour
{
    private GameObject cameraObj;
    private bool sliding = false;
    [SerializeField] private float slideDuration = 1f;

    private void Start()
    {
        cameraObj = Camera.main.transform.gameObject;
    }

    private IEnumerator TimerCoroutine(float duration, float targetPosX)
    {
        float elapsed = 0f;

        // Record the starting position
        Vector3 startPosition = cameraObj.transform.position;
        Vector3 targetPosition = new Vector3(targetPosX, startPosition.y, startPosition.z);

        while (elapsed < duration)
        {
            // Increase the elapsed time
            elapsed += Time.deltaTime;

            // Calculate the interpolation factor (progress) between 0 and 1
            float progress = Mathf.Clamp01(elapsed / duration);

            // Perform the action: in this case, we linearly interpolate the position
            cameraObj.transform.position = Vector3.Lerp(startPosition, targetPosition, progress);

            // Wait for the next frame
            yield return null;
        }

        // Ensure the final position is set (in case elapsed went slightly over)
        cameraObj.transform.position = targetPosition;
        sliding = false;
    }


    public void SlideToTheLeft()
    {
        if (!sliding)
        {
            StartCoroutine(TimerCoroutine(slideDuration, -154));
            sliding = true;
        }
    }

    public void SlideToTheRight()
    {
        if (!sliding)
        {
            StartCoroutine(TimerCoroutine(slideDuration, 0));
            sliding = true;
        }
    }
}
