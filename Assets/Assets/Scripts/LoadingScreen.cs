using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    public GameObject loadingScreen;
    public GameObject uiGamePlay;
    public void LoadScene()
    {
        StartCoroutine(LoadSceneAsync());
        Time.timeScale = 1f;
    }

    private IEnumerator LoadSceneAsync()
    {
        loadingScreen.SetActive(true);
        yield return new WaitForSecondsRealtime(3);
        Time.timeScale = 1f;
        uiGamePlay.SetActive(true);
        loadingScreen.SetActive(false);
        
    }
}
