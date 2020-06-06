using GameFramework.Data;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEditorInternal;

namespace UnityGameFramework.Editor
{
    [CustomEditor(typeof(DataComponent))]
    internal sealed class DataComponentInspector : GameFrameworkInspector
    {
        //private SerializedProperty m_AvailableDataTypeNames = null;

        //private string[] m_DataTypeNames = null;
        //private List<string> m_CurrentAvailableDataTypeNames = null;

        //public override void OnInspectorGUI()
        //{
        //    base.OnInspectorGUI();

        //    serializedObject.Update();

        //    DataComponent t = (DataComponent)target;

        //    EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
        //    {
        //        GUILayout.Label("Available Datas", EditorStyles.boldLabel);
        //        if (m_DataTypeNames.Length > 0)
        //        {
        //            EditorGUILayout.BeginVertical("box");
        //            {
        //                foreach (string dataTypeName in m_DataTypeNames)
        //                {
        //                    bool selected = m_CurrentAvailableDataTypeNames.Contains(dataTypeName);
        //                    if (selected != EditorGUILayout.ToggleLeft(dataTypeName, selected))
        //                    {
        //                        if (!selected)
        //                        {
        //                            m_CurrentAvailableDataTypeNames.Add(dataTypeName);
        //                            WriteAvailableDataTypeNames();
        //                        }
        //                    }
        //                }
        //            }
        //            EditorGUILayout.EndVertical();
        //        }
        //        else
        //        {
        //            EditorGUILayout.HelpBox("There is no available data.", MessageType.Warning);
        //        }

        //        if (m_CurrentAvailableDataTypeNames.Count > 0)
        //        {
        //            EditorGUILayout.Separator();
        //        }
        //        else
        //        {
        //            EditorGUILayout.HelpBox("Select available datas first.", MessageType.Info);
        //        }
        //    }
        //    EditorGUI.EndDisabledGroup();

        //    serializedObject.ApplyModifiedProperties();

        //    Repaint();
        //}

        //protected override void OnCompileComplete()
        //{
        //    base.OnCompileComplete();

        //    RefreshTypeNames();
        //}

        //private void OnEnable()
        //{
        //    m_AvailableDataTypeNames = serializedObject.FindProperty("m_AvailableDataTypeNames");

        //    RefreshTypeNames();
        //}

        //private void RefreshTypeNames()
        //{
        //    m_DataTypeNames = Type.GetTypeNames(typeof(Data));
        //    ReadAvailableDataTypeNames();
        //    int oldCount = m_CurrentAvailableDataTypeNames.Count;
        //    m_CurrentAvailableDataTypeNames = m_CurrentAvailableDataTypeNames.Where(x => m_DataTypeNames.Contains(x)).ToList();
        //    if (m_CurrentAvailableDataTypeNames.Count != oldCount)
        //    {
        //        WriteAvailableDataTypeNames();
        //    }

        //    serializedObject.ApplyModifiedProperties();
        //}

        //private void ReadAvailableDataTypeNames()
        //{
        //    m_CurrentAvailableDataTypeNames = new List<string>();
        //    int count = m_AvailableDataTypeNames.arraySize;
        //    for (int i = 0; i < count; i++)
        //    {
        //        m_CurrentAvailableDataTypeNames.Add(m_AvailableDataTypeNames.GetArrayElementAtIndex(i).stringValue);
        //    }
        //}

        //private void WriteAvailableDataTypeNames()
        //{
        //    m_AvailableDataTypeNames.ClearArray();
        //    if (m_CurrentAvailableDataTypeNames == null)
        //    {
        //        return;
        //    }

        //    m_CurrentAvailableDataTypeNames.Sort();
        //    int count = m_CurrentAvailableDataTypeNames.Count;
        //    for (int i = 0; i < count; i++)
        //    {
        //        m_AvailableDataTypeNames.InsertArrayElementAtIndex(i);
        //        m_AvailableDataTypeNames.GetArrayElementAtIndex(i).stringValue = m_CurrentAvailableDataTypeNames[i];
        //    }
        //}

        private List<DataItem> dataItems;
        private ReorderableList reorderableList;

        private void OnEnable()
        {
            dataItems = new List<DataItem>();

            RefreshDataItems();

            reorderableList = new ReorderableList(dataItems, typeof(DataItem), true, true, true, true);

            reorderableList.drawHeaderCallback += DrawHeader;
            reorderableList.drawElementCallback += DrawElement;

            reorderableList.displayAdd = false;
            reorderableList.displayRemove = false;

            reorderableList.onReorderCallbackWithDetails += OnReorder;
        }

        private void RefreshDataItems()
        {
            dataItems.Clear();

            DataComponent dataComponent = (DataComponent)target;

            string[] dataTypeNames = Type.GetTypeNames(typeof(Data));

            foreach (var item in dataComponent.dataItems)
            {
                if (dataTypeNames.Contains(item.dataTypeName))
                    dataItems.Add(item);
            }

            foreach (var dataTypeName in dataTypeNames)
            {
                bool contain = false;

                foreach (var item in dataComponent.dataItems)
                {
                    if (item.dataTypeName == dataTypeName)
                    {
                        contain = true;
                        break;
                    }
                }

                if (!contain)
                {
                    DataItem dataItem = new DataItem() { enable = false, dataTypeName = dataTypeName };
                    dataItems.Add(dataItem);
                }

            }
        }

        private void WriteData()
        {
            DataComponent dataComponent = (DataComponent)target;
            dataComponent.dataItems = new DataItem[dataItems.Count];
            for (int i = 0; i < dataItems.Count; i++)
            {
                dataComponent.dataItems[i] = dataItems[i];
            }
        }


        private void OnDisable()
        {
            reorderableList.drawHeaderCallback -= DrawHeader;
            reorderableList.drawElementCallback -= DrawElement;
            reorderableList.onReorderCallbackWithDetails += OnReorder;
        }

        private void DrawHeader(Rect rect)
        {
            GUI.Label(rect, "Data List");
        }

        private void DrawElement(Rect rect, int index, bool active, bool focused)
        {
            DataItem item = dataItems[index];

            EditorGUI.BeginChangeCheck();
            item.enable = EditorGUI.Toggle(new Rect(rect.x, rect.y, 18, rect.height), item.enable);
            EditorGUI.LabelField(new Rect(rect.x + 18, rect.y, rect.width - 18, rect.height), item.dataTypeName);
            if (EditorGUI.EndChangeCheck())
            {
                WriteData();
                EditorUtility.SetDirty(target);
            }
        }

        private void OnReorder(ReorderableList list, int oldIndex, int newIndex)
        {
            WriteData();
            EditorUtility.SetDirty(target);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            reorderableList.DoLayoutList();
        }
    }
}
