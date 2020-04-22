using System;
using System.Collections.Generic;
using System.Linq;
using Core.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;
using Debug = System.Diagnostics.Debug;
using UnityInput = UnityEngine.Input;

namespace Core.Input
{
	/// <summary>
	/// Class to manage tap/drag/pinch gestures and other controls
	/// </summary>
	public class InputController : Singleton<InputController>
	{
		/// <summary>
		/// How quickly flick velocity is accumulated with movements
		/// </summary>
		const float k_FlickAccumulationFactor = 0.8f;

		/// <summary>
		/// How far fingers must move before starting a drag
		/// </summary>
		public float dragThresholdTouch = 5;

		/// <summary>
		/// How far mouse must move before starting a drag
		/// </summary>
		public float dragThresholdMouse;

		/// <summary>
		/// How long before a touch can no longer be considered a tap
		/// </summary>
		public float tapTime = 0.2f;

		/// <summary>
		/// How long before a touch is considered a hold
		/// </summary>
		public float holdTime = 0.8f;

		/// <summary>
		/// Sensitivity of mouse-wheel based zoom
		/// </summary>
		public float mouseWheelSensitivity = 1.0f;

		/// <summary>
		/// How many mouse buttons to track
		/// </summary>
		public int trackMouseButtons = 2;

		/// <summary>
		/// Flick movement threshold
		/// </summary>
		public float flickThreshold = 2f;

		/// <summary>
		/// All the touches we're tracking
		/// </summary>
		List<TouchInfo> m_Touches;

		/// <summary>
		/// Mouse button info
		/// </summary>
		List<MouseButtonInfo> m_MouseInfo;

		/// <summary>
		/// Gets the number of active touches
		/// </summary>
		public int activeTouchCount
		{
			get { return m_Touches.Count; }
		}

		/// <summary>
		/// Tracks if any of the mouse buttons were pressed this frame
		/// </summary>
		public bool mouseButtonPressedThisFrame { get; private set; }

		/// <summary>
		/// Tracks if the mouse moved this frame
		/// </summary>
		public bool mouseMovedOnThisFrame { get; private set; }

		/// <summary>
		/// Tracks if a touch began this frame
		/// </summary>
		public bool touchPressedThisFrame { get; private set; }

		/// <summary>
		/// Current mouse pointer info
		/// </summary>
		public PointerInfo basicMouseInfo { get; private set; }

		/// <summary>
		/// Event called when a pointer press is detected
		/// </summary>
		public event Action<PointerActionInfo> pressed;

		/// <summary>
		/// Event called when a pointer is released
		/// </summary>
		public event Action<PointerActionInfo> released;

		/// <summary>
		/// Event called when a pointer is tapped
		/// </summary>
		public event Action<PointerActionInfo> tapped;

		/// <summary>
		/// Event called when a drag starts
		/// </summary>
		public event Action<PointerActionInfo> startedDrag;

		/// <summary>
		/// Event called when a pointer is dragged
		/// </summary>
		public event Action<PointerActionInfo> dragged;

		/// <summary>
		/// Event called when a pointer starts a hold
		/// </summary>
		public event Action<PointerActionInfo> startedHold;

		/// <summary>
		/// Event called when the user scrolls the mouse wheel
		/// </summary>
		public event Action<WheelInfo> spunWheel;

		/// <summary>
		/// Event called when the user performs a pinch gesture
		/// </summary>
		public event Action<PinchInfo> pinched;

		/// <summary>
		/// Event called whenever the mouse is moved
		/// </summary>
		public event Action<PointerInfo> mouseMoved;

		protected override void Awake()
		{
			base.Awake();
			m_Touches = new List<TouchInfo>();

			// Mouse specific initialization
			if (UnityInput.mousePresent)
			{
				m_MouseInfo = new List<MouseButtonInfo>();
				basicMouseInfo = new MouseCursorInfo {currentPosition = UnityInput.mousePosition};

				for (int i = 0; i < trackMouseButtons; ++i)
				{
					m_MouseInfo.Add(new MouseButtonInfo
					{
						currentPosition = UnityInput.mousePosition,
						mouseButtonId = i
					});
				}
			}

			UnityInput.simulateMouseWithTouches = false;
		}

		/// <summary>
		/// Update all input
		/// </summary>
		void Update()
		{
			if (basicMouseInfo != null)
			{
				// Mouse was detected as present
				UpdateMouse();
			}
			// Handle touches
			UpdateTouches();
		}

