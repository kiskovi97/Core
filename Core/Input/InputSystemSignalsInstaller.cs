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


            Container.DeclareSignal<MoveSignal>().OptionalSubscriber();
            Container.DeclareSignal<ActionHoldSignal>().OptionalSubscriber();
            Container.DeclareSignal<ActionPressedSignal>().OptionalSubscriber();
            Container.DeclareSignal<CancelPressedSignal>().OptionalSubscriber();

            Container.DeclareSignal<ToggleMapSignal>().OptionalSubscriber();

            Container.DeclareSignal<PauseGameRequestSignal>().OptionalSubscriber();

            Container.BindInterfacesAndSelfTo<UISignalSender>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<MovementSignalSender>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<InteractionSignalSender>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<MapSignalSender>().AsSingle().NonLazy();
        }
    }
}
