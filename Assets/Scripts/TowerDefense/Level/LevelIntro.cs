using System;
using UnityEngine;

namespace TowerDefense.Level
{
	/// <summary>
	/// Abstract base class representing a level intro
	/// </summary>
	public abstract class LevelIntro : MonoBehaviour
	{
		/// <summary>
		/// Called when the Intro is completed
		/// </summary>
		public event Action introCompleted;

		/// <summary>
		/// Should be fired by the derived classes to mark that the intro is completed
		/// </summary>
		protected void SafelyCallIntroCompleted()
		{
			if (introCompleted != null)
			{
				introCompleted();
			}
		}
	}
}