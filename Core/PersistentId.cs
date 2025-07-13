using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Kiskovi.Core
{
    [DisallowMultipleComponent]
    [ExecuteAlways]
    public class PersistentId : MonoBehaviour
    {
        [SerializeField, HideInInspector]   // kept out of the default inspector
        private string id = string.Empty;

        /// Expose read‑only access for runtime code
        public string Id => id;

#if UNITY_EDITOR
        // Automatically create an ID for prefab *instances* or scene objects
        private void OnValidate()
        {
            if (Application.isPlaying) return;
            if (!string.IsNullOrEmpty(id)) return;                       // already has one
            if (PrefabUtility.IsPartOfPrefabAsset(gameObject)) return;   // editing the asset itself

            GenerateNewId();
        }

        /// Generates a new GUID and marks the object dirty
        public void GenerateNewId()
        {
            id = Guid.NewGuid().ToString("N");
            EditorUtility.SetDirty(this);                                // scene object
            PrefabUtility.RecordPrefabInstancePropertyModifications(this); // prefab instance
        }
#endif
    }
}
