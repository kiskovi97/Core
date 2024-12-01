namespace Kiskovi.Core
{
    internal class TeleportToAction : TriggerAction
    {
        public PlayerController character;
        public PlayerTeleportPoint teleportPoint;

        public override void Trigger(params object[] parameter)
        {
            character.transform.position = teleportPoint.targetPoint.position;
        }
    }
}
