using System;
using UnityEngine;
using Zenject;

namespace Kiskovi.Core
{
    [Serializable]
    public class PermissionSettings : IPermissionSettings
    {
        public bool isDemo;
        public bool isConvention;

        public bool IsPermissionType(PermissionType type)
        {
            switch (type)
            {
                case PermissionType.Demo:
                    return isDemo;
                case PermissionType.Convention:
                    return isConvention;
                case PermissionType.Editor:
                    return Application.isEditor;
                default:
                    return false;
            }
        }
    }

    public enum PermissionType
    {
        Demo,
        Convention,
        Editor,
    }

    public interface IPermissionSettings
    {
        bool IsPermissionType(PermissionType type);
    }

    public class PermissionInstaller : Installer<PermissionSettings, PermissionInstaller>
    {
        private PermissionSettings _settings;
        public PermissionInstaller(PermissionSettings settings)
        {
            _settings = settings;
        }

        public override void InstallBindings()
        {
            Container.Bind<IPermissionSettings>().FromInstance(_settings).AsSingle();
        }
    }
}
