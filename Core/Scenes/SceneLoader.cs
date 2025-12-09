using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

using Zenject;

using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using System;

namespace Kiskovi.Core
{
    public class SceneLoadRequestSignal 
    { 
        public SceneEnum scene;
        public bool force = false;
        public float delayTime = 0f;
    }

    public class ReloadSceneSignal 
    {
    }

    internal class SceneLoader : MonoBehaviour
    {
        [SerializeField] private GameObject loadingPanel;
        [SerializeField] private Animator animator;
        [SerializeField] private float beforeLoadAnimationTime = 1f;
        [SerializeField] private float afterLoadAnimationTime = 1f;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private GameObject uiBlock;
        [SerializeField] private TriggerAction onSceneChange;

        private static readonly string DE_LOAD_DEFAULT = "deLoad";
        private static readonly string DE_LOAD_NONE = "deLoad_none";
        private static readonly string LOAD_DEFAULT = "Load";
        private static readonly string LOAD_NONE = "Load_none";


        private static AssetReference lastScreenLoaded;
        private static SceneEnum loadingScreen = 0;
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
            _signalBus.Subscribe<ReloadSceneSignal>(OnReload);
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Debug.LogWarning("SceneLoaderDestroyed");
            }
            _signalBus.TryUnsubscribe<SceneLoadRequestSignal>(OnSceneLoadRequest);
            _signalBus.TryUnsubscribe<ReloadSceneSignal>(OnReload);
        }

        private static void SetInstance(SceneLoader instance)
        {
            if (Instance == null)
            {
                Instance = instance;
                DontDestroyOnLoad(instance.gameObject);
                loadingScreen = SceneEnum.None;
                if (Instance.loadingPanel != null)
                    Instance.loadingPanel.SetObjectActive(false);
            }
            else
            {
                Destroy(instance.loadingPanel);
                Destroy(instance);
            }
        }

        private void OnReload(ReloadSceneSignal signal)
        {
            StartCoroutine(ReLoadSceneAsync());
        }

        private void OnSceneLoadRequest(SceneLoadRequestSignal signal)
        {
            Debug.Log("Load Scene by index: " + signal.scene);
            if (!signal.force && loadingScreen == signal.scene) //|| index == SceneManager.GetActiveScene().buildIndex
            {
                Debug.Log("sceneIndex is the same as the loadingScreen: " + loadingScreen);
                return;
            }
            loadingScreen = signal.scene;
            _LoadScene(signal.scene, signal.delayTime);
        }


        private void _LoadScene(SceneEnum sceneIndex, float delayTime)
        {
            ObjectsVisibilityManager.Clear();
            StartCoroutine(LoadAsyncronosly(sceneIndex, delayTime));
        }

        IEnumerator LoadWithotAnimationAsyncronosly(SceneEnum sceneIndex, float delayTime)
        {
            yield return BeforeLoad(delayTime, DE_LOAD_NONE, beforeLoadAnimationTime);
            Debug.Log("Load Scene started: " + sceneIndex);
            lastScreenLoaded = _sceneProvider.GetScene(sceneIndex);
            yield return lastScreenLoaded.LoadSceneAsync(LoadSceneMode.Single);
            yield return AfterLoad(LOAD_NONE, afterLoadAnimationTime);
        }

        IEnumerator LoadAsyncronosly(SceneEnum sceneIndex, float delayTime)
        {
            yield return BeforeLoad(delayTime, DE_LOAD_DEFAULT, beforeLoadAnimationTime);
            Debug.Log("Load Scene started: " + sceneIndex);      
            var scene = _sceneProvider.GetScene(sceneIndex);
            if (scene != null)
            {
                AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(scene, LoadSceneMode.Single);
                yield return handle;

                if (handle.Status == AsyncOperationStatus.Succeeded)
                    lastScreenLoaded = scene;
                else
                    Debug.LogError("There was a problem loading the scene");
            } else
            {
                Debug.LogError("There was a problem loading the scene, because it is null");
            }
            yield return AfterLoad(LOAD_DEFAULT, afterLoadAnimationTime);
        }

        IEnumerator ReLoadSceneAsync()
        {
            yield return BeforeLoad(0f, DE_LOAD_DEFAULT, beforeLoadAnimationTime);
            Debug.Log("ReLoad Scene started");

            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);

            yield return AfterLoad(LOAD_DEFAULT, afterLoadAnimationTime);
        }

        IEnumerator BeforeLoad(float delayTime, string deloadTriggerName, float animationTime)
        {
            if (uiBlock != null)
                uiBlock.SetActive(true);
            if (loadingPanel != null)
                loadingPanel.SetObjectActive(true);
            yield return new WaitForSecondsRealtime(delayTime);
            if (animator != null)
                animator.SetTrigger(deloadTriggerName);

            TriggerAction.Trigger(onSceneChange);

            yield return new WaitForSecondsRealtime(animationTime);
            _timeManager.SetStableTimePausePanel(true);
            if (animator != null)
                animator.ResetTrigger(deloadTriggerName);
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
            if (animator != null)
                animator.ResetTrigger(loadTrigger);
        }
    }
}
