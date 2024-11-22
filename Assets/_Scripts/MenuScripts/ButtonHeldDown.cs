
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonHeldDown : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler {
	public UnityEvent OnButtonHeldDown;
	public UnityEvent OnButtonClicked;

	[SerializeField] private float _settingsPanelDelay = 5f;

	private float _startedHoldingButton = 0f;
	private bool _isPointerInside = false;

	public void OnPointerDown (PointerEventData eventData) {
		_startedHoldingButton = Time.realtimeSinceStartup;
		_isPointerInside = true;
	}

	public void OnPointerUp (PointerEventData eventData) {
		if (!_isPointerInside) {
			return;
		}
		
		if (Time.realtimeSinceStartup - _startedHoldingButton > _settingsPanelDelay) {
			OnButtonHeldDown?.Invoke();
		} else {
			OnButtonClicked?.Invoke();
		}
		
		_startedHoldingButton = 0f;
		_isPointerInside = false;
	}

	public void OnPointerExit(PointerEventData eventData) {
		_isPointerInside = false;
	}
}
