using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class BattleScript : MonoBehaviourPun
{
    public GameObject uI3DgameObject;
    public GameObject deathUIPanelPrefab;

    public Spinner spinnerScript;
    public Image spinSpeedBar_Image;
    public TextMeshProUGUI spinSpeedRatio_Text;
    public float common_Damage_Coefficient = 0.04f;
    public bool isAttacker;
    public bool isDefender;

    [Header("Player Type Damage Coefficients")]
    public float doDamage_Coefficient_Attacker = 10f;
    public float getDamaged_Coefficient_Attacker = 1.2f;

    public float doDamage_Coefficient_Defender = 0.75f;
    public float getDamaged_Coefficient_Defender = 0.2f;

    // Start is called before the first frame update
    private void Awake()
    {
        _startSpinSpeed = spinnerScript.SpinSpeed;
        _currentSpinSpeed = spinnerScript.SpinSpeed;
        spinSpeedBar_Image.fillAmount = _currentSpinSpeed / _startSpinSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //main battle logic
        if (collision.gameObject.CompareTag("Player"))
        {
            //compare the speed of both spinnertops
            float mySpeed = gameObject.GetComponent<Rigidbody>().velocity.magnitude;
            float otherPlayerSpeed = collision.collider.gameObject.GetComponent<Rigidbody>().velocity.magnitude;

            if (mySpeed > otherPlayerSpeed)
            {
                Debug.Log("You damage other player");

                float default_damage_amount = gameObject.GetComponent<Rigidbody>().velocity.magnitude * 3600 * common_Damage_Coefficient;

                if (isAttacker)
                {
                    default_damage_amount *= doDamage_Coefficient_Attacker;
                }
                else if (isDefender)
                {
                    default_damage_amount *= doDamage_Coefficient_Defender;
                }

                // check if the slower one is the local client,if doesn't have this check, will apply damage as many times as the player numbers 
                if (collision.collider.gameObject.GetComponent<PhotonView>().IsMine)
                {
                    //apply damage
                    collision.collider.gameObject.GetComponent<PhotonView>().RPC("DoDamage", RpcTarget.AllBuffered, default_damage_amount);
                }
            }

        }
    }


    [PunRPC]
    public void DoDamage(float damageAmount)
    {
        if (_isDead) return;

        if (isAttacker)
        {
            damageAmount *= getDamaged_Coefficient_Attacker;
        }
        else if (isDefender)
        {
            damageAmount *= getDamaged_Coefficient_Defender;
        }

        spinnerScript.SpinSpeed -= damageAmount;
        _currentSpinSpeed = spinnerScript.SpinSpeed;

        spinSpeedBar_Image.fillAmount = _currentSpinSpeed / _startSpinSpeed;
        spinSpeedRatio_Text.text = _currentSpinSpeed.ToString("F0") + " / " + _startSpinSpeed;

        if (_currentSpinSpeed < 100)
        {
            //Die
            Die();
        }
    }

    void Die()
    {
        _isDead = true;

        GetComponent<MovementController>().enabled = false;
        _rb.freezeRotation = false;
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        spinnerScript.SpinSpeed = 0f;
        uI3DgameObject.SetActive(false);

        if (photonView.IsMine)
        {
            //countdown for respawn
            StartCoroutine(Respawn());
        }
    }

    IEnumerator Respawn()
    {
        GameObject canvasObj = GameObject.Find("Canvas");
        if (_deathUIPanelObj == null)
        {
            _deathUIPanelObj = Instantiate(deathUIPanelPrefab, canvasObj.transform);

        }
        else
        {
            _deathUIPanelObj.SetActive(true);
        }
        Text respawnTimeText = _deathUIPanelObj.transform.Find("RespawnTimeText").GetComponent<Text>();
        float respawnTime = 8.0f;
        respawnTimeText.text = respawnTime.ToString(".00");

        while (respawnTime > 0.0f)
        {
            yield return new WaitForSeconds(1.0f);
            respawnTime -= 1.0f;
            respawnTimeText.text = respawnTime.ToString(".00");
            GetComponent<MovementController>().enabled = false;
        }

        _deathUIPanelObj.SetActive(false);
        GetComponent<MovementController>().enabled = true;

        photonView.RPC("ReBorn", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void ReBorn()
    {
        _isDead = false;

        spinnerScript.SpinSpeed = _startSpinSpeed;
        _currentSpinSpeed = spinnerScript.SpinSpeed;

        spinSpeedBar_Image.fillAmount = _currentSpinSpeed / _startSpinSpeed;
        spinSpeedRatio_Text.text = _currentSpinSpeed + "/" + _startSpinSpeed;

        _rb.freezeRotation = true;
        transform.rotation = Quaternion.Euler(Vector3.zero);

        uI3DgameObject.SetActive(true);
    }

    private void CheckPlayerType()
    {
        if (gameObject.name.Contains("Attacker"))
        {
            isAttacker = true;
            isDefender = false;
        }
        else if (gameObject.name.Contains("Defender"))
        {
            isDefender = true;
            isAttacker = false;
            spinnerScript.SpinSpeed = 4400;
            _startSpinSpeed = spinnerScript.SpinSpeed;
            _currentSpinSpeed = spinnerScript.SpinSpeed;
            spinSpeedRatio_Text.text = _currentSpinSpeed + "/" + _startSpinSpeed;
        }
    }


    private void Start()
    {
        CheckPlayerType();
        _rb = GetComponent<Rigidbody>();
    }

    private float _startSpinSpeed;
    private float _currentSpinSpeed;
    private Rigidbody _rb;
    private GameObject _deathUIPanelObj;
    private bool _isDead = false;
}
