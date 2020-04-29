using UnityEngine;

namespace ActionGameFramework.Spawning
{
	/// <summary>
	/// Random hit object spawner - implementation of the hit spawner which chooses a random hit spawner from a weighted list
	/// </summary>
	public class RandomHitObjectSpawner : HitObjectSpawner
	{
		public WeightedObjectList objectList;

		protected override GameObject GetGameObjectToInstantiate()
		{
			return objectList.WeightedSelection();
		}
	}
}