namespace Core.Input
{
	/// <summary>
	/// Information about a pinch gesture
	/// </summary>
	public struct PinchInfo
	{
		/// <summary>
		/// The first touch involved in the pinch
		/// </summary>
		public TouchInfo touch1;

		/// <summary>
		/// The second touch involved in the pinch
		/// </summary>
		public TouchInfo touch2;
	}
}