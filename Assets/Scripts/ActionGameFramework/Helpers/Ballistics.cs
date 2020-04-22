using ActionGameFramework.Projectiles;
using UnityEngine;

namespace ActionGameFramework.Helpers
{
	/// <summary>
	/// Helper class to assist with calculation of common projectile ballistics problems.
	/// </summary>
	public static class Ballistics
	{
		/// <summary>
		/// Calculates the initial velocity of a linear projectile aimed at a given world coordinate.
		/// </summary>
		/// <param name="firePosition">Starting point of the projectile.</param>
		/// <param name="targetPosition">Intended target point of the projectile.</param>
		/// <param name="launchSpeed">Initial speed of the projectile.</param>
		/// <returns>Vector3 describing initial velocity for this projectile. Vector3.zero if no solution.</returns>
		public static Vector3 CalculateLinearFireVector(Vector3 firePosition, Vector3 targetPosition,
		                                                float launchSpeed)
		{
			// If we're starting with a zero initial velocity, we give the vector a tiny base magnitude
			if (Mathf.Abs(launchSpeed) < float.Epsilon)
			{
				launchSpeed = 0.001f;
			}

			return (targetPosition - firePosition).normalized * launchSpeed;
		}

		/// <summary>
		/// Calculates the time taken for a linear projectile to reach the specified destination, with a given
		/// start speed and acceleration.
		/// </summary>
		/// <param name="firePosition">Starting point of the projectile.</param>
		/// <param name="targetPosition">Intended target point of the projectile.</param>
		/// <param name="launchSpeed">Initial speed of the projectile.</param>
		/// <param name="acceleration">Post-firing acceleration of the projectile.</param>
		/// <returns>Time in seconds to complete flight to target.</returns>
		public static float CalculateLinearFlightTime(Vector3 firePosition, Vector3 targetPosition,
		                                              float launchSpeed, float acceleration)
		{
			float flightDistance = (targetPosition - firePosition).magnitude;

			// v^2 = u^2 + 2as
			float endV = Mathf.Sqrt((launchSpeed * launchSpeed) + (2 * acceleration * flightDistance));

			// t = 2s/(u+v)
			return (2f * flightDistance) / (launchSpeed + endV);
		}

		/// <summary>
		/// Calculates a leading target point that ensures a linear projectile will impact a moving target.
		/// Assumes target has constant velocity. Precision can be adjusted parametrically.
		/// </summary>
		/// <param name="firePosition">Starting point of the projectile.</param>
		/// <param name="targetPosition">The current position of the intended target.</param>
		/// <param name="targetVelocity">Vector representing the velocity of the intended target.</param>
		/// <param name="launchSpeed">Initial speed of the projectile.</param>
		/// <param name="acceleration">Post-firing acceleration of the projectile.</param>
		/// <param name="precision">Number of iterations to approximate the correct position. Higher precision is better for faster targets.</param>
		/// <returns>Vector3 representing the leading target point.</returns>
		public static Vector3 CalculateLinearLeadingTargetPoint(Vector3 firePosition, Vector3 targetPosition,
		                                                        Vector3 targetVelocity, float launchSpeed, float acceleration,
		                                                        int precision = 2)
		{
			// No precision means no leading, so we early-out.
			if (precision <= 0)
			{
				return targetPosition;
			}

			Vector3 testPosition = targetPosition;

			for (int i = 0; i < precision; i++)
			{
				float impactTime = CalculateLinearFlightTime(firePosition, testPosition, launchSpeed,
				                                             acceleration);

				testPosition = targetPosition + (targetVelocity * impactTime);
			}

			return testPosition;
		}

		/// <summary>
		/// Calculates the launch velocity for a parabolic-path projectile to hit a given target point when fired
		/// at a given angle.
		/// </summary>
		/// <param name="firePosition">Position from which the projectile is fired.</param>
		/// <param name="targetPosition">Intended target position.</param>
		/// <param name="launchAngle">Angle at which the projectile is to be fired.</param>
		/// <param name="gravity">Gravitational constant (Vertical only. Positive = down)</param>
		/// <returns>Vector3 representing launch velocity to hit the target. Vector3.zero if no solution.</returns>
		public static Vector3 CalculateBallisticFireVectorFromAngle(Vector3 firePosition, Vector3 targetPosition,
		                                                            float launchAngle, float gravity)
		{
			Vector3 target = targetPosition;
			target.y = firePosition.y;
			Vector3 toTarget = target - firePosition;
			float targetDistance = toTarget.magnitude;
			float shootingAngle = launchAngle;
			float relativeY = firePosition.y - targetPosition.y;

			float theta = Mathf.Deg2Rad * shootingAngle;
			float cosTheta = Mathf.Cos(theta);
			float num = targetDistance * Mathf.Sqrt(gravity) * Mathf.Sqrt(1 / cosTheta);
			float denom = Mathf.Sqrt((2 * targetDistance * Mathf.Sin(theta)) + (2 * relativeY * cosTheta));

			if (denom > 0)
			{
				float v = num / denom;

				// Flatten aim vector so we can rotate it
				Vector3 aimVector = toTarget / targetDistance;
				aimVector.y = 0;
				Vector3 rotAxis = Vector3.Cross(aimVector, Vector3.up);
				Quaternion rotation = Quaternion.AngleAxis(shootingAngle, rotAxis);
				aimVector = rotation * aimVector.normalized;

				return aimVector * v;
			}

			return Vector3.zero;
		}

