using System;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public enum Drivetrain
    {
        FWD,
        RWD,
        AWD
    }

    public float topSpeed;
    [SerializeField] private float currentSpeed;
    public float carWidth;
    public Drivetrain drivetrain;
    public Suspension FrontLeft, FrontRight;
    public Suspension RearLeft, RearRight;
    public List<Suspension> wheels;

    public float currentSteeringAngle = 0;
    public float maxSteeringAngle = 25;
    public float steeringSpeed = 30;
    public float centeringSpeed = 20;

    public float steeringSpeedRatio;

    public CurveAsset torqueCurve;

    public CurveAsset frontTraction;
    public CurveAsset rearTraction;

    public CurveAsset steeringCurve;



    public float RestingDistance;
    public float SpringStrength;

    public float suctionForceRatio = 0.5f;
    public float DampingForce;
    public float maxExtension = 1.0f;
    public float accelerationForce;
    public float brakingForce;

    public float rollingFriction = 0.05f;
    public float WheelRadius = 0.375f;
    public float WheelWidth = 0.2f;

    private int acceleratorInput;

    private Vector3 initialPosition;
    private Vector3 initialRotation;

    [SerializeField] private Rigidbody rb;
    [SerializeField] private BoxCollider rbCollider;
    [SerializeField] private Transform carMesh;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reset();
        }

        int steeringInput = 0;
        if (Input.GetKey(KeyCode.LeftArrow)) { steeringInput = -1; }
        if (Input.GetKey(KeyCode.RightArrow)) { steeringInput = 1; }

        acceleratorInput = 0;
        if (Input.GetKey(KeyCode.UpArrow)) { acceleratorInput = 1; }
        if (Input.GetKey(KeyCode.DownArrow)) { acceleratorInput = -1; }



        if (acceleratorInput == 1)
        {
            if (drivetrain == Drivetrain.FWD)
            {
                FrontLeft.acceleratorInput = acceleratorInput;
                FrontRight.acceleratorInput = acceleratorInput;
            }
            else if (drivetrain == Drivetrain.RWD)
            {
                RearLeft.acceleratorInput = acceleratorInput;
                RearRight.acceleratorInput = acceleratorInput;
            }
        }
        else
        {
            FrontLeft.acceleratorInput = acceleratorInput;
            FrontRight.acceleratorInput = acceleratorInput;
            RearLeft.acceleratorInput = acceleratorInput;
            RearRight.acceleratorInput = acceleratorInput;
        }

        currentSpeed = rb.linearVelocity.magnitude;

        updateSteeringAngle(steeringInput);

        broadcastSuspensionParameters();

        updateRigidBody();



    }

    void updateRigidBody()
    {
        Vector3 colliderSize = rbCollider.size;
        Vector3 meshScale = carMesh.localScale;

        rbCollider.size = new Vector3(carWidth + (WheelWidth * 2), colliderSize.y, colliderSize.z);
        carMesh.localScale = new Vector3(carWidth, meshScale.y, meshScale.z);
    }

    void broadcastSuspensionParameters()
    {
        foreach (Suspension wheel in wheels)
        {
            wheel.RestingDistance = RestingDistance;
            wheel.SpringStrength = SpringStrength;
            wheel.suctionForceRatio = suctionForceRatio;
            wheel.DampingForce = DampingForce;
            wheel.maxExtension = maxExtension;
            wheel.accelerationForce = accelerationForce;
            wheel.brakingForce = brakingForce;
            wheel.topSpeed = topSpeed;
            wheel.rollingFriction = rollingFriction;
            wheel.WheelRadius = WheelRadius;

            wheel.wheel.radius = WheelRadius;
            wheel.wheel.width = WheelWidth;
        }
    }

    void updateSteeringAngle(int input)
    {
        steeringSpeedRatio = steeringCurve.curve.Evaluate(currentSpeed / topSpeed);
        currentSteeringAngle += steeringSpeed * steeringSpeedRatio * input * Time.deltaTime;

        if (input == 0 && currentSteeringAngle != 0)
        {
            int previousSign = Math.Sign(currentSteeringAngle);

            currentSteeringAngle += -Mathf.Sign(currentSteeringAngle) * centeringSpeed * steeringSpeedRatio * Time.deltaTime;

            if (Math.Sign(currentSteeringAngle) != previousSign)
            {
                currentSteeringAngle = 0;
            }
        }

        currentSteeringAngle = Mathf.Clamp(currentSteeringAngle, -maxSteeringAngle, maxSteeringAngle);

        FrontLeft.transform.localEulerAngles = new Vector3(0, currentSteeringAngle, 0);
        FrontRight.transform.localEulerAngles = new Vector3(0, currentSteeringAngle, 0);
    }
    void FixedUpdate()
    {

    }

    void Start()
    {
        initialPosition = this.transform.position;
        initialRotation = this.transform.eulerAngles;

        wheels = new List<Suspension> { FrontLeft, FrontRight, RearLeft, RearRight };

        FrontLeft.setTractionCurve(frontTraction.curve);
        FrontRight.setTractionCurve(frontTraction.curve);

        RearLeft.setTractionCurve(rearTraction.curve);
        RearRight.setTractionCurve(rearTraction.curve);

        FrontLeft.setTorqueCurve(torqueCurve.curve);
        FrontRight.setTorqueCurve(torqueCurve.curve);
        RearLeft.setTorqueCurve(torqueCurve.curve);
        RearRight.setTorqueCurve(torqueCurve.curve);
    }

    void Reset()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        this.transform.position = initialPosition;
        this.transform.eulerAngles = initialRotation;
    }

    public float getCurrentSpeed()
    {
        return currentSpeed;
    }
}
