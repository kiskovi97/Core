using System;
using UnityEngine;
using Zenject;

namespace Kiskovi.Core
{
    public interface IDialogSystem
    {
        bool isDialogActive { get; }
        DialogLine? CurrentDialogLine { get; }
        float LineProgress { get; }
        void StartDialog(DialogData dialog, bool restart = false);
        void EndDialog();
    }

    internal class DialogSystem : ITickable, IDialogSystem
    {
        public static readonly float SECOND_PER_CHARACTER = 0.1f;
        public static readonly float WAIT_TIME_AFTER_END = 1f;
        public static readonly float MIN_TIMER = 1f;

        private DialogData _currentDialog;
        private int _dialogIndex = 0;
        private float _dialogTimer = 0f;

        public bool isDialogActive => _currentDialog != null;
        public DialogLine? CurrentDialogLine
        {
            get
            {
                if (!isDialogActive) return null;
                if (_dialogIndex >= _currentDialog.Lines.Count) return null;
                return _currentDialog.Lines[_dialogIndex];
            }
        }

        public float LineProgress
        {
            get
            {
                var line = CurrentDialogLine;
                if (line == null) return 0f;
                var duration = CalculateDuration(line.Value);
                return Mathf.Max(0, duration - _dialogTimer) / duration;
            }
        }

        public void StartDialog(DialogData dialog, bool restart = false)
        {
            if (!restart && _currentDialog == dialog) return;

            _currentDialog = dialog;
            _dialogIndex = 0;

            var line = CurrentDialogLine;
            if (line != null)
            {
                _dialogTimer = CalculateDuration(line.Value);
            } else
            {
                _dialogTimer = 0f;
            }
        }

        public void EndDialog()
        {
            _currentDialog = null;
            _dialogIndex = 0;
            _dialogTimer = 0f;
        }

        public void Tick()
        {
            if (_currentDialog == null) return;

            if (_dialogTimer <= -WAIT_TIME_AFTER_END)
            {
                _dialogIndex++;
                if (_dialogIndex < _currentDialog.Lines.Count)
                {
                    var line = _currentDialog.Lines[_dialogIndex];
                    _dialogTimer = CalculateDuration(line);
                }
                else
                {
                    EndDialog();
                }
            }
            else
            {
                _dialogTimer -= UnityEngine.Time.deltaTime;
            }
        }

        private float CalculateDuration(DialogLine line)
        {
            int charCount = line.text.GetLocalizedString().Length;
            return Math.Max(MIN_TIMER, charCount * SECOND_PER_CHARACTER);
        }
    }
}
