using TowerDefense.Towers;
using UnityEngine;

namespace TowerDefense.UI.HUD
{
	/// <summary>
	/// Tower placement "ghost" that indicates the position of the tower to be placed and its validity for placement.
	/// This is built with mouse in mind for testing, but it should be possible to abstract a lot of this into a child 
	/// class for the purposes of a touch UI.
	/// 
	/// Should exist on its own layer to ensure best placement.
	/// </summary>
	[RequireComponent(typeof(Collider))]
	public class TowerPlacementGhost : MonoBehaviour
	{
		/// <summary>
		/// The tower we represent
		/// </summary>
		public Tower controller { get; private set; }

		/// <summary>
		/// Prefab used to visualize effect radius of tower
		/// </summary>
		public GameObject radiusVisualizer;

		/// <summary>
		/// Offset height for radius visualizer
		/// </summary>
		public float radiusVisualizerHeight = 0.02f;

		/// <summary>
		/// Movement damping factor
		/// </summary>
		public float dampSpeed = 0.075f;

		/// <summary>
		/// The two materials used to represent valid and invalid placement, respectively
		/// </summary>
		public Material material;
		
		public Material invalidPositionMaterial;

		/// <summary>
		/// The list of attached mesh renderers 
		/// </summary>
		protected MeshRenderer[] m_MeshRenderers;

		/// <summary>
		/// Movement velocity for smooth damping
		/// </summary>
		protected Vector3 m_MoveVel;

		/// <summary>
		/// Target world position
		/// </summary>
		protected Vector3 m_TargetPosition;

		/// <summary>
		/// True if we're at a valid world position
		/// </summary>
		protected bool m_ValidPos;

		/// <summary>
		/// The attached the collider
		/// </summary>
		public Collider ghostCollider { get; private set; }

		/// <summary>
		/// Initialize this ghost
		/// </summary>
		/// <param name="tower">The tower controller we're a ghost of</param>
		public virtual void Initialize(Tower tower)
		{
			m_MeshRenderers = GetComponentsInChildren<MeshRenderer>();
			controller = tower;
			if (GameUI.instanceExists)
			{
				GameUI.instance.SetupRadiusVisualizer(controller, transform);
			}
			ghostCollider = GetComponent<Collider>();
			m_MoveVel = Vector3.zero;
			m_ValidPos = false;
		}

		/// <summary>
		/// Hide this ghost
		/// </summary>
		public virtual void Hide()
		{
			gameObject.SetActive(false);
		}

		/// <summary>
		/// Show this ghost
		/// </summary>
		public virtual void Show()
		{
			if (!gameObject.activeSelf)
			{
				gameObject.SetActive(true);
				m_MoveVel = Vector3.zero;

				m_ValidPos = false;
			}
		}

		/// <summary>
		/// Moves this ghost to a given world position
		/// </summary>
		/// <param name="worldPosition">The new position to move to in world space</param>
		/// <param name="rotation">The new rotation to adopt, in world space</param>
		/// <param name="validLocation">Whether or not this position is valid. Ghost may display differently
		/// over invalid locations</param>
		public virtual void Move(Vector3 worldPosition, Quaternion rotation, bool validLocation)
		{
			m_TargetPosition = worldPosition;

			if (!m_ValidPos)
			{
				// Immediately move to the given position
				m_ValidPos = true;
				transform.position = m_TargetPosition;
			}
			
			transform.rotation = rotation;
			foreach (MeshRenderer meshRenderer in m_MeshRenderers)
			{
				meshRenderer.sharedMaterial = validLocation ? material : invalidPositionMaterial;
			}
		}


		/// <summary>
		/// Damp the movement of the ghost
		/// </summary>
		protected virtual void Update()
		{
			Vector3 currentPos = transform.position;

			if (Vector3.SqrMagnitude(currentPos - m_TargetPosition) > 0.01f)
			{
				currentPos = Vector3.SmoothDamp(currentPos, m_TargetPosition, ref m_MoveVel, dampSpeed);

				transform.position = currentPos;
			}
			else
			{
				m_MoveVel = Vector3.zero;
			}
		}
	}
}