using Core.Health;
using UnityEngine;

namespace ActionGameFramework.Spawning
{
	/// <summary>
	/// Hit object spawner - provides a public method for spawning game objects based on a hit
	/// The spawned game object may have a HitObject component, which consumes the hit information
	/// </summary>
	public abstract class HitObjectSpawner : MonoBehaviour
	{
		/// <summary>
		/// Gets the game object to instantiate.
		/// This is needed to that we can provide different mechanisms for choosing game objects to instantiate
		/// </summary>
		/// <returns>The game object to instantiate.</returns>
		protected abstract GameObject GetGameObjectToInstantiate();

		/// <summary>
		/// The public method for instantiating a hit object - this can be accessed by methods on the DamageableListener
		/// </summary>
		/// <param name="hitInfo">Hit info.</param>
		public virtual void InstantiateHitObject(HitInfo hitInfo)
		{
			GameObject gameObjectToInstantiate = GetGameObjectToInstantiate();
			GameObject gameObjectInstance = Instantiate(gameObjectToInstantiate, hitInfo.damagePoint, Quaternion.identity);
			HitObject[] hitObjects = gameObjectInstance.GetComponentsInChildren<HitObject>();
			int length = hitObjects.Length;
			for (int i = 0; i < length; i++)
			{
				HitObject hitObject = hitObjects[i];
				hitObject.SetHitInfo(hitInfo);
			}
		}
	}
}