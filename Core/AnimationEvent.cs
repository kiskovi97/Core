﻿using System;
using UnityEngine;

namespace Kiskovi.Core
{
    [RequireComponent(typeof(Animator))]
    internal class AnimationEvent : MonoBehaviour
    {
        [Serializable]
        public struct ParameterEvents
        {
            public string name;
            public TriggerAction action;
        }

        public ParameterEvents[] parameterEvents;

        public void TriggerWIthParameter(string parameter)
        {
            foreach (var parameterEvent in parameterEvents)
            {
                if (parameterEvent.name == parameter)
                {
                    TriggerAction.Trigger(parameterEvent.action);
                }
            }
        }
    }
}
