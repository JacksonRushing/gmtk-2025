using UnityEngine;

public class CarToSpeedometer : MonoBehaviour
{
    [SerializeField] private Car car;
    [SerializeField] private Speedometer speedometer;

    void Update()
    {
        speedometer.topSpeed = car.topSpeed;
        speedometer.speed = car.getCurrentSpeed();
    }

}
