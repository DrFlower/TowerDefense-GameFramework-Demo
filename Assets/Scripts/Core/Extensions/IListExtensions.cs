using System;
using System.Collections.Generic;
using UnityDebug = UnityEngine.Debug;
using UnityRandom = UnityEngine.Random;

namespace Core.Extensions
{
	/// <summary>
	/// Extension methods for ILists
	/// </summary>
	public static class IListExtensions
	{
		static readonly Random s_SharedRandom = new Random();

		/// <summary>
		/// Select an item from a list using a weighted selection.
		/// </summary>
		/// <remarks>This is an O(n) operation, not constant-time like equal random selection.</remarks>
		/// <param name="elements">An <see cref="System.Collections.Generic.IList{T}" /> of elements to choose from</param>
		/// <param name="weightSum">The sum of all the weights of the elements</param>
		/// <param name="getElementWeight">A delegate to retrieve the weight of a specific element</param>
		/// <returns>An element randomly selected from <paramref name="elements" /></returns>
		public static T WeightedSelection<T>(this IList<T> elements, int weightSum, Func<T, int> getElementWeight)
		{
			int index = elements.WeightedSelectionIndex(weightSum, getElementWeight);
			return elements[index];
		}

		/// <summary>
		/// Select an item from a list using a weighted selection.
		/// </summary>
		/// <remarks>This is an O(n) operation, not constant-time like equal random selection.</remarks>
		/// <param name="elements">An <see cref="System.Collections.Generic.IList{T}" /> of elements to choose from</param>
		/// <param name="weightSum">The sum of all the weights of the elements</param>
		/// <param name="getElementWeight">A delegate to retrieve the weight of a specific element</param>
		/// <returns>An element randomly selected from <paramref name="elements" /></returns>
		public static T WeightedSelection<T>(this IList<T> elements, float weightSum, Func<T, float> getElementWeight)
		{
			int index = elements.WeightedSelectionIndex(weightSum, getElementWeight);
			return elements[index];
		}

		/// <summary>
		/// Select the index of an item from a list using a weighted selection.
		/// </summary>
		/// <remarks>This is an O(n) operation, not constant-time like equal random selection.</remarks>
		/// <param name="elements">An <see cref="System.Collections.Generic.IList{T}" /> of elements to choose from</param>
		/// <param name="weightSum">The sum of all the weights of the elements</param>
		/// <param name="getElementWeight">A delegate to retrieve the weight of a specific element</param>
		/// <returns>The index of an element randomly selected from <paramref name="elements" /></returns>
		public static int WeightedSelectionIndex<T>(this IList<T> elements, int weightSum, Func<T, int> getElementWeight)
		{
			if (weightSum <= 0)
			{
				throw new ArgumentException("WeightSum should be a positive value", "weightSum");
			}

			int selectionIndex = 0;
			int selectionWeightIndex = UnityRandom.Range(0, weightSum);
			int elementCount = elements.Count;

			if (elementCount == 0)
			{
				throw new InvalidOperationException("Cannot perform selection on an empty collection");
			}

			int itemWeight = getElementWeight(elements[selectionIndex]);
			while (selectionWeightIndex >= itemWeight)
			{
				selectionWeightIndex -= itemWeight;
				selectionIndex++;

				if (selectionIndex >= elementCount)
				{
					throw new ArgumentException("Weighted selection exceeded indexable range. Is your weightSum correct?",
					                            "weightSum");
				}

				itemWeight = getElementWeight(elements[selectionIndex]);
			}

			return selectionIndex;
		}

		/// <summary>
		/// Select the index of an item from a list using a weighted selection.
		/// </summary>
		/// <remarks>This is an O(n) operation, not constant-time like equal random selection.</remarks>
		/// <param name="elements">An <see cref="System.Collections.Generic.IList{T}" /> of elements to choose from</param>
		/// <param name="weightSum">The sum of all the weights of the elements</param>
		/// <param name="getElementWeight">A delegate to retrieve the weight of a specific element</param>
		/// <returns>The index of an element randomly selected from <paramref name="elements" /></returns>
		public static int WeightedSelectionIndex<T>(this IList<T> elements, float weightSum, Func<T, float> getElementWeight)
		{
			if (weightSum <= 0)
			{
				throw new ArgumentException("WeightSum should be a positive value", "weightSum");
			}

			int selectionIndex = 0;

			double selectedWeight = s_SharedRandom.NextDouble() * weightSum;
			int elementCount = elements.Count;

			if (elementCount == 0)
			{
				throw new InvalidOperationException("Cannot perform selection on an empty collection");
			}

			double itemWeight = getElementWeight(elements[selectionIndex]);
			while (selectedWeight >= itemWeight)
			{
				selectedWeight -= itemWeight;
				selectionIndex++;

				if (selectionIndex >= elementCount)
				{
					throw new ArgumentException("Weighted selection exceeded indexable range. Is your weightSum correct?",
					                            "weightSum");
				}

				itemWeight = getElementWeight(elements[selectionIndex]);
			}

			return selectionIndex;
		}

		/// <summary>
		/// Shuffle this List into a new array copy
		/// </summary>
		public static T[] Shuffle<T>(this IList<T> original)
		{
			int numItems = original.Count;
			T[] result = new T[numItems];

			for (int i = 0; i < numItems; ++i)
			{
				int j = UnityRandom.Range(0, i + 1);

				if (j != i)
				{
					result[i] = result[j];
				}

				result[j] = original[i];
			}

			return result;
		}

		/// <summary>
		/// Goes to the next element of the list
		/// </summary>
		/// <param name="elements">An <see cref="System.Collections.Generic.IList{T}" /> of elements to choose from</param>
		/// <param name="currentIndex">The current index to be changed via reference</param>
		/// <param name="wrap">if the list should wrap</param>
		/// <typeparam name="T">The generic parameter for the list</typeparam>
		/// <returns>true if there is a next item in the list</returns>
		public static bool Next<T>(this IList<T> elements, ref int currentIndex, bool wrap = false)
		{
			int count = elements.Count;
			if (count == 0)
			{
				return false;
			}

			currentIndex++;

			if (currentIndex >= count)
			{
				if (wrap)
				{
					currentIndex = 0;
					return true;
				}
				currentIndex = count - 1;
				return false;
			}

			return true;
		}

		/// <summary>
		/// Goes to the previous element of the list
		/// </summary>
		/// <param name="elements">An <see cref="System.Collections.Generic.IList{T}" /> of elements to choose from</param>
		/// <param name="currentIndex">The current index to be changed via reference</param>
		/// <param name="wrap">if the list should wrap</param>
		/// <typeparam name="T">The generic parameter for the list</typeparam>
		/// <returns>true if there is a previous item in the list</returns>
		public static bool Prev<T>(this IList<T> elements, ref int currentIndex, bool wrap = false)
		{
			int count = elements.Count;
			if (count == 0)
			{
				return false;
			}

			currentIndex--;

			if (currentIndex < 0)
			{
				if (wrap)
				{
					currentIndex = count - 1;
					return true;
				}
				currentIndex = 0;
				return false;
			}

			return true;
		}
	}
}