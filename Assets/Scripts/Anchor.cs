using UnityEngine;

public class Anchor : MonoBehaviour
{
    public Rigidbody frontAnchor, rearAnchor, anchor, vehicle;
    public float width = 5;
    public float strength = 10;
    [SerializeField] private SpringJoint a, b;


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
        float distanceToA = Vector3.Distance(a.transform.position, anchor.transform.position);
        float distanceToB = Vector3.Distance(a.transform.position, anchor.transform.position);
        a.connectedBody = anchor;
        b.connectedBody = anchor;

        a.spring = strength;
        b.spring = strength;
        // a.connectedAnchor = vehicle.transform.InverseTransformPoint(frontAnchor.position);
        // b.connectedAnchor = vehicle.transform.InverseTransformPoint(rearAnchor.position);

        a.minDistance = distanceToA;
        b.minDistance = distanceToB;

        a.maxDistance = distanceToA + width;
        b.maxDistance = distanceToB + width;

        Debug.Log("connected");
    }

    void disconnect()
    {
        a.spring = 0;
        b.spring = 0;
        a.connectedBody = null;
        b.connectedBody = null;

        Debug.Log("disconnected");
    }

}
