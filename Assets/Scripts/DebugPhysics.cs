using Unity.VisualScripting;
using UnityEngine;

public class DebugPhysics : MonoBehaviour
{
    public Rigidbody anchor, vehicle;
    public float vehicleForce;
    public float vehicleFriction;
    private Vector2 input;
    private Joint joint;

    void Update()
    {
        input = new Vector2(0, 0);
        if (Input.GetKey(KeyCode.UpArrow))
        {
            input.y = 1;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            input.y = -1;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            input.x = -1;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            input.x = 1;
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            removeJoint();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            removeJoint();
            addSpringJoint();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            removeJoint();
            addConfigurableJoint();
        }


    }

    void FixedUpdate()
    {
        vehicle.AddForce(input.x * vehicleForce, 0, input.y * vehicleForce);

        Vector3 currentVelocity = vehicle.linearVelocity;

        Vector3 frictionForce = -currentVelocity * vehicleFriction * Time.fixedDeltaTime;

        vehicle.AddForce(frictionForce);
    }

    void removeJoint()
    {
        Destroy(joint);
        joint = null;
    }

    void addSpringJoint()
    {
        joint = anchor.AddComponent<SpringJoint>();
        joint.connectedBody = vehicle;
    }

    void addConfigurableJoint()
    {
        joint = anchor.AddComponent<ConfigurableJoint>();
        joint.connectedBody = vehicle;
    }
}
