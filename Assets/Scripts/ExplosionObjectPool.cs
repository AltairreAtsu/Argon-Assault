using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionObjectPool : MonoBehaviour {

	[SerializeField]
	private int poolSize = 5;
	[SerializeField]
	private GameObject explosionPrefab;

	private Queue<GameObject> pool;

	private void Start () {
		pool = new Queue<GameObject>();
		for (int i = 0; i < poolSize; i++)
		{
			GameObject explosion = Instantiate(explosionPrefab);
			explosion.transform.SetParent(transform);
			pool.Enqueue(explosion);
		}

	}
	
	public void SpawnExplosion(Vector3 position)
	{
		GameObject explosion = pool.Dequeue();

		explosion.transform.position = position;
		explosion.GetComponent<AudioSource>().Play();
		ParticleSystem[] particleSystems = explosion.GetComponentsInChildren<ParticleSystem>();
		foreach (ParticleSystem particleSystem in particleSystems)
		{
			particleSystem.Play();
		}

		pool.Enqueue(explosion);
	}
}
