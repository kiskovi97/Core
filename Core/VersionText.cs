using TMPro;
using UnityEngine;

namespace Kiskovi.Core
{
    public class VersionText : MonoBehaviour
    {
        public TMP_Text text;

        void Start()
        {
            text.text = Application.version;
        }
    }
}
