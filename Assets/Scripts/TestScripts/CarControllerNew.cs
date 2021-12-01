using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CarControllerNew : MonoBehaviour
{
    [SerializeField] private List<AxleInfo> AxilInfosList = new List<AxleInfo>();

    [SerializeField] private float maxMotorTorque;
    [SerializeField] private float maxSteeringAngles;
    [SerializeField] private float currBreakForce;
    [SerializeField] private float breakForce;

    [Serializable]
    private class AxleInfo
    {
        public WheelCollider leftWheel;
        public WheelCollider rightWheel;

        public bool motor;
        public bool steering;
    }

    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    private void Start()
    {
        
    }

    private void FixedUpdate()
        => HandleMotor_And_Inputs();

    private void HandleMotor_And_Inputs()
    {
        float motor = Input.GetAxis(VERTICAL) * maxMotorTorque;
        float steering = Input.GetAxis(HORIZONTAL) * maxSteeringAngles;

        bool isBreaking = Input.GetKey(KeyCode.Space);
        currBreakForce = isBreaking ? breakForce : 0f;

        foreach (AxleInfo axleInfo in AxilInfosList)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }

            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }

            ApplyBreak(axleInfo.leftWheel);
            ApplyBreak(axleInfo.rightWheel);

            ApplyLocalPositionToVisual(axleInfo.leftWheel);
            ApplyLocalPositionToVisual(axleInfo.rightWheel);
        }
    }

    private void ApplyLocalPositionToVisual(WheelCollider wheelCollider)
    {
        if (wheelCollider.transform.childCount == 0)
            return;

        //int numberOfChilds = 5;
        //for (int i = 0; i < numberOfChilds; i++)
        //{
        //    Transform visualWheel = wheelCollider.transform.GetChild(i);

        //    Vector3 position;
        //    Quaternion rotation;

        //    wheelCollider.GetWorldPose(out position, out rotation);

        //    visualWheel.transform.position = position;
        //    visualWheel.transform.rotation = rotation;
        //}

        Transform visualWheel = wheelCollider.transform;

        Vector3 position;
        Quaternion rotation;

        wheelCollider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }

    private void ApplyBreak(WheelCollider wheelCollider)
    {
        if (wheelCollider.transform.childCount == 0)
            return;

        wheelCollider.brakeTorque = currBreakForce;
    }
}
