using UnityEngine;

namespace TowerDefense.UI
{
	/// <summary>
	/// A simple component that plays an animation
	/// </summary>
	[RequireComponent(typeof(Animation))]
	public class PlayAnimation : MonoBehaviour
	{
		Animation m_Animation;
		
		public void Play(string animationName)
		{
			m_Animation.Play(animationName);
		}

		void Start()
		{
			m_Animation = GetComponent<Animation>();
		}
		
	}
}
