
using UnityEngine;

public class GalleryFloat : MonoBehaviour
{
	public RectTransform rectTransform; // Reference to the RectTransform
	public AnimationCurve floatCurve; // Animation curve for the floating motion
	public float floatDuration = 2f; // Duration of one complete float cycle
	public float floatAmplitude = 50f; // How far the object moves up and down

	private float timeElapsed;

	void Start () {
		// Create a sinusoidal curve for smooth floating motion
		floatCurve = new AnimationCurve();
		// Keyframes to approximate a smooth sine wave
		floatCurve.AddKey( new Keyframe( 0f, 0f, Mathf.PI * 2, Mathf.PI * 2 ) ); // Start point
		floatCurve.AddKey( new Keyframe( 0.25f, 1f, 0f, 0f ) ); // Peak
		floatCurve.AddKey( new Keyframe( 0.5f, 0f, -Mathf.PI * 2, -Mathf.PI * 2 ) ); // Midpoint (zero crossing)
		floatCurve.AddKey( new Keyframe( 0.75f, -1f, 0f, 0f ) ); // Trough
		floatCurve.AddKey( new Keyframe( 1f, 0f, Mathf.PI * 2, Mathf.PI * 2 ) ); // End point
	}

	void Update () {
		// Calculate the elapsed time and loop it based on floatDuration
		timeElapsed += Time.deltaTime;
		float time = (timeElapsed / floatDuration) % 1f; // Normalize time to loop (0 to 1)

		// Apply the animation curve to move the RectTransform up and down
		float yOffset = floatCurve.Evaluate( time ) * floatAmplitude;
		rectTransform.anchoredPosition = new Vector2( rectTransform.anchoredPosition.x, yOffset );
	}
}
