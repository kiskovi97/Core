using System;
using Zenject;

namespace Kiskovi.Core
{
    internal class PlayerIdInstaller : MonoInstaller
    {
        public PlayerId playerId;

        public override void InstallBindings()
        {
            Container.Bind<PlayerId>().WithId("PlayerId").FromInstance(playerId);
        }
    }
}
