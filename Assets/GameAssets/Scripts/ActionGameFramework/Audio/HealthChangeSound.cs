using System;
using UnityEngine;

namespace ActionGameFramework.Audio
{
	/// <summary>
	/// Health change sound - maps a health change to an AudioClip
	/// </summary>
	[Serializable]
	public class HealthChangeSound
	{
		[Tooltip("Health Change should be in ascending order")]
		public float healthChange;

		public AudioClip sound;
	}
}