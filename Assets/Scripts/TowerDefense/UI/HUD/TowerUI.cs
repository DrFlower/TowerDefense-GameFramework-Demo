using TowerDefense.Level;
using TowerDefense.Towers;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense.UI.HUD
{
	/// <summary>
	/// Controls the UI objects that draw the tower data
	/// </summary>
	[RequireComponent(typeof(Canvas))]
	public class TowerUI : MonoBehaviour
	{
		/// <summary>
		/// The text object for the name
		/// </summary>
		public Text towerName;

		/// <summary>
		/// The text object for the description
		/// </summary>
		public Text description;
		
		public Text upgradeDescription;

		/// <summary>
		/// The attached sell button
		/// </summary>
		public Button sellButton;

		/// <summary>
		/// The attached upgrade button
		/// </summary>
		public Button upgradeButton;

		/// <summary>
		/// Component to display the relevant information of the tower
		/// </summary>
		public TowerInfoDisplay towerInfoDisplay;

		public RectTransform panelRectTransform;

		public GameObject[] confirmationButtons;

		/// <summary>
		/// The main game camera
		/// </summary>
		protected Camera m_GameCamera;

		/// <summary>
		/// The current tower to draw
		/// </summary>
		protected Tower m_Tower;

		/// <summary>
		/// The canvas attached to the gameObject
		/// </summary>
		protected Canvas m_Canvas;

		/// <summary>
		/// Draws the tower data on to the canvas
		/// </summary>
		/// <param name="towerToShow">
		/// The tower to gain info from
		/// </param>
		public virtual void Show(Tower towerToShow)
		{
			if (towerToShow == null)
			{
				return;
			}
			m_Tower = towerToShow;
			AdjustPosition();

			m_Canvas.enabled = true;

			int sellValue = m_Tower.GetSellLevel();
			if (sellButton != null)
			{
				sellButton.gameObject.SetActive(sellValue > 0);
			}
			if (upgradeButton != null)
			{
				upgradeButton.interactable = 
					LevelManager.instance.currency.CanAfford(m_Tower.GetCostForNextLevel());
				bool maxLevel = m_Tower.isAtMaxLevel;
				upgradeButton.gameObject.SetActive(!maxLevel);
				if (!maxLevel)
				{
					upgradeDescription.text =
						m_Tower.levels[m_Tower.currentLevel + 1].upgradeDescription.ToUpper();
				}
			}
			LevelManager.instance.currency.currencyChanged += OnCurrencyChanged;
			towerInfoDisplay.Show(towerToShow);
			foreach (var button in confirmationButtons)
			{
				button.SetActive(false);
			}
		}

		/// <summary>
		/// Hides the tower info UI and the radius visualizer
		/// </summary>
		public virtual void Hide()
		{
			m_Tower = null;
			if (GameUI.instanceExists)
			{
				GameUI.instance.HideRadiusVisualizer();
			}
			m_Canvas.enabled = false;
			LevelManager.instance.currency.currencyChanged -= OnCurrencyChanged;
		}

		/// <summary>
		/// Upgrades the tower through <see cref="GameUI"/>
		/// </summary>
		public void UpgradeButtonClick()
		{
			GameUI.instance.UpgradeSelectedTower();
		}

		/// <summary>
		/// Sells the tower through <see cref="GameUI"/>
		/// </summary>
		public void SellButtonClick()
		{
			GameUI.instance.SellSelectedTower();
		}

		/// <summary>
		/// Get the text attached to the buttons
		/// </summary>
		protected virtual void Awake()
		{
			m_Canvas = GetComponent<Canvas>();
		}

		/// <summary>
		/// Fires when tower is selected/deselected
		/// </summary>
		/// <param name="newTower"></param>
		protected virtual void OnUISelectionChanged(Tower newTower)
		{
			if (newTower != null)
			{
				Show(newTower);
			}
			else
			{
				Hide();
			}
		}

		/// <summary>
		/// Subscribe to mouse button action
		/// </summary>
		protected virtual void Start()
		{
			m_GameCamera = Camera.main;
			m_Canvas.enabled = false;
			if (GameUI.instanceExists)
			{
				GameUI.instance.selectionChanged += OnUISelectionChanged;
				GameUI.instance.stateChanged += OnGameUIStateChanged;
			}
		}

		/// <summary>
		/// Adjust position when the camera moves
		/// </summary>
		protected virtual void Update()
		{
			AdjustPosition();
		}

		/// <summary>
		/// Unsubscribe from currencyChanged
		/// </summary>
		protected virtual void OnDisable()
		{
			if (LevelManager.instanceExists)
			{
				LevelManager.instance.currency.currencyChanged -= OnCurrencyChanged;
			}
		}

		/// <summary>
		/// Adjust the position of the UI
		/// </summary>
		protected void AdjustPosition()
		{
			if (m_Tower == null)
			{
				return;
			}
			Vector3 point = m_GameCamera.WorldToScreenPoint(m_Tower.position);
			point.z = 0;
			panelRectTransform.transform.position = point;
		}

		/// <summary>
		/// Fired when the <see cref="GameUI"/> state changes
		/// If the new state is <see cref="GameUI.State.GameOver"/> we need to hide the <see cref="TowerUI"/>
		/// </summary>
		/// <param name="oldState">The previous state</param>
		/// <param name="newState">The state to transition to</param>
		protected void OnGameUIStateChanged(GameUI.State oldState, GameUI.State newState)
		{
			if (newState == GameUI.State.GameOver)
			{
				Hide();
			}
		}

		/// <summary>
		/// Check if player can afford upgrade on currency changed
		/// </summary>
		void OnCurrencyChanged()
		{
			if (m_Tower != null && upgradeButton != null)
			{
				upgradeButton.interactable = 
					LevelManager.instance.currency.CanAfford(m_Tower.GetCostForNextLevel());
			}
		}

		/// <summary>
		/// Unsubscribe from GameUI selectionChanged and stateChanged
		/// </summary>
		void OnDestroy()
		{
			if (GameUI.instanceExists)
			{
				GameUI.instance.selectionChanged -= OnUISelectionChanged;
				GameUI.instance.stateChanged -= OnGameUIStateChanged;
			}
		}
	}
}