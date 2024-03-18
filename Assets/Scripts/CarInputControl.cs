using UnityEngine;

public class CarInputControl : MonoBehaviour
{
    [SerializeField] private Car car;
    private void Update()
    {
        car.throttleControl = Input.GetAxis("Vertical");
        car.brakeControl = Input.GetAxis("Jump");
        car.steerControl = Input.GetAxis("Horizontal");
    }
}
