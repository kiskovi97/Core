using UnityEngine;

namespace Kiskovi.Core
{
    public class Condition : MonoBehaviour
    {
        public virtual bool Evaulate()
        {
            return true;
        }
    }
}
