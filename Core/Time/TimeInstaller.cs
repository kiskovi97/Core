using Zenject;

namespace Kiskovi.Core
{
    public class TimeInstaller : Installer<TimeInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<TimeManager>().AsSingle().NonLazy();
        }
    }
}
