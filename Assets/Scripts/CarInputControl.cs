using System;
using UnityEngine;

public class CarInputControl : MonoBehaviour
{
    [SerializeField] private Car _car;
    [SerializeField] private AnimationCurve _brakeCurve;
    [SerializeField] private AnimationCurve _steerCurve;

    [SerializeField] [Range(0f, 1f)] private float _autoBrakeStrength = 0.05f;
    private float _wheelSpeed;

    private float _verticalAxis;
    private float _horizontalAxis;
    private float _handbrakeAxis;
    private void Update()
    {
        _wheelSpeed = _car.WheelSpeed;

        UpdateAxis();
        UpdateTorqueAndBrake();
        UpdateSteer();
        UpdateAutoBreak();

        //Test
        if (Input.GetKeyDown(KeyCode.E))
        {
            _car.UpGear();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _car.DownGear();
        }

    }

    private void UpdateSteer()
    {
        _car.steerControl = _steerCurve.Evaluate(_car.WheelSpeed / _car.MaxSpeed) * _horizontalAxis;
    }

    private void UpdateTorqueAndBrake()
    {
        if (Mathf.Sign(_verticalAxis) == Mathf.Sign(_wheelSpeed) || Mathf.Abs(_wheelSpeed) < 0.5f)
        {
            _car.throttleControl = Mathf.Abs(_verticalAxis);
            _car.brakeControl = 0;
        }
        else
        {
            _car.throttleControl = 0;
            _car.brakeControl = _brakeCurve.Evaluate(_wheelSpeed / _car.MaxSpeed);
        }

        //Gears
        if (_verticalAxis < 0 && _wheelSpeed > -0.5f && _wheelSpeed <= 0.5f)
        {
            _car.ShiftToReverseGear();
        }

        if (_verticalAxis > 0 && _wheelSpeed > -0.5f && _wheelSpeed < 0.5f)
        {
            _car.ShiftToFirstGear();
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
            _car.brakeControl = _brakeCurve.Evaluate(_wheelSpeed / _car.MaxSpeed) * _autoBrakeStrength;
        }
    }
}
