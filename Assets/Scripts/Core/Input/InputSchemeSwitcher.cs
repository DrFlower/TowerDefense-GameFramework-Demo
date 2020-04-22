using UnityEngine;

namespace Core.Input
{
	/// <summary>
	/// Base component that switches between active input schemes
	/// </summary>
	[DisallowMultipleComponent]
	public class InputSchemeSwitcher : MonoBehaviour
	{
		/// <summary>
		/// The attached input schemes
		/// </summary>
		protected InputScheme[] m_InputSchemes;

		/// <summary>
		/// The default scheme based on the platform
		/// </summary>
		protected InputScheme m_DefaultScheme;

		/// <summary>
		/// The current scheme activated
		/// </summary>
		protected InputScheme m_CurrentScheme;

		/// <summary>
		/// Cache the schemes and activate the default
		/// </summary>
		protected virtual void Awake()
		{
			m_InputSchemes = GetComponents<InputScheme>();
			foreach (InputScheme scheme in m_InputSchemes)
			{
				scheme.Deactivate(null);
				if (m_CurrentScheme == null && scheme.isDefault)
				{
					m_DefaultScheme = scheme;
				}
			}
			if (m_DefaultScheme == null)
			{
				Debug.LogError("[InputSchemeSwitcher] Default scheme not set.");
				return;
			}
			m_DefaultScheme.Activate(null);
			m_CurrentScheme = m_DefaultScheme;
		}

		/// <summary>
		/// Checks the different schemes and activates them if needed
		/// </summary>
		protected virtual void Update()
		{
			foreach (InputScheme scheme in m_InputSchemes)
			{
				if (scheme.enabled || !scheme.shouldActivate)
				{
					continue;
				}
				if (m_CurrentScheme != null)
				{
					m_CurrentScheme.Deactivate(scheme);
				}
				scheme.Activate(m_CurrentScheme);
				m_CurrentScheme = scheme;
				break;
			}
		}
	}
}