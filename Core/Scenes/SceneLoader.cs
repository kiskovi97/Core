using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

using Zenject;

using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace Kiskovi.Core
{
    public class SceneLoadRequestSignal 
    { 
        public int index;
        public bool force = false;
        public float delayTime = 0f;
    }

    internal class SceneLoader : MonoBehaviour
    {
        [SerializeField] private GameObject loadingPanel;
        [SerializeField] private Animator animator;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private GameObject uiBlock;
        [SerializeField] private TriggerAction onSceneChange;

        private static readonly string DE_LOAD_DEFAULT = "deLoad";
        private static readonly string DE_LOAD_NONE = "deLoad_none";
        private static readonly string LOAD_DEFAULT = "Load";
        private static readonly string LOAD_NONE = "Load_none";


        private static AssetReference lastScreenLoaded;
        private static int loadingScreen = 0;
        private SceneProvider _sceneProvider;
        private SignalBus _signalBus;

        private static SceneLoader Instance;

        private ITimeManager _timeManager;

        [Inject]
        internal void Initialize(SceneProvider sceneProvider, SignalBus signalBus, ITimeManager timeManager) //ITimeManager timeManager, 
        {
            _timeManager = timeManager;
            _sceneProvider = sceneProvider;
            _signalBus = signalBus;
        }

        private void Start()
        {
            SetInstance(this);
            if (uiBlock != null)
                uiBlock.SetActive(false);
            _signalBus.Subscribe<SceneLoadRequestSignal>(OnSceneLoadRequest);
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Debug.LogWarning("SceneLoaderDestroyed");
            }
            _signalBus.TryUnsubscribe<SceneLoadRequestSignal>(OnSceneLoadRequest);
        }

        private static void SetInstance(SceneLoader instance)
        {
            if (Instance == null)
            {
                Instance = instance;
                DontDestroyOnLoad(instance.gameObject);
                loadingScreen = SceneManager.GetActiveScene().buildIndex;
                if (Instance.loadingPanel != null)
                    Instance.loadingPanel.SetObjectActive(false);
            }
            else
            {
                Destroy(instance.loadingPanel);
                Destroy(instance);
            }
        }

        private void OnSceneLoadRequest(SceneLoadRequestSignal signal)
        {
            Debug.Log("Load Scene by index: " + signal.index);
            if (!signal.force && loadingScreen == signal.index) //|| index == SceneManager.GetActiveScene().buildIndex
            {
                Debug.Log("sceneIndex is the same as the loadingScreen: " + loadingScreen);
                return;
            }
            loadingScreen = signal.index;
            _LoadScene(signal.index, signal.delayTime);
        }


        private void _LoadScene(int sceneIndex, float delayTime)
        {
            ObjectsVisibilityManager.Clear();
            StartCoroutine(LoadAsyncronosly(sceneIndex, delayTime));
        }

        IEnumerator LoadWithotAnimationAsyncronosly(int sceneIndex, float delayTime)
        {
            yield return BeforeLoad(delayTime, DE_LOAD_NONE, 1f);
            Debug.Log("Load Scene started: " + sceneIndex);
            lastScreenLoaded = _sceneProvider.GetScene(sceneIndex);
            yield return lastScreenLoaded.LoadSceneAsync(LoadSceneMode.Single);
            yield return AfterLoad(LOAD_NONE, 0.1f);
        }

        IEnumerator LoadAsyncronosly(int sceneIndex, float delayTime)
        {
            yield return BeforeLoad(delayTime, DE_LOAD_DEFAULT, 1f);
            Debug.Log("Load Scene started: " + sceneIndex);            
            lastScreenLoaded = _sceneProvider.GetScene(sceneIndex);
            yield return lastScreenLoaded.LoadSceneAsync(LoadSceneMode.Single);
            yield return AfterLoad(LOAD_DEFAULT, 1f);
        }

        IEnumerator ReLoadSceneAsync()
        {
            yield return BeforeLoad(0f, DE_LOAD_DEFAULT, 1f);
            //yield return _sceneProvider.GetScene(0).LoadSceneAsync(LoadSceneMode.Single);  // TODO
            //yield return lastScreenLoaded.LoadSceneAsync(LoadSceneMode.Single);
            yield return AfterLoad(LOAD_DEFAULT, 1f);
        }

        IEnumerator BeforeLoad(float delayTime, string deloadTriggerName, float animationTime)
        {
            if (uiBlock != null)
                uiBlock.SetActive(true);
            if (loadingPanel != null)
                loadingPanel.SetObjectActive(true);
            if (animator != null)
                animator.SetTrigger("Idle");
            yield return new WaitForSecondsRealtime(delayTime);
            if (animator != null)
                animator.SetTrigger(deloadTriggerName);

            TriggerAction.Trigger(onSceneChange);

            yield return new WaitForSecondsRealtime(animationTime);
            _timeManager.SetStableTimePausePanel(true);
        }

        IEnumerator AfterLoad(string loadTrigger, float animationDelay)
        {
            yield return new WaitForSecondsRealtime(animationDelay);
            _timeManager.ResetStableTime();
            if (uiBlock != null)
                uiBlock.SetActive(false);
            if (animator != null)
                animator.SetTrigger(loadTrigger);
            yield return new WaitForSecondsRealtime(animationDelay);
            if (loadingPanel != null)
                loadingPanel.SetObjectActive(false);
        }

        public void ReLoadScene()
        {
            StartCoroutine(ReLoadSceneAsync());
        }
    }
}
