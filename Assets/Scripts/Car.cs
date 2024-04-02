using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CarChassis))]
public class Car : MonoBehaviour
{
    public event UnityAction<string> gearChanged;

    [SerializeField] private float _maxSteerAngle;
    [SerializeField] private float _maxBrakeTorque;


    [Header("Engine")]
    [SerializeField] private AnimationCurve _engineTorqueCurve;
    [SerializeField] private float _engineMaxTorque;
    [SerializeField] private float _engineMinRpm;
    [SerializeField] private float _engineMaxRpm;
    [SerializeField] private float _upShiftEngineRpm;
    [SerializeField] private float _downShigftEngineRpm;

    [Header("Gearbox")]
    [SerializeField] private float[] _gears;
    [SerializeField] private float _finalDriveRatio;


    [SerializeField] private float _maxSpeed;

    public float LinearVelocity => _chassis.LinearVelocity;
    public float WheelSpeed => _chassis.GetWheelSpeed();
    public float MaxSpeed => _maxSpeed;
    public float EngineRpm => _engineRpm;
    public float EngineMaxRpm => _engineMaxRpm;
    private CarChassis _chassis;
    
    [Header("Debug Only")]
    public float linearVelocity;
    public float throttleControl;
    public float steerControl;
    public float brakeControl;
    public float engineTorque;
    [SerializeField] private float _engineRpm;
    [SerializeField] private float _selectedGear;
    [SerializeField] private float _rearGear;
    [SerializeField] private int _selectedGearIndex;


    private void Start()
    {
        _chassis = GetComponent<CarChassis>();
    }

    private void Update()
    {
        linearVelocity = LinearVelocity;
        
        UpdateEngineTorque();
        AutoGearShift();

        if (LinearVelocity >= _maxSpeed)
        {
            engineTorque = 0;
        }

        _chassis.motorTorque = throttleControl * engineTorque;
        _chassis.steerAngle = steerControl * _maxSteerAngle;
        _chassis.brakeTorque = brakeControl * _maxBrakeTorque;
    }

    private void UpdateEngineTorque()
    {
        _engineRpm = _engineMinRpm + Mathf.Abs(_chassis.GetAvarageRpm() * _selectedGear * _finalDriveRatio);
        _engineRpm = Mathf.Clamp(_engineRpm, _engineMinRpm, _engineMaxRpm);

        engineTorque = _engineTorqueCurve.Evaluate(_engineRpm / _engineMaxRpm) * _engineMaxTorque * _finalDriveRatio 
                                                                                * Math.Sign(_selectedGear) * _gears[0];
    }


#region GearBox

    public string GetSelectedGearName()
    {
        if (_selectedGear == _rearGear) return "R";

        if (_selectedGear == 0) return "N";

        return (_selectedGearIndex + 1).ToString();
    }
    private void AutoGearShift()
    {
        if(_selectedGear < 0) return;

        if (_engineRpm >= _upShiftEngineRpm)
        {
            UpGear();
        }
        if (_engineRpm < _downShigftEngineRpm)
        {
            DownGear();
        }
    } 
    public void UpGear()
    {
        ShiftGear(_selectedGearIndex + 1);
    }
    public void DownGear()
    {
        ShiftGear(_selectedGearIndex - 1);
    }
    public void ShiftToReverseGear()
    {
        _selectedGear = _rearGear;
        gearChanged?.Invoke(GetSelectedGearName());
    }
    public void ShiftToFirstGear()
    {
        ShiftGear(0);
    }
    public void ShiftToNetral()
    {
        _selectedGear = 0;
        gearChanged?.Invoke(GetSelectedGearName());
    }
#endregion
    private void ShiftGear(int gearIndex)
    {
        gearIndex = Mathf.Clamp(gearIndex, 0, _gears.Length - 1);
        _selectedGear = _gears[gearIndex];
        _selectedGearIndex = gearIndex;
        gearChanged?.Invoke(GetSelectedGearName());
    }
}
