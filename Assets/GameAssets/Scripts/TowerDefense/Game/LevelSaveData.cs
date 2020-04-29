using System;

namespace TowerDefense.Game
{
	/// <summary>
	/// A calss to save level data
	/// </summary>
	[Serializable]
	public class LevelSaveData
	{
		public string id;
		public int numberOfStars;

		public LevelSaveData(string levelId, int numberOfStarsEarned)
		{
			id = levelId;
			numberOfStars = numberOfStarsEarned;
		}
	}
}