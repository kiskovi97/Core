using Zenject;

namespace Kiskovi.Core
{
    public class DialogInstaller : Installer<DialogInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<DialogSystem>().AsSingle().NonLazy();
        }
    }
}
