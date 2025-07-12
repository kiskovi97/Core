using Zenject;

namespace Kiskovi.Core
{
    public class SceneInstaller : Installer<SceneList, SceneInstaller>
    {
        public SceneList levelList;

        public SceneInstaller(SceneList levelList)
        {
            this.levelList = levelList;
        }

        public override void InstallBindings()
        {
            Container.DeclareSignal<SceneLoadRequestSignal>().OptionalSubscriber();
            Container.DeclareSignal<ReloadSceneSignal>().OptionalSubscriber();

            Container.BindInterfacesAndSelfTo<SceneProvider>().AsSingle().WithArguments(levelList).NonLazy();
        }
    }
}
