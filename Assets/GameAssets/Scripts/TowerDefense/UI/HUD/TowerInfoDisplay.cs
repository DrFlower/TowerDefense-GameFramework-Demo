using TowerDefense.Towers;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense.UI.HUD
{
	/// <summary>
	/// Used to display infomation about a tower using Unity UI
	/// </summary>
	public class TowerInfoDisplay : MonoBehaviour
	{
		/// <summary>
		/// The text component for the name
		/// </summary>
		public Text towerName;

		/// <summary>
		/// The text component for the description
		/// </summary>
		public Text description;

		/// <summary>
		/// The text component for the description
		/// </summary>
		public Text dps;

		/// <summary>
		/// The text component for the level
		/// </summary>
		public Text level;

		/// <summary>
		/// The text component for the health
		/// </summary>
		public Text health;

		/// <summary>
		/// The text component for the dimensions
		/// </summary>
		public Text dimensions;

		/// <summary>
		/// The text component for the dimensions
		/// </summary>
		public Text upgradeCost;

		/// <summary>
		/// The text component for the dimensions
		/// </summary>
		public Text sellPrice;

		/// <summary>
		/// Draws the tower data on to the canvas, if the relevant text components are populated
		/// </summary>
		/// <param name="tower">
		/// The tower to gain info from
		/// </param>
		public void Show(Tower tower)
		{
			int levelOfTower = tower.currentLevel;
			Show(tower, levelOfTower);
		}

		/// <summary>
		/// Draws the tower data on to the canvas, if the relevant text components are populated
		/// </summary>
		/// <param name="tower">The tower to gain info from</param>
		/// <param name="levelOfTower">The level of the tower</param>
		public void Show(Tower tower, int levelOfTower)
		{
			if (levelOfTower >= tower.levels.Length)
			{
				return;
			}
			TowerLevel towerLevel = tower.levels[levelOfTower];
			DisplayText(towerName, tower.towerName);
			DisplayText(description, towerLevel.description);
			DisplayText(dps, towerLevel.GetTowerDps().ToString("f2"));
			DisplayText(health, string.Format("{0}/{1}", tower.configuration.currentHealth, towerLevel.maxHealth));
			DisplayText(level, (levelOfTower + 1).ToString());
			DisplayText(dimensions, string.Format("{0}, {1}", tower.dimensions.x, tower.dimensions.y));
			if (levelOfTower + 1 < tower.levels.Length)
			{
				DisplayText(upgradeCost, tower.levels[levelOfTower + 1].cost.ToString());
			}

			int sellValue = tower.GetSellLevel(levelOfTower);
			DisplayText(sellPrice, sellValue.ToString());
		}

		/// <summary>
		/// Draws the text if the text component is populated
		/// </summary>
		/// <param name="textBox"></param>
		/// <param name="text"></param>
		static void DisplayText(Text textBox, string text)
		{
			if (textBox != null)
			{
				textBox.text = text;
			}
		}
	}
}