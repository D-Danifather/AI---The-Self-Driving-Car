using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSensorLines : MonoBehaviour
{
    [SerializeField] private LineRenderer leftLR;
    [SerializeField] private LineRenderer middleLR;
    [SerializeField] private LineRenderer rightLR;
    [SerializeField] private LineRenderer middleOnlyLR;

    [SerializeField] private Transform leftSensor;
    [SerializeField] private Transform middleSensor;
    [SerializeField] private Transform rightSensor;
    [SerializeField] private Transform middleOnlySensor;

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.IsOneSensor)
        {
            if (NeuralController.sensorDetectedWall) 
            {
                leftLR.SetColors(Color.green, Color.green);
                middleLR.SetColors(Color.green, Color.green);
                rightLR.SetColors(Color.green, Color.green);
            }
            else
            {
                leftLR.SetColors(Color.red, Color.red);
                middleLR.SetColors(Color.red, Color.red);
                rightLR.SetColors(Color.red, Color.red);
            }

            leftLR.SetPosition(0, transform.position);
            leftLR.SetPosition(1, leftSensor.position);

            middleLR.SetPosition(0, transform.position);
            middleLR.SetPosition(1, middleSensor.position);

            rightLR.SetPosition(0, transform.position);
            rightLR.SetPosition(1, rightSensor.position);

        }
        else
        {
            if (NeuralController.sensorDetectedWall)
                middleOnlyLR.SetColors(Color.green, Color.green);
            else
                middleOnlyLR.SetColors(Color.red, Color.red);

            middleOnlyLR.SetPosition(0, transform.position);
            middleOnlyLR.SetPosition(1, middleOnlySensor.position);
        }
    }
}
