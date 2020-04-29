using UnityEngine;

namespace ActionGameFramework.Spawning
{
	/// <summary>
	/// Single hit object spawner - concrete implementation that provides one game object to spawn
	/// </summary>
	public class SingleHitObjectSpawner : HitObjectSpawner
	{
		public GameObject gameObjectToSpawn;

		protected override GameObject GetGameObjectToInstantiate()
		{
			return gameObjectToSpawn;
		}
	}
}