using Core.Input;
using TowerDefense.UI.HUD;
using State = TowerDefense.UI.HUD.GameUI.State;

namespace TowerDefense.UI
{
	/// <summary>
	/// TD Specific input switcher that also disables controls when the game is paused
	/// </summary>
	public class TowerDefenseInputSchemeSwitcher : InputSchemeSwitcher
	{
		/// <summary>
		/// Gets whether the game is in a paused state
		/// </summary>
		public bool isPaused
		{
			get { return GameUI.instance.state == State.Paused; }
		}

		/// <summary>
		/// Register GameUI's stateChanged event
		/// </summary>
		protected virtual void Start()
		{
			if (GameUI.instanceExists)
			{
				GameUI.instance.stateChanged += OnUIStateChanged;
			}
		}

		/// <summary>
		/// Do nothing when game is paused
		/// </summary>
		protected override void Update()
		{
			if (isPaused)
			{
				return;
			}
			
			base.Update();
		}

		/// <summary>
		/// Unregister from GameUI's stateChanged event
		/// </summary>
		protected virtual void OnDestroy()
		{
			if (GameUI.instanceExists)
			{
				GameUI.instance.stateChanged -= OnUIStateChanged;
			}
		}

		/// <summary>
		/// Activate or deactivate the current input scheme when the game pauses/unpauses
		/// </summary>
		void OnUIStateChanged(State oldState, State newState)
		{
			if (m_CurrentScheme == null)
			{
				return;
			}
			if (newState == State.Paused)
			{
				m_CurrentScheme.Deactivate(null);
			}
			else
			{
				m_CurrentScheme.Activate(null);
			}
		}
	}
}