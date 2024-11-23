namespace Kiskovi.Core
{
    internal class UIBasePanel : UIPanel
    {
        public static UIBasePanel Instance { get; set; }

        private void Awake()
        {
            Instance = this;
            OnOpened();
            ToFront();
        }

        public void ToFront()
        {
            OnFront();
        }

        public void ToBack()
        {
            OnBackground();
        }
    }
}
