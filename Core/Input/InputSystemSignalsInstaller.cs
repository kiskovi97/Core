using System;
using System.Xml;
using UnityEngine.InputSystem;
using Zenject;

namespace Kiskovi.Core
{
    [Serializable]
    public class InputSettings
    {
        public InputIconSettings iconSettings;
        public InputActionAsset[] actions;
    }

    public class InputSystemSignalsInstaller : Installer<InputSettings, InputSystemSignalsInstaller>
    {
        [Inject]
        private InputSettings settings;

        public override void InstallBindings()
        {
            Container.DeclareSignal<InputSignals.ControlSchemeChanged>().OptionalSubscriber();

            Container.DeclareSignal<UIInteractions.Navigate>().OptionalSubscriber();
            Container.DeclareSignal<UIInteractions.NavigateUI>().OptionalSubscriber();
            Container.DeclareSignal<UIInteractions.DragUI>().OptionalSubscriber();
            Container.DeclareSignal<UIInteractions.NavigateTabsSignal>().OptionalSubscriber();
            Container.DeclareSignal<UIInteractions.AcceptSignal>().OptionalSubscriber();
            Container.DeclareSignal<UIInteractions.DeclineSignal>().OptionalSubscriber();
            Container.DeclareSignal<UIInteractions.ExitSignal>().OptionalSubscriber();
            Container.DeclareSignal<UIInteractions.DeleteSignal>().OptionalSubscriber();
            Container.DeclareSignal<UIInteractions.ModifyValueSignal>().OptionalSubscriber();
            Container.DeclareSignal<UIInteractions.SkipSignal>().OptionalSubscriber();
            Container.DeclareSignal<UIInteractions.SkipDialogSignal>().OptionalSubscriber();

            Container.DeclareSignal<MoveSignal>().WithId(PlayerId.Player1).OptionalSubscriber();
            Container.DeclareSignal<MoveSignal>().WithId(PlayerId.Player2).OptionalSubscriber();
            Container.DeclareSignal<PauseGameRequestSignal>().OptionalSubscriber();
            Container.DeclareSignal<BindingChangedSignal>().OptionalSubscriber();

            Container.BindInterfacesAndSelfTo<AvailableInputManager>().AsSingle().NonLazy();
            Container
                .BindInterfacesAndSelfTo<InputIconManager>()
                .AsSingle()
                .WithArguments(settings.iconSettings)
                .NonLazy();

            Container.Bind<PlayerId>().WithId("PlayerId").FromInstance(PlayerId.Player1);
            Container
                .BindInterfacesAndSelfTo<RebindSaveLoad>()
                .AsSingle()
                .WithArguments(settings.actions)
                .NonLazy();
        }
    }
}
