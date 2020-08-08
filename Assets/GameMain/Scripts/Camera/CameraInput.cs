using UnityEngine;
using UnityInput = UnityEngine.Input;

namespace Flower
{
    /// <summary>
    /// Abstract base input scheme for schemes that control the CameraRig
    /// </summary>
    public class CameraInput : MonoBehaviour, IPause
    {
        private bool pause;

        /// <summary>
        /// Camera rig to control
        /// </summary>
        public CameraControl cameraControl;

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
            return cameraControl != null ?
                Mathf.Lerp(nearZoomPanSpeedModifier, 1, cameraControl.CalculateZoomRatio()) :
                1.0f;
        }

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
        /// Do screen edge panning with the given screen coordinates
        /// </summary>
        /// <param name="screenPosition">The screen position of the cursor panning the camera</param>
        /// <param name="screenEdgeThreshold">The screen edge threshold in pixels</param>
        /// <param name="panSpeed">Speed of panning</param>
        private void PanWithScreenCoordinates(Vector2 screenPosition, float screenEdgeThreshold, float panSpeed)
        {
            // Calculate zoom ratio
            float zoomRatio = GetPanSpeedForZoomLevel();

            // Left
            if ((screenPosition.x < screenEdgeThreshold))
            {
                float panAmount = (screenEdgeThreshold - screenPosition.x) / screenEdgeThreshold;
                panAmount = Mathf.Clamp01(Mathf.Log(panAmount) + 1);
                if (cameraControl.trackingObject == null)
                {
                    cameraControl.PanCamera(Vector3.left * Time.deltaTime * panSpeed * panAmount * zoomRatio);
                    cameraControl.StopTracking();
                }
            }

            // Right
            if ((screenPosition.x > Screen.width - screenEdgeThreshold))
            {
                float panAmount = ((screenEdgeThreshold - Screen.width) + screenPosition.x) / screenEdgeThreshold;
                panAmount = Mathf.Clamp01(Mathf.Log(panAmount) + 1);

                if (cameraControl.trackingObject == null)
                {
                    cameraControl.PanCamera(Vector3.right * Time.deltaTime * panSpeed * panAmount * zoomRatio);
                }
                cameraControl.StopTracking();
            }

            // Down
            if ((screenPosition.y < screenEdgeThreshold))
            {
                float panAmount = (screenEdgeThreshold - screenPosition.y) / screenEdgeThreshold;
                panAmount = Mathf.Clamp01(Mathf.Log(panAmount) + 1);

                if (cameraControl.trackingObject == null)
                {
                    cameraControl.PanCamera(Vector3.back * Time.deltaTime * panSpeed * panAmount * zoomRatio);

                    cameraControl.StopTracking();
                }
            }

            // Up
            if ((screenPosition.y > Screen.height - screenEdgeThreshold))
            {
                float panAmount = ((screenEdgeThreshold - Screen.height) + screenPosition.y) / screenEdgeThreshold;
                panAmount = Mathf.Clamp01(Mathf.Log(panAmount) + 1);

                if (cameraControl.trackingObject == null)
                {
                    cameraControl.PanCamera(Vector3.forward * Time.deltaTime * panSpeed * panAmount * zoomRatio);

                    cameraControl.StopTracking();
                }
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
                cameraControl.PanCamera(Vector3.left * Time.deltaTime * mouseEdgePanSpeed * zoomRatio);

                cameraControl.StopTracking();
            }

            // Right
            if (UnityInput.GetKey(KeyCode.RightArrow) || UnityInput.GetKey(KeyCode.D))
            {
                cameraControl.PanCamera(Vector3.right * Time.deltaTime * mouseEdgePanSpeed * zoomRatio);

                cameraControl.StopTracking();
            }

            // Down
            if (UnityInput.GetKey(KeyCode.DownArrow) || UnityInput.GetKey(KeyCode.S))
            {
                cameraControl.PanCamera(Vector3.back * Time.deltaTime * mouseEdgePanSpeed * zoomRatio);

                cameraControl.StopTracking();
            }

            // Up
            if (UnityInput.GetKey(KeyCode.UpArrow) || UnityInput.GetKey(KeyCode.W))
            {
                cameraControl.PanCamera(Vector3.forward * Time.deltaTime * mouseEdgePanSpeed * zoomRatio);

                cameraControl.StopTracking();
            }
        }

        /// <summary>
        /// Perform mouse screen-edge panning
        /// </summary>
        private void DoScreenEdgePan()
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
        /// Decay the zoom if it's springy
        /// </summary>
        protected void DecayZoom()
        {
            cameraControl.ZoomDecay();
        }

        private void Update()
        {
            if (cameraControl != null && !pause)
            {
                DoScreenEdgePan();
                DoKeyboardPan();
                DecayZoom();
            }
        }

        public void Pause()
        {
            pause = true;
        }

        public void Resume()
        {
            pause = false;
        }
    }
}