using Zenject;

namespace Kiskovi.Core
{
    public class SelectionInstaller : Installer<SelectionInstaller>
    {
        public override void InstallBindings()
        {
            Container.DeclareSignal<SelectionClearSignal>();

            Container.BindInterfacesAndSelfTo<GlobalSelectionSystem>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GlobalSelectionNavigationSystem>().AsSingle().NonLazy();

        }
    }
}
