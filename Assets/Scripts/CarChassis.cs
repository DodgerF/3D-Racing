using UnityEngine;

public class CarChassis : MonoBehaviour
{
    [SerializeField] private WheelAxle[] _wheelAxles;

    //DEBUG ONLY
    public float motorTorque;
    public float brakeTorque;
    public float steerAngle;

    private void FixedUpdate()
    {
        UpdateWheelAxles();
    }

    private void UpdateWheelAxles()
    {
        for(int i = 0; i < _wheelAxles.Length; i++)
        {
            _wheelAxles[i].Update();
            _wheelAxles[i].ApplyMotorTorque(motorTorque);
            _wheelAxles[i].ApplyBrakeTorque(brakeTorque);
            _wheelAxles[i].ApplySteerAngle(steerAngle);
        }
    }
}
