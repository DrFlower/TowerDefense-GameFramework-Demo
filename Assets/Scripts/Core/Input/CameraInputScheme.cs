using Core.Camera;
using UnityEngine;

namespace Core.Input
{
	/// <summary>
	/// Abstract base input scheme for schemes that control the CameraRig
	/// </summary>
	public abstract class CameraInputScheme : InputScheme
	{
		/// <summary>
		/// Camera rig to control
		/// </summary>
		public CameraRig cameraRig;

		/// <summary>
		/// Pan speed factor when fully zoomed-in
		/// </summary>
		public float nearZoomPanSpeedModifier = 0.2f;

		/// <summary>
		/// Gets our pan speed multiplier for the given zoom level
		/// </summary>
		/// <returns></returns>
		protected float GetPanSpeedForZoomLevel()
		{
			return cameraRig != null ? 
				Mathf.Lerp(nearZoomPanSpeedModifier, 1, cameraRig.CalculateZoomRatio()) : 
				1.0f;
		}

		/// <summary>
		/// Do screen edge panning with the given screen coordinates
		/// </summary>
		/// <param name="screenPosition">The screen position of the cursor panning the camera</param>
		/// <param name="screenEdgeThreshold">The screen edge threshold in pixels</param>
		/// <param name="panSpeed">Speed of panning</param>
		protected void PanWithScreenCoordinates(Vector2 screenPosition, float screenEdgeThreshold, float panSpeed)
		{
			// Calculate zoom ratio
			float zoomRatio = GetPanSpeedForZoomLevel();

			// Left
			if ((screenPosition.x < screenEdgeThreshold))
			{
				float panAmount = (screenEdgeThreshold - screenPosition.x) / screenEdgeThreshold;
				panAmount = Mathf.Clamp01(Mathf.Log(panAmount) + 1);

				if (cameraRig.trackingObject == null)
				{
					cameraRig.PanCamera(Vector3.left * Time.deltaTime * panSpeed * panAmount * zoomRatio);

					cameraRig.StopTracking();
				}
			}

			// Right
			if ((screenPosition.x > Screen.width - screenEdgeThreshold))
			{
				float panAmount = ((screenEdgeThreshold - Screen.width) + screenPosition.x) / screenEdgeThreshold;
				panAmount = Mathf.Clamp01(Mathf.Log(panAmount) + 1);

				if (cameraRig.trackingObject == null)
				{
					cameraRig.PanCamera(Vector3.right * Time.deltaTime * panSpeed * panAmount * zoomRatio);
				}
				cameraRig.StopTracking();
			}

			// Down
			if ((screenPosition.y < screenEdgeThreshold))
			{
				float panAmount = (screenEdgeThreshold - screenPosition.y) / screenEdgeThreshold;
				panAmount = Mathf.Clamp01(Mathf.Log(panAmount) + 1);

				if (cameraRig.trackingObject == null)
				{
					cameraRig.PanCamera(Vector3.back * Time.deltaTime * panSpeed * panAmount * zoomRatio);

					cameraRig.StopTracking();
				}
			}

			// Up
			if ((screenPosition.y > Screen.height - screenEdgeThreshold))
			{
				float panAmount = ((screenEdgeThreshold - Screen.height) + screenPosition.y) / screenEdgeThreshold;
				panAmount = Mathf.Clamp01(Mathf.Log(panAmount) + 1);

				if (cameraRig.trackingObject == null)
				{
					cameraRig.PanCamera(Vector3.forward * Time.deltaTime * panSpeed * panAmount * zoomRatio);

					cameraRig.StopTracking();
				}
			}
		}
	}
}