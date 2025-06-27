using UnityEngine;
using UnityEngine.UI;
using Zenject;

using System;

namespace Kiskovi.Core
{
    internal class Tabulators : MonoBehaviour
    {
        [SerializeField] private Button leftButton;
        [SerializeField] private Button rightButton;
        [SerializeField] private GameObject[] panels;
        [SerializeField] private TabulatorButton[] buttons;
        private int currentIndex = 0;

        [Inject] private SignalBus _signalBus;

        // Start is called before the first frame update
        void Start()
        {
            ChangeIndex(0);
            if (leftButton != null)
                leftButton.onClick.AddListener(LeftSlide);
            if (rightButton != null)
                rightButton.onClick.AddListener(RightSlide);
            _signalBus.Subscribe<UIInteractions.NavigateTabsSignal>(OnChangeTab);
            _signalBus.Subscribe<UIInteractions.ExitSignal>(OnExit);
        }

        private void OnDestroy()
        {
            _signalBus.TryUnsubscribe<UIInteractions.NavigateTabsSignal>(OnChangeTab);
            _signalBus.TryUnsubscribe<UIInteractions.ExitSignal>(OnExit);
        }

        private void RightSlide()
        {
            ChangeIndex(currentIndex + 1);
        }

        private void LeftSlide()
        {
            ChangeIndex(currentIndex - 1);
        }

        internal void SetIndex(TabulatorButton button)
        {
            var index = Array.IndexOf(buttons, button);
            if (index < 0) return;

            ChangeIndex(index);
        }

        void ChangeIndex(int index)
        {
            currentIndex = (int)Mathf.Repeat(index, panels.Length);

            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i] != null) buttons[i].SetActive(i == currentIndex);
            }
            for (int i = 0; i < panels.Length; i++)
            {
                if (panels[i] != null) panels[i].SetActive(i == currentIndex);
            }

            if (leftButton != null)
                leftButton.interactable = currentIndex != 0;
            if (rightButton != null)
                rightButton.interactable = currentIndex < panels.Length - 1;
        }

        public void OnExit()
        {
            ChangeIndex(0);
        }

        public void OnChangeTab(UIInteractions.NavigateTabsSignal signal)
        {
            if (signal.value)
                RightSlide();
            else
                LeftSlide();
        }
    }
}
