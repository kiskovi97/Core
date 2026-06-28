using System;
using UnityEngine;
using Zenject;

namespace Kiskovi.Core
{
    [Serializable]
    public class SaveSystemSettings
    {
        public string saveKey = "data";
    }

    public class SaveSystemInstaller : Installer<SaveSystemSettings, SaveSystemInstaller>
    {
        [Inject]
        private SaveSystemSettings settings;

        public override void InstallBindings()
        {
            Container.DeclareSignal<IDatabaseManager.DatabaseChangedSignal>().OptionalSubscriber();

            Container
                .BindInterfacesAndSelfTo<SaveSystem>()
                .AsSingle()
                .WithArguments(Application.persistentDataPath, settings.saveKey)
                .NonLazy();
            Container.BindInterfacesAndSelfTo<DatabaseManager>().AsSingle().NonLazy();
        }
    }
}
