using System.Collections.Generic;

using UnityEngine;

namespace Kiskovi.Core
{
    public static class ObjectsVisibilityManager
    {
        private static Dictionary<int, HashSet<int>> inactiveObjects = new Dictionary<int, HashSet<int>>();

        public static void SetObjectActive(this GameObject obj, bool isActive, int otherKey = 0)
        {
            if (obj == null) return;
            var key = obj.GetInstanceID();
            if (!inactiveObjects.ContainsKey(key))
                inactiveObjects.Add(key, new HashSet<int>());
            if (!isActive && !inactiveObjects[key].Contains(otherKey))
                inactiveObjects[key].Add(otherKey);
            if (isActive && inactiveObjects[key].Contains(otherKey))
                inactiveObjects[key].Remove(otherKey);
            if (obj != null)
                obj.SetActive(inactiveObjects[key].Count <= 0);
        }

        public static void Clear()
        {
            inactiveObjects.Clear();
        }
    }
}
