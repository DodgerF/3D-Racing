using TMPro;
using UnityEngine;

public class CarSpeedIndicator : MonoBehaviour
{
    [SerializeField] private Car _car;
    [SerializeField] private TextMeshProUGUI _speedIndicator;

    private void Awake()
    {
        if (_speedIndicator == null)
        {
            _speedIndicator = GetComponentInChildren<TextMeshProUGUI>();
        }
    }
    
    private void Update()
    {
        _speedIndicator.text = _car.LinearVelocity.ToString("F0");
    }
}
