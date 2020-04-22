namespace ActionGameFramework.Projectiles
{
	/// <summary>
	/// Ballistic arc calculation priorities/preferences.
	/// </summary>
	public enum BallisticArcHeight
	{
		/// <summary>
		/// High "underarm" arc
		/// </summary>
		UseHigh,

		/// <summary>
		/// Low "overarm" arc
		/// </summary>
		UseLow,

		/// <summary>
		/// Use high arc if valid, fall back to low if possible.
		/// </summary>
		PreferHigh,

		/// <summary>
		/// Use low arc if valid, fall back to high if possible.
		/// </summary>
		PreferLow
	}
}