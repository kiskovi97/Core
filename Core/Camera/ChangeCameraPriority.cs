using Cinemachine;

namespace Kiskovi.Core
{
    internal class ChangeCameraPriority : TriggerAction
    {
        public CinemachineVirtualCamera virtualCamera;
        public int priority;

        public override void Trigger(params object[] parameter)
        {
            if (virtualCamera != null)
            {
                virtualCamera.Priority = priority;
            }
        }
    }
}
