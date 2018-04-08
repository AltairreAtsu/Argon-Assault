using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	private ExplosionObjectPool explosionPool;

	private void Start()
	{
		explosionPool = FindObjectOfType<ExplosionObjectPool>();
	}

	private void OnParticleCollision(GameObject other)
	{
		print("Particles COllided with: " + gameObject.name);
		explosionPool.SpawnExplosion(transform.position);
		Destroy(gameObject);
	}
}
