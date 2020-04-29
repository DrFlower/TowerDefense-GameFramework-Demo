using UnityEngine;
using UnityInput = UnityEngine.Input;

namespace Core.Input
{
	/// <summary>
	/// Base control scheme for desktop devices, which performs CameraRig motion
	/// </summary>
	public class KeyboardMouseInput : CameraInputScheme
	{
		/// <summary>
		/// Pan threshold (how near to the edge before we pan. Also the denominator for RMB pan)
		/// </summary>
		public float screenPanThreshold = 40f;

		/// <summary>
		/// Pan speed for edge panning
		/// </summary>
		public float mouseEdgePanSpeed = 30f;

		/// <summary>
		/// Pan speed for RMB panning
		/// </summary>
		public float mouseRmbPanSpeed = 15f;
		
		
		/// <summary>
		/// Gets whether the scheme should be activated or not
		/// </summary>
		public override bool shouldActivate
		{
			get
			{
				if (UnityInput.touchCount > 0)
				{
					return false;
				}
				bool anyKey = UnityInput.anyKey;
				bool buttonPressedThisFrame = InputController.instance.mouseButtonPressedThisFrame;
				bool movedMouseThisFrame = InputController.instance.mouseMovedOnThisFrame;

				return (anyKey || buttonPressedThisFrame || movedMouseThisFrame);
			}
		}

		/// <summary>
		/// This is the default scheme on desktop devices
		/// </summary>
		public override bool isDefault
		{
			get
			{
#if UNITY_STANDALONE || UNITY_EDITOR
				return true;
#else
				return false;
#endif
			}
		}

		/// <summary>
		/// Register input events
		/// </summary>
		protected virtual void OnEnable()
		{
			if (!InputController.instanceExists)
			{
				Debug.LogError("[UI] Keyboard and Mouse UI requires InputController");
				return;
			}

			InputController controller = InputController.instance;

			controller.spunWheel += OnWheel;
			controller.dragged += OnDrag;
			controller.pressed += OnPress;
		}

		/// <summary>
		/// Deregister input events
		/// </summary>
		protected virtual void OnDisable()
		{
			if (!InputController.instanceExists)
			{
				return;
			}

			InputController controller = InputController.instance;

			controller.pressed -= OnPress;
			controller.dragged -= OnDrag;
			controller.spunWheel -= OnWheel;
		}
		
		/// <summary>
		/// Handle camera panning behaviour
		/// </summary>
		protected virtual void Update()
		{
			if (cameraRig != null)
			{
				DoScreenEdgePan();
				DoKeyboardPan();
				DecayZoom();
			}
		}

		/// <summary>
		/// Called when we drag
		/// </summary>
		protected virtual void OnDrag(PointerActionInfo pointer)
		{
			if (cameraRig != null)
			{
				DoRightMouseDragPan(pointer);
			}
		}

		/// <summary>
		/// Called on mouse wheel input
		/// </summary>
		protected virtual void OnWheel(WheelInfo wheel)
		{
			if (cameraRig != null)
			{
				DoWheelZoom(wheel);
			}
		}

		/// <summary>
		/// Called on input press, for MMB panning
		/// </summary>
		protected virtual void OnPress(PointerActionInfo pointer)
		{
			if (cameraRig != null)
			{
				DoMiddleMousePan(pointer);
			}
		}

		/// <summary>
		/// Perform mouse screen-edge panning
		/// </summary>
		protected void DoScreenEdgePan()
		{
			Vector2 mousePos = UnityInput.mousePosition;

			bool mouseInside = (mousePos.x >= 0) &&
			                   (mousePos.x < Screen.width) &&
			                   (mousePos.y >= 0) &&
			                   (mousePos.y < Screen.height);

			// Mouse can be outside of our window
			if (mouseInside)
			{
				PanWithScreenCoordinates(mousePos, screenPanThreshold, mouseEdgePanSpeed);
			}
		}

