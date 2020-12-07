using MLAPI;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

public class CollectableSpawner : NetworkedBehaviour
{
	public GameObject collectablePrefab;

	Transform[] spawnPoints;
	int lastUsedIndex = -1;

	public void StartCollectableSpawner()
	{
		if (IsServer)
		{
			spawnPoints = GetComponentsInChildren<Transform>();

			//StartCoroutine(Wait());

			SpawnCollectable();
		}

	}

	void SpawnCollectable()
	{
		if (IsServer)
		{
			int i = Random.Range(1, spawnPoints.Length);

			while (i == lastUsedIndex && spawnPoints.Length > 1)
				i = Random.Range(1, spawnPoints.Length);


			GameObject obj =
				Instantiate(collectablePrefab, spawnPoints[i].position, spawnPoints[i].rotation) as GameObject;
			obj.GetComponent<NetworkedObject>().Spawn();

			obj.GetComponent<Collectable>().spawner = this;

			lastUsedIndex = i;
		}
	}

	public void CollectableTaken()
	{
		StartCoroutine(Wait());
	}

	IEnumerator Wait()
	{
		Debug.Log("Test1");
		yield return new WaitForSeconds(10);
		Debug.Log("Test2");
		SpawnCollectable();
	}

}
