using UnityEditor;

using UnityEngine;

namespace Kiskovi.Core
{
    [CustomEditor(typeof(TriggerAction), editorForChildClasses: true)]
    public class TriggerActionEditor : Editor
    {
        TriggerAction triggerAction;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            triggerAction = (TriggerAction)target;

            if (triggerAction == null)
                return;

            EditorGUILayout.Space();
            if (GUILayout.Button("Test Trigger"))
            {
                triggerAction.Trigger();
            }


        }
    }
}
