using System;
using UnityEngine;

namespace Core.Utilities
{
	/// <summary>
	/// Structure to contain cubic coordinates for hexagonal grids. Provides a derived Z coordinate where
	/// z = x + y, providing a new third axis
	/// </summary>
	public struct HexPoint : IEquatable<HexPoint>
	{
		/// <summary>
		/// X-coordinate of hexagon point
		/// </summary>
		public readonly int x;

		/// <summary>
		/// Y-coordinate of hexagon point
		/// </summary>
		public readonly int y;

		/// <summary>
		/// Z-coordinate of hexagon point. This value is derived from x and y
		/// </summary>
		public readonly int z;

		/// <summary>
		/// Calculates the magnitude of this Hex point vector (its hex distance from the origin
		/// </summary>
		public int magnitude
		{
			get { return (Mathf.Abs(x) + Mathf.Abs(y) + Mathf.Abs(z)) / 2; }
		}

		/// <summary>
		/// Initialize a new hex point with two x,y coordinates
		/// </summary>
		public HexPoint(int x, int y)
		{
			this.x = x;
			this.y = y;
			this.z = x + y;
		}

		/// <summary>
		/// Initialized a new hex point with x and z coordinates
		/// </summary>
		public static HexPoint FromXZ(int x, int z)
		{
			int y = z - x;
			return new HexPoint(x, y);
		}

		/// <summary>
		/// Initialized a new hex point with y and z coordinates
		/// </summary>
		public static HexPoint FromYZ(int y, int z)
		{
			int x = z - y;
			return new HexPoint(x, y);
		}

		public bool Equals(HexPoint other)
		{
			return other.x == x && other.y == y;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}
			return obj is HexPoint && Equals((HexPoint) obj);
		}

		/// <summary>
		/// Simple hash multiplying by two primes
		/// </summary>
		public override int GetHashCode()
		{
			unchecked
			{
				return (x.GetHashCode() * 22447) ^ (y.GetHashCode() * 31);
			}
		}

		public override string ToString()
		{
			return string.Format("X: {0}, Y: {1}, Z: {2}", x, y, z);
		}

		/// <summary>
		/// Rotate the given hex point by 60 degrees counterclockwise, around the origin
		/// </summary>
		public static HexPoint RotateLeft(HexPoint original)
		{
			return new HexPoint(-original.y, original.z);
		}

		/// <summary>
		/// Rotate the given hex point by 60 degrees counterclockwise, around the given point
		/// </summary>
		public static HexPoint RotateLeft(HexPoint original, HexPoint origin)
		{
			return RotateLeft(original - origin) + origin;
		}

		/// <summary>
		/// Rotate the given hex point by 60 degrees clockwise, around the origin
		/// </summary>
		public static HexPoint RotateRight(HexPoint original)
		{
			return new HexPoint(original.z, -original.x);
		}

		/// <summary>
		/// Rotate the given hex point by 60 degrees clockwise, around the given point
		/// </summary>
		public static HexPoint RotateRight(HexPoint original, HexPoint origin)
		{
			return RotateRight(original - origin) + origin;
		}

		/// <summary>
		/// Rotate the given hex point by 120 degrees counterclockwise, around the origin
		/// </summary>
		public static HexPoint RotateLeft120(HexPoint original)
		{
			return new HexPoint(-original.z, original.x);
		}

		/// <summary>
		/// Rotate the given hex point by 120 degrees counterclockwise, around the the given point
		/// </summary>
		public static HexPoint RotateLeft120(HexPoint original, HexPoint origin)
		{
			return RotateLeft120(original - origin) + origin;
		}

		/// <summary>
		/// Rotate the given hex point by 120 degrees clockwise, around the origin
		/// </summary>
		public static HexPoint RotateRight120(HexPoint original)
		{
			return new HexPoint(original.y, -original.z);
		}

		/// <summary>
		/// Rotate the given hex point by 120 degrees clockwise, around the the given point
		/// </summary>
		public static HexPoint RotateRight120(HexPoint original, HexPoint origin)
		{
			return RotateRight120(original - origin) + origin;
		}

		/// <summary>
		/// Rotate the given hex point 180 degrees around the origin
		/// </summary>
		public static HexPoint Rotate180(HexPoint original)
		{
			return new HexPoint(-original.x, -original.y);
		}

		/// <summary>
		/// Rotate the given hex point 180 degrees around the the given point
		/// </summary>
		public static HexPoint Rotate180(HexPoint original, HexPoint origin)
		{
			return Rotate180(original - origin) + origin;
		}

		/// <summary>
		/// Reflect the given hex point around the x-axis
		/// </summary>
		public static HexPoint ReflectX(HexPoint original)
		{
			int x = original.z;
			int y = -original.y;

			return new HexPoint(x, y);
		}

		/// <summary>
		/// Reflect the given hex point around the line where y is the given value
		/// </summary>
		public static HexPoint ReflectX(HexPoint original, int y)
		{
			var offset = new HexPoint(0, y);

			return ReflectX(original - offset) + offset;
		}

		/// <summary>
		/// Reflect the given hex point around the y-axis
		/// </summary>
		public static HexPoint ReflectY(HexPoint original)
		{
			int x = -original.x;
			int y = original.z;

			return new HexPoint(x, y);
		}

		/// <summary>
		/// Reflect the given hex point around the line where x is the given value
		/// </summary>
		public static HexPoint ReflectY(HexPoint original, int x)
		{
			var offset = new HexPoint(x, 0);

			return ReflectY(original - offset) + offset;
		}

		/// <summary>
		/// Reflect the given hex point around the z-axis
		/// </summary>
		public static HexPoint ReflectZ(HexPoint original)
		{
			int x = -original.y;
			int y = -original.x;

			return new HexPoint(x, y);
		}

		/// <summary>
		/// Reflect the given hex point around the line where z is the given value
		/// </summary>
		public static HexPoint ReflectZ(HexPoint original, int z)
		{
			var offset = FromXZ(0, z);

			return ReflectZ(original - offset) + offset;
		}
		
		// Math operators and conversions
		// Equality operators
		public static bool operator ==(HexPoint left, HexPoint right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(HexPoint left, HexPoint right)
		{
			return !left.Equals(right);
		}
		
		// Conversion to and from IntVector2
		public static explicit operator IntVector2(HexPoint hexPoint)
		{
			return new IntVector2(hexPoint.x, hexPoint.y);
		}
		
		public static explicit operator HexPoint(IntVector2 vector)
		{
			return new HexPoint(vector.x, vector.y);
		}
		
		// Math operators
		public static HexPoint operator +(HexPoint left, HexPoint right)
		{
			return new HexPoint(left.x + right.x, left.y + right.y);
		}

		public static HexPoint operator -(HexPoint left, HexPoint right)
		{
			return new HexPoint(left.x - right.x, left.y - right.y);
		}

		public static HexPoint operator *(int scale, HexPoint right)
		{
			return new HexPoint(right.x * scale, right.y * scale);
		}

		public static HexPoint operator *(HexPoint left, int scale)
		{
			return new HexPoint(left.x * scale, left.y * scale);
		}

		public static HexPoint operator -(HexPoint left)
		{
			return new HexPoint(-left.x, -left.y);
		}
	}
}