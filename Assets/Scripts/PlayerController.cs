using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour {

	[Header ("Screen position clamps and movement Speed")]
	[SerializeField][Tooltip ("In ms^-1")]
	private float xSpeed = 4f;
	[SerializeField][Tooltip("In ms^-1")]
	private float ySpeed = 4f;
	[SerializeField][Tooltip ("The amount of padding to have on the sides of the screen to prevent player movement.")]
	private float xClamp = 4.5f;
	[SerializeField][Tooltip("The amount of padding to have on the sides of the screen to prevent player movement.")]
	private float yClamp = 4.5f;

	[Header ("Rotation Controls")]
	[SerializeField]
	private float positionPitchFactor = -5f;
	[SerializeField]
	private float controlPitchFactor = -20f;
	[SerializeField]
	private float positionYawFactor = 5f;
	[SerializeField]
	private float controlRollFactor = -30f;

	[Header("Linked Objects")]
	[SerializeField][Tooltip ("The Explosion Prefab GFX from the Ship itself.")]
	private GameObject explosionPrefab;
	[SerializeField][Tooltip ("The Bullet Particle System Holder Gameobject.")]
	private GameObject bulletsObject;

	private MeshRenderer[] renderers;
	private ParticleSystem[] bulletSystems;

	private float xThrow, yThrow;
	private bool alive = true;

	// Use this for initialization
	void Start () {
		var collisionHandler = GetComponent<PlayerCollisonHandler>();
		renderers = GetComponentsInChildren<MeshRenderer>();
		bulletSystems = bulletsObject.GetComponentsInChildren<ParticleSystem>();

		collisionHandler.PlayerCollisionObservers += StartDeathSequence;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(!alive) { return;  }

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

	private void StartDeathSequence()
	{
		if (alive)
		{
			explosionPrefab.GetComponent<AudioSource>().Play();
			ParticleSystem[] particleSystems = explosionPrefab.GetComponentsInChildren<ParticleSystem>();
			foreach (ParticleSystem particleSystem in particleSystems)
			{
				particleSystem.Play();
			}

			foreach (MeshRenderer render in renderers)
			{
				render.enabled = false;
			}

			foreach (ParticleSystem particleSystem in bulletSystems)
			{
				particleSystem.Stop();
			}
			alive = false;
		}
	}


}
