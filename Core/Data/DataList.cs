using System.Collections.Generic;

using UnityEngine;

using Zenject;

namespace Kiskovi.Core
{
    public class DataList<T> : MonoBehaviour where T : class, IData
    {
        [SerializeField] private DataHolder<T> prefab;
        [SerializeField] private GameObject separator;
        [SerializeField] private int preLoadedItemCount = 3;
        [SerializeField] private Transform parentTransform;

        [Inject] private DiContainer container;

        private List<DataHolder<T>> list = new List<DataHolder<T>>();
        private List<GameObject> separators = new List<GameObject>();

        private int currentIndex = -1;

        public int CurrentIndex => currentIndex;

        private void Awake()
        {
            if (prefab != null && prefab.transform.IsChildOf(parentTransform)) prefab.gameObject.SetActive(false);
            if (separator != null && separator.transform.IsChildOf(parentTransform)) separator.gameObject.SetActive(false);
            for (int i = 0; i < preLoadedItemCount; i++)
            {
                var newItem = CreateNewObject();
                list.Add(newItem);
                if (separator != null)
                {
                    var separator = CreateSeparator();
                    separators.Add(separator);
                }
            }
        }

        public virtual void AddItem(T itemData)
        {
            currentIndex++;
            while (currentIndex >= list.Count)
            {
                var newItem = CreateNewObject();
                list.Add(newItem);
                if (separator != null)
                {
                    var separator = CreateSeparator();
                    separators.Add(separator);
                }
            }
            var element = list[currentIndex];
            element.SetData(itemData);
            element.gameObject.SetActive(true);
            if (currentIndex > 0 && separator != null)
            {
                var separator = separators[currentIndex - 1];
                separator.gameObject.SetActive(true);
            }
        }

        public void Clear()
        {
            foreach (var item in list)
                item.gameObject.SetActive(false);
            foreach (var item in separators)
                item.gameObject.SetActive(false);
            currentIndex = -1;
        }

        public void Refresh()
        {
            foreach (var item in list)
                item.Refresh();
        }

        public T GetItem(int index)
        {
            if (index < list.Count)
            {
                var element = list[index];
                return element.Data;
            }
            return null;
        }

        public DataHolder<T> GetItemObject(int index)
        {
            if (index < list.Count)
            {
                var element = list[index];
                return element;
            }
            return null;
        }

        protected virtual DataHolder<T> CreateNewObject()
        {
            var newItem = container.InstantiatePrefabForComponent<DataHolder<T>>(prefab, parentTransform);
            newItem.gameObject.SetActive(false);
            newItem.transform.localScale = Vector3.one;
            return newItem;
        }

        protected virtual GameObject CreateSeparator()
        {
            var newItem = container.InstantiatePrefab(separator, parentTransform);
            newItem.gameObject.SetActive(false);
            newItem.transform.localScale = Vector3.one;
            return newItem;
        }
    }
}