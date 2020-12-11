using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] Joystick joyStick = null;
    [SerializeField] float speed = 2f;
    [SerializeField] float maxDeltaVelocity = 4f;
    [SerializeField] float tiltAmount = 10f;

    // Start is called before the first frame update
    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(joyStick.Vertical * speed * tiltAmount, 0, -joyStick.Horizontal * speed * tiltAmount);
    }

    // Get direction from joystick
    private Vector3 GetInputDir()
    {
        Vector3 xMove = Vector3.right * joyStick.Horizontal;
        Vector3 yMove = Vector3.forward * joyStick.Vertical;
        return (xMove + yMove).normalized;
    }

    // update physics movement in FixedUpdate
    private void FixedUpdate()
    {
        Vector3 inputVel = GetInputDir() * speed;//velocity per frame
        if (inputVel != Vector3.zero)
        {
            Vector3 vel = m_rb.velocity;
            Vector3 deltaVel = inputVel - vel;
            deltaVel.y = 0;
            deltaVel.x = Mathf.Clamp(deltaVel.x, -maxDeltaVelocity, maxDeltaVelocity);
            deltaVel.z = Mathf.Clamp(deltaVel.z, -maxDeltaVelocity, maxDeltaVelocity);

            m_rb.AddForce(deltaVel, ForceMode.Acceleration);
        }

    }

    public Joystick GetJoystick()
    {
        return joyStick;
    }


    // private member variables
    private Rigidbody m_rb = null;
}
