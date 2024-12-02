using Zenject;

namespace Kiskovi.Core
{
    public class SaveSystemInstaller : Installer<SelectionInstaller>
    {
        public override void InstallBindings()
        {
            Container.DeclareSignal<IDatabaseManager.DatabaseChangedSignal>().OptionalSubscriber();

            Container.BindInterfacesAndSelfTo<SaveSystem>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<DatabaseManager>().AsSingle().NonLazy();

        }
    }
}
