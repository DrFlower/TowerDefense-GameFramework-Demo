using UnityEngine;

namespace Core.UI
{
	/// <summary>
	/// Abstract base class for menu pages which animates the process of enabling and disabling
	/// Handles activation/deactivation of the page
	/// </summary>
	public abstract class AnimatingMainMenuPage : MonoBehaviour, IMainMenuPage
	{
		/// <summary>
		/// Canvas to disable. If this object is set, then the canvas is disabled instead of the game object 
		/// </summary>
		public Canvas canvas;
		
		/// <summary>
		/// Deactivates this page
		/// </summary>
		public virtual void Hide()
		{
			BeginDeactivatingPage();
		}

		/// <summary>
		/// Activates this page
		/// </summary>
		public virtual void Show()
		{
			BeginActivatingPage();
		}

		/// <summary>
		/// Starts the deactivation process. e.g. begins fading page out. Call FinishedDeactivatingPage when done
		/// </summary>
		protected abstract void BeginDeactivatingPage();

		/// <summary>
		/// Ends the deactivation process and turns off the associated gameObject/canvas
		/// </summary>
		protected virtual void FinishedDeactivatingPage()
		{
			if (canvas != null)
			{
				canvas.enabled = false;
			}
			else
			{
				gameObject.SetActive(false);
			}
		}

		/// <summary>
		/// Starts the activation process by turning on the associated gameObject/canvas.  Call FinishedActivatingPage when done
		/// </summary>
		protected virtual void BeginActivatingPage()
		{
			if (canvas != null)
			{
				canvas.enabled = true;
			}
			else
			{
				gameObject.SetActive(true);
			}
		}

		/// <summary>
		/// Finishes the activation process. e.g. Turning on input
		/// </summary>
		protected abstract void FinishedActivatingPage();
	}
}