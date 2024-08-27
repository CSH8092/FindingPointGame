using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SceneLoader : MonoBehaviour
{

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
        
    }

    void Update()
    {
        
    }

    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadJumpScene(int jumpIndex)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + jumpIndex);
    }

    public void LoadSceneBySlider(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            Debug.Log(asyncLoad.progress); // 추후 slider bar 추가
            yield return null;
        }
    }
}
