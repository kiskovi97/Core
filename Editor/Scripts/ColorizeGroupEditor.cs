using UnityEditor;

using UnityEngine;

namespace Kiskovi.Core
{
    [CustomEditor(typeof(ColorizeGroup), editorForChildClasses: true)]
    internal class ColorizeGroupEditor : Editor
    {
        ColorizeGroup triggerAction;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            triggerAction = (ColorizeGroup)target;

            if (triggerAction == null)
                return;

            EditorGUILayout.Space();
            if (GUILayout.Button("Test OnEnable"))
            {
                triggerAction.OnEnable();
            }
        }
    }
}
