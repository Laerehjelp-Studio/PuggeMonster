using UnityEngine;

public class ParallaxingManager : MonoBehaviour
{
	[SerializeField] private RectTransform[] _backgroundLayers;

    [SerializeField] private Camera Camera;
	public void SetBackgroundScale(int width, int height) {
		for (int i = 0; i < _backgroundLayers.Length; i++) {
			RectTransform layer = _backgroundLayers[i];
            /*
            float verticalFoV = Camera.fieldOfView * Mathf.Deg2Rad;

            float distanceFromCamera = Vector3.Distance(Camera.transform.position, layer.transform.position);

            // Calculate the frustum height at the specified distance from the camera
            float frustumHeight = 2f * distanceFromCamera * Mathf.Tan(verticalFoV / 2f);

            // Get the aspect ratio (width / height)
            float aspectRatio = Camera.aspect;

            // Calculate the frustum width based on the aspect ratio
            float frustumWidth = frustumHeight * aspectRatio;

			layer.transform.localScale = new Vector3(frustumWidth * 0.0043f, frustumHeight * 0.00605f, layer.transform.localScale.z);
           

            //layer.SetSizeWithCurrentAnchors( RectTransform.Axis.Horizontal, width );
            //layer.SetSizeWithCurrentAnchors( RectTransform.Axis.Vertical, height );*/
        }
		
	}
}
