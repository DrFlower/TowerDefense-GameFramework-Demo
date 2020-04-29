using UnityEngine;

namespace Core.Input
{
	/// <summary>
	/// Class to track information about an active pointer input
	/// </summary>
	public class PointerActionInfo : PointerInfo
	{
		/// <summary>
		/// Position where the input started
		/// </summary>
		public Vector2 startPosition;

		/// <summary>
		/// Flick velocity is a moving average of deltas
		/// </summary>
		public Vector2 flickVelocity;

		/// <summary>
		/// Total movement for this pointer, since being held down
		/// </summary>
		public float totalMovement;

		/// <summary>
		/// Time hold started
		/// </summary>
		public float startTime;

		/// <summary>
		/// Has this input been dragged?
		/// </summary>
		public bool isDrag;

		/// <summary>
		/// Is this input holding?
		/// </summary>
		public bool isHold;

		/// <summary>
		/// Was this input previously holding, then dragged?
		/// </summary>
		public bool wasHold;
	}
}