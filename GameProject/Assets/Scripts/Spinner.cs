using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    [SerializeField] float _spinSpeed = 3600.0f;
    [SerializeField] bool _doSpin = true;
    [SerializeField] GameObject _playerGraphics = null;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_doSpin)
        {
            _playerGraphics.transform.Rotate(new Vector3(0, _spinSpeed * Time.deltaTime, 0));
        }
    }

    public float SpinSpeed 
    {
        get { return _spinSpeed; } 
        set { _spinSpeed = value; }
    }

    private Rigidbody _rb = null;
}
