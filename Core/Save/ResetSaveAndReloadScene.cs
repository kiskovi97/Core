using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

using Zenject;

namespace Kiskovi.Core
{
    internal class ResetSaveAndReloadScene : TriggerAction
    {
        [Inject] private IDatabaseManager databaseManager;
        [Inject] private SignalBus signalBus;

        public override async void Trigger(params object[] parameter)
        {
            base.Trigger(parameter);
            await databaseManager.ClearAll();
            signalBus.TryFire(new ReloadSceneSignal());
        }
    }
}
