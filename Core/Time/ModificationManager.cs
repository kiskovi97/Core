using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using Zenject;

namespace Kiskovi.Core
{
    internal class ModificationManager : IInitializable, ITickable, IDisposable
    {
        public float timeDefault = 1f;
        protected float modification = 1f;
        private Dictionary<int, float> modifications = new Dictionary<int, float>();
        protected Dictionary<int, float> timeToResets = new Dictionary<int, float>();
        private float goalModification = 1f;

        public virtual void Modify(float modification, float timeToReset = 30f, int index = 0)
        {
            if (modifications.ContainsKey(index))
            {
                modifications[index] = modification;
            }
            else
            {
                modifications.Add(index, modification);
            }
            if (timeToResets.ContainsKey(index))
            {
                timeToResets[index] = timeToReset;
            }
            else
            {
                timeToResets.Add(index, timeToReset);
            }
        }

        public virtual void Initialize()
        {
            modification = timeDefault;
        }

        public virtual void Dispose()
        {
        }

        public virtual void Tick()
        {
            goalModification = timeDefault;
            foreach (var key in timeToResets.Keys.ToArray())
            {
                timeToResets[key] -= Time.unscaledDeltaTime;
                if (timeToResets[key] > 0f)
                    goalModification *= modifications[key];
            }
            if (goalModification == timeDefault)
            {
                modifications.Clear();
                timeToResets.Clear();
            }
            if (modification != goalModification)
            {
                modification = Mathf.MoveTowards(modification, goalModification, Time.unscaledDeltaTime * 3f);
            }
        }
    }
}
