
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] private WheelCollider[] _wheelColliders;
    [SerializeField] private Transform[] _wheelMeshs;
    [SerializeField] private float _torque;
    [SerializeField] private float _breakTorque;

    [SerializeField] private float _steeringAngle;
    private void Update()
    {
        for (int i = 0; i < _wheelColliders.Length; i++)
        {
            _wheelColliders[i].motorTorque = Input.GetAxis("Vertical") * _torque;
            _wheelColliders[i].brakeTorque = Input.GetAxis("Jump") * _breakTorque;


            Vector3 position;
            Quaternion rotation;
            _wheelColliders[i].GetWorldPose(out position, out rotation);

            _wheelMeshs[i].position = position;
            _wheelMeshs[i].rotation = rotation;
        }

        _wheelColliders[0].steerAngle = Input.GetAxis("Horizontal") * _steeringAngle;
        _wheelColliders[1].steerAngle = Input.GetAxis("Horizontal") * _steeringAngle;
    }
}
