using System;
using UnityEngine;

namespace ActionGameFramework.Audio
{
	/// <summary>
	/// Weighted audio clip.
	/// This is so that individual clips can be given a higher probability of selection
	/// </summary>
	[Serializable]
	public class WeightedAudioClip
	{
		/// <summary>
		/// The audio clip.
		/// </summary>
		public AudioClip clip;

		/// <summary>
		/// The weight - used to ensure that individual clips can be given a higher probability of selection
		/// </summary>
		public int weight = 1;
	}
}