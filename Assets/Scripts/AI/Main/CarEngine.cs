using System.Collections.Generic;
using UnityEngine;

public class CarEngine : MonoBehaviour
{
    [SerializeField] private Transform path;

    [Header("Car wheel collider settings")]
    // For rotaion and motor toque --> front wheel colliders
    [SerializeField] private WheelCollider FLC;
    [SerializeField] private WheelCollider FRC;

    // For rotation and position --> back wheel transfor
    [SerializeField] private WheelCollider BLC;
    [SerializeField] private WheelCollider BRC;

    [Header("Car wheel transform settings")]
    // For rotation and position --> front wheel transform
    [SerializeField] private Transform FLT;
    [SerializeField] private Transform FRT;

    // For break torque --> back wheel colliders
    [SerializeField] private Transform BLT;
    [SerializeField] private Transform BRT;

    [Header("Car motor settings")]
    [SerializeField] private float maxSteeringAngle = 30f;
    [SerializeField] private float maxMotorTorque = 80f;
    [SerializeField] private float currentSpeed;
    [SerializeField] private float maxSpeed = 100f;
    [SerializeField] private float maxBreakTorque = 200f;

    [SerializeField] private bool isBraking = false;

    [SerializeField] private Vector3 centerOfMass;

    [Header("Car Sensors")]
    [SerializeField] private float sensorLength = 5f;
    [SerializeField] private float frontSensorPosition = 0.5f;

    private readonly List<Transform> nodes = new List<Transform>();

    private int currentNode = 0;

    private void Start()
    {
        GetComponent<Rigidbody>().centerOfMass = centerOfMass;

        Transform[] pathTransform = path.GetComponentsInChildren<Transform>();

        for (int i = 0; i < pathTransform.Length; i++)
        {
            if (pathTransform[i] != path.transform)
                nodes.Add(pathTransform[i]);
        }
    }

    private void FixedUpdate()
    {
        if (!GameManager.IsClassic)
        {
            if (GameManager.IsNeuralNetwork)
            {
                // NNGA
                //maxMotorTorque = 200;

                float motor = maxMotorTorque * (float)NeuralController.motor + 0.5f;
                float steering = maxSteeringAngle * (float)(NeuralController.steering - 0.5f) * 2;
                //float brakeing = maxBreakTorque * (float)NeuralController.braking - 0.5f;

                // Recurrent NN = output als input mitgeben
                FRC.motorTorque = motor;
                FLC.motorTorque = motor;

                FRC.steerAngle = steering;
                FLC.steerAngle = steering;

                //BRC.steerAngle = brakeing;
                //BLC.steerAngle = brakeing;
            }
        }
        else
        {
            // Classic
            Sensors();
            ApplySteering();
            Drive();
            Seek_WayPointsDistance();
            Braking();
            WheelsUpdate();
        }
    }

    private void Sensors()
    {
        Vector3 sensorStartPosition = transform.position;
        sensorStartPosition.z += frontSensorPosition;

        if (Physics.Raycast(sensorStartPosition, transform.forward, out RaycastHit hit, sensorLength))
        {

        }

        Debug.DrawLine(sensorStartPosition, hit.point, Color.red);
    }

    private void ApplySteering()
    {
        Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].position);
        //print(relativeVector);
        //relativeVector /= relativeVector.magnitude;
        float newSteer = Mathf.Lerp(relativeVector.x, (relativeVector.x / relativeVector.magnitude) * maxSteeringAngle, 5f);

        FRC.steerAngle = newSteer;
        FLC.steerAngle = newSteer;
    }

    private void Drive()
    {
        // Calculates the current speed, based how fast the wheel is spinning --> like real cars
        currentSpeed = 2 * Mathf.PI * FLC.radius * FLC.rpm * 60 / 100; 

        // If car is below max speed, it will shutdown the motor torque, for better drive between lines
        // And also if the car is not braking --> do not rotate front wheels while the car stops and still is braking 
        if (currentSpeed < maxSpeed && !isBraking)
        {
            FRC.motorTorque = maxMotorTorque;
            FLC.motorTorque = maxMotorTorque;
        }
        else
        {
            FRC.motorTorque = 0;
            FLC.motorTorque = 0;
        }
    }

    private void Seek_WayPointsDistance()
    {
        // For classsic AI im using a Equivalent Seek Methode, which we used in Drone-Projekt :)
        if (Vector3.Distance(transform.position, nodes[currentNode].position) < 1f)
            if (currentNode == nodes.Count - 1)
                currentNode = 0;
            else
                currentNode++;
    }

    private void Braking()
    {
        if (isBraking)
        {
            BLC.brakeTorque = maxBreakTorque;
            BRC.brakeTorque = maxBreakTorque;
        }
        else
        {
            BLC.brakeTorque = 0;
            BRC.brakeTorque = 0;
        }
    }

    private void WheelsUpdate()
    {
        WheelsSingleUpdate(FLC, FLT);
        WheelsSingleUpdate(FRC, FRT);
        WheelsSingleUpdate(BLC, BRT);
        WheelsSingleUpdate(BRC, BRT);
    }

    private void WheelsSingleUpdate(WheelCollider wheelCollider, Transform wheelTransform)
    {

        wheelCollider.GetWorldPose(out Vector3 position, out Quaternion rotation);

        wheelTransform.SetPositionAndRotation(position, rotation);

        /*Vector3 position;
        Quaternion rotation;

        wheelCollider.GetWorldPose(out position, out rotation);

        wheelTransform.position = position;
        wheelTransform.rotation = rotation;*/
    }
}
