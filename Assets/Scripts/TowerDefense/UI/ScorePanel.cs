using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense.UI
{
	/// <summary>
	/// UI object to display final score
	/// </summary>
	public class ScorePanel : MonoBehaviour
	{
		/// <summary>
		/// Objects that represent the stars
		/// </summary>
		public Image[] starImages;

		public Sprite achievedStarSprite;

		/// <summary>
		/// Show the correct number of stars for the score
		/// </summary>
		/// <param name="score">The final score</param>
		public void SetStars(int score)
		{
			if (score <= 0)
			{
				return;
			}
			score = Mathf.Clamp(score, 0, starImages.Length);
			for (int i = 0; i < score; i++)
			{
				starImages[i].sprite = achievedStarSprite;
			}
		}
	}
}