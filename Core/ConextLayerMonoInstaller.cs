using System;

using Zenject;

namespace Kiskovi.Core
{
    internal class ConextLayerMonoInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            // Bind a unique ID for this container
            string uniqueId = Guid.NewGuid().ToString();

            Container.DeclareSignal<MoveSignal>().WithId(uniqueId).OptionalSubscriber();
            Container.DeclareSignal<ActionHoldSignal>().WithId(uniqueId).OptionalSubscriber();
            Container.DeclareSignal<ActionPressedSignal>().WithId(uniqueId).OptionalSubscriber();   
            Container.DeclareSignal<CancelPressedSignal>().WithId(uniqueId).OptionalSubscriber();

            Container.Bind<string>().WithId("PlayerId").FromInstance(uniqueId);
        }
    }
}
