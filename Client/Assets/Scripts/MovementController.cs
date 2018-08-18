using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour {

    [SerializeField]
    private Vector2 _velocity = Vector2.zero;

    [SerializeField]
    private float _angularVelocity = 0f;

    [SerializeField]
    private float _acceleration = 100f;

    [SerializeField]
    private float _angularAcceleration = 3500f;

    [SerializeField]
    private float _linearDamping = 40f;

    [SerializeField]
    private float _angularDamping = 1600f;

    [SerializeField]
    private float _angularVelocityCap = 300;

    [SerializeField]
    private float _linearVelocityCap = 5;

	public void Start ()
    {
	}
	
	public void Update ()
    {
        var controls = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        ApplyControls(controls, Time.deltaTime);

        transform.Translate(CalculatePositionStep(Time.deltaTime));
        transform.Rotate(0, 0, CalculateRotationStep(Time.deltaTime));

        if (controls.y == 0)
        {
            ApplyLinearDrag(Time.deltaTime);
        }

        if (controls.x == 0)
        {
            ApplyAngularDrag(Time.deltaTime);
        }
	}

    private void ApplyControls(Vector2 controls, float deltaTime)
    {
        _velocity.y += controls.y * _acceleration * deltaTime;


        if (_velocity.y > _linearVelocityCap)
        {
            _velocity.y = _linearVelocityCap;
        }
        else if (_velocity.y < -_linearVelocityCap)
        {
            _velocity.y = -_linearVelocityCap;
        }

        _angularVelocity -= controls.x * _angularAcceleration * deltaTime;

        if (_angularVelocity > _angularVelocityCap)
        {
            _angularVelocity = _angularVelocityCap;
        }
        else if (_angularVelocity < -_angularVelocityCap)
        {
            _angularVelocity = -_angularVelocityCap;
        }
    }

    private Vector2 CalculatePositionStep(float deltaTime)
    {
        return new Vector2(_velocity.x * deltaTime, _velocity.y * deltaTime);
    }

    private float CalculateRotationStep(float deltaTime)
    {
        return _angularVelocity * deltaTime;
    }

    private void ApplyLinearDrag(float deltaTime)
    {
        if (_velocity.x > 0)
        {
            _velocity.x -= Mathf.Min(_linearDamping * deltaTime, _velocity.x);
        }
        else if (_velocity.x < 0)
        {
            _velocity.x += Mathf.Min(_linearDamping * deltaTime, _velocity.x * -1);
        }

        if (_velocity.y > 0)
        {
            _velocity.y -= Mathf.Min(_linearDamping * deltaTime, _velocity.y);
        }
        else if (_velocity.y < 0)
        {
            _velocity.y += Mathf.Min(_linearDamping * deltaTime, _velocity.y * -1);
        }
    }

    private void ApplyAngularDrag(float deltaTime)
    {
        if (_angularVelocity > 0)
        {
            _angularVelocity -= Mathf.Min(_angularDamping * deltaTime, _angularVelocity);
        }
        else if (_angularVelocity < 0)
        {
            _angularVelocity += Mathf.Min(_angularDamping * deltaTime, _angularVelocity * -1);
        }
    }
}
