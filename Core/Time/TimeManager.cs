using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Kiskovi.Core
{
    public interface ITimeManager
    {
        public void ResetStableTime();
        public void ChangePause(string key, bool value);

        public void SetStableTimePausePanel(bool isStop);
    }

    internal class TimeManager : ModificationManager, ITimeManager
    {
        private static bool isStoppingWindow => UIWindow.IsWindowOpen;
        private static bool isStoppingPausePanel = false;

        private HashSet<string> pauseKeys = new HashSet<string>();

        public TimeManager() { }

        public void ResetStableTime()
        {
            isStoppingPausePanel = false;
            timeToResets.Clear();
            pauseKeys.Clear();
        }

        public void SetStableTimePausePanel(bool isStop)
        {
            isStoppingPausePanel = isStop;
        }

        public override void Tick()
        {
            base.Tick();
            Time.timeScale = (isStoppingPausePanel || isStoppingWindow || pauseKeys.Any() ? 0f : modification);
        }

        public void ChangePause(string key, bool value)
        {
            if (value)
            {
                if (!pauseKeys.Contains(key))
                    pauseKeys.Add(key);
            }
            else
            {
                if (pauseKeys.Contains(key))
                    pauseKeys.Remove(key);
            }
        }
    }
}
