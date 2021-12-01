using System.Collections.Generic;
using UnityEngine;

public enum FitnessMeasure
{
	Distance_And_Time,
	Only_Time,
	Only_Distance
}

public class NeuralController : MonoBehaviour
{
	public List<float> pointsList = new List<float>();
	public int sensorLenght;

	[HideInInspector] public int population;
	public static int staticPopulation;

	public static double driveTime = 0;
	public static double maxDriveTime;
	public static double steering;
	public static double braking;
	public static double motor;
	public static int inputNumbers = 3;
	public static int neuronNumbers;
	public static int outputNumbers = 3;
	public static int hiddenLayerNumbers;
	public static float resetAfterThisTime;

	public static int generation = 0;
	public double[] points;
	public double[] results;
	public static int currentNeuralNetwork = 0;

	[HideInInspector] public static double[] sensors;

	private static float bestPoints = 0;
	public static float firstBestDistance = 0;
	public static float secondBestDistance = 0;
	public static float thirdBestDistance = 0;

	public static string fitnessString;

	public static bool resetCar = false;

	private Rigidbody rigidbody;

	private FitnessMeasure fitnessMeasure;

	private Vector3 forward;
	private Vector3 right;
	private Vector3 left;

	private Network[] networks;
	private RaycastHit hit;

	private Vector3 position;

	private static Vector3 pos;
	private static Quaternion rot;

	private int[] parameters;

	public static Vector3 leftSensor;

	public static bool sensorDetectedWall;

	private bool shouldUpdate = true;

