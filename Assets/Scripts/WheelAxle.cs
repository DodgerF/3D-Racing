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
    #endregion

    #region Public API
    public void Update()
    {
        SyncMeshTransform();
    }

    public void ApplySteerAngle(float steerAngle)
    {
        if (!_isSteer) return;

        _leftWheelCollider.steerAngle = steerAngle;
        _rightWheelCollider.steerAngle = steerAngle;
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
