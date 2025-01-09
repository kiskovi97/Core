using UnityEditor;

using UnityEngine;

namespace Kiskovi.Core
{
    [CustomEditor(typeof(Colorize), editorForChildClasses: true)]
    internal class ColorizeEditor : Editor
    {
        Colorize triggerAction;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            triggerAction = (Colorize)target;

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
