using System;
using System.Collections.Generic;
using Core.Health;
using UnityEngine;

namespace ActionGameFramework.Audio
{
	/// <summary>
	/// Health change sound selector
	/// </summary>
	[Serializable]
	public class HealthChangeSoundSelector
	{
		/// <summary>
		/// The array of health change sounds
		/// This array should be in ascending order of Health Difference
		/// </summary>
		[Tooltip("Health change should be in ascending order")]
		public List<HealthChangeSound> healthChangeSounds;

		/// <summary>
		/// Gets a value indicating whether this <see cref="ActionGameFramework.Audio.HealthChangeSoundSelector" /> is set up.
		/// </summary>
		/// <value><c>true</c> if there are Health Change Sounds; otherwise, <c>false</c>.</value>
		public bool isSetUp
		{
			get { return healthChangeSounds.Count > 0; }
		}

		/// <summary>
		/// Gets the clip from health change info.
		/// </summary>
		/// <returns>The clip from health change info.</returns>
		/// <param name="info">The HealthChangeInfo</param>
		public virtual AudioClip GetClipFromHealthChangeInfo(HealthChangeInfo info)
		{
			int count = healthChangeSounds.Count;

			for (int i = 0; i < count; i++)
			{
				HealthChangeSound sound = healthChangeSounds[i];

				// if the absolute health change is less than the sound health change 
				// then this is the sound clip to use 
				if (info.absHealthDifference <= sound.healthChange)
				{
					return sound.sound;
				}
			}

			Debug.LogFormat("Could not find sound for healthChange of {0}", info.absHealthDifference);
			return null;
		}
	}
}