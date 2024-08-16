namespace Kiskovi.Core
{
    public class DataHolder<T> : DataHolderBase where T : class, IData
    {
        public T Data { get; protected set; }

        public virtual bool IsAvailable => true;

        public override void SetData(IData itemData)
        {
            Data = itemData as T;
            base.SetData(itemData);
        }

        public void Refresh()
        {
            SetData(Data);
        }
    }
}
