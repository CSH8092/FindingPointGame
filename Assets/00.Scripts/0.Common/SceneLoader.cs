using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SceneLoader : MonoBehaviour
{
    public CanvasGroup canvas_this;
    public TextMeshProUGUI text_percentText;

    private SceneLoader() { }

    private static SceneLoader instance = null;
    public static SceneLoader Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SceneLoader();
            }
            return instance;
        }
    }

    void Start()
    {
        if (instance != null)
        {
            DestroyImmediate(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += StartScene;
    }

    void Update()
    {
#if true
        // debug code
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneLoader.Instance.LoadSceneByName(SceneManager.GetActiveScene().name);
        }
#endif
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= StartScene;
    }

    void StartScene(Scene scene, LoadSceneMode mode)
    {
        canvas_this.DOFade(0, 1).OnComplete(() => canvas_this.blocksRaycasts = false);
    }

    public void LoadSceneByName(string sceneName)
    {
        // canvas group init
        canvas_this.gameObject.SetActive(true);
        canvas_this.alpha = 0;
        canvas_this.blocksRaycasts = false;
        text_percentText.text = "0";

        // play fade in
        canvas_this.DOFade(1, 1).OnStart(() => canvas_this.blocksRaycasts = true).OnComplete(() => StartSceneCoroutine(sceneName));
    }

    public void StartSceneCoroutine(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        asyncLoad.allowSceneActivation = false; // ÀÏ´Ü stop

        float time_check = 0;
        float percent = 0;

        while (!asyncLoad.isDone)
        {
            yield return null;

            time_check += Time.deltaTime;

            if (percent >= 90)
            {
                percent = Mathf.Lerp(percent, 100, time_check);

                if (percent == 100)
                {
                    asyncLoad.allowSceneActivation = true;
                }
            }
            else
            {
                percent = Mathf.Lerp(percent, asyncLoad.progress * 100f, time_check);
                if (percent >= 90) time_check = 0;
            }

            text_percentText.text = percent.ToString("0");
        }
    }

    /*
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadJumpScene(int jumpIndex)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + jumpIndex);
    }
    */
}
