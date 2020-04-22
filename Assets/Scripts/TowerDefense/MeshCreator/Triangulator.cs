using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense.MeshCreator
{
	public class Triangulator
	{
		readonly List<Vector2> m_Points;

		public Triangulator(Vector2[] points)
		{
			m_Points = new List<Vector2>(points);
		}

		/// <summary>
		/// Triangulates the mesh
		/// </summary>
		/// <returns>Array of triangle indices</returns>
		public int[] Triangulate()
		{
			List<int> indices = new List<int>();

			int numPoints = m_Points.Count;
			if (numPoints < 3)
			{
				return indices.ToArray();
			}

			int[] workingIndices = new int[numPoints];
			if (Area() > 0)
			{
				for (int v = 0; v < numPoints; v++)
				{
					workingIndices[v] = v;
				}
			}
			else
			{
				for (int v = 0; v < numPoints; v++)
				{
					workingIndices[v] = numPoints - 1 - v;
				}
			}

			int nv = numPoints;
			int count = 2 * nv;

			for (int v = nv - 1; nv > 2;)
			{
				if (count-- <= 0)
				{
					return indices.ToArray();
				}

				int u = v;
				if (nv <= u)
				{
					u = 0;
				}

				v = u + 1;
				if (nv <= v)
				{
					v = 0;
				}

				int w = v + 1;
				if (nv <= w)
				{
					w = 0;
				}

				if (Snip(u, v, w, nv, workingIndices))
				{
					int a, b, c, s, t;
					a = workingIndices[u];
					b = workingIndices[v];
					c = workingIndices[w];
					indices.Add(a);
					indices.Add(b);
					indices.Add(c);

					for (s = v, t = v + 1; t < nv; s++, t++)
					{
						workingIndices[s] = workingIndices[t];
					}
					nv--;
					count = 2 * nv;
				}
			}

			indices.Reverse();
			return indices.ToArray();
		}

		/// <summary>
		/// Area of the triangle
		/// </summary>
		/// <returns>Area of triangel</returns>
		float Area()
		{
			int n = m_Points.Count;
			float area = 0.0f;
			for (int p = n - 1, q = 0; q < n; p = q++)
			{
				Vector2 pval = m_Points[p];
				Vector2 qval = m_Points[q];
				area += (pval.x * qval.y) - (qval.x * pval.y);
			}
			return area * 0.5f;
		}

		bool Snip(int u, int v, int w, int count, int[] indices)
		{
			Vector2 a = m_Points[indices[u]];
			Vector2 b = m_Points[indices[v]];
			Vector2 c = m_Points[indices[w]];

			if (Mathf.Epsilon > ((b.x - a.x) * (c.y - a.y)) - ((b.y - a.y) * (c.x - a.x)))
			{
				return false;
			}

			for (int i = 0; i < count; i++)
			{
				if ((i == u) || (i == v) || (i == w))
				{
					continue;
				}

				Vector2 p = m_Points[indices[i]];

				if (InsideTriangle(a, b, c, p))
				{
					return false;
				}
			}
			return true;
		}

		static bool InsideTriangle(Vector2 a, Vector2 b, Vector2 c, Vector2 p)
		{
			float ax, ay, bx, by, cx, cy, apx, apy, bpx, bpy, cpx, cpy;
			float cCrosSap, bCrosScp, aCrosSbp;

			ax = c.x - b.x;
			ay = c.y - b.y;
			bx = a.x - c.x;
			by = a.y - c.y;
			cx = b.x - a.x;
			cy = b.y - a.y;
			apx = p.x - a.x;
			apy = p.y - a.y;
			bpx = p.x - b.x;
			bpy = p.y - b.y;
			cpx = p.x - c.x;
			cpy = p.y - c.y;

			aCrosSbp = (ax * bpy) - (ay * bpx);
			cCrosSap = (cx * apy) - (cy * apx);
			bCrosScp = (bx * cpy) - (by * cpx);

			return (aCrosSbp >= 0.0f) && (bCrosScp >= 0.0f) && (cCrosSap >= 0.0f);
		}
	}
}