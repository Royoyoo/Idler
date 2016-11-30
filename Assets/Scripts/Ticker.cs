using UnityEngine;
using System.Collections;

public class Ticker : MonoBehaviour {

	[HideInInspector] public float Timer;
	public float TickInterval;

	public delegate void TickAction(float interval);
	public static event TickAction OnTickEvent;

	public static Ticker instance;

	void Start () {
		if (instance == null)
			instance = this;
		else {
			if (instance != this)
				Destroy (this);
		}

		Timer = 0;
	}

	void Update () {

		Timer += Time.deltaTime;

		if(Timer > TickInterval)
		{
			Timer -= TickInterval;
			if(OnTickEvent != null)
				OnTickEvent(TickInterval);
		}
	}
}
