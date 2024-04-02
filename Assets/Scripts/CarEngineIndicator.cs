using UnityEngine.UI;
using UnityEngine;

[System.Serializable]
class CarEngineIndicatorColor
{
    public float MaxRpm;
    public Color color;
}

public class CarEngineIndicator : MonoBehaviour
{
    [SerializeField] private Image _img;
    [SerializeField] private Car _car;
    [SerializeField] private CarEngineIndicatorColor[] _colors;

    private void Awake()
    {
        if (_img == null)
        {
            _img = GetComponentInChildren<Image>();
        }
    }
    private void Update()
    {
        _img.fillAmount = _car.EngineRpm / _car.EngineMaxRpm;
        //TODO: figure out why _img is not displayed when changing color
        // for (int i = 0; i < _colors.Length; i++)
        // {
        //     if (_car.EngineRpm <= _colors[i].MaxRpm)
        //     {
        //         _img.color = _colors[i].color;
        //         break;
        //     }
        // }
    }

}
