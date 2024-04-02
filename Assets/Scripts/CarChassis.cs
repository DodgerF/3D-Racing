using System;
using Unity.Properties;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarChassis : MonoBehaviour
{
    [SerializeField] private WheelAxle[] _wheelAxles;
    [SerializeField] private float _wheelBaseLength;

    [SerializeField] private Transform _centerOfMass;

    [Header("DownForce")]
    [SerializeField] private float _downForceMin;
    [SerializeField] private float _downForceMax;
    [SerializeField] private float _downForceFactor;

    [Header("AngularDrag")]
    [SerializeField] private float _angularDragMin;
    [SerializeField] private float _angularDragMax;
    [SerializeField] private float _angularDragFactor; 

    [Header("Debug only")]
    public float motorTorque;
    public float brakeTorque;
    public float steerAngle;

    public float LinearVelocity => _rigidbody.velocity.magnitude * 3.6f; //Convert meters per second to km/h

    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        if (_centerOfMass)
        {
            _rigidbody.centerOfMass = _centerOfMass.localPosition;
        }

        for (int i = 0; i < _wheelAxles.Length; i++)
        {
            _wheelAxles[i].ConfigureVehicleSubsteps(70, 70, 70); 
        }
    }

    private void FixedUpdate()
    {
        UpdateAngularDrag();

        UpdateDownForce();

        UpdateWheelAxles();
    }

    public float GetAvarageRpm()
    {
        float sum = 0;

        for (int i =0; i < _wheelAxles.Length; i++)
        {
            sum += _wheelAxles[i].GetAvarageRpm();
        }

        return sum / _wheelAxles.Length;
    }

    public float GetWheelSpeed()
    {
        return GetAvarageRpm() * _wheelAxles[0].GetRadius() * 2 * 0.1885f; //2piR * 3.6f
    }

    private void UpdateAngularDrag()
    {
        _rigidbody.angularDrag = Mathf.Clamp(_angularDragFactor * LinearVelocity, _angularDragMin, _angularDragMax);
    }

    private void UpdateDownForce()
    {
        float downForce = Mathf.Clamp(_downForceFactor * LinearVelocity, _downForceMin, _downForceMax);

        _rigidbody.AddForce(-transform.up * downForce);
    }

    private void UpdateWheelAxles()
    {
        int amountMotorWheel = 0;

        for (int i = 0; i < _wheelAxles.Length; i++)
        {
            if (_wheelAxles[i].IsMotor)
            {
                amountMotorWheel += 2;
            }
        }

        for(int i = 0; i < _wheelAxles.Length; i++)
        {
            _wheelAxles[i].Update();
            _wheelAxles[i].ApplyMotorTorque(motorTorque);
            _wheelAxles[i].ApplyBrakeTorque(brakeTorque);
            _wheelAxles[i].ApplySteerAngle(steerAngle, _wheelBaseLength);
        }
    }
}
