namespace Kiskovi.Core
{
    public interface IData
    {
    }

    public interface ICopyableData<T> : IData where T : IData
    {
        T Copy();
    }
}