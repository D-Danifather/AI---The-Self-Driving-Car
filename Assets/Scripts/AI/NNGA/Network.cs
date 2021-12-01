using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Network
{
	public static float mutation;
	public static float currNumOfMutations;
	public static float currWeights;
	public double[][][] weights;
	public int[] parameters;

	public int lenght;

	double Sigmoid(double x)
		=> 1 / (1 + Mathf.Exp(-(float)x));

	void InitializeVariables()
	{
		this.weights = new double[parameters.Length - 1][][];
		this.lenght = parameters.Length;
	}

	public Network(Network Dad, Network Mom)
	{
		int weightCounter = 0;

		this.parameters = Mom.parameters;
		InitializeVariables();

		for (int i = 0; i < parameters.Length - 1; i++)
		{

			weights[i] = new double[parameters[i]][];

			for (int j = 0; j < parameters[i]; j++)
			{

				weights[i][j] = new double[parameters[i + 1]];

				for (int k = 0; k < parameters[i + 1]; k++)
				{
					weightCounter++;

					// Crossing
					if (Random.Range(0, 2) == 0)
						weights[i][j][k] = Mom.weights[i][j][k];
					else
						weights[i][j][k] = Dad.weights[i][j][k];
				}
			}
		}

		currWeights = weightCounter;

		float mutationRate = mutation / 100; // Value x --> mutation rate in MainPanel given the number as float to use in the for loop down der => in % vom gesamten Gewicht
											 // + 0.01f cause if the number should be exactly in the middle --> (2.5) it will rount it to => 3
		float finalNumOfMutations = Mathf.Round(weightCounter * mutationRate + 0.01f); // This could be calculated by size of the weight arrays * by x% --> x times commits randomly a connection between neurons and then mutates it
																					   // --> Ansonst gibt es wenig Änderung für die Gene => so wird es sehr lange dauern, für eine gute Lösung
		currNumOfMutations = finalNumOfMutations;

		//Debug.Log("numOfMutations:  " + finalNumOfMutations + " mutationrate = " + mutationRate);

		for (int i = 0; i < finalNumOfMutations; i++)
		{
			int mutationLayer = Random.Range(0, weights.Length); // Layers
			int mutationLeft = Random.Range(0, weights[mutationLayer].Length); // Neurons
			int mutationRight = Random.Range(0, weights[mutationLayer][mutationLeft].Length); // Connection

			weights[mutationLayer][mutationLeft][mutationRight] = GetRandomWeight();
		}
	}

	public Network(int[] parameters)
	{
		this.parameters = parameters;
		//int a = 0;
		//{3,5,2}

		InitializeVariables();

		for (int i = 0; i < parameters.Length - 1; i++)
		{

			weights[i] = new double[parameters[i]][];

			for (int j = 0; j < parameters[i]; j++)
			{
				int nrOfWeightsWithBias = parameters[i + 1] + 1;
				//weights[i][j] = new double[parameters[i + 1]];
				weights[i][j] = new double[nrOfWeightsWithBias];

				for (int k = 0; k < nrOfWeightsWithBias; k++)
				{
					weights[i][j][k] = GetRandomWeight();
				}

			}
		}

	}

	public double[] Process(double[] inputs)
	{
		//int a = 0;

		if (inputs.Length != parameters[0])
			return null;

		double[] outputs;
		//Debug.Log (lenght);
		//for each layer
		for (int i = 0; i < (lenght - 1); i++)
		{
			//output values, they all start at 0 by default, checked that in C# Documentation ;)
			outputs = new double[parameters[i + 1]];

			//for each input neuron
			for (int j = 0; j < inputs.Length; j++)
			{
				//and for each output neuron
				for (int k = 0; k < outputs.Length; k++)
				{
					//Debug.Log (i + " " + j + " " + k);
					//a++;
					//increase the load of an output neuron by the value of each input neuron multiplied by the weight between them
					if (k == outputs.Length - 1)
						// Bias
						outputs[k] += 1f + weights[i][j][k];
					else
						outputs[k] += inputs[j] * weights[i][j][k];
				}
			}

			//we have the proper output values, now we have to use them as inputs to the next layer and so on, until we hit the last layer
			inputs = new double[outputs.Length];

			//after all output neurons have their values summed up, apply the activation function and save the value into new inputs
			for (int l = 0; l < outputs.Length; l++)
			{
				inputs[l] = Sigmoid(outputs[l] * 5);
			}
		}

		//Debug.Log (a);

		return inputs;

		// inputy to wartości odległości z czujników (0-1), są 3 (na razie)

		//outputy to wartości do sterowania (zakręt i silnik), są 2

		//old way of processing, not working
		//return processRecurrent (inputs, 0);
	}

	//	this is DEPRECATED
	public double[] ProcessRecurrent(double[] inputs, int layer)
	{
		if (layer == parameters.Length - 1)
			return inputs;

		layer++;

		double[] outputs = new double[parameters[layer]];

		for (int i = 0; i < parameters[layer]; i++)
		{
			outputs[i] = 0;
		}

		for (int i = 0; i < parameters[layer]; i++)
		{
			for (int j = 0; j < inputs.Length; j++)
			{
				outputs[i] += inputs[j] * weights[layer - 1][j][i];
			}
		}

		for (int i = 0; i < parameters[layer]; i++)
		{
			outputs[i] = Sigmoid(outputs[i]);
		}

		return ProcessRecurrent(outputs, layer);

	}

	double GetRandomWeight()
		=> Random.Range(-1.0f, 1.0f);
}