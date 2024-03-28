using UnityEngine;

[RequireComponent(typeof(CarChassis))]
public class Car : MonoBehaviour
{
    [SerializeField] private float _maxSteerAngle;
    [SerializeField] private float _maxBrakeTorque;

    [SerializeField] private AnimationCurve _engineTorqueCurve;
    [SerializeField] private float _maxMotorTorque;
    [SerializeField] private float _maxSpeed;

    public float LinearVelocity => _chassis.LinearVelocity;
    public float WheelSpeed => _chassis.GetWheelSpeed();
    public float MaxSpeed => _maxSpeed;

    private CarChassis _chassis;
    
    [Header("Debug Only")]
    public float linearVelocity;
    public float throttleControl;
    public float steerControl;
    public float brakeControl;

    private void Start()
    {
        _chassis = GetComponent<CarChassis>();
    }

    private void Update()
    {
        linearVelocity = LinearVelocity;
        float engineTorque = _engineTorqueCurve.Evaluate(LinearVelocity / _maxSpeed) * _maxMotorTorque;

        if (_chassis.LinearVelocity >= _maxSpeed)
        {
            engineTorque = 0;
        }

        _chassis.motorTorque = throttleControl * engineTorque;
        _chassis.steerAngle = steerControl * _maxSteerAngle;
        _chassis.brakeTorque = brakeControl * _maxBrakeTorque;
    }
}
