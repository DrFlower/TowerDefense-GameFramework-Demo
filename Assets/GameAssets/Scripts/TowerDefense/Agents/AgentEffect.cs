using UnityEngine;

namespace TowerDefense.Agents
{
	/// <summary>
	/// A component that will apply various effects on an agent
	/// </summary>
	public abstract class AgentEffect : MonoBehaviour
	{
		/// <summary>
		/// Reference to the agent that will be affected
		/// </summary>
		protected Agent m_Agent;

		public virtual void Awake()
		{
			LazyLoad();
		}

		/// <summary>
		/// A lazy way to ensure that <see cref="m_Agent"/> will not be null
		/// </summary>
		public virtual void LazyLoad()
		{
			if (m_Agent == null)
			{
				m_Agent = GetComponent<Agent>();
			}
		}
	}
}