using System;
using Core.Utilities;
using TowerDefense.UI.HUD;
using UnityEngine;

namespace TowerDefense.Towers.Placement
{
	/// <summary>
	/// A tower placement location made from a grid.
	/// Its origin is centered in the middle of the lower-right cell. It can be oriented in any direction
	/// </summary>
	[RequireComponent(typeof(BoxCollider))]
	public class TowerPlacementGrid : MonoBehaviour, IPlacementArea
	{
		/// <summary>
		/// Prefab used to visualise the grid
		/// </summary>
		public PlacementTile placementTilePrefab;
		
		/// <summary>
		/// Visualisation prefab to instantiate on mobile platforms
		/// </summary>
		public PlacementTile placementTilePrefabMobile;

		/// <summary>
		/// The dimensions of the grid 
		/// </summary>
		public IntVector2 dimensions;

		/// <summary>
		/// Size of the edge of a cell
		/// </summary>
		[Tooltip("The size of the edge of one grid cell for this area. Should match the physical grid size of towers")]
		public float gridSize = 1;

		/// <summary>
		/// Inverted grid size, to multiply with
		/// </summary>
		float m_InvGridSize;

		/// <summary>
		/// Array of available cells
		/// </summary>
		bool[,] m_AvailableCells;

		/// <summary>
		/// Array of <see cref="PlacementTile"/>s
		/// </summary>
		PlacementTile[,] m_Tiles;

		/// <summary>
		/// Converts a location in world space into local grid coordinates.
		/// </summary>
		/// <param name="worldLocation"><see cref="Vector3"/> indicating world space coordinates to convert.</param>
		/// <param name="sizeOffset"><see cref="IntVector2"/> indicating size of object to center.</param>
		/// <returns><see cref="IntVector2"/> containing the grid coordinates corresponding to this location.</returns>
		public IntVector2 WorldToGrid(Vector3 worldLocation, IntVector2 sizeOffset)
		{
			Vector3 localLocation = transform.InverseTransformPoint(worldLocation);

			// Scale by inverse grid size
			localLocation *= m_InvGridSize;

			// Offset by half size
			var offset = new Vector3(sizeOffset.x * 0.5f, 0.0f, sizeOffset.y * 0.5f);
			localLocation -= offset;

			int xPos = Mathf.RoundToInt(localLocation.x);
			int yPos = Mathf.RoundToInt(localLocation.z);

			return new IntVector2(xPos, yPos);
		}

		/// <summary>
		/// Returns the world coordinates corresponding to a grid location.
		/// </summary>
		/// <param name="gridPosition">The coordinate in grid space</param>
		/// <param name="sizeOffset"><see cref="IntVector2"/> indicating size of object to center.</param>
		/// <returns>Vector3 containing world coordinates for specified grid cell.</returns>
		public Vector3 GridToWorld(IntVector2 gridPosition, IntVector2 sizeOffset)
		{
			// Calculate scaled local position
			Vector3 localPos = new Vector3(gridPosition.x + (sizeOffset.x * 0.5f), 0, gridPosition.y + (sizeOffset.y * 0.5f)) *
			                   gridSize;

			return transform.TransformPoint(localPos);
		}

		/// <summary>
		/// Tests whether the indicated cell range represents a valid placement location.
		/// </summary>
		/// <param name="gridPos">The grid location</param>
		/// <param name="size">The size of the item</param>
		/// <returns>Whether the indicated range is valid for placement.</returns>
		public TowerFitStatus Fits(IntVector2 gridPos, IntVector2 size)
		{
			// If the tile size of the tower exceeds the dimensions of the placement area, immediately decline placement.
			if ((size.x > dimensions.x) || (size.y > dimensions.y))
			{
				return TowerFitStatus.OutOfBounds;
			}

			IntVector2 extents = gridPos + size;

			// Out of range of our bounds
			if ((gridPos.x < 0) || (gridPos.y < 0) ||
			    (extents.x > dimensions.x) || (extents.y > dimensions.y))
			{
				return TowerFitStatus.OutOfBounds;
			}

			// Ensure there are no existing towers within our tile silhuette.
			for (int y = gridPos.y; y < extents.y; y++)
			{
				for (int x = gridPos.x; x < extents.x; x++)
				{
					if (m_AvailableCells[x, y])
					{
						return TowerFitStatus.Overlaps;
					}
				}
			}

			// If we've got this far, we've got a valid position.
			return TowerFitStatus.Fits;
		}

		/// <summary>
		/// Sets a cell range as being occupied by a tower.
		/// </summary>
		/// <param name="gridPos">The grid location</param>
		/// <param name="size">The size of the item</param>
		public void Occupy(IntVector2 gridPos, IntVector2 size)
		{
			IntVector2 extents = gridPos + size;

			// Validate the dimensions and size
			if ((size.x > dimensions.x) || (size.y > dimensions.y))
			{
				throw new ArgumentOutOfRangeException("size", "Given dimensions do not fit in our grid");
			}

			// Out of range of our bounds
			if ((gridPos.x < 0) || (gridPos.y < 0) ||
			    (extents.x > dimensions.x) || (extents.y > dimensions.y))
			{
				throw new ArgumentOutOfRangeException("gridPos", "Given footprint is out of range of our grid");
			}

			// Fill those positions
			for (int y = gridPos.y; y < extents.y; y++)
			{
				for (int x = gridPos.x; x < extents.x; x++)
				{
					m_AvailableCells[x, y] = true;
					
					// If there's a placement tile, clear it
					if (m_Tiles != null && m_Tiles[x, y] != null)
					{
						m_Tiles[x, y].SetState(PlacementTileState.Filled);
					}
				}
			}
		}

