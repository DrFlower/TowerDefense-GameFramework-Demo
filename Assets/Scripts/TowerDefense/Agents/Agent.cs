using System;
using ActionGameFramework.Health;
using Core.Utilities;
using TowerDefense.Affectors;
using TowerDefense.Level;
using TowerDefense.Nodes;
using UnityEngine;
using UnityEngine.AI;

namespace TowerDefense.Agents
{
	/// <summary>
	/// An agent will follow a path of nodes
	/// </summary>
	[RequireComponent(typeof(NavMeshAgent)), RequireComponent(typeof(AttackAffector))]
	public abstract class Agent : Targetable
	{	
		/// <summary>
		/// A means of keeping track of the agent along its path
		/// </summary>
		public enum State
		{
			/// <summary>
			/// When the agent is on a path that is not blocked
			/// </summary>
			OnCompletePath,

			/// <summary>
			/// When the agent is on a path is blocked
			/// </summary>
			OnPartialPath,

			/// <summary>
			/// When the agent has reached the end of a blocked path
			/// </summary>
			Attacking,

			/// <summary>
			/// For flying agents, when they move over obstacles
			/// </summary>
			PushingThrough,
			
			/// <summary>
			/// When the agent has completed their path
			/// </summary>
			PathComplete
		}

		/// <summary>
		/// Event fired when agent reached its final node
		/// </summary>
		public event Action<Node> destinationReached;

		/// <summary>
		/// Position offset for an applied affect
		/// </summary>
		public Vector3 appliedEffectOffset = Vector3.zero;
		
		/// <summary>
		/// Scale adjustment for an applied affect
		/// </summary>
		public float appliedEffectScale = 1;

		/// <summary>
		/// The NavMeshAgent component attached to this
		/// </summary>
		protected NavMeshAgent m_NavMeshAgent;

		/// <summary>
		/// The Current node that the agent must navigate to
		/// </summary>
		protected Node m_CurrentNode;

		/// <summary>
		/// Reference to the level manager
		/// </summary>
		protected LevelManager m_LevelManager;

		/// <summary>
		/// Stores the Destination to the next node so we don't need to get new random positions every time
		/// </summary>
		protected Vector3 m_Destination;
		
		/// <summary>
		/// Gets the attached nav mesh agent velocity
		/// </summary>
		public override Vector3 velocity
		{
			get { return m_NavMeshAgent.velocity; }
		}
		
		/// <summary>
		/// The current state of the agent along the path
		/// </summary>
		public State state { get; protected set; }

		/// <summary>
		/// Accessor to <see cref="m_NavMeshAgent"/>
		/// </summary>
		public NavMeshAgent navMeshNavMeshAgent
		{
			get { return m_NavMeshAgent; }
			set { m_NavMeshAgent = value; }
		}

		/// <summary>
		/// The area mask of the attached nav mesh agent
		/// </summary>
		public int navMeshMask
		{
			get { return m_NavMeshAgent.areaMask; }
		}

		/// <summary>
		/// Gets this agent's original movement speed
		/// </summary>
		public float originalMovementSpeed { get; private set; }

		/// <summary>
		/// Checks if the path is blocked
		/// </summary>
		/// <value>
		/// The status of the agent's path
		/// </value>
		protected virtual bool isPathBlocked
		{
			get { return m_NavMeshAgent.pathStatus == NavMeshPathStatus.PathPartial; }
		}

		/// <summary>
		/// Is the Agent close enough to its destination?
		/// </summary>
		protected virtual bool isAtDestination
		{
			get { return navMeshNavMeshAgent.remainingDistance <= navMeshNavMeshAgent.stoppingDistance; }
		}

		/// <summary>
		/// Sets the node to navigate to
		/// </summary>
		/// <param name="node">The node that the agent will navigate to</param>
		public virtual void SetNode(Node node)
		{
			m_CurrentNode = node;
		}

		/// <summary>
		/// Stops the navMeshAgent and attempts to return to pool
		/// </summary>
		public override void Remove()
		{
			base.Remove();
			
			m_LevelManager.DecrementNumberOfEnemies();
			if (m_NavMeshAgent.enabled)
			{
				m_NavMeshAgent.isStopped = true;
			}
			m_NavMeshAgent.enabled = false;

			Poolable.TryPool(gameObject);
		}

		/// <summary>	
		/// Setup all the necessary parameters for this agent from configuration data
		/// </summary>
		public virtual void Initialize()
		{
			ResetPositionData();
			LazyLoad();
			configuration.SetHealth(configuration.maxHealth);
			state = isPathBlocked ? State.OnPartialPath : State.OnCompletePath;

			m_NavMeshAgent.enabled = true;
			m_NavMeshAgent.isStopped = false;
			
			m_LevelManager.IncrementNumberOfEnemies();
		}

