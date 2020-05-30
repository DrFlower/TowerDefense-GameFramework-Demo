using GameFramework.Data;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace UnityGameFramework.Editor
{
    [CustomEditor(typeof(DataComponent))]
    internal sealed class DataComponentInspector : GameFrameworkInspector
    {
        private SerializedProperty m_AvailableDataTypeNames = null;

        private string[] m_DataTypeNames = null;
        private List<string> m_CurrentAvailableDataTypeNames = null;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            DataComponent t = (DataComponent)target;

            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                GUILayout.Label("Available Datas", EditorStyles.boldLabel);
                if (m_DataTypeNames.Length > 0)
                {
                    EditorGUILayout.BeginVertical("box");
                    {
                        foreach (string dataTypeName in m_DataTypeNames)
                        {
                            bool selected = m_CurrentAvailableDataTypeNames.Contains(dataTypeName);
                            if (selected != EditorGUILayout.ToggleLeft(dataTypeName, selected))
                            {
                                if (!selected)
                                {
                                    m_CurrentAvailableDataTypeNames.Add(dataTypeName);
                                    WriteAvailableDataTypeNames();
                                }
                            }
                        }
                    }
                    EditorGUILayout.EndVertical();
                }
                else
                {
                    EditorGUILayout.HelpBox("There is no available data.", MessageType.Warning);
                }

                if (m_CurrentAvailableDataTypeNames.Count > 0)
                {
                    EditorGUILayout.Separator();
                }
                else
                {
                    EditorGUILayout.HelpBox("Select available datas first.", MessageType.Info);
                }
            }
            EditorGUI.EndDisabledGroup();

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
            m_AvailableDataTypeNames = serializedObject.FindProperty("m_AvailableDataTypeNames");

            RefreshTypeNames();
        }

        private void RefreshTypeNames()
        {
            m_DataTypeNames = Type.GetTypeNames(typeof(Data));
            ReadAvailableDataTypeNames();
            int oldCount = m_CurrentAvailableDataTypeNames.Count;
            m_CurrentAvailableDataTypeNames = m_CurrentAvailableDataTypeNames.Where(x => m_DataTypeNames.Contains(x)).ToList();
            if (m_CurrentAvailableDataTypeNames.Count != oldCount)
            {
                WriteAvailableDataTypeNames();
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void ReadAvailableDataTypeNames()
        {
            m_CurrentAvailableDataTypeNames = new List<string>();
            int count = m_AvailableDataTypeNames.arraySize;
            for (int i = 0; i < count; i++)
            {
                m_CurrentAvailableDataTypeNames.Add(m_AvailableDataTypeNames.GetArrayElementAtIndex(i).stringValue);
            }
        }

        private void WriteAvailableDataTypeNames()
        {
            m_AvailableDataTypeNames.ClearArray();
            if (m_CurrentAvailableDataTypeNames == null)
            {
                return;
            }

            m_CurrentAvailableDataTypeNames.Sort();
            int count = m_CurrentAvailableDataTypeNames.Count;
            for (int i = 0; i < count; i++)
            {
                m_AvailableDataTypeNames.InsertArrayElementAtIndex(i);
                m_AvailableDataTypeNames.GetArrayElementAtIndex(i).stringValue = m_CurrentAvailableDataTypeNames[i];
            }
        }
    }
}
