using Unity.Mathematics;
using UnityEngine;

public class Suspension : MonoBehaviour
{
    public Wheel wheel;
    public Rigidbody carRB;
    public float RestingDistance;
    public float SpringStrength;
    public float suctionForceRatio = 0.5f;
    public float DampingForce;
    public float Offset;

    public float maxExtension = 1.0f;
    public float accelerationForce;
    public float brakingForce;

    public float WheelRadius = 0.375f;

    public int acceleratorInput;

    private AnimationCurve tractionCurve;
    private AnimationCurve acceleratorCurve;

    //debug

    public Arrow springArrow;
    public Arrow slipArrow;
    public Arrow slipCorrectionArrow;

    public Arrow accelerationArrow;
    public float springScale, slipScale, accelerationScale;
    public float slipPercentage;
    public float slipVelocity;
    public float slipCorrectionForce;
    public float rideVelocity;

    public float absSlip;
    public float absRide;

    public float tractionResult;

    public bool didHit;

    // public Shapes.Line suspension;
    // public GameObject restPos;
    // public GameObject hitPos;

    //public GameObject wheelPos;

    void FixedUpdate()
    {
        Vector3 powerDir = this.transform.forward;
        Vector3 springDir = this.transform.up;
        Vector3 wheelWorldVelocity = carRB.GetPointVelocity(this.transform.position);

        // suspension.Start = new Vector3(0, -RestingDistance, 0);
        // suspension.End = new Vector3(0, -RestingDistance, 0);

        Ray ray = new Ray(this.transform.position, -transform.up);
        RaycastHit hit;

        float rayLength = RestingDistance + maxExtension + WheelRadius;
        didHit = Physics.Raycast(ray, out hit, rayLength);

        // restPos.transform.localPosition = new Vector3(0, -RestingDistance, 0);

        wheel.transform.localPosition = new Vector3(0, -RestingDistance, 0);


        // suspension.End = Vector3.zero;
        if (didHit)
        {
            // suspension.End = new Vector3(0, -hit.distance + WheelRadius, 0);
            // hitPos.SetActive(true);
            // hitPos.transform.localPosition = new Vector3(0, -hit.distance, 0);
            //positive offset is compression, negative is extension
            Offset = (RestingDistance) - (hit.distance - WheelRadius);
            Offset = Mathf.Clamp(Offset, -RestingDistance - maxExtension, RestingDistance);

            wheel.transform.localPosition = new Vector3(0, -RestingDistance + Offset, 0);

            // suspension.End = new Vector3(0, -(RestingDistance - Offset), 0);
            float wheelVelSpring = Vector3.Dot(wheelWorldVelocity, springDir);
            float force = (Offset * SpringStrength) - (DampingForce * wheelVelSpring);

            if (force < 0)
            {
                force *= suctionForceRatio;
            }

            carRB.AddForceAtPosition(force * springDir, this.transform.position);
            springArrow.end = force * springDir * springScale;
            Debug.DrawLine(this.transform.position, this.transform.position + (force * springDir * springScale), Color.green);

            if (acceleratorInput != 0)
            {
                if (acceleratorInput == 1)
                {
                    carRB.AddForceAtPosition(accelerationForce * powerDir, this.transform.position);
                    accelerationArrow.end = accelerationForce * powerDir * accelerationScale;
                    Debug.DrawLine(this.transform.position, this.transform.position + (accelerationForce * powerDir * accelerationScale));
                }
                else
                {
                    carRB.AddForceAtPosition(brakingForce * -wheelWorldVelocity.normalized, this.transform.position);
                    accelerationArrow.end = brakingForce * -wheelWorldVelocity.normalized * accelerationScale;
                    Debug.DrawLine(this.transform.position, this.transform.position + (brakingForce * -wheelWorldVelocity.normalized * accelerationScale), Color.blue);
                }



            }

            //steering
            slipVelocity = Vector3.Dot(wheelWorldVelocity, this.transform.right);
            rideVelocity = Vector3.Dot(wheelWorldVelocity, this.transform.forward);

            Debug.DrawLine(this.transform.position, this.transform.position + wheelWorldVelocity, Color.cyan);

            absSlip = Mathf.Abs(slipVelocity);
            absRide = Mathf.Abs(rideVelocity);

            //get percentage of velocity that is slip
            slipPercentage = absSlip / (absSlip + absRide);
            tractionResult = tractionCurve.Evaluate(slipPercentage);

            slipCorrectionForce = -slipVelocity * tractionResult * carRB.mass;

            carRB.AddForceAtPosition(slipCorrectionForce * this.transform.right, this.transform.position);

            slipArrow.end = slipVelocity * this.transform.right * slipScale;
            slipCorrectionArrow.start = this.transform.position;
            slipCorrectionArrow.end = slipCorrectionForce * this.transform.right * slipScale;

            Vector3 slipEnd = this.transform.position + (slipVelocity * this.transform.right * slipScale);
            Debug.DrawLine(this.transform.position, slipEnd, Color.red);
            Debug.DrawLine(this.transform.position + new Vector3(0, 0.1f, 0), this.transform.position + (slipCorrectionForce * this.transform.right * slipScale) + new Vector3(0, 0.1f, 0), Color.magenta);

        }
        // else
        // {
        //     hitPos.SetActive(false);
        // }

        //wheel.localPosition = ((-this.transform.up) * (RestingDistance + Offset));
    }

    void Start()
    {
        wheel.transform.localPosition = new Vector3(0, -RestingDistance, 0);
    }

    public void setTractionCurve(AnimationCurve curve)
    {
        tractionCurve = curve;
    }
}
