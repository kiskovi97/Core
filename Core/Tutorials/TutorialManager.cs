using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine.PlayerLoop;

namespace Kiskovi.Core
{
    public interface ITutorialManager
    {
        void SetupTutorials(IEnumerable<string> finishedTutorials);

        void CompleteTutorial(TutorialReference reference);
        bool IsTutorialAvailable(TutorialReference reference);

        bool IsTutorialComplete(TutorialReference reference);

        void ClearTutorials();

        IEnumerable<string> FinishedTutorials { get; }

        event Action onChanged;
    }

    internal class TutorialManager : ITutorialManager
    {
        private TutorialReference[] _tutorials = new TutorialReference[0];

        private List<string> _finishedTutorials = new List<string>();

        public IEnumerable<string> FinishedTutorials => _finishedTutorials;
        public event Action onChanged;

        public TutorialManager(IEnumerable<TutorialReference> tutorials)
        {
            _tutorials = tutorials.ToArray();

            SendChange();
        }

        public void SetupTutorials(IEnumerable<string> finishedTutorials)
        {
            _finishedTutorials = finishedTutorials.ToList();
            SendChange();
        }

        public void ClearTutorials()
        {
            _finishedTutorials.Clear();
            SendChange();
        }

        public void CompleteTutorial(TutorialReference reference)
        {
            if (!_finishedTutorials.Contains(reference.key))
                _finishedTutorials.Add(reference.key);
            SendChange();
        }

        public bool IsTutorialComplete(TutorialReference reference)
        {
            return _finishedTutorials.Contains(reference.key);
        }

        private void SendChange()
        {
            onChanged?.Invoke();
        }

        public bool IsTutorialAvailable(TutorialReference reference)
        {
            return reference.dependencies.All(IsTutorialComplete);
        }
    }
}
