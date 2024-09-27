using System.Collections;
using UnityEngine;

public class PlayButtonBehaviour : MonoBehaviour
{
    private bool sliding = false;
    
	[SerializeField] private float slideDuration = 1f;
	[SerializeField] private RectTransform panningTransform;

    private IEnumerator TimerCoroutine(RectTransform panningTransform, float duration)
    {
        float elapsed = 0f;
		// Record the starting position
		float startPosition = panningTransform.anchoredPosition.x;
		float targetPosition = panningTransform.anchoredPosition.x * -1;

		while (elapsed < duration)
        {
            // Increase the elapsed time
            elapsed += Time.deltaTime;

            // Calculate the interpolation factor (progress) between 0 and 1
            float progress = Mathf.Clamp01(elapsed / duration);

			// Perform the action: in this case, we linearly interpolate the position
			float currentX = Mathf.Lerp( startPosition, targetPosition, progress );
			panningTransform.anchoredPosition = new Vector2(currentX, panningTransform.anchoredPosition.y );

			// Wait for the next frame
			yield return null;
        }

        // Ensure the final position is set (in case elapsed went slightly over)
        panningTransform.anchoredPosition = new Vector2(targetPosition, panningTransform.anchoredPosition.y );
        
		sliding = false;
    }

    public void Slide()
    {
        if (!sliding)
        {
            StartCoroutine(TimerCoroutine(panningTransform, slideDuration) );
            sliding = true;
        }
    }
}
