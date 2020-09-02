using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MeditativeBowls
{
    public class SceneManager : MonoBehaviour
    {
        public static SceneManager Instance;

        [Header("States Handling")]
        public MenuManager.MenuStates prevState;
        public MenuManager.MenuStates currentState;

        [Header("Loading Screen")]
        public Animator panelAnim;
        public Image progressBar;

        AsyncOperation loadingOperation;
        bool start = false;

        private void Awake()
        {
            DontDestroyOnLoad(this);

            if(Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        private void OnEnable()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            panelAnim.gameObject.SetActive(false);
        }

        public void LoadScene(int sceneIndex)
        {
            StartCoroutine(LoadSceneE(sceneIndex));
        }

        public bool IsSceneLoaded(int sceneIndex)
        {
            return UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex.Equals(sceneIndex);
        }

        IEnumerator LoadSceneE(int sceneIndex)
        {
            panelAnim.gameObject.SetActive(true);
            panelAnim.Play("Load In");
            yield return new WaitForSeconds(.5f);

            progressBar.fillAmount = 0;
            float randomDelay = 0;
            float randomStop = Random.Range(0.65f, 0.85f);
            while (progressBar.fillAmount < randomStop)
            {
                randomDelay = Random.Range(0, 0.2f);
                progressBar.fillAmount += randomDelay;
                yield return new WaitForSeconds(randomDelay);
            }

            loadingOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);

            while (!loadingOperation.isDone)
            {
                yield return null;
            }

            progressBar.fillAmount = 1;

            yield return new WaitForSeconds(randomStop);
        }
    }
}
