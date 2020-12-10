using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [Header("Login UI")]
    public InputField playerNameInputField;
    public GameObject UI_LoginGameObject;

    [Header("Lobby UI")]
    public GameObject UI_LobbyGameObject;
    public GameObject UI_3DGameObject;

    [Header("Connection Status UI")]
    public GameObject UI_ConnectionStatusGameObject;
    public Text connectionStatusText;
    public bool showConnectStatus = false;





    #region Unity Methods
    
    void Start()
    {
        UI_LobbyGameObject.SetActive(false);
        UI_3DGameObject.SetActive(false);
        UI_ConnectionStatusGameObject.SetActive(false);

        UI_LoginGameObject.SetActive(true);
    }

    
    void Update()
    {
        if(showConnectStatus)
        {
            connectionStatusText.text = "Connection Status: " + PhotonNetwork.NetworkClientState;
        }
    }
    #endregion






    #region UI Callback Methods

    public void OnEnterGameButtonClicked()
    {
        string playerName = playerNameInputField.text;

        if(!string.IsNullOrEmpty(playerName))
        {
            UI_LobbyGameObject.SetActive(false);
            UI_3DGameObject.SetActive(false);
            UI_LoginGameObject.SetActive(false);

            UI_ConnectionStatusGameObject.SetActive(true);

            showConnectStatus = true;

            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.LocalPlayer.NickName = playerName;

                PhotonNetwork.ConnectUsingSettings();
            }
        }
        else
        {
            Debug.Log("Player name is invalid or empty!");
        }
    }


    public void OnQuickMatchButtonClicked()
    {
        //SceneManager.LoadScene("Scene_Loading");
        SceneLoader.Instance.LoadScene("Scene_PlayerSelection");
    }

    #endregion










    #region Photon Callback Methods

    public override void OnConnected()
    {
        Debug.Log("We connected to Internet");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " is connected to Photon Server.");

        UI_LobbyGameObject.SetActive(true);
        UI_3DGameObject.SetActive(true);
        UI_LoginGameObject.SetActive(false);

        UI_ConnectionStatusGameObject.SetActive(false);

    }

    #endregion
}
