using Zenject;

namespace Kiskovi.Core
{
    internal class AvailableInputList : DataList<InputInfo>
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
            Clear();
            foreach(var item in manager.AvailableInputs)
            {
                AddItem(item);
            }
        }
    }
}
