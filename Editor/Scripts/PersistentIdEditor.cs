#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Kiskovi.Core
{
    [CustomEditor(typeof(PersistentId))]
    [CanEditMultipleObjects]
    public class PersistentIdEditor : Editor
    {
        private SerializedProperty idProp;

        private void OnEnable() => idProp = serializedObject.FindProperty("id");

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // Draw the ID in a disabled field (read‑only UI)
            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUILayout.PropertyField(idProp, new GUIContent("Persistent ID"));
            }

            // Regenerate button
            if (GUILayout.Button("Regenerate ID"))
            {
                foreach (var t in targets)
                {
                    var pid = (PersistentId)t;
                    Undo.RecordObject(pid, "Regenerate Persistent ID");
                    pid.GenerateNewId();
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif
