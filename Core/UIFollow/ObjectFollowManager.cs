using System.Collections.Generic;

using UnityEngine;

namespace Kiskovi.Core
{
    public interface IObjectFollowManager
    {
        void SubscribeTarget(FollowTargetData target);

        void UnSubscribeTarget(Transform target);
    }

    internal class ObjectFollowManager : DataList<FollowTargetData>, IObjectFollowManager
    {
        private Dictionary<Transform, FollowTargetData> targets = new Dictionary<Transform, FollowTargetData>();

        public void SubscribeTarget(FollowTargetData target)
        {
            if (targets.ContainsKey(target.goal))
            {
                targets[target.goal] = target;
            } else
            {
                targets.Add(target.goal, target);
            }

            RecalculateTargets();
        }

        public void UnSubscribeTarget(Transform target)
        {
            if (targets.ContainsKey(target))
            {
                targets.Remove(target);
                RecalculateTargets();
            }
        }

        private void RecalculateTargets()
        {
            Clear();
            foreach (var target in targets.Values)
            {
                AddItem(target);
            }
        }
    }
}
