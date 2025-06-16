using System;
using System.Linq;

using UnityEditor;

using UnityEngine;

namespace Kiskovi.Core
{
    [CustomEditor(typeof(InputSignalSender), true, isFallback = true)]
    internal class InputActionSignalSenderEditor : Editor
    {
        private int _selectedIndex;
        private string[] _typeOptions;

        private void OnEnable()
        {
            _typeOptions = InputActionSignalTypeCache.GetTypes(true).ToArray();

            var targetScript = (InputSignalSender)target;
            _selectedIndex = Array.FindIndex(_typeOptions, name => name == targetScript.inputActionSignalTypeName);
            if (_selectedIndex < 0) _selectedIndex = 0;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var targetScript = (InputSignalSender)target;

            EditorGUILayout.LabelField("Input Action Signal Type");
            _selectedIndex = EditorGUILayout.Popup(_selectedIndex, _typeOptions);

            targetScript.inputActionSignalTypeName = _typeOptions[_selectedIndex];

            if (GUI.changed)
            {
                EditorUtility.SetDirty(targetScript);
            }
        }
    }
}
