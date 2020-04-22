using System.Collections.Generic;
using TowerDefense.Towers;
using UnityEngine;

namespace TowerDefense.UI
{
	public class RadiusVisualizerController : MonoBehaviour
	{
		/// <summary>
		/// Prefab used to visualize effect radius of tower
		/// </summary>
		public GameObject radiusVisualizerPrefab;

		public float radiusVisualizerHeight = 0.02f;

		/// <summary>
		/// The local euler angles
		/// </summary>
		public Vector3 localEuler;

		readonly List<GameObject> m_RadiusVisualizers = new List<GameObject>();

		/// <summary>
		/// Sets up the radius visualizer for a tower or ghost tower
		/// </summary>
		/// <param name="tower">
		/// The tower to get the data from
		/// </param>
		/// <param name="ghost">Transform of ghost to parent the visualiser to.</param>
		public void SetupRadiusVisualizers(Tower tower, Transform ghost = null)
		{
			// Create necessary affector radius visualizations
			List<ITowerRadiusProvider> providers =
				tower.levels[tower.currentLevel].GetRadiusVisualizers();

			int length = providers.Count;
			for (int i = 0; i < length; i++)
			{
				if (m_RadiusVisualizers.Count < i + 1)
				{
					m_RadiusVisualizers.Add(Instantiate(radiusVisualizerPrefab));
				}

				ITowerRadiusProvider provider = providers[i];

				GameObject radiusVisualizer = m_RadiusVisualizers[i];
				radiusVisualizer.SetActive(true);
				radiusVisualizer.transform.SetParent(ghost == null ? tower.transform : ghost);
				radiusVisualizer.transform.localPosition = new Vector3(0, radiusVisualizerHeight, 0);
				radiusVisualizer.transform.localScale = Vector3.one * provider.effectRadius * 2.0f;
				radiusVisualizer.transform.localRotation = new Quaternion {eulerAngles = localEuler};

				var visualizerRenderer = radiusVisualizer.GetComponent<Renderer>();
				if (visualizerRenderer != null)
				{
					visualizerRenderer.material.color = provider.effectColor;
				}
			}
		}

		/// <summary>
		/// Hides the radius visualizers
		/// </summary>
		public void HideRadiusVisualizers()
		{
			foreach (GameObject radiusVisualizer in m_RadiusVisualizers)
			{
				radiusVisualizer.transform.parent = transform;
				radiusVisualizer.SetActive(false);
			}
		}
	}
}