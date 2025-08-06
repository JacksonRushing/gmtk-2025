using Unity.VisualScripting;
using UnityEngine;

public class VehicleJoint : MonoBehaviour
{
    public Rigidbody vehicle;
    public float width = 5;
    public float strength = 10;
    public bool debugHold;

    [SerializeField] private Anchor anchor;
    [SerializeField] private Joint joint;
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
            if (connected && !debugHold)
            {
                connected = false;
                disconnect();
            }
        }
    }
    void connect()
    {
        anchor = trackController.getClosestAnchor(car);
        joint = anchor.GetRigidbody().AddComponent<HingeJoint>();
        joint.autoConfigureConnectedAnchor = true;
        joint.axis = Vector3.up;
        float distanceToJoint = Vector3.Distance(vehicle.transform.position, anchor.transform.position);

        //joint.minDistance = distanceToJoint - (width / 2.0f);
        //joint.maxDistance = distanceToJoint + (width / 2.0f);

        //Debug.Log($"distance: {distanceToJoint}, mindistance: {joint.minDistance}, max: {joint.maxDistance}");

        joint.connectedBody = vehicle;
        //joint.connectedAnchor = car.transform.position;

        //joint.spring = strength;

        Debug.Log("connected");
    }

    void disconnect()
    {
        Destroy(joint);
        anchor = null;

        Debug.Log("disconnected");
    }

}