	// Use this for initialization
	private void Start()
	{
		// To set the exact position and rotation after reseting the car, like at the beginning
		pos = this.transform.position;
		rot = this.transform.rotation;

		// Gesamt Gewicht Berechnung -->
		// 3 Inputs - 2 Hidden Layers a 10 Neuronen - 2 Outputs
		// --> Links nach rechts => Jeder Neuron a 3 Inputs => 3 * 10 = 30
		// --> 10 Neurons Verbindung mit a 10 Neuronen vom Letzten Hidden Layers => 10 * 10 = 100
		// --> 2 Outputs Verbindung mit a 10 Neuronen vom letzten Hidden Layers => 10 * 2 = 20
		// --> Gesamt Gewicht => 30 + 100 + 20 = 150 --> davon 10% => 15 Neuronen verändern

		// This new Code automates the dynamic type of numbers of Hidden Layers | an old example is down below
		parameters = new int[hiddenLayerNumbers + 2];

        for (int i = 0; i < parameters.Length; i++)
        {
			if (i == 0)
				parameters[i] = inputNumbers;
			else if (i == parameters.Length - 1)
				parameters[parameters.Length - 1] = outputNumbers;
			else
				parameters[i] = neuronNumbers;
		}

		// Access to how many Hidden Layers should be used
		//switch (hiddenLayerNumbers)
		//{
		//	case 1:
		//		parameters = new int[] { inputNumbers, neuronNumbers, outputNumbers };
		//		break;
		//	case 2:
		//		parameters = new int[] { inputNumbers, neuronNumbers, neuronNumbers, outputNumbers };
		//		break;
		//	case 3:
		//		parameters = new int[] { inputNumbers, neuronNumbers, neuronNumbers, neuronNumbers, outputNumbers };
		//		break;
		//	case 4:
		//		parameters = new int[] { inputNumbers, neuronNumbers, neuronNumbers, neuronNumbers, neuronNumbers, outputNumbers };
		//		break;
		//	case 5:
		//		parameters = new int[] { inputNumbers, neuronNumbers, neuronNumbers, neuronNumbers, neuronNumbers, neuronNumbers, outputNumbers };
		//		break;
		//	case 6:
		//		parameters = new int[] { inputNumbers, neuronNumbers, neuronNumbers, neuronNumbers, neuronNumbers, neuronNumbers, neuronNumbers, outputNumbers };
		//		break;
		//	case 7:
		//		parameters = new int[] { inputNumbers, neuronNumbers, neuronNumbers, neuronNumbers, neuronNumbers, neuronNumbers, neuronNumbers, neuronNumbers, outputNumbers };
		//		break;
		//	case 8:
		//		parameters = new int[] { inputNumbers, neuronNumbers, neuronNumbers, neuronNumbers, neuronNumbers, neuronNumbers, neuronNumbers, neuronNumbers, neuronNumbers, outputNumbers };
		//		break;
		//	case 9:
		//		parameters = new int[] { inputNumbers, neuronNumbers, neuronNumbers, neuronNumbers, neuronNumbers, neuronNumbers, neuronNumbers, neuronNumbers, neuronNumbers, neuronNumbers, outputNumbers };
		//		break;
		//	case 10:
		//		parameters = new int[] { inputNumbers, neuronNumbers, neuronNumbers, neuronNumbers, neuronNumbers, neuronNumbers, neuronNumbers, neuronNumbers, neuronNumbers, neuronNumbers, neuronNumbers, outputNumbers };
		//		break;
		//	default:
		//		break;
		//}

		// Access and add the Fitness Mode und add it to the NN
		switch (CanvasManager.fitnessNumber)
		{
			case 0:
				fitnessMeasure = FitnessMeasure.Distance_And_Time;
				fitnessString = "Distance_And_Time";
				break;
			case 1:
				fitnessMeasure = FitnessMeasure.Only_Time;
				fitnessString = "Only_Time";
				break;
			case 2:
				fitnessMeasure = FitnessMeasure.Only_Distance;
				fitnessString = "Only_Distance";
				break;
			default:
				break;
		}

		//print(fitnessMeasure);

		//int[] parameters = { 3, neuronNumbers, 3 };

		staticPopulation = population;

		// Test - Basis \\
		/********************************************************************************************/

		//for (int i = neuronNumbers; i < hiddenLayers; i++)
		//{
		//	//string.Format("{0:n0}", i);
		//	i.ToString("0,#");
		//	Debug.Log(i);
		//	// Gesamt Gewicht Berechnung -->
		//	// 3 Inputs - 2 Hidden Layers a 10 Neuronen - 2 Outputs
		//	// --> Links nach rechts => Jeder Neuron a 3 Inputs => 3 * 10 = 30
		//	// --> 10 Neurons Verbindung mit a 10 Neuronen vom Letzten Hidden Layers => 10 * 10 = 100
		//	// --> 2 Outputs Verbindung mit a 10 Neuronen vom letzten Hidden Layers => 10 * 2 = 20
		//	// --> Gesamt Gewicht => 30 + 100 + 20 = 150 --> davon 10% => 15 Neuronen verändern
		//	parameters = new int[] { 3, i, 3 };
		//}

		/********************************************************************************************/

		//if (GameManager.IsOneSensor)
		//	sensorLenght = 5;

		rigidbody = GetComponent<Rigidbody>();

		results = new double[2];
		points = new double[population];
		sensors = new double[3];

		//default vector values
		//forward = Vector3.forward * 2;
		//right = new Vector3(0.4f, 0, 0.7f);
		//left = new Vector3(-0.4f, 0, 0.7f);

		// Set new values in y-position, cause the drivving street is not totally flat!
		//forward = GameManager.IsOneSensor ? new Vector3(-0.84f, 0.005f, 2) : new Vector3(0, 0.05f, 2);

		right = new Vector3(0.4f, 0.05f, 0.7f);
		left = new Vector3(-0.4f, 0.05f, 0.7f);

		position = transform.position;
		networks = new Network[population];

		for (int i = 0; i < population; i++)
			networks[i] = new Network(parameters);

		leftSensor = left;
	}
	private void FixedUpdate()
	{
		if (GameManager.IsOneSensor)
			sensors[0] = GetSensor(forward);
		else
		{
			sensors[0] = GetSensor(left);
			sensors[1] = GetSensor(forward);
			sensors[2] = GetSensor(right);
		}

		steering = results[0];
		motor = results[1];
		results = networks[currentNeuralNetwork].Process(sensors);

		driveTime += Time.deltaTime;

		points[currentNeuralNetwork] += Vector3.Distance(position, transform.position);
		position = transform.position;
	}