		/// <summary>
		/// Calculates the launch velocity for a parabolic-path projectile to hit a given target point when fired
		/// at a given angle. Uses vertical gravity constant defined in project Physics settings.
		/// </summary>
		/// <param name="firePosition">Position from which the projectile is fired.</param>
		/// <param name="targetPosition">Intended target position.</param>
		/// <param name="launchAngle">Angle at which the projectile is to be fired.</param>
		/// <returns>Vector3 representing launch velocity to hit the target. Vector3.zero if no solution.</returns>
		public static Vector3 CalculateBallisticFireVectorFromAngle(Vector3 firePosition, Vector3 targetPosition,
		                                                            float launchAngle)
		{
			return CalculateBallisticFireVectorFromAngle(firePosition, targetPosition, launchAngle,
			                                             Mathf.Abs(Physics.gravity.y));
		}

		/// <summary>
		/// Calculates the launch velocity for a parabolic-path projectile to hit a given target point when
		/// fired at a given speed.
		/// </summary>
		/// <param name="firePosition">Position from which the projectile is fired.</param>
		/// <param name="targetPosition">Intended target position.</param>
		/// <param name="launchSpeed">The speed that the projectile is launched at.</param>
		/// <param name="arcHeight">Preference between parabolic ("underhand") or direct ("overhand") projectile arc.</param>
		/// <param name="gravity">Gravitational constant (Vertical only. Positive = down)</param>
		/// <returns>Vector3 representing launch launchSpeed to hit the target. Vector3.zero if no solution.</returns>
		public static Vector3 CalculateBallisticFireVectorFromVelocity(Vector3 firePosition, Vector3 targetPosition,
		                                                               float launchSpeed, BallisticArcHeight arcHeight,
		                                                               float gravity)
		{
			float theta = CalculateBallisticFireAngle(firePosition, targetPosition, launchSpeed, arcHeight, gravity);

			// If our angle is impossible, we early-out.
			if (float.IsNaN(theta))
			{
				return Vector3.zero;
			}

			Vector3 target = targetPosition;
			target.y = firePosition.y;
			Vector3 toTarget = target - firePosition;

			float targetDistance = toTarget.magnitude;

			Vector3 aimVector = Vector3.forward;

			if (targetDistance > 0f)
			{
				// Flatten aim vector so we can rotate it
				aimVector = toTarget / targetDistance;
				aimVector.y = 0;
			}

			Vector3 rotAxis = Vector3.Cross(aimVector, Vector3.up);
			Quaternion rotation = Quaternion.AngleAxis(theta, rotAxis);
			aimVector = rotation * aimVector.normalized;

			return aimVector * launchSpeed;
		}

		/// <summary>
		/// Calculates the launch velocity for a parabolic-path projectile to hit a given target point when
		/// fired at a given speed. Uses vertical gravity constant defined in project Physics settings.
		/// </summary>
		/// <param name="firePosition">Position from which the projectile is fired.</param>
		/// <param name="targetPosition">Intended target position.</param>
		/// <param name="launchSpeed">The speed that the projectile is launched at.</param>
		/// <param name="arcHeight">Preference between parabolic ("underhand") or direct ("overhand") projectile arc.</param>
		/// <returns>Vector3 representing launch launchSpeed to hit the target. Vector3.zero if no solution.</returns>
		public static Vector3 CalculateBallisticFireVectorFromVelocity(Vector3 firePosition, Vector3 targetPosition,
		                                                               float launchSpeed, BallisticArcHeight arcHeight)
		{
			return CalculateBallisticFireVectorFromVelocity(firePosition, targetPosition, launchSpeed, arcHeight,
			                                                Mathf.Abs(Physics.gravity.y));
		}

