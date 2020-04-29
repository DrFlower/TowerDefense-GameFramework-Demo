using UnityEngine;

namespace TowerDefense.UI
{
	/// <summary>
	/// Simple script to open a URL
	/// </summary>
	public class UrlOpen : MonoBehaviour
	{
		/// <summary>
		/// Open the given url
		/// </summary>
		public void OpenUrl(string url)
		{
			Application.OpenURL(url);
		}
	}
}