using Core.UI;
using UnityEngine;

namespace TowerDefense.UI.HUD
{
	/// <summary>
	/// Main menu implementation for tower defense
	/// </summary>
	public class TowerDefenseMainMenu : MainMenu
	{
		/// <summary>
		/// Reference to options menu
		/// </summary>
		public OptionsMenu optionsMenu;
		
		/// <summary>
		/// Reference to title menu
		/// </summary>
		public SimpleMainMenuPage titleMenu;
		
		/// <summary>
		/// Reference to level select menu
		/// </summary>
		public LevelSelectScreen levelSelectMenu;

		/// <summary>
		/// Bring up the options menu
		/// </summary>
		public void ShowOptionsMenu()
		{
			ChangePage(optionsMenu);
		}
		
		/// <summary>
		/// Bring up the options menu
		/// </summary>
		public void ShowLevelSelectMenu()
		{
			ChangePage(levelSelectMenu);
		}
		
		/// <summary>
		/// Returns to the title screen
		/// </summary>
		public void ShowTitleScreen()
		{
			Back(titleMenu);
		}

		/// <summary>
		/// Set initial page
		/// </summary>
		protected virtual void Awake()
		{
			ShowTitleScreen();
		}

		/// <summary>
		/// Escape key input
		/// </summary>
		protected virtual void Update()
		{
			if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
			{
				if ((SimpleMainMenuPage)m_CurrentPage == titleMenu)
				{
					Application.Quit();
				}
				else
				{
					Back();
				}
			}
		}
	}
}