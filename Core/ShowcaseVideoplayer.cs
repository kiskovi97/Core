using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Video;

namespace Kiskovi.Core
{
    public class ShowcaseVideoplayer : MonoBehaviour
    {
        public VideoPlayer videoPlayer;
        public GameObject uiCanvas;
        public float timeLimit = 60f;

        private float time = 0f;

        public InputActionReference[] inputsToStop;

        // Start is called before the first frame update
        void Start()
        {
            foreach (var input in inputsToStop)
            {
                input.action.Enable();
            }
        }

        private void OnEnable()
        {
            foreach (var input in inputsToStop)
            {
                input.action.performed += Action_performed;
            }
            videoPlayer.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            foreach (var input in inputsToStop)
            {
                input.action.performed -= Action_performed;
            }
        }

        private void Update()
        {
            time += Time.deltaTime;
            if (time > timeLimit)
            {
                ShowVideoPlayer();
            }
        }

        private void Action_performed(InputAction.CallbackContext obj)
        {
            time = 0f;
            HideVideoPlayer();
        }

        private void ShowVideoPlayer()
        {
            if (uiCanvas.activeSelf)
            {
                videoPlayer.gameObject.SetActive(true);
                videoPlayer.Play();
                uiCanvas.SetActive(false);
            }
        }

        private void HideVideoPlayer()
        {
            if (!uiCanvas.activeSelf)
            {
                videoPlayer.Stop();
                videoPlayer.gameObject.SetActive(false);
                uiCanvas.SetActive(true);
            }
        }
    }
}
