using Core.Utilities;
using UnityEngine;

namespace TowerDefense.Towers.Projectiles
{
	/// <summary>
	/// For objects that destroyer themselves on contact
	/// </summary>
	public class ContactDestroyer : MonoBehaviour
	{
		/// <summary>
		/// The y-value of the position the object will destroy itself
		/// </summary>
		public float yDestroyPoint = -50;

		/// <summary>
		/// The attached collider
		/// </summary>
		protected Collider m_AttachedCollider;

		/// <summary>
		/// Caches the attached collider
		/// </summary>
		protected virtual void Awake()
		{
			m_AttachedCollider = GetComponent<Collider>();
		}

		/// <summary>
		/// Checks the y-position against <see cref="yDestroyPoint"/>
		/// </summary>
		protected virtual void Update()
		{
			if (transform.position.y < yDestroyPoint)
			{
				ReturnToPool();
			}
		}

		void OnCollisionEnter(Collision other)
		{
			ReturnToPool();
		}

		/// <summary>
		/// Returns the object to pool if possible, otherwise destroys
		/// </summary>
		void ReturnToPool()
		{
			if (!gameObject.activeInHierarchy)
			{
				return;
			}
			Poolable.TryPool(gameObject);
		}
	}
}