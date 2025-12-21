// ============================================================================
// SCENE MANAGER
// ============================================================================
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

public class SceneLoader : Singleton<SceneLoader>
{
    private bool _isLoading;
    public bool IsLoading => _isLoading;

    public void LoadScene(string sceneName, Action onComplete = null)
    {
        if (_isLoading) return;
        StartCoroutine(LoadSceneAsync(sceneName, onComplete));
    }

    public void LoadScene(int sceneIndex, Action onComplete = null)
    {
        if (_isLoading) return;
        StartCoroutine(LoadSceneAsync(sceneIndex, onComplete));
    }

    public void ReloadCurrentScene()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator LoadSceneAsync(string sceneName, Action onComplete)
    {
        _isLoading = true;
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            GameEvents.OnLoadingProgress?.Invoke(progress);
            yield return null;
        }

        _isLoading = false;
        onComplete?.Invoke();
    }

    private IEnumerator LoadSceneAsync(int sceneIndex, Action onComplete)
    {
        _isLoading = true;
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            GameEvents.OnLoadingProgress?.Invoke(progress);
            yield return null;
        }

        _isLoading = false;
        onComplete?.Invoke();
    }
}
