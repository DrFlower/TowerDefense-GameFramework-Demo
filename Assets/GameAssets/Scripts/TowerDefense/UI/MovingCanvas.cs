using UnityEngine;

namespace TowerDefense.UI
{
	/// <summary>
	/// A class for controlling conditional motion of the canvas
	/// </summary>
	[RequireComponent(typeof(Canvas))]
	public class MovingCanvas : MonoBehaviour
	{
		/// <summary>
		/// The RectTransform used to check against the screen bounds
		/// </summary>
		public RectTransform content;

		/// <summary>
		/// To offset the position the canvas is placed at
		/// </summary>
		public Vector2 offset;

		/// <summary>
		/// The attached canvas
		/// </summary>
		Canvas m_Canvas;

		/// <summary>
		/// Property for disabling and enabling the attached canvas
		/// </summary>
		public bool canvasEnabled
		{
			get
			{
				if (m_Canvas == null)
				{
					m_Canvas = GetComponent<Canvas>();
				}
				return m_Canvas.enabled;
			}
			set
			{
				if (m_Canvas == null)
				{
					m_Canvas = GetComponent<Canvas>();
				}
				m_Canvas.enabled = value;
			}
		}

		/// <summary>
		/// Try to move the canvas based on <see cref="content"/>'s rect
		/// </summary>
		/// <param name="position">
		/// The position to move to
		/// </param>
		public void TryMove(Vector3 position)
		{
			Rect rect = content.rect;
			position += (Vector3) offset;
			rect.position = position;

			if (rect.xMin < rect.width * 0.5f)
			{
				position.x = rect.width * 0.5f;
			}
			if (rect.xMax > Screen.width - rect.width * 0.5f)
			{
				position.x = Screen.width - rect.width * 0.5f;
			}
			if (rect.yMin < rect.height * 0.5f)
			{
				position.y = rect.height * 0.5f;
			}
			if (rect.yMax > Screen.height - rect.height * 0.5f)
			{
				position.y = Screen.height - rect.height * 0.5f;
			}
			transform.position = position;
		}

		/// <summary>
		/// Cache the attached canvas
		/// </summary>
		protected virtual void Awake()
		{
			canvasEnabled = false;
		}
	}
}