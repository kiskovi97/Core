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
                case PermissionType.PC:
                    return Application.platform == RuntimePlatform.WindowsPlayer
                        || Application.platform == RuntimePlatform.OSXPlayer
                        || Application.platform == RuntimePlatform.LinuxPlayer;
                case PermissionType.Mobile:
                    return Application.platform == RuntimePlatform.Android
                        || Application.platform == RuntimePlatform.IPhonePlayer;
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
        PC,
        Mobile,
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
