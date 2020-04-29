using UnityEngine;
using UnityEngine.AI;

namespace TowerDefense.Agents
{
	/// <summary>
	/// Agent that can pass "over" towers that block the path
	/// </summary>
	public class FlyingAgent : Agent
	{
		/// <summary>
		/// Time to wait to clear the navmesh obstacles
		/// </summary>
		protected float m_WaitTime = 0.5f;

		/// <summary>
		/// The current time to wait until we can resume agent movement as normal
		/// </summary>
		protected float m_CurrentWaitTime;

		/// <summary>
		/// If flying agents are blocked, they should still move through obstacles
		/// </summary>
		protected override void OnPartialPathUpdate()
		{
			if (!isPathBlocked)
			{
				state = State.OnCompletePath;
				return;
			}
			if (!isAtDestination)
			{
				return;
			}
			m_NavMeshAgent.enabled = false;
			m_CurrentWaitTime = m_WaitTime;
			state = State.PushingThrough;
		}
		
		/// <summary>
		/// Controls behaviour based on the states <see cref="Agent.State.OnCompletePath"/>, <see cref="Agent.State.OnPartialPath"/> 
		/// and <see cref="Agent.State.PushingThrough"/>
		/// </summary>
		protected override void PathUpdate()
		{
			switch (state)
			{
				case State.OnCompletePath:
					OnCompletePathUpdate();
					break;
				case State.OnPartialPath:
					OnPartialPathUpdate();
					break;
				case State.PushingThrough:
					PushingThrough();
					break;
			}
		}

		/// <summary>
		/// When flying agents are pushing through, give them a small amount of time to clear the gap and turn on their agent
		/// once time elapses
		/// </summary>
		protected void PushingThrough()
		{
			m_CurrentWaitTime -= Time.deltaTime;

			// Move the agent, overriding its NavMeshAgent until it reaches its destination
			transform.LookAt(m_Destination, Vector3.up);
			transform.position += transform.forward * m_NavMeshAgent.speed * Time.deltaTime;
			if (m_CurrentWaitTime > 0)
			{
				return;
			}
			// Check if there is a navmesh under the agent, if not, then reset the timer
			NavMeshHit hit;
			if (!NavMesh.Raycast(transform.position + Vector3.up, Vector3.down, out hit, navMeshMask))
			{
				m_CurrentWaitTime = m_WaitTime;
			}
			else
			{
				// If the time elapses, and there is a NavMesh under it, resume agent movement as normal
				m_NavMeshAgent.enabled = true;
				NavigateTo(m_Destination);
				state = isPathBlocked ? State.OnPartialPath : State.OnCompletePath;
			}
		}
	}
}