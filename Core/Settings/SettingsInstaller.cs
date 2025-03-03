using Zenject;

namespace Kiskovi.Core
{
    public class SettingsInstaller : Installer<SettingsInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<SettingsTable>().AsSingle().NonLazy();
        }
    }
}
