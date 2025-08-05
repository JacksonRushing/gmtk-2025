using Unity.VisualScripting;
using UnityEngine;

public class VehicleJoint : MonoBehaviour
{
    public Rigidbody anchor, vehicle;
    public float width = 5;
    public float strength = 10;
    [SerializeField] private SpringJoint joint;


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
        joint = vehicle.AddComponent<SpringJoint>();
        float distanceToJoint = Vector3.Distance(vehicle.transform.position, anchor.transform.position);

        joint.minDistance = distanceToJoint - (width / 2.0f);
        joint.maxDistance = distanceToJoint + (width / 2.0f);

        joint.spring = strength;

        Debug.Log("connected");
    }

    void disconnect()
    {
        Destroy(joint);

        Debug.Log("disconnected");
    }

}
