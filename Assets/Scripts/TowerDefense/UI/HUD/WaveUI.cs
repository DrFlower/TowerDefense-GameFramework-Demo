using TowerDefense.Level;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense.UI.HUD
{
	/// <summary>
	/// A class for displaying the wave feedback
	/// </summary>
	[RequireComponent(typeof(Canvas))]
	public class WaveUI : MonoBehaviour
	{
		/// <summary>
		/// The text element to display information on
		/// </summary>
		public Text display;

		public Image waveFillImage;

		/// <summary>
		/// The total amount of waves for this level
		/// </summary>
		protected int m_TotalWaves;

		protected Canvas m_Canvas;

		/// <summary>
		/// cache the total amount of waves
		/// Update the display 
		/// and Subscribe to waveChanged
		/// </summary>
		protected virtual void Start()
		{
			m_Canvas = GetComponent<Canvas>();
			m_Canvas.enabled = false;
			m_TotalWaves = LevelManager.instance.waveManager.totalWaves;
			LevelManager.instance.waveManager.waveChanged += UpdateDisplay;
		}

		/// <summary>
		/// Write the current wave amount to the display
		/// </summary>
		protected void UpdateDisplay()
		{
			m_Canvas.enabled = true;
			int currentWave = LevelManager.instance.waveManager.waveNumber;
			string output = string.Format("{0}/{1}", currentWave, m_TotalWaves);
			display.text = output;
		}

		protected virtual void Update()
		{
			waveFillImage.fillAmount = LevelManager.instance.waveManager.waveProgress;
		}

		/// <summary>
		/// Unsubscribe from events
		/// </summary>
		protected void OnDestroy()
		{
			if (LevelManager.instanceExists)
			{
				LevelManager.instance.waveManager.waveChanged -= UpdateDisplay;
			}
		}
	}
}