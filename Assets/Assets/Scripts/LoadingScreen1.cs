using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LoadingScreen1 : MonoBehaviour
{
    public GameObject loadingScreen;
    public Image LoadingFillbar;

    public void Start()
    {
        StartCoroutine(LoadSceneAsync(1));
    }

    public void LoadScene( int sceneID)
    {
        StartCoroutine(LoadSceneAsync(1));
        Time.timeScale = 1f;
    }

    private IEnumerator LoadSceneAsync(int sceneID)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneID);
        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 1f);
            LoadingFillbar.fillAmount = progressValue;
            yield return null;
        }
        
    }
}
