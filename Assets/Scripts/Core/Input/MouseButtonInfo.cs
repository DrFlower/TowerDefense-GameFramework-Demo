namespace Core.Input
{
	/// <summary>
	/// Info for mouse
	/// </summary>
	public class MouseButtonInfo : PointerActionInfo
	{
		/// <summary>
		/// Is this mouse button down
		/// </summary>
		public bool isDown;

		/// <summary>
		/// Our mouse button id
		/// </summary>
		public int mouseButtonId;
	}
}