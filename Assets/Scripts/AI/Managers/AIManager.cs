using UnityEngine;

public enum AICase
{
    Classic,
    NeuralNetwork,
}

public class AIManager : MonoBehaviour
{
    private AICase aICase;

    private void Start()
        => aICase = AICase.Classic;

    public static void GetClassicSetting()
    {
        GameManager.IsClassic = true;
        GameManager.IsNeuralNetwork = false;
        GameObject.Find("Car").GetComponent<CarEngine>().enabled = true;
        GameObject.Find("Car").GetComponent<NeuralController>().enabled = false;
    }

    public static void GetNeuralNetworkSetting()
    {
        GameManager.IsClassic = false;
        GameManager.IsNeuralNetwork = true;
        GameObject.Find("Car").GetComponent<CarEngine>().enabled = true;
        GameObject.Find("Car").GetComponent<NeuralController>().enabled = true;
    }
}
