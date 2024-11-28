using UnityEngine;

namespace Kiskovi.Core
{
    public interface ITimeManager
    {
        public void ResetStableTime();

        public void SetStableTimePausePanel(bool isStop);
    }

    internal class TimeManager : ModificationManager, ITimeManager
    {
        private static bool isStoppingWindow => UIWindow.IsWindowOpen;
        private static bool isStoppingPausePanel = false;

        public TimeManager() { }

        public void ResetStableTime()
        {
            isStoppingPausePanel = false;
            timeToResets.Clear();
        }

        public void SetStableTimePausePanel(bool isStop)
        {
            isStoppingPausePanel = isStop;
        }

        public override void Tick()
        {
            base.Tick();
            Time.timeScale = (isStoppingPausePanel || isStoppingWindow ? 0f : modification);
        }
    }
}
