
using Core.Game;
using Core.Health;
using TowerDefense.Game;
using TowerDefense.Level;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TowerDefense.UI
{
	/// <summary>
	/// UI to display the game over screen
	/// </summary>
	public class EndGameScreen : MonoBehaviour
	{
		/// <summary>
		/// AudioClip to play when victorious
		/// </summary>
		public AudioClip victorySound;

		/// <summary>
		/// AudioClip to play when failed
		/// </summary>
		public AudioClip defeatSound;

		/// <summary>
		/// AudioSource that plays the sound
		/// </summary>
		public AudioSource audioSource;

		/// <summary>
		/// The containing panel of the End Game UI
		/// </summary>
		public Canvas endGameCanvas;

		/// <summary>
		/// Reference to the Text object that displays the result message
		/// </summary>
		public Text endGameMessageText;

		/// <summary>
		/// Panel that shows final star rating
		/// </summary>
		public ScorePanel scorePanel;

		/// <summary>
		/// Name of level select screen
		/// </summary>
		public string menuSceneName = "MainMenu";

		/// <summary>
		/// Text to be displayed on popup
		/// </summary>
		public string levelCompleteText = "{0} COMPLETE!";
		
		public string levelFailedText = "{0} FAILED!";

		/// <summary>
		/// Background image
		/// </summary>
		public Image background;

		/// <summary>
		/// Color to set background
		/// </summary>
		public Color winBackgroundColor;
		
		public Color loseBackgroundColor;

		/// <summary>
		/// The Canvas that holds the button to go to the next level
		/// if the player has beaten the level
		/// </summary>
		public Canvas nextLevelButton;

		/// <summary>
		/// Reference to the <see cref="LevelManager" />
		/// </summary>
		protected LevelManager m_LevelManager;

		/// <summary>
		/// Safely unsubscribes from <see cref="LevelManager" /> events.
		/// Go back to the main menu scene
		/// </summary>
		public void GoToMainMenu()
		{
			SafelyUnsubscribe();
			SceneManager.LoadScene(menuSceneName);
		}

		/// <summary>
		/// Safely unsubscribes from <see cref="LevelManager" /> events.
		/// Reloads the active scene
		/// </summary>
		public void RestartLevel()
		{
			SafelyUnsubscribe();
			string currentSceneName = SceneManager.GetActiveScene().name;
			SceneManager.LoadScene(currentSceneName);
		}

		/// <summary>
		/// Safely unsubscribes from <see cref="LevelManager" /> events.
		/// Goes to the next scene if valid
		/// </summary>
		public void GoToNextLevel()
		{
			SafelyUnsubscribe();
			if (!GameManager.instanceExists)
			{
				return;
			}
			GameManager gm = GameManager.instance;
			LevelItem item = gm.GetLevelForCurrentScene();
			LevelList list = gm.levelList;
			int levelCount = list.Count;
			int index = -1;
			for (int i = 0; i < levelCount; i++)
			{
				if (item == list[i])
				{
					index = i + 1;
					break;
				}
			}
			if (index < 0 || index >= levelCount)
			{
				return;
			}
			LevelItem nextLevel = gm.levelList[index];
			SceneManager.LoadScene(nextLevel.sceneName);
		}

		/// <summary>
		/// Hide the panel if it is active at the start.
		/// Subscribe to the <see cref="LevelManager" /> completed/failed events.
		/// </summary>
		protected void Start()
		{
			LazyLoad();
			endGameCanvas.enabled = false;
			nextLevelButton.enabled = false;
			nextLevelButton.gameObject.SetActive(false);

			m_LevelManager.levelCompleted += Victory;
			m_LevelManager.levelFailed += Defeat;
		}

		/// <summary>
		/// Shows the end game screen
		/// </summary>
		protected void OpenEndGameScreen(string endResultText)
		{
			LevelItem level = GameManager.instance.GetLevelForCurrentScene();
			endGameCanvas.enabled = true;

			int score = CalculateFinalScore();
			scorePanel.SetStars(score);
			if (level != null) 
			{
				endGameMessageText.text = string.Format (endResultText, level.name.ToUpper ());
				GameManager.instance.CompleteLevel (level.id, score);
			} 
			else 
			{
				// If the level is not in LevelList, we should just use the name of the scene. This will not store the level's score.
				string levelName = SceneManager.GetActiveScene ().name;
				endGameMessageText.text = string.Format (endResultText, levelName.ToUpper ());
			}


			if (!HUD.GameUI.instanceExists)
			{
				return;
			}
			if (HUD.GameUI.instance.state == HUD.GameUI.State.Building)
			{
				HUD.GameUI.instance.CancelGhostPlacement();
			}
			HUD.GameUI.instance.GameOver();
		}

		/// <summary>
		/// Occurs when the level is sucessfully completed
		/// </summary>
		protected void Victory()
		{
			OpenEndGameScreen(levelCompleteText);
			if ((victorySound != null) && (audioSource != null))
			{
				audioSource.PlayOneShot(victorySound);
			}
			background.color = winBackgroundColor;

			//first check if there are any more levels after this one
			if (nextLevelButton == null || !GameManager.instanceExists)
			{
				return;
			}
			GameManager gm = GameManager.instance;
			LevelItem item = gm.GetLevelForCurrentScene();
			LevelList list = gm.levelList;
			int levelCount = list.Count;
			int index = -1;
			for (int i = 0; i < levelCount; i++)
			{
				if (item == list[i])
				{
					index = i;
					break;
				}
			}
			//if the level does not exist or this is the last level
			//hide the next level button
			if (index < 0 || index == levelCount - 1)
			{
				nextLevelButton.enabled = false;
				nextLevelButton.gameObject.SetActive(false);
				return;
			}
			nextLevelButton.enabled = true;
			nextLevelButton.gameObject.SetActive(true);
		}

		/// <summary>
		/// Occurs when level is failed
		/// </summary>
		protected void Defeat()
		{
			OpenEndGameScreen(levelFailedText);
			if (nextLevelButton != null)
			{
				nextLevelButton.enabled = false;
				nextLevelButton.gameObject.SetActive(false);
			}
			if ((defeatSound != null) && (audioSource != null))
			{
				audioSource.PlayOneShot(defeatSound);
			}
			background.color = loseBackgroundColor;
		}

		/// <summary>
		/// Safely unsubscribes from <see cref="LevelManager" /> events.
		/// </summary>
		protected void OnDestroy()
		{
			SafelyUnsubscribe();
			if (HUD.GameUI.instanceExists)
			{
				HUD.GameUI.instance.Unpause();
			}
		}

		/// <summary>
		/// Ensure that <see cref="LevelManager" /> events are unsubscribed from when necessary
		/// </summary>
		protected void SafelyUnsubscribe()
		{
			LazyLoad();
			m_LevelManager.levelCompleted -= Victory;
			m_LevelManager.levelFailed -= Defeat;
		}

		/// <summary>
		/// Ensure <see cref="m_LevelManager" /> is not null
		/// </summary>
		protected void LazyLoad()
		{
			if ((m_LevelManager == null) && LevelManager.instanceExists)
			{
				m_LevelManager = LevelManager.instance;
			}
		}

		/// <summary>
		/// Add up the health of all the Home Bases and return a score
		/// </summary>
		/// <returns>Final score</returns>
		protected int CalculateFinalScore()
		{
			int homeBaseCount = m_LevelManager.numberOfHomeBases;
			PlayerHomeBase[] homeBases = m_LevelManager.playerHomeBases;

			float totalRemainingHealth = 0f;
			float totalBaseHealth = 0f;
			for (int i = 0; i < homeBaseCount; i++)
			{
				Damageable config = homeBases[i].configuration;
				totalRemainingHealth += config.currentHealth;
				totalBaseHealth += config.maxHealth;
			}
			int score = CalculateScore(totalRemainingHealth, totalBaseHealth);
			return score;
		}

		/// <summary>
		/// Take the final remaining health of all bases and rates them
		/// </summary>
		/// <param name="remainingHealth">the total remaining health of all home bases</param>
		/// <param name="maxHealth">the total maximum health of all home bases</param>
		/// <returns>0 to 3 depending on how much health is remaining</returns>
		protected int CalculateScore(float remainingHealth, float maxHealth)
		{
			float normalizedHealth = remainingHealth / maxHealth;
			if (Mathf.Approximately(normalizedHealth, 1f))
			{
				return 3;
			}
			if ((normalizedHealth <= 0.9f) && (normalizedHealth >= 0.5f))
			{
				return 2;
			}
			if ((normalizedHealth < 0.5f) && (normalizedHealth > 0f))
			{
				return 1;
			}
			return 0;
		}
	}
}