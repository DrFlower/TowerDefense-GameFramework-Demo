using System;
using Core.Economy;
using Core.Utilities;
using UnityEngine;

namespace TowerDefense.Economy
{
	/// <summary>
	/// A class for currency gain
	/// </summary>
	[Serializable]
	public class CurrencyGainer
	{
		/// <summary>
		/// The amount gained with the gain rate
		/// </summary>
		public int constantCurrencyAddition;

		/// <summary>
		/// The speed of currency gain in units-per-second
		/// </summary>
		[Header("The Gain Rate in additions-per-second")]
		public float constantCurrencyGainRate;

		/// <summary>
		/// Event for when the currency is changed
		/// </summary>
		public event Action<CurrencyChangeInfo> currencyChanged;

		/// <summary>
		/// The timer for constant currency gain
		/// </summary>
		protected RepeatingTimer m_GainTimer;

		/// <summary>
		/// Gets the currency that this CurrencyGainer modifes
		/// </summary>
		public Currency currency { get; private set; }

		/// <summary>
		/// Initializes the currency gainer with new data
		/// </summary>
		/// <param name="currencyController">
		/// The currency controller to modify with this currency gainer
		/// </param>
		/// <param name="gainAddition">
		/// The currency gained with each addition
		/// </param>
		/// <param name="gainRate">
		/// The rate of gain
		/// </param>
		public void Initialize(Currency currencyController, int gainAddition, float gainRate)
		{
			constantCurrencyAddition = gainAddition;
			constantCurrencyGainRate = gainRate;
			Initialize(currencyController);
		}

		/// <summary>
		/// Initializes the currency gainer
		/// </summary>
		public void Initialize(Currency currencyController)
		{
			currency = currencyController;
			UpdateGainRate(constantCurrencyGainRate);
		}

		/// <summary>
		/// For updating the gain timer
		/// </summary>
		/// <param name="deltaTime">
		/// The change in time to update the timer
		/// </param>
		public void Tick(float deltaTime)
		{
			if (m_GainTimer == null)
			{
				return;
			}
			m_GainTimer.Tick(Time.deltaTime);
		}

		/// <summary>
		/// Sets the currency gain rate and activates the timer
		/// </summary>
		/// <param name="currencyGainRate">
		/// The amount to set the constant gain rate to
		/// </param>
		public void UpdateGainRate(float currencyGainRate)
		{
			constantCurrencyGainRate = currencyGainRate;
			if (currencyGainRate < 0)
			{
				throw new ArgumentOutOfRangeException("currencyGainRate");
			}
			if (m_GainTimer == null)
			{
				m_GainTimer = new RepeatingTimer(1 / constantCurrencyGainRate, ConstantGain);
			}
			else
			{
				m_GainTimer.SetTime(1 / constantCurrencyGainRate);
			}
		}

		/// <summary>
		/// Increase the currency by m_ConstantCurrencyAddition
		/// </summary>
		protected void ConstantGain()
		{
			int previousCurrency = currency.currentCurrency;
			currency.AddCurrency(constantCurrencyAddition);
			int currentCurrency = currency.currentCurrency;
			var info = new CurrencyChangeInfo(previousCurrency, currentCurrency);
			if (currencyChanged != null)
			{
				currencyChanged(info);
			}
		}
	}
}