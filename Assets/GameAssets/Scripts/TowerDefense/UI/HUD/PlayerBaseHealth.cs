using System.Globalization;
using Core.Health;
using TowerDefense.Level;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense.UI.HUD
{
	/// <summary>
	/// A simple implementation of UI for player base health
	/// </summary>
	public class PlayerBaseHealth : MonoBehaviour
	{
		/// <summary>
		/// The text element to display information on
		/// </summary>
		public Text display;

		/// <summary>
		/// The highest health that the base can go to
		/// </summary>
		protected float m_MaxHealth;

		/// <summary>
		/// Get the max health of the player base
		/// </summary>
		protected virtual void Start()
		{
			LevelManager levelManager = LevelManager.instance;
			if (levelManager == null)
			{
				return;
			}
			if (levelManager.numberOfHomeBases > 0)
			{
				Damageable baseConfig = levelManager.playerHomeBases[0].configuration;
				baseConfig.damaged += OnBaseDamaged;
				float currentHealth = baseConfig.currentHealth;
				float noramlisedHealth = baseConfig.normalisedHealth;
				m_MaxHealth = currentHealth / noramlisedHealth;
			}
			UpdateDisplay();
		}

		/// <summary>
		/// Subscribes to the player base health died event
		/// </summary>
		/// <param name="info">
		/// The associated health change information
		/// </param>
		protected virtual void OnBaseDamaged(HealthChangeInfo info)
		{
			UpdateDisplay();
		}

		/// <summary>
		/// Get the current health of the home base and display it on m_Display
		/// </summary>
		protected void UpdateDisplay()
		{
			LevelManager levelManager = LevelManager.instance;
			if (levelManager == null)
			{
				return;
			}
			float currentHealth = levelManager.GetAllHomeBasesHealth();
			display.text = currentHealth.ToString(CultureInfo.InvariantCulture);
		}
	}
}