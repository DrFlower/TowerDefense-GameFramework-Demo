using System;

namespace Core.Game
{
	/// <summary>
	/// Element describing a level
	/// </summary>
	[Serializable]
	public class LevelItem
	{
		/// <summary>
		/// The id - used in persistence
		/// </summary>
		public string id;

		/// <summary>
		/// The human readable level name
		/// </summary>
		public string name;

		/// <summary>
		/// The description of the level - flavour text
		/// </summary>
		public string description;

		/// <summary>
		/// The name of the scene to load
		/// </summary>
		public string sceneName;
	}
}