using System.Collections.Generic;
using UnityEngine;

namespace Core.Utilities
{
	public static class VectorHelper
	{
		/// <summary>
		/// A helper function that finds the average position of several component objects, 
		/// specifically because they have transforms
		/// </summary>
		/// <param name="components">
		/// The list of components to average
		/// </param>
		/// <typeparam name="TComponent">
		/// The Unity Component which has a transform
		/// </typeparam>
		/// <returns>
		/// The average position
		/// </returns>
		public static Vector3 FindAveragePosition<TComponent>(TComponent[] components) where TComponent : Component
		{
			Vector3 output = Vector3.zero;
			foreach (TComponent component in components)
			{
				if (component == null)
				{
					continue;
				}
				output += component.transform.position;
			}
			return output / components.Length;
		}

		/// <summary>
		/// A helper function that finds the average position of several component objects, 
		/// specifically because they have transforms
		/// </summary>
		/// <param name="components">
		/// The list of components to average
		/// </param>
		/// <typeparam name="TComponent">
		/// The Unity Component which has a transform
		/// </typeparam>
		/// <returns>
		/// The average velocity
		/// </returns>
		public static Vector3 FindAverageVelocity<TComponent>(TComponent[] components) where TComponent : Component
		{
			Vector3 output = Vector3.zero;
			foreach (TComponent component in components)
			{
				if (component == null)
				{
					continue;
				}
				var rigidbody = component.GetComponent<Rigidbody>();
				if (rigidbody == null)
				{
					continue;
				}
				output += rigidbody.velocity;
			}
			return output / components.Length;
		}

		/// <summary>
		/// A helper function that finds the average position of several component objects, 
		/// specifically because they have transforms
		/// </summary>
		/// <param name="components">
		/// The list of components to average
		/// </param>
		/// <typeparam name="TComponent">
		/// The Unity Component which has a transform
		/// </typeparam>
		/// <returns>
		/// The average position
		/// </returns>
		public static Vector3 FindAveragePosition<TComponent>(List<TComponent> components) where TComponent : Component
		{
			Vector3 output = Vector3.zero;
			foreach (TComponent component in components)
			{
				if (component == null)
				{
					continue;
				}
				output += component.transform.position;
			}
			return output / components.Count;
		}

		/// <summary>
		/// A helper function that finds the average position of several component objects, 
		/// specifically because they have transforms
		/// </summary>
		/// <param name="components">
		/// The list of components to average
		/// </param>
		/// <typeparam name="TComponent">
		/// The Unity Component which has a transform
		/// </typeparam>
		/// <returns>
		/// The average velocity
		/// </returns>
		public static Vector3 FindAverageVelocity<TComponent>(List<TComponent> components) where TComponent : Component
		{
			Vector3 output = Vector3.zero;
			foreach (TComponent component in components)
			{
				if (component == null)
				{
					continue;
				}
				var rigidbody = component.GetComponent<Rigidbody>();
				if (rigidbody == null)
				{
					continue;
				}
				output += rigidbody.velocity;
			}
			return output / components.Count;
		}
	}
}