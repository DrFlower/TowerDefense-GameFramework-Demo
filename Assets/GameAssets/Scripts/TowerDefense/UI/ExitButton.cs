using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TowerDefense.UI
{
	/// <summary>
	/// A button for exiting the game
	/// </summary>
	public class ExitButton : Button
	{
		/// <summary>
		/// Close the game when this button is clicked
		/// </summary>
		public override void OnPointerClick(PointerEventData eventData)
		{
			Application.Quit();
		}

		/// <summary>
		/// Disable this button on mobile platforms
		/// </summary>
		protected override void Awake()
		{
			base.Awake();

#if UNITY_ANDROID || UNITY_IOS
			if (Application.isPlaying)
			{
				gameObject.SetActive(false);
			}
#endif
		}
	}
}