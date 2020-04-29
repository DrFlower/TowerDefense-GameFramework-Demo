using System.Collections.Generic;
using Core.Health;
using UnityEngine;

namespace ActionGameFramework.Audio
{
	/// <summary>
	/// Health change audio source - a helper for playing sounds on Health Change
	/// </summary>
	[RequireComponent(typeof(AudioSource))]
	public class HealthChangeAudioSource : MonoBehaviour
	{
		/// <summary>
		/// The sound selector. A mechanism of specifying how sounds are selected based on HealthChangeInfo
		/// </summary>
		public HealthChangeSoundSelector soundSelector;

		/// <summary>
		/// The audio source
		/// </summary>
		protected AudioSource m_Source;

		/// <summary>
		/// Assign the required AudioSource reference at runtime
		/// </summary>
		protected virtual void Awake()
		{
			m_Source = GetComponent<AudioSource>();
		}

		/// <summary>
		/// Play the AudioSource
		/// </summary>
		public virtual void PlaySound()
		{
			m_Source.Play();
		}

		/// <summary>
		/// Play a clip when certain health change requirements are met
		/// </summary>
		/// <param name="info">Uses <see cref="HealthChangeInfo"/> to determine what clip to play</param>
		public virtual void PlayHealthChangeSound(HealthChangeInfo info)
		{
			if (soundSelector != null && soundSelector.isSetUp)
			{
				AudioClip newClip = soundSelector.GetClipFromHealthChangeInfo(info);
				if (newClip != null)
				{
					m_Source.clip = newClip;
				}
			}

			m_Source.Play();
		}

		/// <summary>
		/// Sorts the <see cref="soundSelector"/> sound list
		/// </summary>
		public void Sort()
		{
			if (soundSelector.healthChangeSounds == null || soundSelector.healthChangeSounds.Count <= 0)
			{
				return;
			}
			soundSelector.healthChangeSounds.Sort(new HealthChangeSoundComparer());
		}
	}

	/// <summary>
	/// Provides a way to compare 2 <see cref="HealthChangeSound"/>s
	/// </summary>
	public class HealthChangeSoundComparer : IComparer<HealthChangeSound>
	{
		/// <summary>
		/// Compares 2 <see cref="HealthChangeSound"/>
		/// </summary>
		public int Compare(HealthChangeSound first, HealthChangeSound second)
		{
			if (first.healthChange == second.healthChange)
			{
				return 0;
			}
			if (first.healthChange < second.healthChange)
			{
				return -1;
			}

			return 1;
		}
	}
}