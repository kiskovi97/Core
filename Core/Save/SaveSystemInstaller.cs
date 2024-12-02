using UnityEngine;

using Zenject;

namespace Kiskovi.Core
{
    public class SaveSystemInstaller : Installer<SaveSystemInstaller>
    {
        public override void InstallBindings()
        {
            Container.DeclareSignal<IDatabaseManager.DatabaseChangedSignal>().OptionalSubscriber();

            Container.BindInterfacesAndSelfTo<SaveSystem>().AsSingle().WithArguments(Application.persistentDataPath).NonLazy();
            Container.BindInterfacesAndSelfTo<DatabaseManager>().AsSingle().NonLazy();

        }
    }
}
