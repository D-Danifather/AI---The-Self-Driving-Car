using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCarController : MonoBehaviour
{

    [SerializeField] private WheelCollider frontDriverW, frontPassangerW;
    [SerializeField] private WheelCollider rearDriverW, rearPassangerW;

    [SerializeField] private Transform frontDriverT, frontPassangerT;
    [SerializeField] private Transform rearDriverT, rearPassangerT;

    [SerializeField] private float maxSteerAngle = 30;
    [SerializeField] private float motorForce = 50;
    [SerializeField] private float breakForce = 500;

    private const string M_HORIZONTAL = "Horizontal";
    private const string M_VERTICAL = "Vertical";

    private float m_horizontalInput;
    private float m_verticalInput;
    private float m_steeringAngle;

    private bool m_breaking;

    void FixedUpdate()
    {
        GetInput();
        Steer();
        Accelerate();
        Break();
        UpdateWheelPoses();
    }

    private void GetInput()
    {
        m_horizontalInput = Input.GetAxis(M_HORIZONTAL);
        m_verticalInput = Input.GetAxis(M_VERTICAL);
        m_breaking = Input.GetKey(KeyCode.Space);
    }
    private void Steer()
    {
        m_steeringAngle = maxSteerAngle * m_horizontalInput;
        frontDriverW.steerAngle = m_steeringAngle;
        frontPassangerW.steerAngle = m_steeringAngle;
    }

    private void Accelerate()
    {
        frontDriverW.motorTorque = m_verticalInput * motorForce;
        frontPassangerW.motorTorque = m_verticalInput * motorForce;
    }
    private void Break()
    {
        float currBreakForce = m_breaking ? breakForce : 0f;

        rearDriverW.brakeTorque = currBreakForce;
        rearPassangerW.brakeTorque = currBreakForce;
    }

    private void UpdateWheelPoses()
    {
        UpdateWheelPose(frontDriverW, frontDriverT);
        UpdateWheelPose(frontPassangerW, frontPassangerT);
        UpdateWheelPose(rearDriverW, rearDriverT);
        UpdateWheelPose(rearPassangerW, rearPassangerT);
    }

    private void UpdateWheelPose(WheelCollider _collider, Transform _transform)
    {
        Vector3 _pos = _transform.position;
        Quaternion _quat = _transform.rotation;

        _collider.GetWorldPose(out _pos, out _quat);

        _transform.position = _pos;
        _transform.rotation = _quat;
    }
}
