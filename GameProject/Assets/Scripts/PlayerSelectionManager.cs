using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class PlayerSelectionManager : MonoBehaviour
{
    public Transform playerSwitcherTransform;

    public int playerSelectionNumber;

    public GameObject[] spinnerTopModels;
    public GameObject ui_Selection;
    public GameObject ui_AfterSelection;


    [Header("UI")]
    public TextMeshProUGUI playerModelType_Text;
    public Button next_Button;
    public Button previous_button;


    #region UNITY Methods
    // Start is called before the first frame update
    void Start()
    {
        ui_Selection.SetActive(true);
        ui_AfterSelection.SetActive(false);


        playerSelectionNumber = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    #endregion


    #region UI Callback Methods
    public void NextPlayer()
    {
        playerSelectionNumber += 1;

        if (playerSelectionNumber >= spinnerTopModels.Length)
        {
            playerSelectionNumber = 0;
        }
        Debug.Log(playerSelectionNumber);
        next_Button.enabled = false;
        previous_button.enabled = false;

        StartCoroutine(Rotate(Vector3.up, playerSwitcherTransform, 90, 1.0f));


        if (playerSelectionNumber == 0 || playerSelectionNumber==1)
        {
            playerModelType_Text.text = "Attack";
        }
        else
        {
            playerModelType_Text.text = "Defend";
        }
    }

    public void PreviousPlayer()
    {
        playerSelectionNumber -= 1;

        if (playerSelectionNumber < 0)
        {
            playerSelectionNumber = spinnerTopModels.Length - 1;
        }
        Debug.Log(playerSelectionNumber);
        next_Button.enabled = false;
        previous_button.enabled = false;

        StartCoroutine(Rotate(Vector3.up, playerSwitcherTransform, -90, 1.0f));

        if (playerSelectionNumber == 0 || playerSelectionNumber == 1)
        {
            playerModelType_Text.text = "Attack";
        }
        else
        {
            playerModelType_Text.text = "Defend";
        }
    }


    public void OnSelectButtonClicked()
    {
        ui_Selection.SetActive(false);
        ui_AfterSelection.SetActive(true);



        ExitGames.Client.Photon.Hashtable playerSelectionProp = new ExitGames.Client.Photon.Hashtable { { MultiplayerARSpinnerTopGame.PLAYER_SELECTION_NUMBER,playerSelectionNumber} };
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerSelectionProp);
    }

    public void OnReselectionButtonClicked()
    {
        ui_Selection.SetActive(true);
        ui_AfterSelection.SetActive(false);
    }


    public void OnBattleButtonClicked()
    {
        SceneLoader.Instance.LoadScene("Scene_Gameplay");
    }


    public void OnBackButtonClicked()
    {
        SceneLoader.Instance.LoadScene("Scene_Lobby");
    }



    #endregion




    #region Private Methods
    IEnumerator Rotate(Vector3 axis, Transform transformToRotate, float angle, float duration = 1.0f)
    {
        Quaternion originalRotation = transformToRotate.rotation;
        Quaternion finalRotation = transformToRotate.rotation * Quaternion.Euler(axis * angle);

        float elapsedTime = 0.0f;
        while (elapsedTime < duration)
        {
            transformToRotate.rotation = Quaternion.Slerp(originalRotation, finalRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transformToRotate.rotation = finalRotation;



        next_Button.enabled = true;
        previous_button.enabled = true;
    }
    #endregion
}
