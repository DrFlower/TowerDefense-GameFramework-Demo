using Core.Health;
using UnityEngine;

namespace ActionGameFramework.Spawning
{
	/// <summary>
	/// A hit object is a special type of GameObject that consumes hit info
	/// e.g. using Damage to scale the size
	/// </summary>
	public abstract class HitObject : MonoBehaviour
	{
		public abstract void SetHitInfo(HitInfo hitInfo);
	}
}