using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private Car playerCar;
    [SerializeField] private TrackController trackController;

    void Start()
    {

    }

    void Update()
    {
        playerCar.closestAnchor = trackController.getClosestAnchor(playerCar);
    }
}
