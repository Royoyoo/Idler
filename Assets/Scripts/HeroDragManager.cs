using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class HeroDragManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	public static GameObject DraggedGO;
	Vector3 startPosition;
	Transform startParent;

	public void OnBeginDrag (PointerEventData eventData)
	{
		DraggedGO = gameObject;
		startPosition = transform.position;
		startParent = transform.parent;
		GetComponent<CanvasGroup> ().blocksRaycasts = false;
	}

	public void OnDrag (PointerEventData eventData)
	{
		transform.position = eventData.position;
	}

	public void OnEndDrag (PointerEventData eventData)
	{
		DraggedGO = null;
		GetComponent<CanvasGroup> ().blocksRaycasts = true;
		if (transform.parent == startParent)
		{
			transform.position = startPosition;
		}
	}
}
