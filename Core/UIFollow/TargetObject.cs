using UnityEngine;

using Zenject;

namespace Kiskovi.Core
{
    internal class TargetObject : MonoBehaviour
    {
        public Sprite icon;
        public bool canSeenAllTheTime;

        [Inject] private IObjectFollowManager followManager;

        private void OnEnable()
        {
            followManager.SubscribeTarget(new FollowTargetData()
            {
                canSeenAllTheTime = canSeenAllTheTime,
                goal = transform,
                icon = icon
            });
        }

        private void OnDisable()
        {
            followManager.UnSubscribeTarget(transform);
        }
    }
}
