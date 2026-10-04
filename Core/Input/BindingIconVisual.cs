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

        public InputIcon m_Visual;

        [Inject]
        private IInputIconManager iconManager;

        void Update()
        {
            var binding = m_Reference.action.bindings.FirstOrDefault(x =>
                x.groups.Contains(InputSignals.SchemeName)
            );
            var icon = iconManager.GetIconData(m_Reference, binding);
            if (m_Visual != null)
            {
                m_Visual.SetData(icon);
            }
        }
    }
}
