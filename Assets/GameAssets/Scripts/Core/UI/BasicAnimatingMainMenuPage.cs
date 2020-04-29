namespace Core.UI
{
	/// <summary>
	/// Simplest form of a MainMenuPage - the activating/deactivating of a page is instantaneous
	/// </summary>
	public class BasicAnimatingMainMenuPage : AnimatingMainMenuPage
	{
		/// <summary>
		/// BeginDeactivatingPage immediately calls FinishedDeactivatingPage
		/// </summary>
		protected override void BeginDeactivatingPage()
		{
			FinishedDeactivatingPage();
		}

		/// <summary>
		/// Don't need to do anything here
		/// </summary>
		protected override void FinishedActivatingPage()
		{
		}
	}
}