		/// <summary>
		/// Perform keyboard panning
		/// </summary>
		protected void DoKeyboardPan()
		{
			// Calculate zoom ratio
			float zoomRatio = GetPanSpeedForZoomLevel();
			
			// Left
			if (UnityInput.GetKey(KeyCode.LeftArrow) || UnityInput.GetKey(KeyCode.A))
			{
				cameraRig.PanCamera(Vector3.left * Time.deltaTime * mouseEdgePanSpeed * zoomRatio);

				cameraRig.StopTracking();
			}

			// Right
			if (UnityInput.GetKey(KeyCode.RightArrow) || UnityInput.GetKey(KeyCode.D))
			{
				cameraRig.PanCamera(Vector3.right * Time.deltaTime * mouseEdgePanSpeed * zoomRatio);

				cameraRig.StopTracking();
			}

			// Down
			if (UnityInput.GetKey(KeyCode.DownArrow) || UnityInput.GetKey(KeyCode.S))
			{
				cameraRig.PanCamera(Vector3.back * Time.deltaTime * mouseEdgePanSpeed * zoomRatio);

				cameraRig.StopTracking();
			}

			// Up
			if (UnityInput.GetKey(KeyCode.UpArrow) || UnityInput.GetKey(KeyCode.W))
			{
				cameraRig.PanCamera(Vector3.forward * Time.deltaTime * mouseEdgePanSpeed * zoomRatio);

				cameraRig.StopTracking();
			}
		}

		/// <summary>
		/// Decay the zoom if it's springy
		/// </summary>
		protected void DecayZoom()
		{
			cameraRig.ZoomDecay();
		}

		/// <summary>
		/// Pan with right mouse
		/// </summary>
		/// <param name="pointer">The drag pointer event</param>
		protected void DoRightMouseDragPan(PointerActionInfo pointer)
		{
			var mouseInfo = pointer as MouseButtonInfo;
			if ((mouseInfo != null) &&
			    (mouseInfo.mouseButtonId == 1))
			{
				// Calculate zoom ratio
				float zoomRatio = GetPanSpeedForZoomLevel();

				Vector2 panVector = mouseInfo.currentPosition - mouseInfo.startPosition;
				panVector = (panVector * Time.deltaTime * mouseRmbPanSpeed * zoomRatio) / screenPanThreshold;

				var camVector = new Vector3(panVector.x, 0, panVector.y);
				cameraRig.PanCamera(camVector);

				cameraRig.StopTracking();
			}
		}

		/// <summary>
		/// Perform mouse wheel zooming
		/// </summary>
		protected void DoWheelZoom(WheelInfo wheel)
		{
			float prevZoomDist = cameraRig.zoomDist;
			cameraRig.ZoomCameraRelative(wheel.zoomAmount * -1);

			// Calculate actual zoom change after clamping
			float zoomChange = cameraRig.zoomDist / prevZoomDist;

			// First get floor position of cursor
			Ray ray = cameraRig.cachedCamera.ScreenPointToRay(UnityInput.mousePosition);

			Vector3 worldPos = Vector3.zero;
			float dist;

			if (cameraRig.floorPlane.Raycast(ray, out dist))
			{
				worldPos = ray.GetPoint(dist);
			}

			// Vector from our current look pos to this point 
			Vector3 offsetValue = worldPos - cameraRig.lookPosition;

			// Pan towards or away from our zoom center
			cameraRig.PanCamera(offsetValue * (1 - zoomChange));
		}

		/// <summary>
		/// Pan with middle mouse
		/// </summary>
		/// <param name="pointer">Pointer with press event</param>
		protected void DoMiddleMousePan(PointerActionInfo pointer)
		{
			var mouseInfo = pointer as MouseButtonInfo;

			// Pan to mouse position on MMB
			if ((mouseInfo != null) &&
			    (mouseInfo.mouseButtonId == 2))
			{
				// First get floor position of cursor
				Ray ray = cameraRig.cachedCamera.ScreenPointToRay(UnityInput.mousePosition);

				float dist;

				if (cameraRig.floorPlane.Raycast(ray, out dist))
				{
					Vector3 worldPos = ray.GetPoint(dist);
					cameraRig.PanTo(worldPos);
				}

				cameraRig.StopTracking();
			}
		}
	}
}