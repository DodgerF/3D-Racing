using System;
using UnityEngine;

public class CarInputControl : MonoBehaviour
{
    [SerializeField] private Car car;
    [SerializeField] private AnimationCurve _brakeCurve;
    [SerializeField] private AnimationCurve _steerCurve;

    [SerializeField] [Range(0f, 1f)] private float _autoBrakeStrength = 0.05f;
    private float _wheelSpeed;

    private float _verticalAxis;
    private float _horizontalAxis;
    private float _handbrakeAxis;
    private void Update()
    {
        _wheelSpeed = car.WheelSpeed;

        UpdateAxis();
        UpdateTorqueAndBrake();
        UpdateSteer();
        //UpdateAutoBreak();
    }

    private void UpdateSteer()
    {
        car.steerControl = _steerCurve.Evaluate(car.WheelSpeed / car.MaxSpeed) * _horizontalAxis;
    }

    private void UpdateTorqueAndBrake()
    {
        if (Mathf.Sign(_verticalAxis) == Mathf.Sign(_wheelSpeed) || Mathf.Abs(_wheelSpeed) < 0.5f)
        {
            car.throttleControl = Input.GetAxis("Vertical");
            car.brakeControl = 0;
        }
        else
        {
            car.throttleControl = 0;
            car.brakeControl = _brakeCurve.Evaluate(_wheelSpeed / car.MaxSpeed);
        }

    }

    private void UpdateAxis()
    {
        _verticalAxis = Input.GetAxis("Vertical");
        _horizontalAxis = Input.GetAxis("Horizontal");
        _handbrakeAxis = Input.GetAxis("Jump");
    }

    private void UpdateAutoBreak()
    {
        if (Input.GetAxis("Vertical") == 0)
        {
            car.brakeControl = _brakeCurve.Evaluate(_wheelSpeed / car.MaxSpeed) * _autoBrakeStrength;
        }
    }
}
