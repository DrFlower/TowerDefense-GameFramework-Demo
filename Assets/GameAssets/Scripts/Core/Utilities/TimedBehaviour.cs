using System.Collections.Generic;
using UnityEngine;

namespace Core.Utilities
{
	/// <summary>
	/// Abstract based class for helping with timing in MonoBehaviours
	/// </summary>
	public abstract class TimedBehaviour : MonoBehaviour
	{
		/// <summary>
		/// List of active timers
		/// </summary>
		readonly List<Timer> m_ActiveTimers = new List<Timer>();

		/// <summary>
		/// Adds the timer to list  of active timers
		/// </summary>
		/// <param name="newTimer">the  timer to be added to the list of active timers</param>
		protected void StartTimer(Timer newTimer)
		{
			if (m_ActiveTimers.Contains(newTimer))
			{
				Debug.LogWarning("Timer already exists!");
			}
			else
			{
				m_ActiveTimers.Add(newTimer);
			}
		}

		/// <summary>
		/// Removes timer from list of active timers
		/// </summary>
		/// <param name="timer">the timer to be removed from the list of active timers</param>
		protected void PauseTimer(Timer timer)
		{
			if (m_ActiveTimers.Contains(timer))
			{
				m_ActiveTimers.Remove(timer);
			}
		}

		/// <summary>
		/// Resets and removes the timer
		/// </summary>
		/// <param name="timer">the timer to be stopped</param>
		protected void StopTimer(Timer timer)
		{
			timer.Reset();
			PauseTimer(timer);
		}

		/// <summary>
		/// Iterates through the list of active timers and ticks
		/// </summary>
		protected virtual void Update()
		{
			for (int i = m_ActiveTimers.Count - 1; i >= 0; i--)
			{
				if (m_ActiveTimers[i].Tick(Time.deltaTime))
				{
					StopTimer(m_ActiveTimers[i]);
				}
			}
		}
	}
}