		/// <summary>
		/// Finds the next node in the path
		/// </summary>
		public virtual void GetNextNode(Node currentlyEnteredNode)
		{
			// Don't do anything if the calling node is the same as the m_CurrentNode
			if (m_CurrentNode != currentlyEnteredNode)
			{
				return;
			}
			if (m_CurrentNode == null)
			{
				Debug.LogError("Cannot find current node");
				return;
			}

			Node nextNode = m_CurrentNode.GetNextNode();
			if (nextNode == null)
			{
				if (m_NavMeshAgent.enabled)
				{
					m_NavMeshAgent.isStopped = true;
				}
				HandleDestinationReached();
				return;
			}
			
			Debug.Assert(nextNode != m_CurrentNode);
			SetNode(nextNode);
			MoveToNode();
		}

		/// <summary>
		/// Moves the agent to a position in the <see cref="Agent.m_CurrentNode" />
		/// </summary>
		public virtual void MoveToNode()
		{
			Vector3 nodePosition = m_CurrentNode.GetRandomPointInNodeArea();
			nodePosition.y = m_CurrentNode.transform.position.y;
			m_Destination = nodePosition;
			NavigateTo(m_Destination);
		}

		/// <summary>
		/// The logic for what happens when the destination is reached
		/// </summary>
		public virtual void HandleDestinationReached()
		{
			state = State.PathComplete;
			if (destinationReached != null)
			{
				destinationReached(m_CurrentNode);
			} 
		}
		
		/// <summary>
		/// Lazy Load, if necesaary and ensure the NavMeshAgent is disabled
		/// </summary>
		protected override void Awake()
		{
			base.Awake();
			LazyLoad();
			m_NavMeshAgent.enabled = false;
		}
		
		/// <summary>
		/// Updates the agent in its different states, 
		/// Reset destination when path is stale
		/// </summary>
		protected virtual void Update()
		{
			// Update behaviour for different states
			PathUpdate();
			
			// If the path becomes invalid, repath the agent to the destination
			bool validStalePath = m_NavMeshAgent.isOnNavMesh && m_NavMeshAgent.enabled &&
			                      (!m_NavMeshAgent.hasPath && !m_NavMeshAgent.pathPending);
			if (validStalePath)
			{
				// Compare against squared stopping distance on agent.
				// We intentionally do not pre-square this value so that it can be changed at runtime dynamically
				float squareStoppingDistance = m_NavMeshAgent.stoppingDistance * m_NavMeshAgent.stoppingDistance;
				if (Vector3.SqrMagnitude(m_Destination - transform.position) < squareStoppingDistance &&
				    m_CurrentNode.GetNextNode() != null)
				{
					// Proceed if we're at our destination
					GetNextNode(m_CurrentNode);
				}
				else
				{
					// Otherwise try repath
					m_NavMeshAgent.SetDestination(m_Destination);
				}
			}
		}

		/// <summary>
		/// Set the NavMeshAgent's destination
		/// </summary>
		/// <param name="nextPoint">The position to navigate to</param>
		protected virtual void NavigateTo(Vector3 nextPoint)
		{
			LazyLoad();
			if (m_NavMeshAgent.isOnNavMesh)
			{
				m_NavMeshAgent.SetDestination(nextPoint);
			}
		}

		/// <summary>
		/// This is a lazy way of caching several components utilised by the Agent
		/// </summary>
		protected virtual void LazyLoad()
		{
			if (m_NavMeshAgent == null)
			{
				m_NavMeshAgent = GetComponent<NavMeshAgent>();
				originalMovementSpeed = m_NavMeshAgent.speed;
			}
			if (m_LevelManager == null)
			{
				m_LevelManager = LevelManager.instance;
			}
		}

		/// <summary>
		/// Move along the path, change to <see cref="Agent.State.OnPartialPath" />
		/// </summary>
		protected virtual void OnCompletePathUpdate()
		{
			if (isPathBlocked)
			{
				state = State.OnPartialPath;
			}
		}

		/// <summary>
		/// Peforms the relevant path update
		/// </summary>
		protected abstract void PathUpdate();

		/// <summary>
		/// The behaviour for when the agent has been blocked
		/// </summary>
		protected abstract void OnPartialPathUpdate();
		
		
#if UNITY_EDITOR
		/// <summary>
		/// Draw the agent's path
		/// </summary>
		protected virtual void OnDrawGizmosSelected()
		{
			if (m_NavMeshAgent != null)
			{
				Vector3[] pathPoints = m_NavMeshAgent.path.corners;
				int count = pathPoints.Length;
				for (int i = 0; i < count - 1; i++)
				{
					Vector3 from = pathPoints[i];
					Vector3 to = pathPoints[i + 1];
					Gizmos.DrawLine(from, to);
				}
				Gizmos.DrawWireSphere(m_NavMeshAgent.destination, 0.2f);
			}
		}
#endif
	}
}
