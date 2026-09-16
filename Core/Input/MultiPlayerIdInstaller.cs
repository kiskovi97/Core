using System;
using System.Linq;
using Zenject;

namespace Kiskovi.Core
{
    internal class MultiPlayerIdInstaller : MonoInstaller
    {
        public PlayerId playerId;

        public override void InstallBindings()
        {
            playerId = MultiPlayerManager.Players.Count() > 1 ? PlayerId.Player2 : PlayerId.Player1;
            Container.Bind<PlayerId>().WithId("PlayerId").FromInstance(playerId);
        }
    }
}
