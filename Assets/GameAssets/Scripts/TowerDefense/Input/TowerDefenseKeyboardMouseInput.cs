using Core.Input;
using TowerDefense.Level;
using TowerDefense.Towers;
using TowerDefense.UI.HUD;
using UnityEngine;
using UnityInput = UnityEngine.Input;
using State = TowerDefense.UI.HUD.GameUI.State;

namespace TowerDefense.Input
{
	[RequireComponent(typeof(GameUI))]
	public class TowerDefenseKeyboardMouseInput : KeyboardMouseInput
	{
		/// <summary>
		/// Cached eference to gameUI
		/// </summary>
		GameUI m_GameUI;

		/// <summary>
		/// Register input events
		/// </summary>
		protected override void OnEnable()
		{
			base.OnEnable();
			
			m_GameUI = GetComponent<GameUI>();

			if (InputController.instanceExists)
			{
				InputController controller = InputController.instance;

				controller.tapped += OnTap;
				controller.mouseMoved += OnMouseMoved;
			}
		}

		/// <summary>
		/// Deregister input events
		/// </summary>
		protected override void OnDisable()
		{
			if (!InputController.instanceExists)
			{
				return;
			}

			InputController controller = InputController.instance;

			controller.tapped -= OnTap;
			controller.mouseMoved -= OnMouseMoved;
		}

		/// <summary>
		/// Handle camera panning behaviour
		/// </summary>
		protected override void Update()
		{
			base.Update();
			
			// Escape handling
			if (UnityInput.GetKeyDown(KeyCode.Escape))
			{
				switch (m_GameUI.state)
				{
					case State.Normal:
						if (m_GameUI.isTowerSelected)
						{
							m_GameUI.DeselectTower();
						}
						else
						{
							m_GameUI.Pause();
						}
						break;
					case State.BuildingWithDrag:
					case State.Building:
						m_GameUI.CancelGhostPlacement();
						break;
				}
			}
			
			// place towers with keyboard numbers
			if (LevelManager.instanceExists)
			{
				int towerLibraryCount = LevelManager.instance.towerLibrary.Count;

				// find the lowest value between 9 (keyboard numbers)
				// and the amount of towers in the library
				int count = Mathf.Min(9, towerLibraryCount);
				KeyCode highestKey = KeyCode.Alpha1 + count;

				for (var key = KeyCode.Alpha1; key < highestKey; key++)
				{
					// add offset for the KeyCode Alpha 1 index to find correct keycodes
					if (UnityInput.GetKeyDown(key))
					{
						Tower controller = LevelManager.instance.towerLibrary[key - KeyCode.Alpha1];
						if (LevelManager.instance.currency.CanAfford(controller.purchaseCost))
						{
							if (m_GameUI.isBuilding)
							{
								m_GameUI.CancelGhostPlacement();
							}
							GameUI.instance.SetToBuildMode(controller);
							GameUI.instance.TryMoveGhost(InputController.instance.basicMouseInfo);
						}
						break;
					}
				}

				// special case for 0 mapping to index 9
				if (count < 10 && UnityInput.GetKeyDown(KeyCode.Alpha0))
				{
					Tower controller = LevelManager.instance.towerLibrary[9];
					GameUI.instance.SetToBuildMode(controller);
					GameUI.instance.TryMoveGhost(InputController.instance.basicMouseInfo);
				}
			}
		}

		/// <summary>
		/// Ghost follows pointer
		/// </summary>
		void OnMouseMoved(PointerInfo pointer)
		{
			// We only respond to mouse info
			var mouseInfo = pointer as MouseCursorInfo;

			if ((mouseInfo != null) && (m_GameUI.isBuilding))
			{
				m_GameUI.TryMoveGhost(pointer, false);
			}
		}

		/// <summary>
		/// Select towers or position ghosts
		/// </summary>
		void OnTap(PointerActionInfo pointer)
		{
			// We only respond to mouse info
			var mouseInfo = pointer as MouseButtonInfo;

			if (mouseInfo != null && !mouseInfo.startedOverUI)
			{
				if (m_GameUI.isBuilding)
				{
					if (mouseInfo.mouseButtonId == 0) // LMB confirms
					{
						m_GameUI.TryPlaceTower(pointer);
					}
					else // RMB cancels
					{
						m_GameUI.CancelGhostPlacement();
					}
				}
				else
				{
					if (mouseInfo.mouseButtonId == 0)
					{
						// select towers
						m_GameUI.TrySelectTower(pointer);
					}
				}
			}
		}
	}
}