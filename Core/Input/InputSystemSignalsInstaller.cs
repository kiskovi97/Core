using Zenject;

namespace Kiskovi.Core
{
    public class InputSystemSignalsInstaller : Installer<InputSystemSignalsInstaller>
    {
        public override void InstallBindings()
        {
            Container.DeclareSignal<InputSignals.ControlSchemeChanged>();

            Container.DeclareSignal<UIInteractions.Navigate>();
            Container.DeclareSignal<UIInteractions.NavigateUI>();
            Container.DeclareSignal<UIInteractions.AccepSignal>();
            Container.DeclareSignal<UIInteractions.DeclineSignal>();
            Container.DeclareSignal<UIInteractions.ExitSignal>();
            Container.DeclareSignal<UIInteractions.DeleteSignal>();
            Container.DeclareSignal<UIInteractions.ModifyValueSignal>();

            Container.DeclareSignal<PauseGameRequestSignal>();

            Container.BindInterfacesAndSelfTo<UISignalSender>().AsSingle().NonLazy();
        }
    }
}
