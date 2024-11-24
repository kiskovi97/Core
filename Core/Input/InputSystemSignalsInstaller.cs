using Zenject;

namespace Kiskovi.Core
{
    public class InputSystemSignalsInstaller : Installer<InputSystemSignalsInstaller>
    {
        public override void InstallBindings()
        {
            Container.DeclareSignal<InputSignals.ControlSchemeChanged>().OptionalSubscriber();

            Container.DeclareSignal<UIInteractions.Navigate>().OptionalSubscriber();
            Container.DeclareSignal<UIInteractions.NavigateUI>().OptionalSubscriber();
            Container.DeclareSignal<UIInteractions.AccepSignal>().OptionalSubscriber();
            Container.DeclareSignal<UIInteractions.DeclineSignal>().OptionalSubscriber();
            Container.DeclareSignal<UIInteractions.ExitSignal>().OptionalSubscriber();
            Container.DeclareSignal<UIInteractions.DeleteSignal>().OptionalSubscriber();
            Container.DeclareSignal<UIInteractions.ModifyValueSignal>().OptionalSubscriber();

            Container.DeclareSignal<PauseGameRequestSignal>().OptionalSubscriber();

            Container.BindInterfacesAndSelfTo<UISignalSender>().AsSingle().NonLazy();
        }
    }
}