		/// <summary>
		/// Calculates the angle at which a projectile with a given initial speed must be fired to impact a target.
		/// </summary>
		/// <param name="firePosition">Position from which the projectile is fired</param>
		/// <param name="targetPosition">Intended target position.</param>
		/// <param name="launchSpeed">The speed that the projectile is launched at.</param>
		/// <param name="arcHeight">Preference between parabolic ("underhand") or direct ("overhand") projectile arc.</param>
		/// <param name="gravity">Gravitational constant (Vertical only. Positive = down)</param>
		/// <returns>The required launch angle in degrees. NaN if no valid solution.</returns>
		public static float CalculateBallisticFireAngle(Vector3 firePosition, Vector3 targetPosition,
		                                                float launchSpeed, BallisticArcHeight arcHeight, float gravity)
		{
			Vector3 target = targetPosition;
			target.y = firePosition.y;
			Vector3 toTarget = target - firePosition;
			float targetDistance = toTarget.magnitude;
			float relativeY = targetPosition.y - firePosition.y;
			float vSquared = launchSpeed * launchSpeed;

			// If the distance to our target is zero, we can assume it's right on top of us (or that we're our own target).
			if (Mathf.Approximately(targetDistance, 0f))
			{
				// If we're preferring a high-angle shot, we just fire straight up.
				if (arcHeight == BallisticArcHeight.UseHigh || arcHeight == BallisticArcHeight.PreferHigh)
				{
					return 90f;
				}

				// If we're doing a low-angle direct shot, we tweak our angle based on relative height of target.
				if (relativeY > 0)
				{
					return 90f;
				}

				if (relativeY < 0)
				{
					return -90f;
				}
			}

			float b = Mathf.Sqrt((vSquared * vSquared) -
			                     (gravity * ((gravity * (targetDistance * targetDistance)) + (2 * relativeY * vSquared))));

			// The "underarm", parabolic arc angle
			float theta1 = Mathf.Atan((vSquared + b) / (gravity * targetDistance));

			// The "overarm", direct arc angle
			float theta2 = Mathf.Atan((vSquared - b) / (gravity * targetDistance));

			bool theta1Nan = float.IsNaN(theta1);
			bool theta2Nan = float.IsNaN(theta2);

			// If both are invalid, we early-out with a NaN to indicate no solution.
			if (theta1Nan && theta2Nan)
			{
				return float.NaN;
			}

			// We'll init with the parabolic arc.
			float returnTheta = theta1;

			// If we want to return the direct arc
			if (arcHeight == BallisticArcHeight.UseLow)
			{
				returnTheta = theta2;
			}

			// If we want to return theta1 wherever valid, but will settle for theta2 if theta1 is invalid
			if (arcHeight == BallisticArcHeight.PreferHigh)
			{
				returnTheta = theta1Nan ? theta2 : theta1;
			}

			// If we want to return theta2 wherever valid, but will settle for theta1 if theta2 is invalid
			if (arcHeight == BallisticArcHeight.PreferLow)
			{
				returnTheta = theta2Nan ? theta1 : theta2;
			}

			return returnTheta * Mathf.Rad2Deg;
		}

		/// <summary>
		/// Calculates the angle at which a projectile with a given initial speed must be fired to impact a target.
		/// Uses vertical gravity constant defined in project Physics settings.
		/// </summary>
		/// <param name="firePosition">Position from which the projectile is fired</param>
		/// <param name="targetPosition">Intended target position.</param>
		/// <param name="launchSpeed">The speed that the projectile is launched at.</param>
		/// <param name="arcHeight">Preference between parabolic ("underhand") or direct ("overhand") projectile arc.</param>
		/// <returns>The required launch angle in degrees. NaN if no valid solution.</returns>
		public static float CalculateBallisticFireAngle(Vector3 firePosition, Vector3 targetPosition,
		                                                float launchSpeed, BallisticArcHeight arcHeight)
		{
			return CalculateBallisticFireAngle(firePosition, targetPosition, launchSpeed, arcHeight,
			                                   Mathf.Abs(Physics.gravity.y));
		}

