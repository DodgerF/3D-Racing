using UnityEngine;

[RequireComponent(typeof(CarChassis))]
public class Car : MonoBehaviour
{
    [SerializeField] private float _maxMotorTorque;
    [SerializeField] private float _maxSteerAngle;
    [SerializeField] private float _maxBrakeTorque;

    private CarChassis _chassis;
    
    //DEBUG ONLY
    public float throttleControl;
    public float steerControl;
    public float brakeControl;

    private void Awake()
    {
        _chassis = GetComponent<CarChassis>();
    }

    private void FixedUpdate()
    {
        _chassis.motorTorque = throttleControl * _maxMotorTorque;
        _chassis.steerAngle = steerControl * _maxSteerAngle;
        _chassis.brakeTorque = brakeControl * _maxBrakeTorque;
    }
}
