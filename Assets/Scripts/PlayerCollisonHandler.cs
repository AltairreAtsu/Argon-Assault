using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisonHandler : MonoBehaviour {

	public delegate void OnPlayerCollisionEvent();
	public event OnPlayerCollisionEvent PlayerCollisionObservers;

	private void OnTriggerEnter(Collider other)
	{
		print("Trigger Entered");
		if(PlayerCollisionObservers != null)
			PlayerCollisionObservers();
	}


}
