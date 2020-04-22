using Core.Input;
using TowerDefense.UI;
using TowerDefense.UI.HUD;
using UnityEngine;
using UnityInput = UnityEngine.Input;
using State = TowerDefense.UI.HUD.GameUI.State;

namespace TowerDefense.Input
{
	[RequireComponent(typeof(GameUI))]
	public class TowerDefenseTouchInput : TouchInput
	{
		/// <summary>
		/// A percentage of the screen where panning occurs while dragging
		/// </summary>
		[Range(0, 0.5f)]
		public float panAreaScreenPercentage = 0.2f;

		/// <summary>
		/// The object that holds the confirmation buttons
		/// </summary>
		public MovingCanvas confirmationButtons;

		/// <summary>
		/// The object that holds the invalid selection
		/// </summary>
		public MovingCanvas invalidButtons;

		/// <summary>
		/// The attached Game UI object
		/// </summary>
		GameUI m_GameUI;

		/// <summary>
		/// Keeps track of whether or not the ghost tower is selected
		/// </summary>
		bool m_IsGhostSelected;

		/// <summary>
		/// The pointer at the edge of the screen
		/// </summary>
		TouchInfo m_DragPointer;

		/// <summary>
		/// Called by the confirm button on the UI
		/// </summary>
		public void OnTowerPlacementConfirmation()
		{
			confirmationButtons.canvasEnabled = false;
			if (!m_GameUI.IsGhostAtValidPosition())
			{
				return;
			}
			m_GameUI.BuyTower();
		}

		/// <summary>
		/// Called by the close button on the UI
		/// </summary>
		public void Cancel()
		{
			GameUI.instance.CancelGhostPlacement();
			confirmationButtons.canvasEnabled = false;
			invalidButtons.canvasEnabled = false;
		}

		/// <summary>
		/// Register input events
		/// </summary>
		protected override void OnEnable()
		{
			base.OnEnable();
			
			m_GameUI = GetComponent<GameUI>();
			
			m_GameUI.stateChanged += OnStateChanged;
			m_GameUI.ghostBecameValid += OnGhostBecameValid;

			// Register tap event
			if (InputController.instanceExists)
			{
				InputController.instance.tapped += OnTap;
				InputController.instance.startedDrag += OnStartDrag;
			}

			// disable pop ups
			confirmationButtons.canvasEnabled = false;
			invalidButtons.canvasEnabled = false;
		
		}

		/// <summary>
		/// Deregister input events
		/// </summary>
		protected override void OnDisable()
		{
			base.OnDisable();
			
			if (confirmationButtons != null)
			{
				confirmationButtons.canvasEnabled = false;
			}
			if (invalidButtons != null)
			{
				invalidButtons.canvasEnabled = false;
			}
			if (InputController.instanceExists)
			{
				InputController.instance.tapped -= OnTap;
				InputController.instance.startedDrag -= OnStartDrag;
			}
			if (m_GameUI != null)
			{
				m_GameUI.stateChanged -= OnStateChanged;
				m_GameUI.ghostBecameValid -= OnGhostBecameValid;
			}
		}

		/// <summary>
		/// Hide UI 
		/// </summary>
		protected virtual void Awake()
		{
			if (confirmationButtons != null)
			{
				confirmationButtons.canvasEnabled = false;
			}
			if (invalidButtons != null)
			{
				invalidButtons.canvasEnabled = false;
			}
		}

