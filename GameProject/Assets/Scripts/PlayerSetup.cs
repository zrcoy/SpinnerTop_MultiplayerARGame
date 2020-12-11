using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerSetup : MonoBehaviourPun
{
    public TextMeshProUGUI playerNameText;

    // when inheritant from MonoBehaviourPun, Start is called when the this object is spawned into the game
    void Start()
    {
        if(photonView.IsMine)
        {
            //the player is the local mine
            GetComponent<MovementController>().enabled = true;
            GetComponent<MovementController>().GetJoystick().gameObject.SetActive(true);
        }
        else
        {
            //the remote player
            GetComponent<MovementController>().enabled = false;
            GetComponent<MovementController>().GetJoystick().gameObject.SetActive(false);
        }

        SetPlayerName();
    }


    void SetPlayerName()
    {
        if(playerNameText!=null)
        {
            if(photonView.IsMine)
            {
                playerNameText.text = "Me";
                playerNameText.color = Color.red;
            }
            else
            {
                playerNameText.text = photonView.Owner.NickName;
            }
        }
    }

}
