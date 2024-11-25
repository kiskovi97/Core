using Zenject;

namespace Kiskovi.Core
{
    public class SceneInstaller : Installer<SceneInstaller>
    {
        public LevelList levelList;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<SceneProvider>().AsSingle().WithArguments(levelList).NonLazy();
        }
    }
}
