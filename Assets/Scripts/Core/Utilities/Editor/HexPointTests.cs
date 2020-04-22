using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Core.Utilities.Editor
{
	public struct TestHexPoint
	{
		public int x, y, z;

		public TestHexPoint(int x, int y, int z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public override string ToString()
		{
			return string.Format("{{{0}, {1}, {2}}}", x, y, z);
		}
	}
	
	[TestFixture]
	public class HexPointTests
	{
		static readonly TestHexPoint s_OffsetPosition = 
			new TestHexPoint(0, 2, 2);
		
		static readonly TestHexPoint[] s_ValidPoints = 
		{
			new TestHexPoint( 0,  0,  0),
			new TestHexPoint(-2,  2,  0),
			new TestHexPoint(-1, -2, -3),
			new TestHexPoint( 2,  0,  2),
			new TestHexPoint( 2,  1,  3),
			new TestHexPoint( 0, -2, -2),
		};
		
		static readonly TestHexPoint[] s_LeftRotatedValidPoints = 
		{
			new TestHexPoint( 0,  0,  0),
			new TestHexPoint(-2,  0, -2),
			new TestHexPoint( 2, -3, -1),
			new TestHexPoint( 0,  2,  2),
			new TestHexPoint(-1,  3,  2),
			new TestHexPoint( 2, -2,  0),
		};
		
		static readonly TestHexPoint[] s_LeftOffsetRotatedValidPoints = 
		{
			new TestHexPoint( 2,  0,  2),
			new TestHexPoint( 0,  0,  0),
			new TestHexPoint( 4, -3,  1),
			new TestHexPoint( 2,  2,  4),
			new TestHexPoint( 1,  3,  4),
			new TestHexPoint( 4, -2,  2),
		};
		
		static readonly TestHexPoint[] s_RightRotatedValidPoints = 
		{
			new TestHexPoint( 0,  0,  0),
			new TestHexPoint( 0,  2,  2),
			new TestHexPoint(-3,  1, -2),
			new TestHexPoint( 2, -2,  0),
			new TestHexPoint( 3, -2,  1),
			new TestHexPoint(-2,  0, -2),
		};
		
		static readonly TestHexPoint[] s_RightOffsetRotatedValidPoints = 
		{
			new TestHexPoint(-2,  2,  0),
			new TestHexPoint(-2,  4,  2),
			new TestHexPoint(-5,  3, -2),
			new TestHexPoint( 0,  0,  0),
			new TestHexPoint( 1,  0,  1),
			new TestHexPoint(-4,  2, -2),
		};
		
		static readonly TestHexPoint[] s_180RotatedValidPoints = 
		{
			new TestHexPoint( 0,  0,  0),
			new TestHexPoint( 2, -2,  0),
			new TestHexPoint( 1,  2,  3),
			new TestHexPoint(-2,  0, -2),
			new TestHexPoint(-2, -1, -3),
			new TestHexPoint( 0,  2,  2),
		};
		
		static readonly TestHexPoint[] s_XMirroredValidPoints = 
		{
			new TestHexPoint( 0,  0,  0),
			new TestHexPoint( 0, -2, -2),
			new TestHexPoint(-3,  2, -1),
			new TestHexPoint( 2,  0,  2),
			new TestHexPoint( 3, -1,  2),
			new TestHexPoint(-2,  2,  0),
		};
		static readonly TestHexPoint[] s_OffsetXMirroredValidPoints = 
		{
			new TestHexPoint(-1,  2,  1),
			new TestHexPoint(-1,  0, -1),
			new TestHexPoint(-4,  4,  0),
			new TestHexPoint( 1,  2,  3),
			new TestHexPoint( 2,  1,  3),
			new TestHexPoint(-3,  4,  1),
		};
		
		static readonly TestHexPoint[] s_YMirroredValidPoints = 
		{
			new TestHexPoint( 0,  0,  0),
			new TestHexPoint( 2,  0,  2),
			new TestHexPoint( 1, -3, -2),
			new TestHexPoint(-2,  2,  0),
			new TestHexPoint(-2,  3,  1),
			new TestHexPoint( 0, -2, -2),
		};
		
		static readonly TestHexPoint[] s_OffsetYMirroredValidPoints = 
		{
			new TestHexPoint( 2, -1,  1),
			new TestHexPoint( 4, -1,  3),
			new TestHexPoint( 3, -4, -1),
			new TestHexPoint( 0,  1,  1),
			new TestHexPoint( 0,  2,  2),
			new TestHexPoint( 2, -3, -1),
		};
		
		static readonly TestHexPoint[] s_ZMirroredValidPoints = 
		{
			new TestHexPoint( 0,  0,  0),
			new TestHexPoint(-2,  2,  0),
			new TestHexPoint( 2,  1,  3),
			new TestHexPoint( 0, -2, -2),
			new TestHexPoint(-1, -2, -3),
			new TestHexPoint( 2,  0,  2),
		};
		
		static readonly TestHexPoint[] s_OffsetZMirroredValidPoints = 
		{
			new TestHexPoint( 1,  1,  2),
			new TestHexPoint(-1,  3,  2),
			new TestHexPoint( 3,  2,  5),
			new TestHexPoint( 1, -1,  0),
			new TestHexPoint( 0, -1, -1),
			new TestHexPoint( 3,  1,  4),
		};

		#pragma warning disable 0414
		static readonly IEnumerable<TestHexPoint> s_AllPoints =
			s_ValidPoints.Concat(s_LeftRotatedValidPoints)
			             .Concat(s_LeftOffsetRotatedValidPoints)
			             .Concat(s_RightRotatedValidPoints)
			             .Concat(s_RightOffsetRotatedValidPoints)
			             .Concat(s_180RotatedValidPoints)
			             .Concat(s_XMirroredValidPoints)
			             .Concat(s_OffsetXMirroredValidPoints)
			             .Concat(s_YMirroredValidPoints)
			             .Concat(s_OffsetYMirroredValidPoints)
			             .Concat(s_ZMirroredValidPoints)
			             .Concat(s_OffsetZMirroredValidPoints);
		#pragma warning restore 0414
		
		[Test]
		public void ZCoordTest(
			[ValueSource("s_AllPoints")] TestHexPoint hexPoint)
		{
			// Arrange
			var point = new HexPoint(hexPoint.x, hexPoint.y);
			
			// Assert
			Assert.AreEqual(hexPoint.z, point.z);
		}
		
		[Test]
		public void XZCreationTest(
			[ValueSource("s_AllPoints")] TestHexPoint hexPoint)
		{
			// Arrange
			var point = HexPoint.FromXZ(hexPoint.x, hexPoint.z);
			
			// Assert
			Assert.AreEqual(hexPoint.y, point.y);
		}
		
		[Test]
		public void YZCreationTest(
			[ValueSource("s_AllPoints")] TestHexPoint hexPoint)
		{
			// Arrange
			var point = HexPoint.FromYZ(hexPoint.y, hexPoint.z);
			
			// Assert
			Assert.AreEqual(hexPoint.x, point.x);
		}

		[Test, Sequential]
		public void LeftRotateTest(
			[ValueSource("s_ValidPoints")] TestHexPoint source, 
			[ValueSource("s_LeftRotatedValidPoints")] TestHexPoint expected)
		{
			// Arrange
			var point = new HexPoint(source.x, source.y);
			var expectedPoint = new HexPoint(expected.x, expected.y);
			
			// Act
			point = HexPoint.RotateLeft(point);
			
			// Assert
			Assert.AreEqual(expectedPoint, point);
		}

		[Test, Sequential]
		public void LeftRotateOffsetTest(
			[ValueSource("s_ValidPoints")] TestHexPoint source, 
			[ValueSource("s_LeftOffsetRotatedValidPoints")] TestHexPoint expected)
		{
			// Arrange
			var point = new HexPoint(source.x, source.y);
			var expectedPoint = new HexPoint(expected.x, expected.y);
			var offsetPoint = new HexPoint(s_OffsetPosition.x, s_OffsetPosition.y);
			
			// Act
			point = HexPoint.RotateLeft(point, offsetPoint);
			
			// Assert
			Assert.AreEqual(expectedPoint, point);
		}

		[Test, Sequential]
		public void RightRotateTest(
			[ValueSource("s_ValidPoints")] TestHexPoint source, 
			[ValueSource("s_RightRotatedValidPoints")] TestHexPoint expected)
		{
			// Arrange
			var point = new HexPoint(source.x, source.y);
			var expectedPoint = new HexPoint(expected.x, expected.y);
			
			// Act
			point = HexPoint.RotateRight(point);
			
			// Assert
			Assert.AreEqual(expectedPoint, point);
		}

		[Test, Sequential]
		public void RightRotateOffsetTest(
			[ValueSource("s_ValidPoints")] TestHexPoint source, 
			[ValueSource("s_RightOffsetRotatedValidPoints")] TestHexPoint expected)
		{
			// Arrange
			var point = new HexPoint(source.x, source.y);
			var expectedPoint = new HexPoint(expected.x, expected.y);
			var offsetPoint = new HexPoint(s_OffsetPosition.x, s_OffsetPosition.y);
			
			// Act
			point = HexPoint.RotateRight(point, offsetPoint);
			
			// Assert
			Assert.AreEqual(expectedPoint, point);
		}

		[Test]
		public void LeftRightRotateIdentity(
			[Random(-10000, 10000, 5)] int x, 
			[Random(-10000, 10000, 5)] int y)
		{
			// Arrange
			var point = new HexPoint(x, y);
			var expectedPoint = new HexPoint(x, y);
			
			// Act
			point = HexPoint.RotateLeft(point);
			point = HexPoint.RotateRight(point);
			
			// Assert
			Assert.AreEqual(expectedPoint, point);
		}

		[Test, Sequential]
		public void OffsetLeftRightRotateIdentity(
			[Random(-10000, 10000, 25)] int x, 
			[Random(-10000, 10000, 25)] int y,
			[Random(-10000, 10000, 25)] int offsetX, 
			[Random(-10000, 10000, 25)] int offsetY)
		{
			// Arrange
			var point = new HexPoint(x, y);
			var expectedPoint = new HexPoint(x, y);
			var offsetPoint = new HexPoint(offsetX, offsetY);
			
			// Act
			point = HexPoint.RotateLeft(point, offsetPoint);
			point = HexPoint.RotateRight(point, offsetPoint);
			
			// Assert
			Assert.AreEqual(expectedPoint, point);
		}

		[Test]
		public void RightLeftRotateIdentity(
			[Random(-10000, 10000, 5)] int x, 
			[Random(-10000, 10000, 5)] int y)
		{
			// Arrange
			var point = new HexPoint(x, y);
			var expectedPoint = new HexPoint(x, y);
			
			// Act
			point = HexPoint.RotateRight(point);
			point = HexPoint.RotateLeft(point);
			
			// Assert
			Assert.AreEqual(expectedPoint, point);
		}

		[Test, Sequential]
		public void OffsetRightLeftRotateIdentity(
			[Random(-10000, 10000, 25)] int x, 
			[Random(-10000, 10000, 25)] int y,
			[Random(-10000, 10000, 25)] int offsetX, 
			[Random(-10000, 10000, 25)] int offsetY)
		{
			// Arrange
			var point = new HexPoint(x, y);
			var expectedPoint = new HexPoint(x, y);
			var offsetPoint = new HexPoint(offsetX, offsetY);
			
			// Act
			point = HexPoint.RotateRight(point, offsetPoint);
			point = HexPoint.RotateLeft(point, offsetPoint);
			
			// Assert
			Assert.AreEqual(expectedPoint, point);
		}

		[Test, Sequential]
		public void Right120RotateTest(
			[ValueSource("s_LeftRotatedValidPoints")] TestHexPoint source, 
			[ValueSource("s_RightRotatedValidPoints")] TestHexPoint expected)
		{
			// Arrange
			var point = new HexPoint(source.x, source.y);
			var expectedPoint = new HexPoint(expected.x, expected.y);
			
			// Act
			point = HexPoint.RotateRight120(point);
			
			// Assert
			Assert.AreEqual(expectedPoint, point);
		}

		[Test, Sequential]
		public void Left120RotateTest(
			[ValueSource("s_RightRotatedValidPoints")] TestHexPoint source, 
			[ValueSource("s_LeftRotatedValidPoints")] TestHexPoint expected)
		{
			// Arrange
			var point = new HexPoint(source.x, source.y);
			var expectedPoint = new HexPoint(expected.x, expected.y);
			
			// Act
			point = HexPoint.RotateLeft120(point);
			
			// Assert
			Assert.AreEqual(expectedPoint, point);
		}

		[Test, Sequential]
		public void Right120OffsetRotateTest(
			[ValueSource("s_LeftOffsetRotatedValidPoints")] TestHexPoint source, 
			[ValueSource("s_RightOffsetRotatedValidPoints")] TestHexPoint expected)
		{
			// Arrange
			var point = new HexPoint(source.x, source.y);
			var expectedPoint = new HexPoint(expected.x, expected.y);
			var offsetPoint = new HexPoint(s_OffsetPosition.x, s_OffsetPosition.y);
			
			// Act
			point = HexPoint.RotateRight120(point, offsetPoint);
			
			// Assert
			Assert.AreEqual(expectedPoint, point);
		}

		[Test, Sequential]
		public void Left120OffsetRotateTest(
			[ValueSource("s_RightOffsetRotatedValidPoints")] TestHexPoint source, 
			[ValueSource("s_LeftOffsetRotatedValidPoints")] TestHexPoint expected)
		{
			// Arrange
			var point = new HexPoint(source.x, source.y);
			var expectedPoint = new HexPoint(expected.x, expected.y);
			var offsetPoint = new HexPoint(s_OffsetPosition.x, s_OffsetPosition.y);
			
			// Act
			point = HexPoint.RotateLeft120(point, offsetPoint);
			
			// Assert
			Assert.AreEqual(expectedPoint, point);
		}

		[Test]
		public void DoubleLeftRotateTest(
			[Random(-10000, 10000, 5)] int x, 
			[Random(-10000, 10000, 5)] int y)
		{
			// Arrange
			var point = new HexPoint(x, y);
			var expectedPoint = HexPoint.RotateLeft(HexPoint.RotateLeft(point));
			
			// Act
			point = HexPoint.RotateLeft120(point);
			
			// Assert
			Assert.AreEqual(expectedPoint, point);
		}

		[Test, Sequential]
		public void DoubleLeftOffsetRotateTest(
			[Random(-10000, 10000, 25)] int x, 
			[Random(-10000, 10000, 25)] int y,
			[Random(-10000, 10000, 25)] int offsetX, 
			[Random(-10000, 10000, 25)] int offsetY)
		{
			// Arrange
			var point = new HexPoint(x, y);
			var offsetPoint = new HexPoint(offsetX, offsetY);
			var expectedPoint = HexPoint.RotateLeft(HexPoint.RotateLeft(point, offsetPoint), offsetPoint);
			
			// Act
			point = HexPoint.RotateLeft120(point, offsetPoint);
			
			// Assert
			Assert.AreEqual(expectedPoint, point);
		}

		[Test]
		public void DoubleRightRotateTest(
			[Random(-10000, 10000, 5)] int x, 
			[Random(-10000, 10000, 5)] int y)
		{
			// Arrange
			var point = new HexPoint(x, y);
			var expectedPoint = HexPoint.RotateRight(HexPoint.RotateRight(point));
			
			// Act
			point = HexPoint.RotateRight120(point);
			
			// Assert
			Assert.AreEqual(expectedPoint, point);
		}

		[Test, Sequential]
		public void DoubleRightOffsetRotateTest(
			[Random(-10000, 10000, 25)] int x, 
			[Random(-10000, 10000, 25)] int y,
			[Random(-10000, 10000, 25)] int offsetX, 
			[Random(-10000, 10000, 25)] int offsetY)
		{
			// Arrange
			var point = new HexPoint(x, y);
			var offsetPoint = new HexPoint(offsetX, offsetY);
			var expectedPoint = HexPoint.RotateRight(HexPoint.RotateRight(point, offsetPoint), offsetPoint);
			
			// Act
			point = HexPoint.RotateRight120(point, offsetPoint);
			
			// Assert
			Assert.AreEqual(expectedPoint, point);
		}

		[Test, Sequential]
		public void Rotate180Test(
			[ValueSource("s_ValidPoints")] TestHexPoint source, 
			[ValueSource("s_180RotatedValidPoints")] TestHexPoint expected)
		{
			// Arrange
			var point = new HexPoint(source.x, source.y);
			var expectedPoint = new HexPoint(expected.x, expected.y);
			
			// Act
			point = HexPoint.Rotate180(point);
			
			// Assert
			Assert.AreEqual(expectedPoint, point);
		}
		
		[Test]
		public void TripleRotateTest(
			[Random(-10000, 10000, 5)] int x, 
			[Random(-10000, 10000, 5)] int y)
		{
			// Arrange
			var pointA = new HexPoint(x, y);
			var pointB = new HexPoint(x, y);
			var expectedPoint = HexPoint.Rotate180(new HexPoint(x, y));
			
			// Act
			pointA = HexPoint.RotateLeft(HexPoint.RotateLeft(HexPoint.RotateLeft(pointA)));
			pointB = HexPoint.RotateRight(HexPoint.RotateRight(HexPoint.RotateRight(pointB)));
			
			// Assert
			Assert.AreEqual(expectedPoint, pointA);
			Assert.AreEqual(expectedPoint, pointB);
		}

		[Test, Sequential]
		public void ReflectXTest(
			[ValueSource("s_ValidPoints")] TestHexPoint source,
			[ValueSource("s_XMirroredValidPoints")] TestHexPoint expected)
		{
			// Arrange
			var point = new HexPoint(source.x, source.y);
			var expectedPoint = new HexPoint(expected.x, expected.y);
			
			// Act
			point = HexPoint.ReflectX(point);
			
			// Assert
			Assert.AreEqual(expectedPoint, point);
		}

		[Test, Sequential]
		public void ReflectXOffsetTest(
			[ValueSource("s_ValidPoints")] TestHexPoint source,
			[ValueSource("s_OffsetXMirroredValidPoints")] TestHexPoint expected)
		{
			// Arrange
			var point = new HexPoint(source.x, source.y);
			var expectedPoint = new HexPoint(expected.x, expected.y);
			
			// Act
			point = HexPoint.ReflectX(point, 1);
			
			// Assert
			Assert.AreEqual(expectedPoint, point);
		}

		[Test, Sequential]
		public void ReflectYTest(
			[ValueSource("s_ValidPoints")] TestHexPoint source,
			[ValueSource("s_YMirroredValidPoints")] TestHexPoint expected)
		{
			// Arrange
			var point = new HexPoint(source.x, source.y);
			var expectedPoint = new HexPoint(expected.x, expected.y);
			
			// Act
			point = HexPoint.ReflectY(point);
			
			// Assert
			Assert.AreEqual(expectedPoint, point);
		}

		[Test, Sequential]
		public void ReflectYOffsetTest(
			[ValueSource("s_ValidPoints")] TestHexPoint source,
			[ValueSource("s_OffsetYMirroredValidPoints")] TestHexPoint expected)
		{
			// Arrange
			var point = new HexPoint(source.x, source.y);
			var expectedPoint = new HexPoint(expected.x, expected.y);
			
			// Act
			point = HexPoint.ReflectY(point, 1);
			
			// Assert
			Assert.AreEqual(expectedPoint, point);
		}

		[Test, Sequential]
		public void ReflectZTest(
			[ValueSource("s_ValidPoints")] TestHexPoint source,
			[ValueSource("s_ZMirroredValidPoints")] TestHexPoint expected)
		{
			// Arrange
			var point = new HexPoint(source.x, source.y);
			var expectedPoint = new HexPoint(expected.x, expected.y);
			
			// Act
			point = HexPoint.ReflectZ(point);
			
			// Assert
			Assert.AreEqual(expectedPoint, point);
		}

		[Test, Sequential]
		public void ReflectZOffsetTest(
			[ValueSource("s_ValidPoints")] TestHexPoint source,
			[ValueSource("s_OffsetZMirroredValidPoints")] TestHexPoint expected)
		{
			// Arrange
			var point = new HexPoint(source.x, source.y);
			var expectedPoint = new HexPoint(expected.x, expected.y);
			
			// Act
			point = HexPoint.ReflectZ(point, 1);
			
			// Assert
			Assert.AreEqual(expectedPoint, point);
		}
	}
}