		/// <summary>
		/// Decay flick
		/// </summary>
		protected override void Update()
		{
			base.Update();

			// Edge pan
			if (m_DragPointer != null)
			{
				EdgePan();
			}

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
					case State.Building:
						m_GameUI.CancelGhostPlacement();
						break;
				}
			}
		}

		/// <summary>
		/// Called on input press
		/// </summary>
		protected override void OnPress(PointerActionInfo pointer)
		{
			base.OnPress(pointer);
			var touchInfo = pointer as TouchInfo;
			// Press starts on a ghost? Then we can pick it up
			if (touchInfo != null)
			{
				if (m_GameUI.state == State.Building)
				{
					m_IsGhostSelected = m_GameUI.IsPointerOverGhost(pointer);
					if (m_IsGhostSelected)
					{
						m_DragPointer = touchInfo;
					}
				}				
			}
		}

		/// <summary>
		/// Called on input release, for flicks
		/// </summary>
		protected override void OnRelease(PointerActionInfo pointer)
		{
			// Override normal behaviour. We only want to do flicks if there's no ghost selected
			// For this reason, we intentionally do not call base
			var touchInfo = pointer as TouchInfo;

			if (touchInfo != null)
			{
				// Show UI on release
				if (m_GameUI.isBuilding)
				{
					Vector2 screenPoint = cameraRig.cachedCamera.WorldToScreenPoint(m_GameUI.GetGhostPosition());
					if (m_GameUI.IsGhostAtValidPosition() && m_GameUI.IsValidPurchase())
					{
						confirmationButtons.canvasEnabled = true;
						invalidButtons.canvasEnabled = false;
						confirmationButtons.TryMove(screenPoint);
					}
					else
					{
						invalidButtons.canvasEnabled = true;
						confirmationButtons.canvasEnabled = false;
						confirmationButtons.TryMove(screenPoint);
					}
					if (m_IsGhostSelected)
					{
						m_GameUI.ReturnToBuildMode();
					}
				}
				if (!m_IsGhostSelected && cameraRig != null)
				{
					// Do normal base behaviour here
					DoReleaseFlick(pointer);
				}
				
				m_IsGhostSelected = false;

				// Reset m_DragPointer if released
				if (m_DragPointer != null && m_DragPointer.touchId == touchInfo.touchId)
				{
					m_DragPointer = null;
				}
			}
		}

		/// <summary>
		/// Called on tap,
		/// calls confirmation of tower placement
		/// </summary>
		protected virtual void OnTap(PointerActionInfo pointerActionInfo)
		{
			var touchInfo = pointerActionInfo as TouchInfo;
			if (touchInfo != null)
			{
				if (m_GameUI.state == State.Normal && !touchInfo.startedOverUI)
				{
					m_GameUI.TrySelectTower(touchInfo);
				}
				else if (m_GameUI.state == State.Building && !touchInfo.startedOverUI)
				{
					m_GameUI.TryMoveGhost(touchInfo, false);
					if (m_GameUI.IsGhostAtValidPosition() && m_GameUI.IsValidPurchase())
					{
						confirmationButtons.canvasEnabled = true;
						invalidButtons.canvasEnabled = false;
						confirmationButtons.TryMove(touchInfo.currentPosition);
					}
					else
					{
						invalidButtons.canvasEnabled = true;
						invalidButtons.TryMove(touchInfo.currentPosition);
						confirmationButtons.canvasEnabled = false;
					}
				}
			}
		}

		/// <summary>
		/// Assigns the drag pointer and sets the UI into drag mode
		/// </summary>
		/// <param name="pointer"></param>
		protected virtual void OnStartDrag(PointerActionInfo pointer)
		{
			var touchInfo = pointer as TouchInfo;
			if (touchInfo != null)
			{
				if (m_IsGhostSelected)
				{
					m_GameUI.ChangeToDragMode();
					m_DragPointer = touchInfo;
				}
			}
		}
		

		/// <summary>
		/// Called when we drag
		/// </summary>
		protected override void OnDrag(PointerActionInfo pointer)
		{
			// Override normal behaviour. We only want to pan if there's no ghost selected
			// For this reason, we intentionally do not call base
			var touchInfo = pointer as TouchInfo;
			if (touchInfo != null)
			{
				// Try to pick up the tower if it was dragged off
				if (m_IsGhostSelected)
				{
					m_GameUI.TryMoveGhost(pointer, false);
				}
				
				if (m_GameUI.state == State.BuildingWithDrag)
				{
					DragGhost(touchInfo);
				}
				else
				{
					// Do normal base behaviour only if no ghost selected
					if (cameraRig != null)
					{
						DoDragPan(pointer);

						if (invalidButtons.canvasEnabled)
						{
							invalidButtons.TryMove(cameraRig.cachedCamera.WorldToScreenPoint(m_GameUI.GetGhostPosition()));
						}
						if (confirmationButtons.canvasEnabled)
						{
							confirmationButtons.TryMove(cameraRig.cachedCamera.WorldToScreenPoint(m_GameUI.GetGhostPosition()));
						}
					}
				}
			}
		}

		/// <summary>
		/// Drags the ghost
		/// </summary>
		void DragGhost(TouchInfo touchInfo)
		{
			if (touchInfo.touchId == m_DragPointer.touchId)
			{
				m_GameUI.TryMoveGhost(touchInfo, false);

				if (invalidButtons.canvasEnabled)
				{
					invalidButtons.canvasEnabled = false;
				}
				if (confirmationButtons.canvasEnabled)
				{
					confirmationButtons.canvasEnabled = false;
				}
			}
		}

		/// <summary>
		/// pans at the edge of the screen
		/// </summary>
		void EdgePan()
		{
			float edgeWidth = panAreaScreenPercentage * Screen.width;
			PanWithScreenCoordinates(m_DragPointer.currentPosition, edgeWidth, panSpeed);
		}
		

		/// <summary>
		/// If the new state is <see cref="GameUI.State.Building"/> then move the ghost to the center of the screen
		/// </summary>
		/// <param name="previousState">
		/// The previous the GameUI was is in
		/// </param>
		/// <param name="currentState">
		/// The new state the GameUI is in
		/// </param>
		void OnStateChanged(State previousState, State currentState)
		{
			// Early return for two reasons
			// 1. We are not moving into Build Mode
			// 2. We are not actually touching
			if (UnityInput.touchCount == 0)
			{
				return;
			}
			if (currentState == State.Building && previousState != State.BuildingWithDrag)
			{
				m_GameUI.MoveGhostToCenter();
				confirmationButtons.canvasEnabled = false;
				invalidButtons.canvasEnabled = false;
			}
			if (currentState == State.BuildingWithDrag)
			{
				m_IsGhostSelected = true;
			}
		}

		/// <summary>
		/// Displays the correct confirmation buttons when the tower has become valid
		/// </summary>
		void OnGhostBecameValid()
		{
			// this only needs to be done if the invalid buttons are already on screen
			if (!invalidButtons.canvasEnabled)
			{
				return;
			}
			Vector2 screenPoint = cameraRig.cachedCamera.WorldToScreenPoint(m_GameUI.GetGhostPosition());
			if (!confirmationButtons.canvasEnabled)
			{
				confirmationButtons.canvasEnabled = true;
				invalidButtons.canvasEnabled = false;
				confirmationButtons.TryMove(screenPoint);
			}
		}
	}
}