		/// <summary>
		/// Calculates the amount of time it will take a projectile to complete its arc.
		/// </summary>
		/// <param name="firePosition">Position from which the projectile is fired</param>
		/// <param name="targetPosition">Intended target position.</param>
		/// <param name="launchSpeed">The speed that the projectile is launched at.</param>
		/// <param name="fireAngle">The angle in degrees that the projectile was fired at.</param>
		/// <param name="gravity">Gravitational constant (Vertical only. Positive = down)</param>
		/// <returns>Time in seconds to complete arc to target. NaN if no valid solution.</returns>
		public static float CalculateBallisticFlightTime(Vector3 firePosition, Vector3 targetPosition, float launchSpeed,
		                                                 float fireAngle, float gravity)
		{
			float relativeY = firePosition.y - targetPosition.y;

			Vector3 targetVector = targetPosition - firePosition;

			targetVector.y = 0;

			float targetDistance = targetVector.magnitude;

			fireAngle *= Mathf.Deg2Rad;

			float sinFireAngle = Mathf.Sin(fireAngle);

			float a = (launchSpeed * Mathf.Sin(fireAngle)) / gravity;
			float b = Mathf.Sqrt((launchSpeed * launchSpeed * (sinFireAngle * sinFireAngle)) + (2 * gravity * relativeY)) /
			          gravity;

			float flightTime1 = a + b;
			float flightTime2 = a - b;

			float flightDistance1 = launchSpeed * Mathf.Cos(fireAngle) * flightTime1;
			float flightDistance2 = launchSpeed * Mathf.Cos(fireAngle) * flightTime2;

			if (flightTime2 > 0)
			{
				if (Mathf.Abs(targetDistance - flightDistance2) < Mathf.Abs(targetDistance - flightDistance1))
				{
					return flightTime2;
				}
			}

			return flightTime1;
		}

		/// <summary>
		/// Calculates the amount of time it will take a projectile to complete its arc.
		/// Uses vertical gravity constant defined in project Physics settings.
		/// </summary>
		/// <param name="firePosition">Position from which the projectile is fired</param>
		/// <param name="targetPosition">Intended target position.</param>
		/// <param name="launchSpeed">The speed that the projectile is launched at.</param>
		/// <param name="fireAngle">The angle in degrees that the projectile was fired at.</param>
		/// <returns>Time in seconds to complete arc to target. NaN if no valid solution.</returns>
		public static float CalculateBallisticFlightTime(Vector3 firePosition, Vector3 targetPosition,
		                                                 float launchSpeed, float fireAngle)
		{
			return CalculateBallisticFlightTime(firePosition, targetPosition, launchSpeed, fireAngle,
			                                    Mathf.Abs(Physics.gravity.y));
		}

		/// <summary>
		/// Calculates an approximate leading target point to ensure a ballistic projectile will impact a moving target assuming a given launch speed.
		/// Assumes constant target velocity and constant projectile speed after launch. Precision can be adjusted parametrically.
		/// </summary>
		/// <param name="firePosition">Starting point of the projectile.</param>
		/// <param name="targetPosition">The current position of the intended target.</param>
		/// <param name="targetVelocity">Vector representing the velocity of the intended target.</param>
		/// <param name="launchSpeed">Initial speed of the projectile.</param>
		/// <param name="arcHeight">Preference between parabolic ("underhand") or direct ("overhand") projectile arc.</param>
		/// <param name="precision">Number of iterations to approximate the correct position. Higher precision is better for faster targets.</param>
		/// <param name="gravity">Gravitational constant (Vertical only. Positive = down)</param>
		/// <returns>Vector3 representing the leading target point. Vector3.zero if no solution.</returns>
		public static Vector3 CalculateBallisticLeadingTargetPointWithSpeed(Vector3 firePosition, Vector3 targetPosition,
		                                                                    Vector3 targetVelocity, float launchSpeed,
		                                                                    BallisticArcHeight arcHeight, float gravity,
		                                                                    int precision = 2)
		{
			// No precision means no leading, so we early-out.
			if (precision <= 1)
			{
				return targetPosition;
			}

			Vector3 testPosition = targetPosition;

			for (int i = 0; i < precision; i++)
			{
				float fireAngle = CalculateBallisticFireAngle(firePosition, testPosition, launchSpeed, arcHeight, gravity);

				float impactTime = CalculateBallisticFlightTime(firePosition, testPosition, launchSpeed, fireAngle, gravity);

				if (float.IsNaN(fireAngle) || float.IsNaN(impactTime))
				{
					return Vector3.zero;
				}

				testPosition = targetPosition + (targetVelocity * impactTime);
			}

			return testPosition;
		}

