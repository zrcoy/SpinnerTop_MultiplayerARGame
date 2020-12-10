using System;
using UnityEngine;
using UnityEngine.UI;

public class LoadingProgressNumberText : MonoBehaviour
{
    void Update()
    {
        GetComponent<TMPro.TextMeshProUGUI>().text = String.Format("{0:0}%", SceneLoader.Instance.GetLoadingPercentage());
    }
}
