using Unity.VisualScripting;
using UnityEngine;

public class VehicleJoint : MonoBehaviour
{
    public Rigidbody vehicle;
    public float width = 5;
    public float strength = 10;

    [SerializeField] private Anchor anchor;
    [SerializeField] private SpringJoint joint;
    [SerializeField] private TrackController trackController;
    [SerializeField] private Car car;


    private bool connected = false;


    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (connected == false)
            {
                connected = true;
                connect();
            }

        }
        else
        {
            if (connected)
            {
                connected = false;
                disconnect();
            }
        }
    }
    void connect()
    {
        anchor = trackController.getClosestAnchor(car);
        joint = vehicle.AddComponent<SpringJoint>();
        joint.autoConfigureConnectedAnchor = false;
        float distanceToJoint = Vector3.Distance(vehicle.transform.position, anchor.transform.position);

        joint.minDistance = distanceToJoint - (width / 2.0f);
        joint.maxDistance = distanceToJoint + (width / 2.0f);

        joint.connectedBody = anchor.GetRigidbody();
        joint.connectedAnchor = car.transform.position;

        joint.spring = strength;

        Debug.Log("connected");
    }

    void disconnect()
    {
        Destroy(joint);
        anchor = null;

        Debug.Log("disconnected");
    }

}
