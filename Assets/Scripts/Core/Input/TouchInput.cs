using UnityEngine;
using UnityInput = UnityEngine.Input;

namespace Core.Input
{
	/// <summary>
	/// Base control scheme for touch devices, which performs CameraRig control
	/// </summary>
	public class TouchInput : CameraInputScheme
	{
		/// <summary>
		/// Configuration of the pan speed
		/// </summary>
		public float panSpeed = 5;

		/// <summary>
		/// How quickly flicks decay
		/// </summary>
		public float flickDecayFactor = 0.2f;

		/// <summary>
		/// Flick direction
		/// </summary>
		Vector3 m_FlickDirection;

		/// <summary>
		/// Gets whether the scheme should be activated or not
		/// </summary>
		public override bool shouldActivate
		{
			get { return UnityInput.touchCount > 0; }
		}

		/// <summary>
		/// This default scheme on IOS and Android devices
		/// </summary>
		public override bool isDefault
		{
			get
			{
#if UNITY_IOS || UNITY_ANDROID
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
			
			// Register drag event
			InputController inputController = InputController.instance;
			inputController.pressed += OnPress;
			inputController.released += OnRelease;
			inputController.dragged += OnDrag;
			inputController.pinched += OnPinch;
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
			
			if (InputController.instanceExists)
			{
				InputController inputController = InputController.instance;
				inputController.pressed -= OnPress;
				inputController.released -= OnRelease;
				inputController.dragged -= OnDrag;
				inputController.pinched -= OnPinch;
			}
		}

		/// <summary>
		/// Perform flick and zoom
		/// </summary>
		protected virtual void Update()
		{
			if (cameraRig != null)
			{
				UpdateFlick();
				DecayZoom();
			}
		}

		/// <summary>
		/// Called on input press
		/// </summary>
		protected virtual void OnPress(PointerActionInfo pointer)
		{
			if (cameraRig != null)
			{
				DoFlickCatch(pointer);
			}
		}
		
		/// <summary>
		/// Called on input release
		/// </summary>
		protected virtual void OnRelease(PointerActionInfo pointer)
		{
			if (cameraRig != null)
			{
				DoReleaseFlick(pointer);
			}
		}

		/// <summary>
		/// Called when we drag
		/// </summary>
		protected virtual void OnDrag(PointerActionInfo pointer)
		{
			// Drag panning for touch input
			if (cameraRig != null)
			{
				DoDragPan(pointer);
			}
		}

		/// <summary>
		/// Called on pinch gestures
		/// </summary>
		protected virtual void OnPinch(PinchInfo pinch)
		{
			if (cameraRig != null)
			{
				DoPinchZoom(pinch);
			}
		}

		/// <summary>
		/// Update current flick velocity
		/// </summary>
		protected void UpdateFlick()
		{
			// Flick?
			if (m_FlickDirection.sqrMagnitude > Mathf.Epsilon)
			{
				cameraRig.PanCamera(m_FlickDirection * Time.deltaTime);
				m_FlickDirection *= flickDecayFactor;
			}
		}

		/// <summary>
		/// Decay the zoom if no touches are active
		/// </summary>
		protected void DecayZoom()
		{
			if (InputController.instance.activeTouchCount == 0)
			{
				cameraRig.ZoomDecay();
			}
		}

		/// <summary>
		/// "Catch" flicks on press, to stop the panning momentum
		/// </summary>
		/// <param name="pointer">The press pointer event</param>
		protected void DoFlickCatch(PointerActionInfo pointer)
		{
			var touchInfo = pointer as TouchInfo;
			// Stop flicks on touch
			if (touchInfo != null)
			{
				m_FlickDirection = Vector2.zero;
				cameraRig.StopTracking();
			}
		}
		
		/// <summary>
		/// Do flicks, on release only
		/// </summary>
		/// <param name="pointer">The release pointer event</param>
		protected void DoReleaseFlick(PointerActionInfo pointer)
		{
			var touchInfo = pointer as TouchInfo;

			if (touchInfo != null && touchInfo.flickVelocity.sqrMagnitude > Mathf.Epsilon)
			{
				// We have a flick!
				// Work out velocity from motion
				Ray prevRay = cameraRig.cachedCamera.ScreenPointToRay(pointer.currentPosition -
																		pointer.flickVelocity);
				Ray currRay = cameraRig.cachedCamera.ScreenPointToRay(pointer.currentPosition);

				Vector3 startPoint = Vector3.zero;
				Vector3 endPoint = Vector3.zero;
				float dist;

				if (cameraRig.floorPlane.Raycast(prevRay, out dist))
				{
					startPoint = prevRay.GetPoint(dist);
				}
				if (cameraRig.floorPlane.Raycast(currRay, out dist))
				{
					endPoint = currRay.GetPoint(dist);
				}

				// Work out that movement in units per second
				m_FlickDirection = (startPoint - endPoint) / Time.deltaTime;
			}
		}

		/// <summary>
		/// Controls the pan with a drag
		/// </summary>
		protected void DoDragPan(PointerActionInfo pointer)
		{
			var touchInfo = pointer as TouchInfo;
			if (touchInfo != null)
			{
				// Work out movement amount by raycasting onto floor plane from delta positions
				// and getting that distance
				Ray currRay = cameraRig.cachedCamera.ScreenPointToRay(touchInfo.currentPosition);

				Vector3 endPoint = Vector3.zero;
				float dist;
				if (cameraRig.floorPlane.Raycast(currRay, out dist))
				{
					endPoint = currRay.GetPoint(dist);
				}
				// Pan
				Ray prevRay = cameraRig.cachedCamera.ScreenPointToRay(touchInfo.previousPosition);
				Vector3 startPoint = Vector3.zero;

				if (cameraRig.floorPlane.Raycast(prevRay, out dist))
				{
					startPoint = prevRay.GetPoint(dist);
				}
				Vector3 panAmount = startPoint - endPoint;
				// If this is a touch, we divide the pan amount by the number of touches
				if (UnityInput.touchCount > 0)
				{
					panAmount /= UnityInput.touchCount;
				}
				
				PanCamera(panAmount);
			}
		}
		
		/// <summary>
		/// Perform a zoom with the given pinch
		/// </summary>
		protected void DoPinchZoom(PinchInfo pinch)
		{
			float currentDistance = (pinch.touch1.currentPosition - pinch.touch2.currentPosition).magnitude;
			float prevDistance = (pinch.touch1.previousPosition - pinch.touch2.previousPosition).magnitude;

			float zoomChange = prevDistance / currentDistance;
			float prevZoomDist = cameraRig.zoomDist;

			cameraRig.SetZoom(zoomChange * cameraRig.rawZoomDist);

			// Calculate actual zoom change after clamping
			zoomChange = cameraRig.zoomDist / prevZoomDist;

			// First get floor position of middle of gesture
			Vector2 averageScreenPos = (pinch.touch1.currentPosition + pinch.touch2.currentPosition) * 0.5f;
			Ray ray = cameraRig.cachedCamera.ScreenPointToRay(averageScreenPos);

			Vector3 worldPos = Vector3.zero;
			float dist;

			if (cameraRig.floorPlane.Raycast(ray, out dist))
			{
				worldPos = ray.GetPoint(dist);
			}

			// Vector from our current look pos to this point 
			Vector3 offsetValue = worldPos - cameraRig.lookPosition;

			// Pan towards or away from our zoom center
			PanCamera(offsetValue * (1 - zoomChange));
		}
		
		/// <summary>
		/// Pans the camera
		/// </summary>
		/// <param name="panAmount">
		/// The vector to pan
		/// </param>
		protected void PanCamera(Vector3 panAmount)
		{
			cameraRig.StopTracking();
			cameraRig.PanCamera(panAmount);
		}
	}
}