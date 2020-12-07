//This script controls the game's collectable pickups. It handles a collectable's visual effects
//as well as tell a controlling object when it is "picked up"
using MLAPI;
using UnityEngine;

public class Collectable : NetworkedBehaviour
{
	public CollectableSpawner spawner;
	public PlayerLifeManager playerLifeManager;
	public int healAmount;
	
	void OnTriggerEnter(Collider other)
	{
		if (IsServer)
		{
			if (other.gameObject.tag.Equals("Player"))
			{
				playerLifeManager = other.gameObject.GetComponent<PlayerLifeManager>();
				playerLifeManager.HealPlayer(healAmount);
				if (spawner != null)
					spawner.CollectableTaken();

				Destroy (gameObject,0);
			}
		}
	}
}
