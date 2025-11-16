using Unity.Cinemachine;

namespace Kiskovi.Core
{
    internal class ChangeCameraPriority : TriggerAction
    {
        public CinemachineCamera virtualCamera;
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
