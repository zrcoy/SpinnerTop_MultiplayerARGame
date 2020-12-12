using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class MySyncScript : MonoBehaviour, IPunObservable
{
    public bool syncVelocity = true;
    public bool synvAngularVelocity = true;
    public bool isTeleportEnabled = true;
    public float teleportIfDistanceGreaterThan = 1.0f;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
        m_photonView = GetComponent<PhotonView>();

        m_networkedPosition = new Vector3();
        m_networkedRotation = new Quaternion();
    }

    void FixedUpdate()
    {
        if (!m_photonView.IsMine)
        {
            m_rb.position = Vector3.MoveTowards(m_rb.position, m_networkedPosition, m_distance * (1.0f / PhotonNetwork.SerializationRate));
            // * 100 to make changes more obvious on remote client
            m_rb.rotation = Quaternion.RotateTowards(m_rb.rotation, m_networkedRotation, m_angle * (1.0f / PhotonNetwork.SerializationRate));
        }
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //if we are controlling the local player
            //we now should send our transform, velocity, etc..data to other players
            stream.SendNext(m_rb.position);
            stream.SendNext(m_rb.rotation);
            if (syncVelocity)
            {
                stream.SendNext(m_rb.velocity);
            }
            if (synvAngularVelocity)
            {
                stream.SendNext(m_rb.angularVelocity);
            }
        }
        else
        {
            // if we are reading data
            // called on my player gameobject that exist on other players game
            m_networkedPosition = (Vector3)stream.ReceiveNext();
            m_networkedRotation = (Quaternion)stream.ReceiveNext();

            if (isTeleportEnabled)
            {
                if (Vector3.Distance(m_rb.position, m_networkedPosition) > teleportIfDistanceGreaterThan)
                {
                    m_rb.position = m_networkedPosition;
                }
            }


            if (synvAngularVelocity || syncVelocity)
            {
                //get the positive lag time between data send time and current consistent network received time
                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));

                if (syncVelocity)
                {
                    m_rb.velocity = (Vector3)stream.ReceiveNext();
                    m_networkedPosition += m_rb.velocity * lag;
                    m_distance = Vector3.Distance(m_rb.position, m_networkedPosition);
                }

                if (synvAngularVelocity)
                {
                    m_rb.angularVelocity = (Vector3)stream.ReceiveNext();

                    m_networkedRotation = Quaternion.Euler(m_rb.angularVelocity * lag) * m_networkedRotation;

                    m_angle = Quaternion.Angle(m_rb.rotation, m_networkedRotation);
                }
            }
        }
    }


    //private fields
    private Rigidbody m_rb;
    private PhotonView m_photonView;
    private Vector3 m_networkedPosition;
    private Quaternion m_networkedRotation;
    private float m_distance;
    private float m_angle;
}
