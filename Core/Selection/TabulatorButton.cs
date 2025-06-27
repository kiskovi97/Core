using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;

namespace Kiskovi.Core
{
    internal class TabulatorButton : MonoBehaviour
    {
        public Button button;
        public GameObject onAcive;
        public GameObject onDeAcive;
        public Vector3 onActiveSize = Vector3.one;
        public string titleKey;

        private Tabulators slidePanel;
        private bool active;

        private void Awake()
        {
            slidePanel = GetComponentInParent<Tabulators>();
        }

        public void SetActive(bool active)
        {
            this.active = active;
            SetupValues();
        }

        public void OnClick()
        {
            slidePanel.SetIndex(this);
        }

        private void SetupValues()
        {
            if (onAcive != null)
            {
                onAcive.SetActive(active);
            }
            if (onDeAcive != null)
            {
                onDeAcive.SetActive(!active);
            }
            transform.localScale = active ? onActiveSize : Vector3.one;
        }
    }
}