		/// <summary>
		/// Perform logic to update mouse/pointing device
		/// </summary>
		void UpdateMouse()
		{
			basicMouseInfo.previousPosition = basicMouseInfo.currentPosition;
			basicMouseInfo.currentPosition = UnityInput.mousePosition;
			basicMouseInfo.delta = basicMouseInfo.currentPosition - basicMouseInfo.previousPosition;
			mouseMovedOnThisFrame = basicMouseInfo.delta.sqrMagnitude >= Mathf.Epsilon;
			mouseButtonPressedThisFrame = false;

			// Move event
			if (basicMouseInfo.delta.sqrMagnitude > Mathf.Epsilon)
			{
				if (mouseMoved != null)
				{
					mouseMoved(basicMouseInfo);
				}
			}
			// Button events
			for (int i = 0; i < trackMouseButtons; ++i)
			{
				MouseButtonInfo mouseButton = m_MouseInfo[i];
				mouseButton.delta = basicMouseInfo.delta;
				mouseButton.previousPosition = basicMouseInfo.previousPosition;
				mouseButton.currentPosition = basicMouseInfo.currentPosition;
				if (UnityInput.GetMouseButton(i))
				{
					if (!mouseButton.isDown)
					{
						// First press
						mouseButtonPressedThisFrame = true;
						mouseButton.isDown = true;
						mouseButton.startPosition = UnityInput.mousePosition;
						mouseButton.startTime = Time.realtimeSinceStartup;
						mouseButton.startedOverUI = EventSystem.current.IsPointerOverGameObject(-mouseButton.mouseButtonId - 1);

						// Reset some stuff
						mouseButton.totalMovement = 0;
						mouseButton.isDrag = false;
						mouseButton.wasHold = false;
						mouseButton.isHold = false;
						mouseButton.flickVelocity = Vector2.zero;

						if (pressed != null)
						{
							pressed(mouseButton);
						}
					}
					else
					{
						float moveDist = mouseButton.delta.magnitude;
						// Dragging?
						mouseButton.totalMovement += moveDist;
						if (mouseButton.totalMovement > dragThresholdMouse)
						{
							bool wasDrag = mouseButton.isDrag;

							mouseButton.isDrag = true;
							if (mouseButton.isHold)
							{
								mouseButton.wasHold = mouseButton.isHold;
								mouseButton.isHold = false;
							}

							// Did it just start now?
							if (!wasDrag)
							{
								if (startedDrag != null)
								{
									startedDrag(mouseButton);
								}
							}
							if (dragged != null)
							{
								dragged(mouseButton);
							}

							// Flick?
							if (moveDist > flickThreshold)
							{
								mouseButton.flickVelocity =
									(mouseButton.flickVelocity * (1 - k_FlickAccumulationFactor)) +
									(mouseButton.delta * k_FlickAccumulationFactor);
							}
							else
							{
								mouseButton.flickVelocity = Vector2.zero;
							}
						}
						else
						{
							// Stationary?
							if (!mouseButton.isHold &&
							    !mouseButton.isDrag &&
							    Time.realtimeSinceStartup - mouseButton.startTime >= holdTime)
							{
								mouseButton.isHold = true;
								if (startedHold != null)
								{
									startedHold(mouseButton);
								}
							}
						}
					}
				}
				else // Mouse button not up
				{
					if (mouseButton.isDown) // Released
					{
						mouseButton.isDown = false;
						// Quick enough (with no drift) to be a tap?
						if (!mouseButton.isDrag &&
						    Time.realtimeSinceStartup - mouseButton.startTime < tapTime)
						{
							if (tapped != null)
							{
								tapped(mouseButton);
							}
						}
						if (released != null)
						{
							released(mouseButton);
						}
					}
				}
			}

			// Mouse wheel
			if (Mathf.Abs(UnityInput.GetAxis("Mouse ScrollWheel")) > Mathf.Epsilon)
			{
				if (spunWheel != null)
				{
					spunWheel(new WheelInfo
					{
						zoomAmount = UnityInput.GetAxis("Mouse ScrollWheel") * mouseWheelSensitivity
					});
				}
			}
		}

