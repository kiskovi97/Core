using System.Linq;
using ModestTree;
using UnityEngine;
using Zenject;

namespace Kiskovi.Core
{
    internal class PermissionObject : MonoBehaviour
    {
        [Inject]
        private IPermissionSettings _settings;

        public PermissionType[] requironments;
        public PermissionType[] vorbidden;

        public void Awake()
        {
            var allowed =
                requironments.IsEmpty()
                || requironments.All(item => _settings.IsPermissionType(item));
            var vorbiden = vorbidden.Any(item => _settings.IsPermissionType(item));
            if (!allowed || vorbiden)
            {
                Destroy(gameObject);
            }
        }
    }
}
