using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Wheels")]
    [SerializeField] private WheelCollider front_Left_Wheel_Collider;
    [SerializeField] private WheelCollider front_Right_Wheel_Collider;
    [SerializeField] private WheelCollider rear_Left_Wheel_Collider;
    [SerializeField] private WheelCollider rear_Right_Wheel_Collider;

    [SerializeField] private Transform front_Left_Wheel_Transform;
    [SerializeField] private Transform front_Right_Wheel_Transform;
    [SerializeField] private Transform rear_Left_Wheel_Transform;
    [SerializeField] private Transform rear_Right_Wheel_Transform;

    [Header("Motor")]
    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float currBreakForce;
    [SerializeField] private float maxSteerAngle;

    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    private float horizontalInput;
    private float verticalInput;
    private float currentBreakForce;
    private float steerAngle;
    private float currentSteerAngle;

    private bool isBreaking;

    void FixedUpdate()
    {
        //GetInput();
        MotorHandle();
        SteeringHandle();
        WheelsUpdate();
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis(HORIZONTAL) * Time.deltaTime;
        verticalInput = Input.GetAxis(VERTICAL) * Time.deltaTime;
        isBreaking = Input.GetKey(KeyCode.Space);

    }

    private void MotorHandle()
    {
        //front_Left_Wheel_Collider.motorTorque = verticalInput * motorForce;
        //front_Right_Wheel_Collider.motorTorque = verticalInput * motorForce;
        
        front_Left_Wheel_Collider.motorTorque = Input.GetAxis(VERTICAL) * Time.deltaTime * motorForce;
        front_Right_Wheel_Collider.motorTorque = Input.GetAxis(VERTICAL) * Time.deltaTime * motorForce;
        isBreaking = Input.GetKey(KeyCode.Space);

        currBreakForce = isBreaking ? breakForce : 0f;

        ApplyBreaking();
    }

    private void ApplyBreaking()
    {
        front_Left_Wheel_Collider.brakeTorque = currBreakForce;
        front_Right_Wheel_Collider.brakeTorque = currBreakForce;
        rear_Left_Wheel_Collider.brakeTorque = currBreakForce;
        rear_Right_Wheel_Collider.brakeTorque = currBreakForce;
    }

    private void SteeringHandle()
    {
        front_Left_Wheel_Collider.steerAngle = Input.GetAxis(HORIZONTAL) * maxSteerAngle * Time.deltaTime;
        front_Right_Wheel_Collider.steerAngle = Input.GetAxis(HORIZONTAL) * maxSteerAngle * Time.deltaTime;
    }

    private void WheelsUpdate()
    {
        WheelsSingleUpdate(front_Left_Wheel_Collider, front_Left_Wheel_Transform);
        WheelsSingleUpdate(front_Right_Wheel_Collider, front_Right_Wheel_Transform);
        WheelsSingleUpdate(rear_Left_Wheel_Collider, rear_Left_Wheel_Transform);
        WheelsSingleUpdate(rear_Right_Wheel_Collider, rear_Right_Wheel_Transform);
    }

    private void WheelsSingleUpdate(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 position;
        Quaternion rotation;

        wheelCollider.GetWorldPose(out position, out rotation);

        wheelTransform.position = position;
        wheelTransform.rotation = rotation;
    }
}
