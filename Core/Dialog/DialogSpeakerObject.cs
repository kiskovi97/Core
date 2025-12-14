using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using Zenject;

namespace Kiskovi.Core
{
    internal class DialogSpeakerObject : MonoBehaviour
    {
        public DialogSpeaker speaker;
        public GameObject dialogObj;
        public TMP_Text dialogText;

        [Inject] private IDialogSystem _dialogSystem;

        private void SetDialog(LocalizedString localizedString, float progress)
        {
            var text = localizedString.GetLocalizedString();
            int charCount = Mathf.FloorToInt(text.Length * progress);

            dialogText.text = text;
            dialogText.maxVisibleCharacters = charCount;
            dialogObj.SetActive(true);
        }

        private void HideDialog()
        {
            dialogObj.SetActive(false);
        }

        private void OnEnable()
        {
            HideDialog();
        }

        private void OnDisable()
        {
            HideDialog();
        }

        private void Update()
        {
            var line = _dialogSystem.CurrentDialogLine;
            if (line != null && line.Value.speaker == speaker)
            {
                SetDialog(line.Value.text, _dialogSystem.LineProgress);
            }
            else
            {
                HideDialog();
            }
        }
    }
}
