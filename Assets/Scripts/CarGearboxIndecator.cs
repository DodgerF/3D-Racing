using TMPro;
using UnityEngine;

public class CarGearboxIndecator : MonoBehaviour
{
    [SerializeField] private Car _car;
    [SerializeField] private TextMeshProUGUI _indecator;

    private void OnEnable()
    {
        _car.gearChanged += OnGearChanged;
    }
    private void OnDisable()
    {
        _car.gearChanged -= OnGearChanged;
    }

    private void OnGearChanged(string gearName)
    {
        _indecator.text = gearName;;
    }
}
