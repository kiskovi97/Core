using UnityEngine;
using Zenject;

namespace Kiskovi.Core
{
    internal class ObjectFollowManagerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container
                .Bind<IObjectFollowManager>()
                .To<ObjectFollowManager>()
                .FromComponentInHierarchy()
                .AsSingle();
        }
    }
}