		/// <summary>
		/// Calculates an approximate leading target point to ensure a ballistic projectile will impact a moving target assuming a given launch speed.
		/// Assumes constant target velocity and constant projectile speed after launch. Precision can be adjusted parametrically.
		/// Uses vertical gravity constant defined in project Physics settings.
		/// </summary>
		/// <param name="firePosition">Starting point of the projectile.</param>
		/// <param name="targetPosition">The current position of the intended target.</param>
		/// <param name="targetVelocity">Vector representing the velocity of the intended target.</param>
		/// <param name="launchSpeed">Initial speed of the projectile.</param>
		/// <param name="arcHeight">Preference between parabolic ("underhand") or direct ("overhand") projectile arc.</param>
		/// <param name="precision">Number of iterations to approximate the correct position. Higher precision is better for faster targets.</param>
		/// <returns>Vector3 representing the leading target point. Vector3.zero if no solution.</returns>
		public static Vector3 CalculateBallisticLeadingTargetPointWithSpeed(Vector3 firePosition, Vector3 targetPosition,
		                                                                    Vector3 targetVelocity, float launchSpeed,
		                                                                    BallisticArcHeight arcHeight, int precision = 2)
		{
			return CalculateBallisticLeadingTargetPointWithSpeed(firePosition, targetPosition, targetVelocity, launchSpeed,
			                                                     arcHeight, Mathf.Abs(Physics.gravity.y), precision);
		}

		/// <summary>
		/// Calculates an approximate leading target point to ensure a ballistic projectile will impact a moving target assuming a given launch angle.
		/// Assumes constant target velocity and constant projectile speed after launch. Precision can be adjusted parametrically.
		/// Uses vertical gravity constant defined in project Physics settings.
		/// </summary>
		/// <param name="firePosition">Starting point of the projectile.</param>
		/// <param name="targetPosition">The current position of the intended target.</param>
		/// <param name="targetVelocity">Vector representing the velocity of the intended target.</param>
		/// <param name="launchAngle">The angle at which the projectile is to be launched.</param>
		/// <param name="arcHeight">Preference between parabolic ("underhand") or direct ("overhand") projectile arc.</param>
		/// <param name="gravity">Gravitational constant (Vertical only. Positive = down)</param>
		/// <param name="precision">Number of iterations to approximate the correct position. Higher precision is better for faster targets.</param>
		/// <returns>Vector3 representing the leading target point. Vector3.zero if no solution.</returns>
		public static Vector3 CalculateBallisticLeadingTargetPointWithAngle(Vector3 firePosition,
		                                                                    Vector3 targetPosition,
		                                                                    Vector3 targetVelocity, float launchAngle,
		                                                                    BallisticArcHeight arcHeight, float gravity,
		                                                                    int precision = 2)
		{
			// No precision means no leading, so we early-out.
			if (precision <= 1)
			{
				return targetPosition;
			}

			Vector3 testPosition = targetPosition;

			for (int i = 0; i < precision; i++)
			{
				float launchSpeed = CalculateBallisticFireVectorFromAngle(firePosition, testPosition, launchAngle, gravity)
					.magnitude;

				float impactTime = CalculateBallisticFlightTime(firePosition, testPosition, launchSpeed, launchAngle, gravity);

				if (float.IsNaN(launchSpeed) || float.IsNaN(impactTime))
				{
					return Vector3.zero;
				}

				testPosition = targetPosition + (targetVelocity * impactTime);
			}

			return testPosition;
		}

		/// <summary>
		/// Calculates an approximate leading target point to ensure a ballistic projectile will impact a moving target assuming a given launch angle.
		/// Assumes constant target velocity and constant projectile speed after launch. Precision can be adjusted parametrically.
		/// Uses vertical gravity constant defined in project Physics settings.
		/// </summary>
		/// <param name="firePosition">Starting point of the projectile.</param>
		/// <param name="targetPosition">The current position of the intended target.</param>
		/// <param name="targetVelocity">Vector representing the velocity of the intended target.</param>
		/// <param name="launchAngle">The angle at which the projectile is to be launched.</param>
		/// <param name="arcHeight">Preference between parabolic ("underhand") or direct ("overhand") projectile arc.</param>
		/// <param name="precision">Number of iterations to approximate the correct position. Higher precision is better for faster targets.</param>
		/// <returns>Vector3 representing the leading target point. Vector3.zero if no solution.</returns>
		public static Vector3 CalculateBallisticLeadingTargetPointWithAngle(Vector3 firePosition,
		                                                                    Vector3 targetPosition,
		                                                                    Vector3 targetVelocity, float launchAngle,
		                                                                    BallisticArcHeight arcHeight, int precision = 2)
		{
			return CalculateBallisticLeadingTargetPointWithAngle(firePosition, targetPosition, targetVelocity,
			                                                     launchAngle, arcHeight, Mathf.Abs(Physics.gravity.y),
			                                                     precision);
		}
	}
}