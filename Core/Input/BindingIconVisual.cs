using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

namespace Kiskovi.Core
{
    public class BindingInputIcon : MonoBehaviour
    {
        public InputActionReference m_Reference;

        [SerializeField]
        private string m_BindingId;

        public InputIcon m_Visual;

        [Inject]
        private IInputIconManager iconManager;

        void Update()
        {
            var bindingId = new Guid(m_BindingId);
            var binding = m_Reference.action.bindings.FirstOrDefault(x => x.id == bindingId);
            var icon = iconManager.GetIconData(m_Reference, binding);
            if (m_Visual != null)
            {
                m_Visual.SetData(icon);
            }
        }
    }
}
