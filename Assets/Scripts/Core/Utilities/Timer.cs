using System;
using UnityEngine;

namespace Core.Utilities
{
	/// <summary>
	/// A timer data model. Consumed/process by the TimedBehaviour
	/// </summary>
	public class Timer
	{
		/// <summary>
		/// Event fired on elapsing
		/// </summary>
		readonly Action m_Callback;

		/// <summary>
		/// The time
		/// </summary>
		float m_Time, m_CurrentTime;

		/// <summary>
		/// Normalized progress of the timer
		/// </summary>
		public float normalizedProgress
		{
			get { return Mathf.Clamp(m_CurrentTime / m_Time, 0f, 1f); }
		}

		/// <summary>
		/// Timer constructor
		/// </summary>
		/// <param name="newTime">the time that timer is counting</param>
		/// <param name="onElapsed">the event fired at the end of the timer elapsing</param>
		public Timer(float newTime, Action onElapsed = null)
		{
			SetTime(newTime);

			m_CurrentTime = 0f;
			m_Callback += onElapsed;
		}

		/// <summary>
		/// Returns the result of AssessTime
		/// </summary>
		/// <param name="deltaTime">change in time between ticks</param>
		/// <returns>true if the timer has elapsed, false otherwise</returns>
		public virtual bool Tick(float deltaTime)
		{
			return AssessTime(deltaTime);
		}

		/// <summary>
		/// Checks if the time has elapsed and fires the tick event
		/// </summary>
		/// <param name="deltaTime">the change in time between assessments</param>
		/// <returns>true if the timer has elapsed, false otherwise</returns>
		protected bool AssessTime(float deltaTime)
		{
			m_CurrentTime += deltaTime;
			if (m_CurrentTime >= m_Time)
			{
				FireEvent();
				return true;
			}

			return false;
		}

		/// <summary>
		/// Resets the current time to 0
		/// </summary>
		public void Reset()
		{
			m_CurrentTime = 0;
		}

		/// <summary>
		/// Fires the associated timer event
		/// </summary>
		public void FireEvent()
		{
			m_Callback.Invoke();
		}

		/// <summary>
		/// Sets the elapsed time
		/// </summary>
		/// <param name="newTime">sets the time to a new value</param>
		public void SetTime(float newTime)
		{
			m_Time = newTime;

			if (newTime <= 0)
			{
				m_Time = 0.1f;
			}
		}
	}
}