		/// <summary>
		/// Removes a tower from a grid, setting its cells as unoccupied.
		/// </summary>
		/// <param name="gridPos">The grid location</param>
		/// <param name="size">The size of the item</param>
		public void Clear(IntVector2 gridPos, IntVector2 size)
		{
			IntVector2 extents = gridPos + size;

			// Validate the dimensions and size
			if ((size.x > dimensions.x) || (size.y > dimensions.y))
			{
				throw new ArgumentOutOfRangeException("size", "Given dimensions do not fit in our grid");
			}

			// Out of range of our bounds
			if ((gridPos.x < 0) || (gridPos.y < 0) ||
			    (extents.x > dimensions.x) || (extents.y > dimensions.y))
			{
				throw new ArgumentOutOfRangeException("gridPos", "Given footprint is out of range of our grid");
			}

			// Fill those positions
			for (int y = gridPos.y; y < extents.y; y++)
			{
				for (int x = gridPos.x; x < extents.x; x++)
				{
					m_AvailableCells[x, y] = false;
					
					// If there's a placement tile, clear it
					if (m_Tiles != null && m_Tiles[x, y] != null)
					{
						m_Tiles[x, y].SetState(PlacementTileState.Empty);
					}
				}
			}
		}

		/// <summary>
		/// Initialize values
		/// </summary>
		protected virtual void Awake()
		{
			ResizeCollider();

			// Initialize empty bool array (defaults are false, which is what we want)
			m_AvailableCells = new bool[dimensions.x, dimensions.y];

			// Precalculate inverted grid size, to save a division every time we translate coords
			m_InvGridSize = 1 / gridSize;

			SetUpGrid();
		}

		/// <summary>
		/// Set collider's size and center
		/// </summary>
		void ResizeCollider()
		{
			var myCollider = GetComponent<BoxCollider>();
			Vector3 size = new Vector3(dimensions.x, 0, dimensions.y) * gridSize;
			myCollider.size = size;

			// Collider origin is our bottom-left corner
			myCollider.center = size * 0.5f;
		}

		/// <summary>
		/// Instantiates Tile Objects to visualise the grid and sets up the <see cref="m_AvailableCells" />
		/// </summary>
		protected void SetUpGrid()
		{		
			PlacementTile tileToUse;
#if UNITY_STANDALONE
			tileToUse = placementTilePrefab;
#else
			tileToUse = placementTilePrefabMobile;
#endif
			
			if (tileToUse != null)
			{
				// Create a container that will hold the cells.
				var tilesParent = new GameObject("Container");
				tilesParent.transform.parent = transform;
				tilesParent.transform.localPosition = Vector3.zero;
				tilesParent.transform.localRotation = Quaternion.identity;
				m_Tiles  = new PlacementTile[dimensions.x, dimensions.y];
				
				for (int y = 0; y < dimensions.y; y++)
				{
					for (int x = 0; x < dimensions.x; x++)
					{
						Vector3 targetPos = GridToWorld(new IntVector2(x, y), new IntVector2(1, 1));
						targetPos.y += 0.01f;
						PlacementTile newTile = Instantiate(tileToUse);
						newTile.transform.parent = tilesParent.transform;
						newTile.transform.position = targetPos;
						newTile.transform.localRotation = Quaternion.identity;

						m_Tiles[x, y] = newTile;
						newTile.SetState(PlacementTileState.Empty);
					}
				}
			}
		}

#if UNITY_EDITOR
		/// <summary>
		/// On editor/inspector validation, make sure we size our collider correctly.
		/// Also make sure the collider component is hidden so nobody can mess with its settings to ensure its integrity.
		/// Also communicates the idea that the user should not need to modify those values ever.
		/// </summary>
		void OnValidate()
		{
			// Validate grid size
			if (gridSize <= 0)
			{
				Debug.LogError("Negative or zero grid size is invalid");
				gridSize = 1;
			}

			// Validate dimensions
			if (dimensions.x <= 0 ||
			    dimensions.y <= 0)
			{
				Debug.LogError("Negative or zero grid dimensions are invalid");
				dimensions = new IntVector2(Mathf.Max(dimensions.x, 1), Mathf.Max(dimensions.y, 1));
			}

			// Ensure collider is the correct size
			ResizeCollider();

			GetComponent<BoxCollider>().hideFlags = HideFlags.HideInInspector;
		}

		/// <summary>
		/// Draw the grid in the scene view
		/// </summary>
		void OnDrawGizmos()
		{
			Color prevCol = Gizmos.color;
			Gizmos.color = Color.cyan;

			Matrix4x4 originalMatrix = Gizmos.matrix;
			Gizmos.matrix = transform.localToWorldMatrix;

			// Draw local space flattened cubes
			for (int y = 0; y < dimensions.y; y++)
			{
				for (int x = 0; x < dimensions.x; x++)
				{
					var position = new Vector3((x + 0.5f) * gridSize, 0, (y + 0.5f) * gridSize);
					Gizmos.DrawWireCube(position, new Vector3(gridSize, 0, gridSize));
				}
			}

			Gizmos.matrix = originalMatrix;
			Gizmos.color = prevCol;
			
			// Draw icon too, in center of position
			Vector3 center = transform.TransformPoint(new Vector3(gridSize * dimensions.x * 0.5f,
			                                                      1,
			                                                      gridSize * dimensions.y * 0.5f));
			Gizmos.DrawIcon(center, "build_zone.png", true);
		}
#endif
	}
}