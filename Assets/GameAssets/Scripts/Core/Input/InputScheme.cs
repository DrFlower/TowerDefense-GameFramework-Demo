using UnityEngine;

namespace Core.Input
{
	/// <summary>
	/// Base class for any input scheme that knows how and when to activate itself
	/// </summary>
	public abstract class InputScheme : MonoBehaviour
	{
		/// <summary>
		/// Gets whether the scheme should be activated or not
		/// </summary>
		public abstract bool shouldActivate { get; }

		/// <summary>
		/// Gets whether this scheme should be default
		/// </summary>
		public abstract bool isDefault { get; }

		/// <summary>
		/// Activate if not already activated
		/// </summary>
		/// <param name="previousScheme">
		/// The scheme that was previously enabled.
		/// Will be null on start up.
		/// </param>
		public virtual void Activate(InputScheme previousScheme)
		{
			if (!enabled)
			{
				enabled = true;
			}
		}

		/// <summary>
		/// Deactivate if not already deactivated
		/// </summary>
		/// <param name="nextScheme">
		/// The next scheme that will be activated.
		/// Will be null on start up.
		/// </param>
		public virtual void Deactivate(InputScheme nextScheme)
		{
			if (enabled)
			{
				enabled = false;
			}
		}
	}
}