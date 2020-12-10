using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoader : Singleton<SceneLoader>
{
    public void LoadScene(string sceneName)
    {
        Init(sceneName);

        StartCoroutine(InitializeSceneLoading());
    }

    private void Init(string sceneName)
    {
        m_sceneName = sceneName;
        m_percentageLoading = 0;
        m_showAnyKeyText = false;
        m_alreadyWait = false;
    }

    IEnumerator InitializeSceneLoading()
    {
        // first load scene 
        yield return SceneManager.LoadSceneAsync("Scene_Loading");

        //load the actual scene
        StartCoroutine(LoadActualScene());
    }


    IEnumerator LoadActualScene()
    {
        var asyncSceneLoading = SceneManager.LoadSceneAsync(m_sceneName);

        //this value disable showing up the scene content while loading
        asyncSceneLoading.allowSceneActivation = false;
        while (!asyncSceneLoading.isDone)
        {
            Debug.Log(asyncSceneLoading.progress);

            m_percentageLoading = asyncSceneLoading.progress * 100;
            if (asyncSceneLoading.progress >= 0.9f)
            {
                m_percentageLoading = 100;
                if (!m_alreadyWait)
                {
                    yield return new WaitForSeconds(1.5f);

                    m_alreadyWait = true;
                }
                m_showAnyKeyText = true;


                if (Input.anyKey)
                {
                    //if target scene to be loaded above 90 % as well as press any key, we can show that scene now
                    asyncSceneLoading.allowSceneActivation = true;
                }

            }

            //wait until the scene fully loaded
            yield return null;
        }
    }



    public float GetLoadingPercentage()
    {
        return m_percentageLoading;
    }

    public bool GetShowAnyKeyText()
    {
        return m_showAnyKeyText;
    }

    private string m_sceneName;
    private float m_percentageLoading = 0;
    private bool m_showAnyKeyText = false;
    private bool m_alreadyWait = false;
}
