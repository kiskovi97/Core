using Zenject;

namespace Kiskovi.Core
{
    public class SelectionInstaller : Installer<SelectionInstaller>
    {
        public override void InstallBindings()
        {
            Container.DeclareSignal<SelectionClearSignal>().OptionalSubscriber();

            Container.BindInterfacesAndSelfTo<GlobalSelectionSystem>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GlobalSelectionNavigationSystem>().AsSingle().NonLazy();

        }
    }
}
