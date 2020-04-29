using TowerDefense.MeshCreator;
using UnityEditor;
using UnityEngine;

namespace TowerDefense.Nodes.Editor
{
	/// <summary>
	/// Editor for Node
	/// </summary>
	[CustomEditor(typeof(Node))]
	public class NodeEditor : UnityEditor.Editor
	{
		protected Node m_Node;

		protected void OnEnable()
		{
			m_Node = (Node)target;
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			GUILayout.Space(5);
			if (GUILayout.Button("Select/Add Mesh"))
			{
				AddMeshCreator();
			}
		}

		/// <summary>
		/// Creates a new AreaMeshCreator object as a child of the node
		/// </summary>
		protected void AddMeshCreator()
		{
			var meshObject = m_Node.GetComponentInChildren<AreaMeshCreator>();

			// AreaMeshCreatorObject already exists so no need to instantiate it again
			if (meshObject != null)
			{
				Selection.activeGameObject = meshObject.gameObject;
				return;
			}
			
			GameObject newGameObject = new GameObject("Node Mesh");
			newGameObject.transform.SetParent(m_Node.transform, false);
			meshObject = newGameObject.AddComponent<AreaMeshCreator>();
			
			Selection.activeGameObject = meshObject.gameObject;
			Undo.RegisterCreatedObjectUndo(meshObject.gameObject, "Created AreaMeshCreator");
		}
	}
}