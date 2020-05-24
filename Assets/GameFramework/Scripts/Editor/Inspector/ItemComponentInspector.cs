using GameFramework;
using GameFramework.Item;
using UnityEditor;
using UnityGameFramework.Runtime;

namespace UnityGameFramework.Editor
{
    [CustomEditor(typeof(ItemComponent))]
    internal sealed class ItemComponentInspector : GameFrameworkInspector
    {
        private SerializedProperty m_EnableShowItemUpdateEvent = null;
        private SerializedProperty m_EnableShowItemDependencyAssetEvent = null;
        private SerializedProperty m_InstanceRoot = null;
        private SerializedProperty m_ItemGroups = null;

        private HelperInfo<ItemHelperBase> m_ItemHelperInfo = new HelperInfo<ItemHelperBase>("Item");
        private HelperInfo<ItemGroupHelperBase> m_ItemGroupHelperInfo = new HelperInfo<ItemGroupHelperBase>("ItemGroup");

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            ItemComponent t = (ItemComponent)target;

            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                EditorGUILayout.PropertyField(m_EnableShowItemUpdateEvent);
                EditorGUILayout.PropertyField(m_EnableShowItemDependencyAssetEvent);
                EditorGUILayout.PropertyField(m_InstanceRoot);
                m_ItemHelperInfo.Draw();
                m_ItemGroupHelperInfo.Draw();
                EditorGUILayout.PropertyField(m_ItemGroups, true);
            }
            EditorGUI.EndDisabledGroup();

            if (EditorApplication.isPlaying && IsPrefabInHierarchy(t.gameObject))
            {
                EditorGUILayout.LabelField("Item Group Count", t.ItemGroupCount.ToString());
                EditorGUILayout.LabelField("Item Count (Total)", t.ItemCount.ToString());
                IItemGroup[] itemGroups = t.GetAllItemGroups();
                foreach (IItemGroup itemGroup in itemGroups)
                {
                    EditorGUILayout.LabelField(Utility.Text.Format("Item Count ({0})", itemGroup.Name), itemGroup.ItemCount.ToString());
                }
            }

            serializedObject.ApplyModifiedProperties();

            Repaint();
        }

        protected override void OnCompileComplete()
        {
            base.OnCompileComplete();

            RefreshTypeNames();
        }

        private void OnEnable()
        {
            m_EnableShowItemUpdateEvent = serializedObject.FindProperty("m_EnableShowItemUpdateEvent");
            m_EnableShowItemDependencyAssetEvent = serializedObject.FindProperty("m_EnableShowItemDependencyAssetEvent");
            m_InstanceRoot = serializedObject.FindProperty("m_InstanceRoot");
            m_ItemGroups = serializedObject.FindProperty("m_ItemGroups");

            m_ItemHelperInfo.Init(serializedObject);
            m_ItemGroupHelperInfo.Init(serializedObject);

            RefreshTypeNames();
        }

        private void RefreshTypeNames()
        {
            m_ItemHelperInfo.Refresh();
            m_ItemGroupHelperInfo.Refresh();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
