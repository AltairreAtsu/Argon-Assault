﻿using System;
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
	[SerializeField][Tooltip ("The Bullet Particle System Holder Gameobject.")]
	private GameObject bulletsObject;

	[Header("Misc")]
	[SerializeField][Tooltip ("The amount to decriment the audiosource volume by per iteraiton when fading out.")]
	private float fadeOffDecriment = .1f;

	private AudioSource audioSource;
	private ExplosionObjectPool explosionPool;
	private MeshRenderer[] renderers;
	private ParticleSystem[] bulletSystems;

	private float xThrow, yThrow;
	private float originalBulletVolume;
	private bool alive = true;

	// Use this for initialization
	void Start () {
		var collisionHandler = GetComponent<PlayerCollisonHandler>();
		audioSource = GetComponent<AudioSource>();
		explosionPool = FindObjectOfType<ExplosionObjectPool>();
		renderers = GetComponentsInChildren<MeshRenderer>();
		bulletSystems = bulletsObject.GetComponentsInChildren<ParticleSystem>();

		collisionHandler.PlayerCollisionObservers += StartDeathSequence;

		originalBulletVolume = audioSource.volume;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(!alive) { return;  }

		ProccessTranslation();
		ProcessRotation();
		ProcessFireing();
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

	private void ProcessFireing()
	{
		if (CrossPlatformInputManager.GetButtonDown("Fire1"))
		{
			foreach(ParticleSystem particleSystem in bulletSystems)
			{
				particleSystem.Play();
				StopAllCoroutines();
				audioSource.volume = originalBulletVolume;
				audioSource.Play();
			}
		}
		else if (CrossPlatformInputManager.GetButtonUp("Fire1"))
		{
			foreach (ParticleSystem particleSystem in bulletSystems)
			{
				particleSystem.Stop();
				StartCoroutine("fadeOutBulletSFX");
			}
		}
	}

	private IEnumerator fadeOutBulletSFX()
	{
		while (audioSource.volume != 0f)
		{
			audioSource.volume -= fadeOffDecriment * Time.deltaTime;
			yield return null;
		}
		audioSource.Stop();
	}

	private void StartDeathSequence()
	{
		if (alive)
		{
			explosionPool.SpawnExplosion(transform.position);
			StopAllCoroutines();
			audioSource.Stop();

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
