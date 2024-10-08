using UnityEngine;

public class ParallaxingManager : MonoBehaviour
{
	[SerializeField] private RectTransform[] _backgroundLayers; 
	public void SetBackgroundScale(int width, int height) {
		for (int i = 0; i < _backgroundLayers.Length; i++) {
			RectTransform layer = _backgroundLayers[i];

			layer.SetSizeWithCurrentAnchors( RectTransform.Axis.Horizontal, width );
			layer.SetSizeWithCurrentAnchors( RectTransform.Axis.Vertical, height );
		}
		
	}
}
