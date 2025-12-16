using Zenject;

namespace Kiskovi.Core
{
    internal class AvailableInputList : DataList<InputInfoGroup>
    {
        [Inject] private IAvailableInputManager manager;

        private void OnEnable()
        {
            manager.OnChanged += Manager_OnChanged;
            Manager_OnChanged();
        }

        private void OnDisable()
        {
            manager.OnChanged -= Manager_OnChanged;
        }

        private void Manager_OnChanged()
        {
            UpdateList(manager.AvailableInputs);
        }
    }
}
