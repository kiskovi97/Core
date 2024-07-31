using System;
using UnityEngine;

namespace Kiskovi.Core
{
  public class DataHolderBase : MonoBehaviour
  {
    public TriggerAction OnItemChanged;

    public event Action OnChanged;
    public IData StoredData { get; protected set; }

    public virtual void SetData(IData itemData)
    {
      StoredData = itemData;

      OnChanged?.Invoke();

      TriggerAction.Trigger(OnItemChanged);
    }
  }
}
