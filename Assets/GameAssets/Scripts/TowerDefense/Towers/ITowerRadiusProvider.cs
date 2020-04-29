using TowerDefense.Targetting;
using UnityEngine;

namespace TowerDefense.Towers
{
	/// <summary>
	/// An interface for tower affectors to implement in order to visualize their affect radius
	/// </summary>
	public interface ITowerRadiusProvider
	{
		float effectRadius { get; }
		Color effectColor { get; }
		Targetter targetter { get; }
	}
}