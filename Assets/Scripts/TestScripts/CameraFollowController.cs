using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Car_Mode { Simple, Klassic, NeuralNetwork}

public class CameraFollowController : MonoBehaviour
{
    [SerializeField] private Transform objectToFollow;

    [SerializeField] private Vector3 offset;

    [SerializeField] private float followSpeed = 10f;
    [SerializeField] private float lookSpeed = 10f;

    private void LateUpdate()
    {
        LookAtTarget();
        MoveToTarget();
    }

    private void LookAtTarget()
    {
        Vector3 _lookDirection = objectToFollow.position - transform.position;
        Quaternion _rot = Quaternion.LookRotation(_lookDirection, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, _rot, lookSpeed * Time.deltaTime); // Pos?
    }

    private void MoveToTarget()
    {
        Vector3 _targetPos = objectToFollow.position +
                             objectToFollow.forward * offset.z +
                             objectToFollow.right * offset.x +
                             objectToFollow.up * offset.y;
        transform.position = Vector3.Lerp(transform.position, _targetPos, followSpeed * Time.deltaTime);
    }
}
