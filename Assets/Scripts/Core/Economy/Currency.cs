using System;

namespace Core.Economy
{
	/// <summary>
	/// A basic model for in game currency
	/// </summary>
	public class Currency
	{
		/// <summary>
		/// How much currency there currently is
		/// </summary>
		public int currentCurrency { get; private set; }

		/// <summary>
		/// Occurs when currency changed.
		/// </summary>
		public event Action currencyChanged;

		/// <summary>
		/// Initializes a new instance of the <see cref="Core.Economy.Currency" /> class.
		/// </summary>
		public Currency(int startingCurrency)
		{
			ChangeCurrency(startingCurrency);
		}

		/// <summary>
		/// Adds the currency.
		/// </summary>
		/// <param name="increment">the change in currency</param>
		public void AddCurrency(int increment)
		{
			ChangeCurrency(increment);
		}

		/// <summary>
		/// Method for trying to purchase, returns false for insufficient funds
		/// </summary>
		/// <returns><c>true</c>, if purchase was successful i.e. enough currency <c>false</c> otherwise.</returns>
		public bool TryPurchase(int cost)
		{
			// Cannot afford this item
			if (!CanAfford(cost))
			{
				return false;
			}
			ChangeCurrency(-cost);
			return true;
		}

		/// <summary>
		/// Determines if the specified cost is affordable.
		/// </summary>
		/// <returns><c>true</c> if this cost is affordable; otherwise, <c>false</c>.</returns>
		public bool CanAfford(int cost)
		{
			return currentCurrency >= cost;
		}

		/// <summary>
		/// Changes the currency.
		/// </summary>
		/// <param name="increment">the change in currency</param>
		protected void ChangeCurrency(int increment)
		{
			if (increment != 0)
			{
				currentCurrency += increment;
				if (currencyChanged != null)
				{
					currencyChanged();
				}
			}
		}
	}
}