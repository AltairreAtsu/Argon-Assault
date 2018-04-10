using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	private AudioSource audioSource;
	private static ExplosionObjectPool explosionPool;
	private static Scoreboard scoreboard;

	private bool selfDestructing = false;

	[SerializeField][Tooltip ("Amount of Points awarded upon killing this enemy.")]
	private int scoreValue = 12;
	[SerializeField][Tooltip ("Ammount of hits an enemy can take.")]
	private int hits = 10;
	
	private void Start()
	{
		AssignSingletons();
		CheckMeshCollider();

		audioSource = GetComponent<AudioSource>();
	}

	private static void AssignSingletons()
	{
		if (!scoreboard)
		{
			scoreboard = FindObjectOfType<Scoreboard>();
		}
		if (!explosionPool)
		{
			explosionPool = FindObjectOfType<ExplosionObjectPool>();
		}
	}

	private void CheckMeshCollider()
	{
		if (!GetComponent<MeshCollider>())
		{
			var meshColl = gameObject.AddComponent<MeshCollider>();
			meshColl.sharedMesh = GetComponent<MeshFilter>().mesh;
		}
	}

	private void OnParticleCollision(GameObject other)
	{
		if (selfDestructing) { return; }

		ProcessHit();

		if (hits <= 0)
		{
			Kill();
		}
	}

	private void ProcessHit()
	{
		hits--;
		if (!audioSource.isPlaying)
		{
			audioSource.Play();
		}
	}

	private void Kill()
	{
		explosionPool.SpawnExplosion(transform.position);
		scoreboard.ScoreHit(scoreValue);
		selfDestructing = true;
		print("Running!");
		Destroy(gameObject);
	}
}
