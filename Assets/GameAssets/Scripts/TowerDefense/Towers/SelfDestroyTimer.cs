using Core.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace TowerDefense.Towers
{
	/// <summary>
	/// A helper component for self destruction
	/// </summary>
	public class SelfDestroyTimer : MonoBehaviour
	{
		/// <summary>
		/// The time before destruction
		/// </summary>
		public float time = 5;

		/// <summary>
		/// The controlling timer
		/// </summary>
		public Timer timer;
		
		/// <summary>
		/// The exposed death callback
		/// </summary>
		public UnityEvent death;

		/// <summary>
		/// Potentially initialize the time if necessary
		/// </summary>
		protected virtual void OnEnable()
		{
			if (timer == null)
			{
				timer = new Timer(time, OnTimeEnd);
			}
			else
			{
				timer.Reset();
			}
		}

		/// <summary>
		/// Update the timer
		/// </summary>
		protected virtual void Update()
		{
			if (timer == null)
			{
				return;
			}
			timer.Tick(Time.deltaTime);
		}

		/// <summary>
		/// Fires at the end of timer
		/// </summary>
		protected virtual void OnTimeEnd()
		{
			death.Invoke();
			Poolable.TryPool(gameObject);
			timer.Reset();
		}
	}
}