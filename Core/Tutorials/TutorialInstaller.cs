using System;

using Zenject;

namespace Kiskovi.Core
{
    [Serializable]
    public class TutorialSettings
    {
        public TutorialReference[] tutorials;
    }

    public class TutorialInstaller : Installer<TutorialSettings, TutorialInstaller>
    {
        public TutorialSettings settings;

        public TutorialInstaller(TutorialSettings settings)
        {
            this.settings = settings;
        }
        public override void InstallBindings()
        {
            Container.Bind<ITutorialManager>().To<TutorialManager>().AsSingle().WithArguments(settings.tutorials).NonLazy();
        }
    }
}
