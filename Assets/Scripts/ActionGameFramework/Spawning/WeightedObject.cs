using System;
using UnityEngine;

namespace ActionGameFramework.Spawning
{
	/// <summary>
	/// Weighted hit object.
	/// This is so that individual objects can be given a higher probability of selection
	/// </summary>
	[Serializable]
	public class WeightedObject
	{
		/// <summary>
		/// The game object.
		/// </summary>
		public GameObject gameObject;

		/// <summary>
		/// The weight - used to ensure that individual objects can be given a higher probability of selection
		/// </summary>
		public int weight = 1;
	}
}