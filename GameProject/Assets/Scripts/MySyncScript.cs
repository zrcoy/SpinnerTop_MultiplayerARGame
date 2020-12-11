using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class MySyncScript : MonoBehaviour, IPunObservable
{


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        photonView = GetComponent<PhotonView>();

        networkedPosition = new Vector3();
        networkedRotation = new Quaternion();
    }

    void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            rb.position = Vector3.MoveTowards(rb.position, networkedPosition, Time.fixedDeltaTime);
            // * 100 to make changes more obvious on remote client
            rb.rotation = Quaternion.RotateTowards(rb.rotation, networkedRotation, Time.fixedDeltaTime * 100);
        }

    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //if we are controlling the local player
            //we now should send our transform, velocity, etc..data to other players
            stream.SendNext(rb.position);
            stream.SendNext(rb.rotation);
        }
        else
        {
            // if we are reading data
            // called on my player gameobject that exist on other players game
            networkedPosition = (Vector3)stream.ReceiveNext();
            networkedRotation = (Quaternion)stream.ReceiveNext();

        }
    }


    //private fields
    Rigidbody rb;
    PhotonView photonView;

    Vector3 networkedPosition;
    Quaternion networkedRotation;
}
