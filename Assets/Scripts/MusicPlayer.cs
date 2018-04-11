using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {

	private AudioSource audioSource;

	private void Awake () {
		MusicPlayer[] musicPlayers = FindObjectsOfType<MusicPlayer>();

		if (musicPlayers.Length > 1)
		{
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(this.gameObject);
	}

	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
	}

	public void Play()
	{
		audioSource.Play();
	}

}
