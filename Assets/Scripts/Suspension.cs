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

    public float rollingFriction = 0.05f;

    private AnimationCurve tractionCurve;

    private AnimationCurve torqueCurve;


    //debug

    public Arrow springArrow;
    public Arrow slipArrow;
    public Arrow slipCorrectionArrow;

    public Arrow accelerationArrow;
    public float springScale, slipScale, accelerationScale;
    public float slipPercentage;
    public float slipVelocity;

    public Vector3 slipVelocityVector;
    public float slipCorrectionForce;
    public float rideVelocity;
    public Vector3 rideVelocityVector;

    public float speedRatio;
    public float torque;

    public float topSpeed;

    public float absSlip;
    public float absRide;

    public float tractionResult;

    public Vector3 springForceVector, driveForceVector, slipCorrectionForceVector;
    public Vector3 powerDir, springDir;

    public float slipCorrectionMultiplier = 1.0f;

    public bool didHit;

    // public Shapes.Line suspension;
    // public GameObject restPos;
    // public GameObject hitPos;

    //public GameObject wheelPos;

    void FixedUpdate()
    {
        powerDir = this.transform.forward;
        springDir = this.transform.up;
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
            springDir = hit.normal;
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

            springForceVector = force * springDir;
            carRB.AddForceAtPosition(springForceVector, this.transform.position);
            springArrow.end = force * springDir * springScale;
            Debug.DrawLine(this.transform.position, this.transform.position + (force * springDir * springScale), Color.green);

            if (acceleratorInput != 0)
            {
                if (acceleratorInput == 1)
                {
                    speedRatio = wheelWorldVelocity.magnitude / topSpeed;
                    torque = torqueCurve.Evaluate(speedRatio);
                    if (speedRatio > 1)
                    {
                        torque = 0;
                    }

                    driveForceVector = accelerationForce * powerDir * torque;
                    carRB.AddForceAtPosition(driveForceVector, this.transform.position);
                    accelerationArrow.end = accelerationForce * powerDir * torque * accelerationScale;
                    Debug.DrawLine(this.transform.position, this.transform.position + (accelerationForce * torque * powerDir * accelerationScale), Color.blue);
                }
                else
                {
                    driveForceVector = brakingForce * -wheelWorldVelocity.normalized;
                    carRB.AddForceAtPosition(driveForceVector, this.transform.position);
                    accelerationArrow.end = brakingForce * -wheelWorldVelocity.normalized * accelerationScale;
                    Debug.DrawLine(this.transform.position, this.transform.position + (brakingForce * -wheelWorldVelocity.normalized * accelerationScale), Color.blue);
                }



            }

            //steering
            slipVelocity = Vector3.Dot(wheelWorldVelocity, this.transform.right);
            rideVelocity = Vector3.Dot(wheelWorldVelocity, this.transform.forward);




            // slipVelocity = slipVelocityVector.magnitude;
            // rideVelocity = rideVelocityVector.magnitude;


            Debug.DrawLine(this.transform.position, this.transform.position + wheelWorldVelocity, Color.yellow);

            absSlip = Mathf.Abs(slipVelocity);
            absRide = Mathf.Abs(rideVelocity);

            //get percentage of velocity that is slip
            slipPercentage = absSlip / (absSlip + absRide);
            tractionResult = tractionCurve.Evaluate(slipPercentage);
            //tractionResult = 1;




            slipCorrectionForceVector = -this.transform.right * slipVelocity * tractionResult * ((carRB.mass * -Physics.gravity.y) / 4.0f);

            //slipCorrectionForce = -slipVelocity * tractionResult * carRB.mass;

            //slipCorrectionForceVector = slipCorrectionForce * this.transform.right * slipCorrectionMultiplier;
            //slipCorrectionForceVector = -slipVelocityVector * carRB.mass * tractionResult * slipCorrectionMultiplier;
            //slipCorrectionForceVector = -slipVelocityVector;
            carRB.AddForceAtPosition(slipCorrectionForceVector, this.transform.position);

            slipArrow.end = slipVelocity * this.transform.right * slipScale;
            slipCorrectionArrow.start = this.transform.position;
            slipCorrectionArrow.end = slipCorrectionForce * this.transform.right * slipScale;

            Vector3 slipEnd = this.transform.position + this.transform.right * slipVelocity;
            Debug.DrawLine(this.transform.position, slipEnd, Color.red);
            Debug.DrawLine(this.transform.position + new Vector3(0, 0.1f, 0), this.transform.position + (slipCorrectionForceVector) + new Vector3(0, 0.1f, 0), Color.magenta);

            //rolling friction
            float forwardVelocity = Vector3.Dot(this.transform.forward, wheelWorldVelocity);

            Vector3 frictionVector = -forwardVelocity * this.transform.forward * rollingFriction * ((carRB.mass * -Physics.gravity.y) / 4.0f);
            carRB.AddForceAtPosition(frictionVector, this.transform.position);
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

    public void setTorqueCurve(AnimationCurve curve)
    {
        torqueCurve = curve;
    }
}
