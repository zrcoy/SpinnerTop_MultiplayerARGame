using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

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

    #region Unity Methods
    // Start is called before the first frame update
    void Start()
    {
        UI_LobbyGameObject.SetActive(false);
        UI_3DGameObject.SetActive(false);
        UI_ConnectionStatusGameObject.SetActive(false);

        UI_LoginGameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    #region UI Callback Methods

    public void OnEnterGameButtonClicked()
    {

        UI_LobbyGameObject.SetActive(false);
        UI_3DGameObject.SetActive(false);
        UI_LoginGameObject.SetActive(false);
        
        UI_ConnectionStatusGameObject.SetActive(true);
        

        string playerName = playerNameInputField.text;

        if(!string.IsNullOrEmpty(playerName))
        {
            if(!PhotonNetwork.IsConnected)
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

    #endregion



    #region Photon Callback Methods

    public override void OnConnected()
    {
        Debug.Log("We connected to Internet");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " is connected to Photon Server.");
    }

    #endregion
}
