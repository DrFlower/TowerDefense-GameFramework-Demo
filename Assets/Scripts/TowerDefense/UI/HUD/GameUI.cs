using System;
using Core.Health;
using Core.Input;
using Core.Utilities;
using JetBrains.Annotations;
using TowerDefense.Level;
using TowerDefense.Towers;
using TowerDefense.Towers.Placement;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TowerDefense.UI.HUD
{
	/// <summary>
	/// A game UI wrapper for a pointer that also contains raycast information
	/// </summary>
	public struct UIPointer
	{
		/// <summary>
		/// The pointer info
		/// </summary>
		public PointerInfo pointer;

		/// <summary>
		/// The ray for this pointer
		/// </summary>
		public Ray ray;

		/// <summary>
		/// The raycast hit object into the 3D scene
		/// </summary>
		public RaycastHit? raycast;

		/// <summary>
		/// True if this pointer started over a UI element or anything the event system catches
		/// </summary>
		public bool overUI;
	}

	/// <summary>
	/// An object that manages user interaction with the game. Its responsibilities deal with
	/// <list type="bullet">
	///     <item>
	///         <description>Building towers</description>
	///     </item>
	///     <item>
	///         <description>Selecting towers and units</description>
	///     </item>
	/// </list>
	/// </summary>
	[RequireComponent(typeof(Camera))]
	public class GameUI : Singleton<GameUI>
	{
		/// <summary>
		/// The states the UI can be in
		/// </summary>
		public enum State
		{
			/// <summary>
			/// The game is in its normal state. Here the player can pan the camera, select units and towers
			/// </summary>
			Normal,

			/// <summary>
			/// The game is in 'build mode'. Here the player can pan the camera, confirm or deny placement
			/// </summary>
			Building,

			/// <summary>
			/// The game is Paused. Here, the player can restart the level, or quit to the main menu
			/// </summary>
			Paused,

			/// <summary>
			/// The game is over and the level was failed/completed
			/// </summary>
			GameOver,
			
			/// <summary>
			/// The game is in 'build mode' and the player is dragging the ghost tower
			/// </summary>
			BuildingWithDrag
		}

		/// <summary>
		/// Gets the current UI state
		/// </summary>
		public State state { get; private set; }

		/// <summary>
		/// The currently selected tower
		/// </summary>
		public LayerMask placementAreaMask;

		/// <summary>
		/// The layer for tower selection
		/// </summary>
		public LayerMask towerSelectionLayer;

		/// <summary>
		/// The physics layer for moving the ghost around the world
		/// when the placement is not valid
		/// </summary>
		public LayerMask ghostWorldPlacementMask;

		/// <summary>
		/// The radius of the sphere cast 
		/// for checking ghost placement
		/// </summary>
		public float sphereCastRadius = 1;

		/// <summary>
		/// Component that manages the radius visualizers of ghosts and towers
		/// </summary>
		public RadiusVisualizerController radiusVisualizerController;

		/// <summary>
		/// The UI controller for displaying individual tower data
		/// </summary>
		public TowerUI towerUI;

		/// <summary>
		/// The UI controller for displaying tower information
		/// whilst placing
		/// </summary>
		public BuildInfoUI buildInfoUI;

		/// <summary>
		/// Fires when the <see cref="State"/> changes
		/// should only allow firing when TouchUI is used
		/// </summary>
		public event Action<State, State> stateChanged;

		/// <summary>
		/// Fires off when the ghost was previously not valid but now is due to currency amount change
		/// </summary>
		public event Action ghostBecameValid;

		/// <summary>
		/// Fires when a tower is selected/deselected
		/// </summary>
		public event Action<Tower> selectionChanged;

		/// <summary>
		/// Placement area ghost tower is currently on
		/// </summary>
		IPlacementArea m_CurrentArea;

		/// <summary>
		/// Grid position ghost tower in on
		/// </summary>
		IntVector2 m_GridPosition;

		/// <summary>
		/// Our cached camera reference
		/// </summary>
		Camera m_Camera;

		/// <summary>
		/// Current tower placeholder. Will be null if not in the <see cref="State.Building" /> state.
		/// </summary>
		TowerPlacementGhost m_CurrentTower;

		/// <summary>
		/// Tracks if the ghost is in a valid location and the player can afford it
		/// </summary>
		bool m_GhostPlacementPossible;

		/// <summary>
		/// Gets the current selected tower
		/// </summary>
		public Tower currentSelectedTower { get; private set; }

		/// <summary>
		/// Gets whether a tower has been selected
		/// </summary>
		public bool isTowerSelected
		{
			get { return currentSelectedTower != null; }
		}

		/// <summary>
		/// Gets whether certain build operations are valid
		/// </summary>
		public bool isBuilding
		{
			get
			{
				return state == State.Building || state == State.BuildingWithDrag;
			}
		}

		/// <summary>
		/// Cancel placing the ghost
		/// </summary>
		public void CancelGhostPlacement()
		{
			if (!isBuilding)
			{
				throw new InvalidOperationException("Can't cancel out of ghost placement when not in the building state.");
			}

			if (buildInfoUI != null)
			{
				buildInfoUI.Hide();
			}
			Destroy(m_CurrentTower.gameObject);
			m_CurrentTower = null;
			SetState(State.Normal);
			DeselectTower();
		}

		/// <summary>
		/// Returns the GameUI to dragging mode with the curent tower
		/// </summary>
		/// /// <exception cref="InvalidOperationException">
		/// Throws exception when not in build mode
		/// </exception>
		public void ChangeToDragMode()
		{
			if (!isBuilding)
			{
				throw new InvalidOperationException("Trying to return to Build With Dragging Mode when not in Build Mode");
			}
			SetState(State.BuildingWithDrag);
		}

		/// <summary>
		/// Returns the GameUI to BuildMode with the current tower
		/// </summary>
		/// <exception cref="InvalidOperationException">
		/// Throws exception when not in Drag mode
		/// </exception>
		public void ReturnToBuildMode()
		{
			if (!isBuilding)
			{
				throw new InvalidOperationException("Trying to return to Build Mode when not in Drag Mode");
			}
			SetState(State.Building);
		}

		
		/// <summary>
		/// Changes the state and fires <see cref="stateChanged"/>
		/// </summary>
		/// <param name="newState">The state to change to</param>
		/// <exception cref="ArgumentOutOfRangeException">thrown on an invalid state</exception>
		void SetState(State newState)
		{
			if (state == newState)
			{
				return;
			}
			State oldState = state;
			if (oldState == State.Paused || oldState == State.GameOver)
			{
				Time.timeScale = 1f;
			}

			switch (newState)
			{
				case State.Normal:
					break;
				case State.Building:
					break;
				case State.BuildingWithDrag:
					break;
				case State.Paused:
				case State.GameOver:
					if (oldState == State.Building)
					{
						CancelGhostPlacement();
					}
					Time.timeScale = 0f;
					break;
				default:
					throw new ArgumentOutOfRangeException("newState", newState, null);
			}
			state = newState;
			if (stateChanged != null)
			{
				stateChanged(oldState, state);
			}
		}

		/// <summary>
		/// Called when the game is over
		/// </summary>
		public void GameOver()
		{
			SetState(State.GameOver);
		}

		/// <summary>
		/// Pause the game and display the pause menu
		/// </summary>
		public void Pause()
		{
			SetState(State.Paused);
		}

		/// <summary>
		/// Resume the game and close the pause menu
		/// </summary>
		public void Unpause()
		{
			SetState(State.Normal);
		}
		
		/// <summary>
		/// Changes the mode to drag
		/// </summary>
		/// <param name="towerToBuild">
		/// The tower to build
		/// </param>
		/// <exception cref="InvalidOperationException">
		/// Throws exception when trying to change to Drag mode when not in Normal Mode
		/// </exception>
		public void SetToDragMode([NotNull] Tower towerToBuild)
		{
			if (state != State.Normal)
			{
				throw new InvalidOperationException("Trying to enter drag mode when not in Normal mode");	
			}
			
			if (m_CurrentTower != null)
			{
				// Destroy current ghost
				CancelGhostPlacement();
			}
			SetUpGhostTower(towerToBuild);
			SetState(State.BuildingWithDrag);
		}

		/// <summary>
		/// Sets the UI into a build state for a given tower
		/// </summary>
		/// <param name="towerToBuild">
		/// The tower to build
		/// </param>
		/// <exception cref="InvalidOperationException">
		/// Throws exception trying to enter Build Mode when not in Normal Mode
		/// </exception>
		public void SetToBuildMode([NotNull] Tower towerToBuild)
		{
			if (state != State.Normal)
			{
				throw new InvalidOperationException("Trying to enter Build mode when not in Normal mode");
			}
			
			if (m_CurrentTower != null)
			{
				// Destroy current ghost
				CancelGhostPlacement();
			}
			SetUpGhostTower(towerToBuild);
			SetState(State.Building);
		}

		/// <summary>
		/// Attempt to position a tower at the given location
		/// </summary>
		/// <param name="pointerInfo">The pointer we're using to position the tower</param>
		public void TryPlaceTower(PointerInfo pointerInfo)
		{
			UIPointer pointer = WrapPointer(pointerInfo);

			// Do nothing if we're over UI
			if (pointer.overUI)
			{
				return;
			}
			BuyTower(pointer);
		}

		/// <summary>
		/// Position the ghost tower at the given pointer
		/// </summary>
		/// <param name="pointerInfo">The pointer we're using to position the tower</param>
		/// <param name="hideWhenInvalid">Optional parameter for configuring if the ghost is hidden when in an invalid location</param>
		public void TryMoveGhost(PointerInfo pointerInfo, bool hideWhenInvalid = true)
		{
			if (m_CurrentTower == null)
			{
				throw new InvalidOperationException("Trying to move the tower ghost when we don't have one");
			}

			UIPointer pointer = WrapPointer(pointerInfo);
			// Do nothing if we're over UI
			if (pointer.overUI && hideWhenInvalid)
			{
				m_CurrentTower.Hide();
				return;
			}
			MoveGhost(pointer, hideWhenInvalid);
		}

		/// <summary>
		/// Sets up the radius visualizer for a tower or ghost tower
		/// </summary>
		public void SetupRadiusVisualizer(Tower tower, Transform ghost = null)
		{
			radiusVisualizerController.SetupRadiusVisualizers(tower, ghost);
		}

		/// <summary>
		/// Hides the radius visualizer
		/// </summary>
		public void HideRadiusVisualizer()
		{
			radiusVisualizerController.HideRadiusVisualizers();
		}

		/// <summary>
		/// Activates the tower controller UI with the specific information
		/// </summary>
		/// <param name="tower">
		/// The tower controller information to use
		/// </param>
		/// <exception cref="InvalidOperationException">
		/// Throws exception when selecting tower when <see cref="State" /> does not equal <see cref="State.Normal" />
		/// </exception>
		public void SelectTower(Tower tower)
		{
			if (state != State.Normal)
			{
				throw new InvalidOperationException("Trying to select whilst not in a normal state");
			}
			DeselectTower();
			currentSelectedTower = tower;
			if (currentSelectedTower != null)
			{
				currentSelectedTower.removed += OnTowerDied;
			}
			radiusVisualizerController.SetupRadiusVisualizers(tower);

			if (selectionChanged != null)
			{
				selectionChanged(tower);
			}
		}

		/// <summary>
		/// Upgrades <see cref="currentSelectedTower" />, if possible
		/// </summary>
		/// <exception cref="InvalidOperationException">
		/// Throws exception when selecting tower when <see cref="State" /> does not equal <see cref="State.Normal" />
		/// or <see cref="currentSelectedTower" /> is null
		/// </exception>
		public void UpgradeSelectedTower()
		{
			if (state != State.Normal)
			{
				throw new InvalidOperationException("Trying to upgrade whilst not in Normal state");
			}
			if (currentSelectedTower == null)
			{
				throw new InvalidOperationException("Selected Tower is null");
			}
			if (currentSelectedTower.isAtMaxLevel)
			{
				return;
			}
			int upgradeCost = currentSelectedTower.GetCostForNextLevel();
			bool successfulUpgrade = LevelManager.instance.currency.TryPurchase(upgradeCost);
			if (successfulUpgrade)
			{
				currentSelectedTower.UpgradeTower();
			}
			towerUI.Hide();
			DeselectTower();
		}

		/// <summary>
		/// Sells <see cref="currentSelectedTower" /> if possible
		/// </summary>
		/// <exception cref="InvalidOperationException">
		/// Throws exception when selecting tower when <see cref="State" /> does not equal <see cref="State.Normal" />
		/// or <see cref="currentSelectedTower" /> is null
		/// </exception>
		public void SellSelectedTower()
		{
			if (state != State.Normal)
			{
				throw new InvalidOperationException("Trying to sell tower whilst not in Normal state");
			}
			if (currentSelectedTower == null)
			{
				throw new InvalidOperationException("Selected Tower is null");
			}
			int sellValue = currentSelectedTower.GetSellLevel();
			if (LevelManager.instanceExists && sellValue > 0)
			{
				LevelManager.instance.currency.AddCurrency(sellValue);
				currentSelectedTower.Sell();
			}
			DeselectTower();
		}

		/// <summary>
		/// Buys the tower and places it in the place that it currently is
		/// </summary>
		/// <exception cref="InvalidOperationException">
		/// Throws exception if trying to buy towers in Build Mode
		/// </exception>
		public void BuyTower()
		{
			if (!isBuilding)
			{
				throw new InvalidOperationException("Trying to buy towers when not in Build Mode");
			}
			if (m_CurrentTower == null || !IsGhostAtValidPosition())
			{
				return;
			}
			int cost = m_CurrentTower.controller.purchaseCost;
			bool successfulPurchase = LevelManager.instance.currency.TryPurchase(cost);
			if (successfulPurchase)
			{
				PlaceTower();
			}
		}

		/// <summary>
		/// Used to buy the tower during the build phase
		/// Checks currency and calls <see cref="PlaceGhost" />
		/// <exception cref="InvalidOperationException">
		/// Throws exception when not in a build mode or when tower is not a valid position
		/// </exception>
		/// </summary>
		public void BuyTower(UIPointer pointer)
		{
			if (!isBuilding)
			{
				throw new InvalidOperationException("Trying to buy towers when not in a Build Mode");
			}
			if (m_CurrentTower == null || !IsGhostAtValidPosition())
			{
				return;
			}
			PlacementAreaRaycast(ref pointer);
			if (!pointer.raycast.HasValue || pointer.raycast.Value.collider == null)
			{
				CancelGhostPlacement();
				return;
			}
			int cost = m_CurrentTower.controller.purchaseCost;
			bool successfulPurchase = LevelManager.instance.currency.TryPurchase(cost);
			if (successfulPurchase)
			{
				PlaceGhost(pointer);
			}
		}

		/// <summary>
		/// Deselect the current tower and hides the UI
		/// </summary>
		public void DeselectTower()
		{
			if (state != State.Normal)
			{
				throw new InvalidOperationException("Trying to deselect tower whilst not in Normal state");
			}
			if (currentSelectedTower != null)
			{
				currentSelectedTower.removed -= OnTowerDied;
			}

			currentSelectedTower = null;

			if (selectionChanged != null)
			{
				selectionChanged(null);
			}
		}

		/// <summary>
		/// Checks the position of the <see cref="m_CurrentTower"/> 
		/// on the <see cref="m_CurrentArea"/>
		/// </summary>
		/// <returns>
		/// True if the placement is valid
		/// </returns>
		/// <exception cref="InvalidOperationException">
		/// Throws exception if the check is done in <see cref="State.Normal"/> state
		/// </exception>
		public bool IsGhostAtValidPosition()
		{
			if (!isBuilding)
			{
				throw new InvalidOperationException("Trying to check ghost position when not in a build mode");
			}
			if (m_CurrentTower == null)
			{
				return false;
			}
			if (m_CurrentArea == null)
			{
				return false;
			}
			TowerFitStatus fits = m_CurrentArea.Fits(m_GridPosition, m_CurrentTower.controller.dimensions);
			return fits == TowerFitStatus.Fits;
		}

		/// <summary>
		/// Checks if buying the ghost tower is possible
		/// </summary>
		/// <returns>
		/// True if can purchase
		/// </returns>
		/// <exception cref="InvalidOperationException">
		/// Throws exception if not in Build Mode or Build With Dragging mode
		/// </exception>
		public bool IsValidPurchase()
		{
			if (!isBuilding)
			{
				throw new InvalidOperationException("Trying to check ghost position when not in a build mode");
			}
			if (m_CurrentTower == null)
			{
				return false;
			}
			if (m_CurrentArea == null)
			{
				return false;
			}
			return LevelManager.instance.currency.CanAfford(m_CurrentTower.controller.purchaseCost);
		}

		/// <summary>
		/// Places a tower where the ghost tower is
		/// </summary>
		/// <exception cref="InvalidOperationException">
		/// Throws exception if not in Build State or <see cref="m_CurrentTower"/> is not at a valid position
		/// </exception>
		public void PlaceTower()
		{
			if ( !isBuilding )
			{
				throw new InvalidOperationException("Trying to place tower when not in a Build Mode");
			}
			if (!IsGhostAtValidPosition())
			{
				throw new InvalidOperationException("Trying to place tower on an invalid area");
			}
			if (m_CurrentArea == null)
			{
				return;
			}
			Tower createdTower = Instantiate(m_CurrentTower.controller);
			createdTower.Initialize(m_CurrentArea, m_GridPosition);

			CancelGhostPlacement();
		}

		/// <summary>
		/// Calculates whether the given pointer is over the current tower ghost
		/// </summary>
		/// <param name="pointerInfo">
		/// The information used to check against the <see cref="m_CurrentTower"/>
		/// </param>
		/// <exception cref="InvalidOperationException">
		/// Throws an exception if not in Build Mode
		/// </exception>
		public bool IsPointerOverGhost(PointerInfo pointerInfo)
		{
			if (state != State.Building)
			{
				throw new InvalidOperationException("Trying to tap on ghost tower when not in Build Mode");
			}
			UIPointer uiPointer = WrapPointer(pointerInfo);
			RaycastHit hit;
			return m_CurrentTower.ghostCollider.Raycast(uiPointer.ray, out hit, float.MaxValue);
		}

		/// <summary>
		/// Selects a tower beneath the given pointer if there is one
		/// </summary>
		/// <param name="info">
		/// The pointer information concerning the selector of the pointer
		/// </param>
		/// <exception cref="InvalidOperationException">
		/// Throws an exception when not in <see cref="State.Normal"/>
		/// </exception>
		public void TrySelectTower(PointerInfo info)
		{
			if (state != State.Normal)
			{
				throw new InvalidOperationException("Trying to select towers outside of Normal state");
			}
			UIPointer uiPointer = WrapPointer(info);
			RaycastHit output;
			bool hasHit = Physics.Raycast(uiPointer.ray, out output, float.MaxValue, towerSelectionLayer);
			if (!hasHit || uiPointer.overUI)
			{
				return;
			}
			var controller = output.collider.GetComponent<Tower>();
			if (controller != null)
			{
				SelectTower(controller);
			}
		}

		/// <summary>
		/// Gets the world position of the ghost tower
		/// </summary>
		/// <exception cref="InvalidOperationException">
		/// Throws an exception when not in the Build Mode or
		/// When a ghost tower does not exist
		/// </exception>
		public Vector3 GetGhostPosition()
		{
			if (!isBuilding)
			{
				throw new InvalidOperationException("Trying to get ghost position when not in a Build Mode");
			}
			if (m_CurrentTower == null)
			{
				throw new InvalidOperationException("Trying to get ghost position for an object that does not exist");
			}
			return m_CurrentTower.transform.position;
		}

		/// <summary>
		/// Moves the ghost to the center of the screen
		/// </summary>
		/// <exception cref="InvalidOperationException">
		/// Throws exception when not in build mode
		/// </exception>
		public void MoveGhostToCenter()
		{
			if (state != State.Building)
			{
				throw new InvalidOperationException("Trying to move ghost when not in Build Mode");
			}
			// try to find a valid placement 
			Ray ray = m_Camera.ScreenPointToRay(new Vector2(Screen.width * 0.5f, Screen.height * 0.5f));
			RaycastHit placementHit;

			if (Physics.SphereCast(ray, sphereCastRadius, out placementHit, float.MaxValue, placementAreaMask))
			{
				MoveGhostWithRaycastHit(placementHit);
			}
			else
			{
				MoveGhostOntoWorld(ray, false);
			}
		}

		/// <summary>
		/// Set initial values, cache attached components
		/// and configure the controls
		/// </summary>
		protected override void Awake()
		{
			base.Awake();

			state = State.Normal;
			m_Camera = GetComponent<Camera>();
		}

		/// <summary>
		/// Reset TimeScale if game is paused
		/// </summary>
		protected override void OnDestroy()
		{
			base.OnDestroy();
			Time.timeScale = 1f;
		}

		/// <summary>
		/// Subscribe to the level manager
		/// </summary>
		protected virtual void OnEnable()
		{
			if (LevelManager.instanceExists)
			{
				LevelManager.instance.currency.currencyChanged += OnCurrencyChanged;
			}
		}

		/// <summary>
		/// Unsubscribe from the level manager
		/// </summary>
		protected virtual void OnDisable()
		{
			if (LevelManager.instanceExists)
			{
				LevelManager.instance.currency.currencyChanged -= OnCurrencyChanged;
			}
		}

		/// <summary>
		/// Creates a new UIPointer holding data object for the given pointer position
		/// </summary>
		protected UIPointer WrapPointer(PointerInfo pointerInfo)
		{
			return new UIPointer
			{
				overUI = IsOverUI(pointerInfo),
				pointer = pointerInfo,
				ray = m_Camera.ScreenPointToRay(pointerInfo.currentPosition)
			};
		}

		/// <summary>
		/// Checks whether a given pointer is over any UI
		/// </summary>
		/// <param name="pointerInfo">The pointer to test</param>
		/// <returns>True if the event system reports this pointer being over UI</returns>
		protected bool IsOverUI(PointerInfo pointerInfo)
		{
			int pointerId;
			EventSystem currentEventSystem = EventSystem.current;

			// Pointer id is negative for mouse, positive for touch
			var cursorInfo = pointerInfo as MouseCursorInfo;
			var mbInfo = pointerInfo as MouseButtonInfo;
			var touchInfo = pointerInfo as TouchInfo;

			if (cursorInfo != null)
			{
				pointerId = PointerInputModule.kMouseLeftId;
			}
			else if (mbInfo != null)
			{
				// LMB is 0, but kMouseLeftID = -1;
				pointerId = -mbInfo.mouseButtonId - 1;
			}
			else if (touchInfo != null)
			{
				pointerId = touchInfo.touchId;
			}
			else
			{
				throw new ArgumentException("Passed pointerInfo is not a TouchInfo or MouseCursorInfo", "pointerInfo");
			}

			return currentEventSystem.IsPointerOverGameObject(pointerId);
		}

		/// <summary>
		/// Move the ghost to the pointer's position
		/// </summary>
		/// <param name="pointer">The pointer to place the ghost at</param>
		/// <param name="hideWhenInvalid">Optional parameter for whether the ghost should be hidden or not</param>
		/// <exception cref="InvalidOperationException">If we're not in the correct state</exception>
		protected void MoveGhost(UIPointer pointer, bool hideWhenInvalid = true)
		{
			if (m_CurrentTower == null || !isBuilding)
			{
				throw new InvalidOperationException(
					"Trying to position a tower ghost while the UI is not currently in the building state.");
			}

			// Raycast onto placement layer
			PlacementAreaRaycast(ref pointer);

			if (pointer.raycast != null)
			{
				MoveGhostWithRaycastHit(pointer.raycast.Value);
			}
			else
			{
				MoveGhostOntoWorld(pointer.ray, hideWhenInvalid);
			}
		}


		/// <summary>
		/// Move ghost with successful raycastHit onto m_PlacementAreaMask
		/// </summary>
		protected virtual void MoveGhostWithRaycastHit(RaycastHit raycast)
		{
			// We successfully hit one of our placement areas
			// Try and get a placement area on the object we hit
			m_CurrentArea = raycast.collider.GetComponent<IPlacementArea>();

			if (m_CurrentArea == null)
			{
				Debug.LogError("There is not an IPlacementArea attached to the collider found on the m_PlacementAreaMask");
				return;
			}
			m_GridPosition = m_CurrentArea.WorldToGrid(raycast.point, m_CurrentTower.controller.dimensions);
			TowerFitStatus fits = m_CurrentArea.Fits(m_GridPosition, m_CurrentTower.controller.dimensions);

			m_CurrentTower.Show();
			m_GhostPlacementPossible = fits == TowerFitStatus.Fits && IsValidPurchase();
			m_CurrentTower.Move(m_CurrentArea.GridToWorld(m_GridPosition, m_CurrentTower.controller.dimensions),
								m_CurrentArea.transform.rotation,
								m_GhostPlacementPossible);
		}


		/// <summary>
		/// Move ghost with the given ray
		/// </summary>
		protected virtual void MoveGhostOntoWorld(Ray ray, bool hideWhenInvalid)
		{
			m_CurrentArea = null;

			if (!hideWhenInvalid)
			{
				RaycastHit hit;
				// check against all layers that the ghost can be on
				Physics.SphereCast(ray, sphereCastRadius, out hit, float.MaxValue, ghostWorldPlacementMask);
				if (hit.collider == null)
				{
					return;
				}
				m_CurrentTower.Show();
				m_CurrentTower.Move(hit.point, hit.collider.transform.rotation, false);
			}
			else
			{
				m_CurrentTower.Hide();
			}
		}

		/// <summary>
		/// Place the ghost at the pointer's position
		/// </summary>
		/// <param name="pointer">The pointer to place the ghost at</param>
		/// <exception cref="InvalidOperationException">If we're not in the correct state</exception>
		protected void PlaceGhost(UIPointer pointer)
		{
			if (m_CurrentTower == null || !isBuilding)
			{
				throw new InvalidOperationException(
					"Trying to position a tower ghost while the UI is not currently in a building state.");
			}

			MoveGhost(pointer);

			if (m_CurrentArea != null)
			{
				TowerFitStatus fits = m_CurrentArea.Fits(m_GridPosition, m_CurrentTower.controller.dimensions);

				if (fits == TowerFitStatus.Fits)
				{
					// Place the ghost
					Tower controller = m_CurrentTower.controller;

					Tower createdTower = Instantiate(controller);
					createdTower.Initialize(m_CurrentArea, m_GridPosition);

					CancelGhostPlacement();
				}
			}
		}

		/// <summary>
		/// Raycast onto tower placement areas
		/// </summary>
		/// <param name="pointer">The pointer we're testing</param>
		protected void PlacementAreaRaycast(ref UIPointer pointer)
		{
			pointer.raycast = null;

			if (pointer.overUI)
			{
				// Pointer is over UI, so no valid position
				return;
			}

			// Raycast onto placement area layer
			RaycastHit hit;
			if (Physics.Raycast(pointer.ray, out hit, float.MaxValue, placementAreaMask))
			{
				pointer.raycast = hit;
			}
		}

		/// <summary>
		/// Modifies the valid rendering of the ghost tower once there is enough currency
		/// </summary>
		protected virtual void OnCurrencyChanged()
		{
			if (!isBuilding || m_CurrentTower == null || m_CurrentArea == null)
			{
				return;
			}
			TowerFitStatus fits = m_CurrentArea.Fits(m_GridPosition, m_CurrentTower.controller.dimensions);
			bool valid = fits == TowerFitStatus.Fits && IsValidPurchase();
			m_CurrentTower.Move(m_CurrentArea.GridToWorld(m_GridPosition, m_CurrentTower.controller.dimensions),
			                    m_CurrentArea.transform.rotation,
			                    valid);
			if (valid && !m_GhostPlacementPossible && ghostBecameValid != null)
			{
				m_GhostPlacementPossible = true;
				ghostBecameValid();
			}
		}

		/// <summary>
		/// Closes the Tower UI on death of tower
		/// </summary>
		protected void OnTowerDied(DamageableBehaviour targetable)
		{
			towerUI.enabled = false;
			radiusVisualizerController.HideRadiusVisualizers();
			DeselectTower();
		}
		
		/// <summary>
		/// Creates and hides the tower and shows the buildInfoUI
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// Throws exception if the <paramref name="towerToBuild"/> is null
		/// </exception>
		void SetUpGhostTower([NotNull] Tower towerToBuild)
		{
			if (towerToBuild == null)
			{
				throw new ArgumentNullException("towerToBuild");
			}

			m_CurrentTower = Instantiate(towerToBuild.towerGhostPrefab);
			m_CurrentTower.Initialize(towerToBuild);
			m_CurrentTower.Hide();

			//activate build info
			if (buildInfoUI != null)
			{
				buildInfoUI.Show(towerToBuild);
			}
		}
	}
}