using UnityEngine;

namespace Core.UI
{
	/// <summary>
	/// Abstract base class for all modals
	/// </summary>
	[RequireComponent(typeof(CanvasGroup))]
	public abstract class Modal : MonoBehaviour
	{
		/// <summary>
		/// The attached CanvasGroup
		/// </summary>
		public CanvasGroup canvasGroup;

		/// <summary>
		/// Closes the modal
		/// </summary>
		public virtual void CloseModal()
		{
			gameObject.SetActive(false);
			DisableInteractivity();
		}

		/// <summary>
		/// Shows the modal
		/// </summary>
		public virtual void Show()
		{
			LazyLoad();
			gameObject.SetActive(true);
			EnableInteractivity();
		}

		/// <summary>
		/// Allows interactions
		/// </summary>
		protected virtual void EnableInteractivity()
		{
			canvasGroup.interactable = true;
		}

		/// <summary>
		/// Turns off interactions
		/// </summary>
		protected virtual void DisableInteractivity()
		{
			canvasGroup.interactable = false;
		}

		/// <summary>
		/// Lazy loads the canvas group into the local variable
		/// </summary>
		protected virtual void LazyLoad()
		{
			if (canvasGroup != null)
			{
				canvasGroup = GetComponent<CanvasGroup>();
			}
		}
	}
}