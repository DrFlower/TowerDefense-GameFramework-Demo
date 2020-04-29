namespace TowerDefense.Towers.Placement
{
	/// <summary>
	/// Enum representing the state of how a tower fits into a placement area
	/// </summary>
	public enum TowerFitStatus
	{
		/// <summary>
		/// Tower fits in this location
		/// </summary>
		Fits,

		/// <summary>
		/// Tower overlaps another tower in the placement area
		/// </summary>
		Overlaps,

		/// <summary>
		/// Tower exceeds bounds of the placement area
		/// </summary>
		OutOfBounds
	}
}