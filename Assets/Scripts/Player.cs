using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour {

	[SerializeField][Tooltip ("In ms^-1")]
	private float xSpeed = 4f;
	[SerializeField][Tooltip("In ms^-1")]
	private float ySpeed = 4f;
	[SerializeField][Tooltip ("The amount of padding to have on the sides of the screen to prevent player movement.")]
	private float xClamp = 4.5f;
	[SerializeField][Tooltip("The amount of padding to have on the sides of the screen to prevent player movement.")]
	private float yClamp = 4.5f;

	[SerializeField]
	private float positionPitchFactor = -5f;
	[SerializeField]
	private float controlPitchFactor = -20f;
	[SerializeField]
	private float positionYawFactor = 5f;
	[SerializeField]
	private float controlRollFactor = -30f;

	private float xThrow, yThrow;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		ProccessTranslation();
		ProcessRotation();
	}

	private void ProcessRotation()
	{
		float pitch = transform.localPosition.y * positionPitchFactor + yThrow * controlPitchFactor;
		float yaw = transform.localPosition.x * positionYawFactor;
		float roll = xThrow * controlRollFactor;
		transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
	}

	private void ProccessTranslation()
	{
		xThrow = CrossPlatformInputManager.GetAxis("Horizontal");
		float xOffset = xThrow * xSpeed * Time.deltaTime;
		float rawXPos = transform.localPosition.x + xOffset;
		float newXPos = Mathf.Clamp(rawXPos, -xClamp, xClamp);

		yThrow = CrossPlatformInputManager.GetAxis("Vertical");
		float yOffset = yThrow * ySpeed * Time.deltaTime;
		float rawYPos = transform.localPosition.y + yOffset;
		float newYPos = Mathf.Clamp(rawYPos, -yClamp, yClamp);

		transform.localPosition = new Vector3(newXPos, newYPos, transform.localPosition.z);
	}
}
