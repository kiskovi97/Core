using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.InputSystem;

using Zenject;

namespace Kiskovi.Core
{
    public class InputBooleanSignal : InputActionSignal
    {
        public bool value;

        public InputBooleanSignal(bool value)
        {
            this.value = value;
        }
    }
    public class InputFloatSignal : InputActionSignal
    {
        public float value;

        public InputFloatSignal(float value)
        {
            this.value = value;
        }
    }
    public class InputVector2Signal : InputActionSignal
    {
        public Vector2 value;

        public InputVector2Signal(Vector2 value)
        {
            this.value = value;
        }
    }
    public class InputSimpleSignal : InputActionSignal { }

    public class InputActionSignal { }

    public static class InputActionSignalTypeCache
    {

        private static Dictionary<string, Type> _signalTypesByName;

        public static IEnumerable<string> GetTypes(bool regenerate)
        {
            EnsureSignalTypes(regenerate);

            return _signalTypesByName.Keys;
        }

        private static void EnsureSignalTypes(bool regenerate = false)
        {
            if (_signalTypesByName != null && !regenerate) return;

            _signalTypesByName = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => typeof(InputActionSignal).IsAssignableFrom(t) && !t.IsAbstract)
                .ToDictionary(t => t.FullName, t => t);
        }

        public static Type GetByName(string fullName)
        {
            EnsureSignalTypes();
            _signalTypesByName.TryGetValue(fullName, out var type);
            return type;
        }
    }


    public abstract class InputSignalSender : MonoBehaviour
    {
        public InputActionReference actionReference;

        [HideInInspector] public string inputActionSignalTypeName;

        private Type _cachedType;
        protected Type CachedType
        {
            get
            {
                if (_cachedType == null)
                    _cachedType = InputActionSignalTypeCache.GetByName(inputActionSignalTypeName);
                return _cachedType;
            }
        }

        [Inject] protected SignalBus _signalBus;

        private void OnEnable()
        {
            if (actionReference == null) return;

            actionReference.action.Enable();
            Subscirbe();
        }

        private void OnDisable()
        {
            if (actionReference == null) return;

            UnSubscirbe();
        }

        protected abstract void Subscirbe();

        protected abstract void UnSubscirbe();
            
    }
}
