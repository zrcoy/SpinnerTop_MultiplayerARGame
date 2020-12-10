using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingSceneController : MonoBehaviour
{
    [SerializeField] GameObject progressNumberText = null;
    [SerializeField] GameObject anyKeyText = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneLoader.Instance.GetShowAnyKeyText())
        {
            progressNumberText.SetActive(false);
            anyKeyText.SetActive(true);
        }
        else
        {
            progressNumberText.SetActive(true);
            anyKeyText.SetActive(false);
        }
    }

}
