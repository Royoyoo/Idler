using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class LocationDropManager : MonoBehaviour, IDropHandler {

	public void OnDrop (PointerEventData eventData)
	{
//		if (eventData.pointerDrag.GetComponent<Hero> ().LocationAssigned != null)
//		{
//			float dist;
//			Debug.Log (MapNavigator.instance.NextDestination (eventData.pointerDrag.GetComponent<Hero> ().LocationAssigned, GetComponent<Location> (), out dist).LocationName);
//			Debug.Log (dist);
//		}
			
		eventData.pointerDrag.GetComponent<Hero> ().StartCoroutine("MoveHero", GetComponent<Location>());

	}
}
