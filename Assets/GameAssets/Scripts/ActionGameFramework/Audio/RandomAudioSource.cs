using UnityEngine;

namespace ActionGameFramework.Audio
{
	/// <summary>
	/// A helper for playing random audio clips
	/// The randomness is not uniform but rather based on weights
	/// </summary>
	[RequireComponent(typeof(AudioSource))]
	public class RandomAudioSource : MonoBehaviour
	{
		/// <summary>
		/// A weighted list of audio clips
		/// </summary>
		public WeightedAudioList clips;

		/// <summary>
		/// Configuration for playing a sound randomly on awake
		/// </summary>
		public bool playOnEnabled;

		/// <summary>
		/// The attached audio source
		/// </summary>
		protected AudioSource m_Source;

		/// <summary>
		/// Cache the audio source and play if necessary
		/// </summary>
		protected virtual void OnEnable()
		{
			if (m_Source == null)
			{
				m_Source = GetComponent<AudioSource>();
			}
			if (playOnEnabled)
			{
				PlayRandomClip();
			}
		}

		/// <summary>
		/// Plays the random clip using the attached audio source
		/// </summary>
		public virtual void PlayRandomClip()
		{
			if (m_Source == null)
			{
				m_Source = GetComponent<AudioSource>();
			}
			PlayRandomClip(m_Source);
		}

		/// <summary>
		/// Plays the random clip using a specified audio source
		/// </summary>
		/// <param name="source">Audio source to use</param>
		public virtual void PlayRandomClip(AudioSource source)
		{
			if (source == null)
			{
				Debug.LogError("[RANDOM AUDIO SOURCE] Missing audio source");
				return;
			}

			AudioClip clip = clips.WeightedSelection();
			if (clip == null)
			{
				Debug.LogError("[RANDOM AUDIO SOURCE] Missing audio clips");
				return;
			}

			source.clip = clip;
			source.Play();
		}
	}
}