	private void Update()
	{
		if (shouldUpdate)
        {
			if (GameManager.IsOneSensor)
				sensorLenght = 5;

			forward = GameManager.IsOneSensor ? new Vector3(-0.84f, 0.005f, 2) : new Vector3(0, 0.05f, 2);

			if (GameManager.IsStartPressing)
				shouldUpdate = false;
		}

		resetCar = false;

		//check if the network is moving
		if (driveTime > 3 && rigidbody.velocity.magnitude < 0.005 || driveTime > resetAfterThisTime)
			OnCollisionEnter(null);
	}

	//game over, friend :/ - :(
	private void OnCollisionEnter(Collision col)
	{
		ResetCarPosition();

		switch (fitnessMeasure)
		{
			case FitnessMeasure.Distance_And_Time:
				points[currentNeuralNetwork] *= points[currentNeuralNetwork];
				points[currentNeuralNetwork] /= driveTime;
				break;
			case FitnessMeasure.Only_Time:
				points[currentNeuralNetwork] /= driveTime;
				break;
			case FitnessMeasure.Only_Distance:
				points[currentNeuralNetwork] *= points[currentNeuralNetwork];
				break;
			default:
				break;
		}

		resetCar = true;

		if (maxDriveTime < driveTime)
			maxDriveTime = driveTime;

		driveTime = 0;

		//now we reproduce
		if (currentNeuralNetwork == population - 1)
		{
			double maxValue = points[0];

			int maxIndex = 0;

			//looking for the two best networks in the generation
			for (int i = 1; i < population; i++)
			{
				if (points[i] > maxValue)
				{
					maxIndex = i;
					maxValue = points[i];
				}
			}

			if (points[maxIndex] > bestPoints)
			{
				bestPoints = (float)points[maxIndex];
				pointsList.Add(bestPoints);
			}

			for (int i = 0; i < pointsList.Count; i++)
			{
				firstBestDistance = pointsList[i];

				if (i > 0)
				{
					secondBestDistance = pointsList[i - 1];

					if (i > 1)
						thirdBestDistance = pointsList[i - 2];

					if (i > 2)
						pointsList.Remove(pointsList[i - 3]);
				}
			}

			points[maxIndex] = -10;

			Network mother = networks[maxIndex];

			maxValue = points[0];
			maxIndex = 0;

			for (int i = 1; i < population; i++)
			{
				if (points[i] > maxValue)
				{
					maxIndex = i;
					maxValue = points[i];
				}
			}

			points[maxIndex] = -10;

			Network father = networks[maxIndex];

			// Elitism --> Da wird die beste Netzwerk übernommen für die neue Generation
			networks[0] = mother;

			for (int i = 1; i < population; i++)
			{
				points[i] = 0;

				//creating new generation of networks with random combinations of genes from two best parents
				networks[i] = new Network(father, mother);
			}

			generation++;

			//because we increment it at the beginning, that's why -1
			currentNeuralNetwork = -1;
		}

		currentNeuralNetwork++;

		//position reset is pretty important, don't forget it :* --> | Done already! :) | 
		position = transform.position;
	}

	private void ResetCarPosition()
	{
		rigidbody.velocity = Vector3.zero;
		rigidbody.angularVelocity = Vector3.zero;
		/*transform.position = new Vector3(33.57f, 2.5f, 147.17f);
		transform.rotation = new Quaternion(0, 250, 0, 0);
		transform.position = pos;
		transform.rotation = rot;*/
		transform.SetPositionAndRotation(pos, rot);
	}

	private double GetSensor(Vector3 direction)
	{
		Vector3 fwd = transform.TransformDirection(direction);

		if (Physics.Raycast(transform.position, fwd, out hit))
		{
			if (hit.distance < sensorLenght)
			{
				Debug.DrawRay(transform.position, fwd * sensorLenght, Color.red, 0, true);
				return 1f - hit.distance / sensorLenght;
			}
            else
				Debug.DrawRay(transform.position, fwd * sensorLenght, Color.green, 0, true);

			// For line rendering
			if (hit.distance < sensorLenght * 2)
				sensorDetectedWall = false;
			else
				sensorDetectedWall = true;
		}
        else
        {
			Debug.DrawRay(transform.position, fwd * sensorLenght, Color.green, 0, true);
			sensorDetectedWall = false;
		}

		return 0;
	}
}
