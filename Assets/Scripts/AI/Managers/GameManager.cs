using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static float timeScale;
    private static float mutationRate;

    private static int populationCount;

    private static bool isMainMenu = true;
    private static bool isClassic = false;
    private static bool isNeuralNetwork = false;
    private static bool isOneSensor = false;
    private static bool isPaused = false;
    private static bool isTPressing = false;
    private static bool isStartPressing = false;

    public static float TimeScale { get => timeScale; set => timeScale = value; }
    public static float MutationRate { get => mutationRate; set => mutationRate = value; }

    public static int PopulationCount { get => populationCount; set => populationCount = value; }

    public static bool IsMainMenu { get => isMainMenu; set => isMainMenu = value; }
    public static bool IsClassic { get => isClassic; set => isClassic = value; }
    public static bool IsNeuralNetwork { get => isNeuralNetwork; set => isNeuralNetwork = value; }
    public static bool IsOneSensor { get => isOneSensor; set => isOneSensor = value; }
    public static bool IsPaused { get => isPaused; set => isPaused = value; }
    public static bool IsTPressing { get => isTPressing; set => isTPressing = value; }
    public static bool IsStartPressing { get => isStartPressing; set => isStartPressing = value; }

    void Start()
    {
        Time.timeScale = timeScale;
        Network.mutation = mutationRate;
    }

    void Update()
    {
        Time.timeScale = timeScale;
        Network.mutation = mutationRate;

        if (isMainMenu)
            Time.timeScale = 0;

        if (Input.GetKeyDown(KeyCode.E))
        {
            isPaused = !isPaused;
            timeScale = isPaused ? 0 : timeScale;
        }

        if (Input.GetKeyDown(KeyCode.R))
            Application.ExternalEval("location.reload(true)");

        if (Input.GetKeyDown(KeyCode.T))
            IsTPressing = !IsTPressing;

        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        //Application.LoadLevel(Application.loadedLevel);
        //Application.Quit();
    }
}
