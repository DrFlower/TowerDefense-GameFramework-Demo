using System;
using UnityEngine;

namespace ActionGameFramework.Projectiles
{
	/// <summary>
	/// Interface allowing specification of generic projectiles.
	/// </summary>
	public interface IProjectile
	{
		/// <summary>
		/// Event fired when this projectile is launched
		/// </summary>
		event Action fired;
		
		/// <summary>
		/// Fires this projectile from a designated start point to a designated world coordinate.
		/// </summary>
		/// <param name="startPoint">Start point of the flight.</param>
		/// <param name="targetPoint">Target point to fly to.</param>
		void FireAtPoint(Vector3 startPoint, Vector3 targetPoint);

		/// <summary>
		/// Fires this projectile in a designated direction.
		/// </summary>
		/// <param name="startPoint">Start point of the flight.</param>
		/// <param name="fireVector">Vector representing direction of flight.</param>
		void FireInDirection(Vector3 startPoint, Vector3 fireVector);

		/// <summary>
		/// Fires this projectile at a designated starting velocity, overriding any starting speeds.
		/// </summary>
		/// <param name="startPoint">Start point of the flight.</param>
		/// <param name="fireVelocity">Vector3 representing launch velocity.</param>
		void FireAtVelocity(Vector3 startPoint, Vector3 fireVelocity);
	}
}