using UnityEngine;

[System.Serializable]
public class WheelAxle
{

    #region Fields
    [SerializeField] private WheelCollider _leftWheelCollider;
    [SerializeField] private WheelCollider _rightWheelCollider;

    [SerializeField] private Transform _leftWheelMesh;
    [SerializeField] private Transform _rightWheelMesh;

    [SerializeField] private bool _isMotor;
    [SerializeField] private bool _isSteer;

    [SerializeField] private float _wheelWidth;

    [SerializeField] private float _antiRollForce;

    [SerializeField] private float _additionalWheelDownForce;

    [SerializeField] private float _baseForwardStiffnes = 1.5f;
    [SerializeField] private float _stabilittyForwardFactor = 1.0f;
    [SerializeField] private float _baseSidewaysStiffnes = 2.0f;
    [SerializeField] private float _stabilittySidewaysFactor = 1.0f;

    private WheelHit _leftWheelHit;
    private WheelHit _rightWheelHit;
    #endregion

    #region Public API
    public void Update()
    {
        UpdateWheelHits();

        ApplyAntiRoll();
        ApplyDownForce();
        CorrectStiffness();

        SyncMeshTransform();
    }

    public void ApplySteerAngle(float steerAngle, float wheelBaseLength)
    {
        if (!_isSteer) return;

        //Ackerman's angle
        float radius = Mathf.Abs(wheelBaseLength * Mathf.Tan(Mathf.Deg2Rad * (90 - Mathf.Abs(steerAngle))));
        float angleSing = Mathf.Sign(steerAngle);

        if(steerAngle > 0)
        {
            _leftWheelCollider.steerAngle = Mathf.Rad2Deg * Mathf.Atan(wheelBaseLength / (radius + (_wheelWidth * 0.5f))) * angleSing;
            _rightWheelCollider.steerAngle = Mathf.Rad2Deg * Mathf.Atan(wheelBaseLength / (radius - (_wheelWidth * 0.5f))) * angleSing;
        }
        else if (steerAngle < 0)
        {
            _leftWheelCollider.steerAngle = Mathf.Rad2Deg * Mathf.Atan(wheelBaseLength / (radius - _wheelWidth * 0.5f)) * angleSing;
            _rightWheelCollider.steerAngle = Mathf.Rad2Deg * Mathf.Atan(wheelBaseLength / (radius + _wheelWidth * 0.5f)) * angleSing;
        }
        else
        {
            _leftWheelCollider.steerAngle = 0;
            _rightWheelCollider.steerAngle = 0;
        }

        
    }

    public void ApplyMotorTorque(float motorTorque)
    {
        if(!_isMotor) return;

        _leftWheelCollider.motorTorque = motorTorque;
        _rightWheelCollider.motorTorque = motorTorque;
    }

    public void ApplyBrakeTorque(float brakeTorque)
    {
        _leftWheelCollider.brakeTorque = brakeTorque;
        _rightWheelCollider.brakeTorque = brakeTorque;
    }
    #endregion

    #region Private Methods

    private void UpdateWheelHits()
    {
        _leftWheelCollider.GetGroundHit(out _leftWheelHit);
        _rightWheelCollider.GetGroundHit(out _rightWheelHit);
    }

    private void CorrectStiffness()
    {
        WheelFrictionCurve leftForward = _leftWheelCollider.forwardFriction;
        WheelFrictionCurve rightForward = _rightWheelCollider.forwardFriction;

        WheelFrictionCurve leftSideways = _leftWheelCollider.sidewaysFriction;
        WheelFrictionCurve rightSideways = _rightWheelCollider.sidewaysFriction;

        leftForward.stiffness = _baseForwardStiffnes + Mathf.Abs(_leftWheelHit.forwardSlip) * _stabilittyForwardFactor;
        rightForward.stiffness = _baseForwardStiffnes + Mathf.Abs(_rightWheelHit.forwardSlip) * _stabilittyForwardFactor;

        leftSideways.stiffness = _baseSidewaysStiffnes + Mathf.Abs(_leftWheelHit.sidewaysSlip) * _stabilittySidewaysFactor;
        rightSideways.stiffness = _baseSidewaysStiffnes + Mathf.Abs(_rightWheelHit.sidewaysSlip) * _stabilittySidewaysFactor;

        _leftWheelCollider.forwardFriction = leftForward;
        _rightWheelCollider.forwardFriction = rightForward;

        _leftWheelCollider.sidewaysFriction = leftSideways;
        _rightWheelCollider.sidewaysFriction = rightSideways;
    }

    private void ApplyDownForce()
    {
        if (_leftWheelCollider.isGrounded)
        {
            _leftWheelCollider.attachedRigidbody.AddForceAtPosition(_leftWheelHit.normal * -_additionalWheelDownForce * 
                        _leftWheelCollider.attachedRigidbody.velocity.magnitude, _leftWheelCollider.transform.position);
        }
        if (_rightWheelCollider.isGrounded)
        {
            _rightWheelCollider.attachedRigidbody.AddForceAtPosition(_rightWheelHit.normal * -_additionalWheelDownForce *
                        _rightWheelCollider.attachedRigidbody.velocity.magnitude, _rightWheelCollider.transform.position);
        }
    }

    private void ApplyAntiRoll()
    {
        float travelL = 1.0f;
        float travelR = 1.0f;

        if (_leftWheelCollider.isGrounded)
        {
            travelL = (-_leftWheelCollider.transform.InverseTransformPoint(_leftWheelHit.point).y - _leftWheelCollider.radius)
                        / _leftWheelCollider.suspensionDistance;
        }

        if (_rightWheelCollider.isGrounded)
        {
            travelR = (-_rightWheelCollider.transform.InverseTransformPoint(_rightWheelHit.point).y - _rightWheelCollider.radius)
                        / _rightWheelCollider.suspensionDistance;
        }

        float forceDir = travelL - travelR;

        if (_leftWheelCollider.isGrounded)
        {
            _leftWheelCollider.attachedRigidbody.AddForceAtPosition(_leftWheelCollider.transform.up * - forceDir * _antiRollForce,
                                                                                     _leftWheelCollider.transform.position);
        }
        if (_rightWheelCollider.isGrounded)
        {
            _rightWheelCollider.attachedRigidbody.AddForceAtPosition(_rightWheelCollider.transform.up * forceDir * _antiRollForce,
                                                                                     _rightWheelCollider.transform.position);
        }
    }
    private void SyncMeshTransform()
    {
        UpdateWheelTransform(_leftWheelCollider, _leftWheelMesh);
        UpdateWheelTransform(_rightWheelCollider, _rightWheelMesh);
    }

    private void UpdateWheelTransform(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 position;
        Quaternion rotation;

        wheelCollider.GetWorldPose(out position, out rotation);
        wheelTransform.position = position;
        wheelTransform.rotation = rotation;
    }
    #endregion
}