		/// <summary>
		/// Update all touches
		/// </summary>
		void UpdateTouches()
		{
			touchPressedThisFrame = false;
			for (int i = 0; i < UnityInput.touchCount; ++i)
			{
				Touch touch = UnityInput.GetTouch(i);

				// Find existing touch, or create new one
				TouchInfo existingTouch = m_Touches.FirstOrDefault(t => t.touchId == touch.fingerId);

				if (existingTouch == null)
				{
					existingTouch = new TouchInfo
					{
						touchId = touch.fingerId,
						startPosition = touch.position,
						currentPosition = touch.position,
						previousPosition = touch.position,
						startTime = Time.realtimeSinceStartup,
						startedOverUI = EventSystem.current.IsPointerOverGameObject(touch.fingerId)
					};

					m_Touches.Add(existingTouch);

					// Sanity check
					Debug.Assert(touch.phase == TouchPhase.Began);
				}
				switch (touch.phase)
				{
					case TouchPhase.Began:
						touchPressedThisFrame = true;
						if (pressed != null)
						{
							pressed(existingTouch);
						}
						break;

					case TouchPhase.Moved:
						bool wasDrag = existingTouch.isDrag;
						UpdateMovingFinger(touch, existingTouch);

						// Is this a drag?
						existingTouch.isDrag = existingTouch.totalMovement >= dragThresholdTouch;

						if (existingTouch.isDrag)
						{
							if (existingTouch.isHold)
							{
								existingTouch.wasHold = existingTouch.isHold;
								existingTouch.isHold = false;
							}
							// Did it just start now?
							if (!wasDrag)
							{
								if (startedDrag != null)
								{
									startedDrag(existingTouch);
								}
							}
							if (dragged != null)
							{
								dragged(existingTouch);
							}

							if (existingTouch.delta.sqrMagnitude > flickThreshold * flickThreshold)
							{
								existingTouch.flickVelocity =
									(existingTouch.flickVelocity * (1 - k_FlickAccumulationFactor)) +
									(existingTouch.delta * k_FlickAccumulationFactor);
							}
							else
							{
								existingTouch.flickVelocity = Vector2.zero;
							}
						}
						else
						{
							UpdateHoldingFinger(existingTouch);
						}
						break;

					case TouchPhase.Canceled:
					case TouchPhase.Ended:
						// Could have moved a bit
						UpdateMovingFinger(touch, existingTouch);
						// Quick enough (with no drift) to be a tap?
						if (!existingTouch.isDrag &&
						    Time.realtimeSinceStartup - existingTouch.startTime < tapTime)
						{
							if (tapped != null)
							{
								tapped(existingTouch);
							}
						}
						if (released != null)
						{
							released(existingTouch);
						}

						// Remove from track list
						m_Touches.Remove(existingTouch);
						break;

					case TouchPhase.Stationary:
						UpdateMovingFinger(touch, existingTouch);
						UpdateHoldingFinger(existingTouch);
						existingTouch.flickVelocity = Vector2.zero;
						break;
				}
			}

			if (activeTouchCount >= 2 && (m_Touches[0].isDrag ||
			                              m_Touches[1].isDrag))
			{
				if (pinched != null)
				{
					pinched(new PinchInfo
					{
						touch1 = m_Touches[0],
						touch2 = m_Touches[1]
					});
				}
			}
		}

		/// <summary>
		/// Update a TouchInfo that might be holding
		/// </summary>
		/// <param name="existingTouch"></param>
		void UpdateHoldingFinger(PointerActionInfo existingTouch)
		{
			if (!existingTouch.isHold &&
			    !existingTouch.isDrag &&
			    Time.realtimeSinceStartup - existingTouch.startTime >= holdTime)
			{
				existingTouch.isHold = true;
				if (startedHold != null)
				{
					startedHold(existingTouch);
				}
			}
		}

		/// <summary>
		/// Update a TouchInfo with movement
		/// </summary>
		/// <param name="touch">The Unity touch object</param>
		/// <param name="existingTouch">The object that's tracking Unity's touch</param>
		void UpdateMovingFinger(Touch touch, PointerActionInfo existingTouch)
		{
			float dragDist = touch.deltaPosition.magnitude;

			existingTouch.previousPosition = existingTouch.currentPosition;
			existingTouch.currentPosition = touch.position;
			existingTouch.delta = touch.deltaPosition;
			existingTouch.totalMovement += dragDist;
		}
	}
}