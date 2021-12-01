using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private AddLines AddLines;

    [Header("Text")]
    [SerializeField] private Text kmhText;
    [SerializeField] private Text generation;
    [SerializeField] private Text population;
    [SerializeField] private Text engine;
    [SerializeField] private Text steering;
    [SerializeField] private Text leftSensor;
    [SerializeField] private Text forwardSensor;
    [SerializeField] private Text rightSensor;
    [SerializeField] private Text top;
    [SerializeField] private Text secondTop;
    [SerializeField] private Text thirdTop;
    [SerializeField] private Text driveTime;
    [SerializeField] private Text resetTime;
    [SerializeField] private Text maxDriveTime;
    [SerializeField] private Text maxVelocity;
    [SerializeField] private Text currTimeScale;
    [SerializeField] private Text currPopulation;
    [SerializeField] private Text currNeurons;
    [SerializeField] private Text currHiddenLayers;
    [SerializeField] private Text currMutationRate;
    [SerializeField] private Text currNNInfos;
    [SerializeField] private Text currentMutationXWeights;
    [SerializeField] private Text currentResetTime;
    [SerializeField] private Text currentFitness;
    [SerializeField] private Text pressTText;

    [Header("Slider")]
    [SerializeField] private Slider timeScaleSlider;
    [SerializeField] private Slider populationSlider;
    [SerializeField] private Slider neuronsSlider;
    [SerializeField] private Slider HiddenLayersSlider;
    [SerializeField] private Slider mutationRateSlider;
    [SerializeField] private Slider resetTimeSlider;

    [Header("Dropdown")]
    [SerializeField] private Dropdown aiMode;
    [SerializeField] private Dropdown fitnessMode;

    [Header("Dropdown")]
    [SerializeField] private Toggle oneSensorToggle;

    [Header("Button")]
    [SerializeField] private Button start;

    [Header("Gameobject")]
    [SerializeField] private GameObject nnWert;
    [SerializeField] private GameObject starting;
    [SerializeField] private GameObject nNSettings;
    [SerializeField] private GameObject nNMode;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject nNInfo;
    [SerializeField] private GameObject lRNNSensors;

    [Header("Others")]
    [SerializeField] private CarEngine car;

    NeuralController neuralController;

    public static int fitnessNumber;

    private float maxCarVelocity;

    private bool isNN = false;

    private int preNeurons;
    private int preHiddens;
    private int preResetTime;

    private void Start()
    {
        neuralController = GameObject.Find("Car").GetComponent<NeuralController>();

        oneSensorToggle.isOn = false;

        starting.SetActive(false);
        nNSettings.SetActive(false);

        SliderSettings(timeScaleSlider, 1, 99, 1);
        SliderSettings(populationSlider, 11, 16, 16);
        SliderSettings(neuronsSlider, 1, 25, 3);
        SliderSettings(HiddenLayersSlider, 1, 10, 1);
        SliderSettings(mutationRateSlider, 2, 25, 10);
        SliderSettings(resetTimeSlider, 30, 500, 300);

        AI_DropDown_PopulateList();
        Fitness_DropDown_PopulateList();
    }

    // Update is called once per frame
    void Update()
    {
        pauseMenu.SetActive(GameManager.IsPaused);

        if (GameManager.IsPaused)
            return;

        if (!GameManager.IsMainMenu)
            starting.SetActive(true);

        GameManager.TimeScale = timeScaleSlider.value;
        currTimeScale.text = timeScaleSlider.value.ToString() + "x";

        NeuralController.staticPopulation = neuralController.population;
        neuralController.population = (int)populationSlider.value;
        currPopulation.text = populationSlider.value.ToString();
        NeuralController.neuronNumbers = (int)neuronsSlider.value;
        currNeurons.text = neuronsSlider.value.ToString();

        NeuralController.hiddenLayerNumbers = (int)HiddenLayersSlider.value;
        currHiddenLayers.text = HiddenLayersSlider.value.ToString();

        GameManager.MutationRate = (int)mutationRateSlider.value;
        currMutationRate.text = " " + mutationRateSlider.value.ToString() + "%";

        NeuralController.resetAfterThisTime = resetTimeSlider.value;
        currentResetTime.text = resetTimeSlider.value + "s";

        float currCarVelocity = car.GetComponent<Rigidbody>().velocity.magnitude * 3.6f;

        kmhText.text = " Speed: " + currCarVelocity.ToString("#0.0") + " Km/h";
        //m_kmhText.text = "Speed: " + Math.Round( m_carVelocity, 2 ) + " Km/h";
        //m_kmhText.text = "Speed: " + (int)m_carVelocity + " Km/h";

        AI_DropDown_IndexChange();

        Fitness_DropDown_IndexChange();

        if (GameManager.IsNeuralNetwork && !GameManager.IsMainMenu)
        {

            nnWert.SetActive(true);

            if (currCarVelocity > maxCarVelocity)
                maxCarVelocity = currCarVelocity;

            generation.text = " Generation: " + NeuralController.generation.ToString();
            population.text = " Population: " + (NeuralController.currentNeuralNetwork + 1) + " / " + NeuralController.staticPopulation.ToString();
            engine.text = " Engine: " + ((float)NeuralController.motor).ToString();
            steering.text = " Steering: " + ((float)((NeuralController.steering - 0.5) * 2)).ToString();

            if (GameManager.IsOneSensor)
            {
                leftSensor.text = " Left Sensor: " + null;
                forwardSensor.text = " Forward Sensor: " + ((float)NeuralController.sensors[1]).ToString();
                rightSensor.text = " Right Sensor: " + null;
            }
            else
            {
                leftSensor.text = " Left Sensor: " + ((float)NeuralController.sensors[0]).ToString();
                forwardSensor.text = " Forward Sensor: " + ((float)NeuralController.sensors[1]).ToString();
                rightSensor.text = " Right Sensor: " + ((float)NeuralController.sensors[2]).ToString();
            }

            top.text = " 1st: " + NeuralController.firstBestDistance.ToString();
            secondTop.text = " 2nd: " + NeuralController.secondBestDistance.ToString();
            thirdTop.text = " 3rd: " + NeuralController.thirdBestDistance.ToString();
            driveTime.text = " Drive Time: " + NeuralController.driveTime.ToString("#0.00") + "/s";
            resetTime.text = " Reset Time: " + NeuralController.resetAfterThisTime.ToString() + "s";
            maxDriveTime.text = " Max. Drive Time: " + NeuralController.maxDriveTime.ToString("#0.00") + "/s";
            if (NeuralController.resetCar)
                maxVelocity.text = " Max. Velocity: " + maxCarVelocity.ToString("#0.00") + " Km/h";
            currNNInfos.text = " NN: " + NeuralController.inputNumbers.ToString() + " Inputs, " + HiddenLayersSlider.value.ToString() + " H.L with " +
                neuronsSlider.value.ToString() + " N, " + NeuralController.outputNumbers.ToString() + " Outputs";
            currentFitness.text = " Fitness Mode: " + NeuralController.fitnessString.ToString();
            currentMutationXWeights.text = " Mutation: " + Network.currWeights.ToString() + " Gewicht davon " +
                mutationRateSlider.value.ToString() + "% = " + Network.currNumOfMutations.ToString() + "x";
        }
        else
            nnWert.SetActive(false);

        if (GameManager.IsNeuralNetwork)
            nNSettings.SetActive(true);
        else
            nNSettings.SetActive(true);

        if (GameManager.IsTPressing)
            nnWert.SetActive(false);
        
        pressTText.text = GameManager.IsTPressing ? "Press T to enable Info": "Press T to disable Info";
        
        if (GameManager.IsOneSensor)
        {
            // Save Neurons and Hidden layer values if one senosr box is unchecked to redo values
            if (neuronsSlider.value != 2)
                preNeurons = (int)neuronsSlider.value;
            if (HiddenLayersSlider.value != 1)
                preHiddens = (int)HiddenLayersSlider.value;
            if (resetTimeSlider.value != 299)
                preResetTime = (int)resetTimeSlider.value;

            // Set one Sensor worked NN
            neuronsSlider.value = 2;
            neuronsSlider.interactable = false;
            HiddenLayersSlider.value = 1;
            HiddenLayersSlider.interactable = false;
            resetTimeSlider.value = 299;
            resetTimeSlider.interactable = false;
        }
        else
        {
            // Get saved values
            if (neuronsSlider.value == 2)
                neuronsSlider.value = preNeurons;
            if (HiddenLayersSlider.value == 1)
                HiddenLayersSlider.value = preHiddens;
            if (resetTimeSlider.value == 299)
                resetTimeSlider.value = preResetTime;

            neuronsSlider.interactable = true;
            HiddenLayersSlider.interactable = true;
            resetTimeSlider.interactable = true;
        }

    }

    private void SliderSettings(Slider slider, float minValue, float maxValue, float value, bool wholeNumber = true)
    {
        slider.minValue = minValue;
        slider.maxValue = maxValue;
        slider.value = value;
        slider.wholeNumbers = wholeNumber;
    }

    public void OneSensorTogle(bool tog)
        => GameManager.IsOneSensor = tog;

    public void AI_DropDown_IndexChange()
    {
        AICase aICase = (AICase)aiMode.value;

        switch (aICase)
        {
            case AICase.Classic:
                AIManager.GetClassicSetting();
                nNMode.SetActive(false);
                break;
            case AICase.NeuralNetwork:
                AIManager.GetNeuralNetworkSetting();
                nNMode.SetActive(true);
                isNN = true;
                break;
            default:
                break;
        }

        aiMode.RefreshShownValue();
    }

    public void Fitness_DropDown_IndexChange()
    {
        FitnessMeasure fitness = (FitnessMeasure)fitnessMode.value;

        switch (fitness)
        {
            case FitnessMeasure.Distance_And_Time:
                fitnessNumber = 0;
                break;
            case FitnessMeasure.Only_Time:
                fitnessNumber = 1;
                break;
            case FitnessMeasure.Only_Distance:
                fitnessNumber = 2;
                break;
            default:
                break;
        }

        fitnessMode.RefreshShownValue();
    }

    private void AI_DropDown_PopulateList()
    {
        string[] enumNames = Enum.GetNames(typeof(AICase));
        List<string> names = new List<string>(enumNames);
        aiMode.ClearOptions();
        aiMode.AddOptions(names);
    }

    private void Fitness_DropDown_PopulateList()
    {
        string[] enumNames = Enum.GetNames(typeof(FitnessMeasure));
        List<string> names = new List<string>(enumNames);
        fitnessMode.ClearOptions();
        fitnessMode.AddOptions(names);
    }

    public void Play()
    {
        GameManager.IsStartPressing = true;

        // IF neural network is activated
        if (isNN)
        {
            // Deactivate classic line renderers
            AddLines.gameObject.SetActive(false);

            // NN wont work with exactly 13 populations... Whatever the reason might be! :(
            int rand = UnityEngine.Random.Range(0, 2) == 0 ? 12 : 14;
            populationSlider.value = populationSlider.value == 13 ? rand : populationSlider.value;

            // Activate NN info
            nNInfo.SetActive(true);

            // Activate NN Senosrs
            lRNNSensors.SetActive(true);
        }

        // Activates script if the ai mode = classic
        AddLines.enabled = aiMode.value == 0;

        GameManager.IsMainMenu = false;
        GameObject.Find("MainPanel").SetActive(false);
    }
}
