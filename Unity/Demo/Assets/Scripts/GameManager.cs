using FMODUnity;
using Telemetry;
using Telemetry.Persistance;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance => _instance;

    private int[] leftHZ;
    private int[] rightHZ;

    private int _enemies = 0;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        else Destroy(this.gameObject);

        leftHZ = new int[6];
        rightHZ = new int[6];
        Tracker.Instance("TFG", Telemetry.Persistance.PersistanceType.File, Telemetry.Serialization.SerializeType.JSON, "datos");
    }

    private void Update()
    {
        if (Tracker.getInstance() != null)
            Tracker.getInstance().update(Time.deltaTime);
    }

    public void ChangeScene(string nameScene)
    {
        if (SceneManager.GetActiveScene().name == "MEM_1" || SceneManager.GetActiveScene().name == "MEM_2" || SceneManager.GetActiveScene().name == "mem2")
        {
            Tracker.getInstance().endGameMemory();
        }
        if (SceneManager.GetActiveScene().name == "FPS_1" || SceneManager.GetActiveScene().name == "FPS_2" || SceneManager.GetActiveScene().name == "fps")
        {
            Tracker.getInstance().endGameFPS();
        }

        SceneManager.LoadScene(nameScene, LoadSceneMode.Single);
    }

    public void SetAudiometry(int[] audiometryLeft, int[] audiometryRight)
    {
        Tracker.getInstance().leftEq(leftHZ);
        Tracker.getInstance().rightE(rightHZ);
        audiometryLeft.CopyTo(leftHZ, 0);
        audiometryRight.CopyTo(rightHZ, 0);
    }

    public void SetGlobalParameter(string parameterName, float value)
    {
        RuntimeManager.StudioSystem.setParameterByName(parameterName, value);
    }

    public void ApplyAudiometryToGlobalFrequencies(bool apply)
    {
        int[] left = new int[6];
        int[] right = new int[6];
        int[] frecuencias = new int[] { 250, 500, 1000, 2000, 4000, 8000 };

        if (apply)
        {
            left = leftHZ;
            right = rightHZ;
            Debug.Log("Aplicando Audiometria");
        }
        else
            Debug.Log("Quitando audiometria");

        FMOD.Studio.System system = RuntimeManager.StudioSystem;

        for (int i = 0; i < frecuencias.Length; i++)
        {
            system.setParameterByName($"Left/L{frecuencias[i]}", left[i]);
            system.setParameterByName($"Right/R{frecuencias[i]}", right[i]);
        }
    }

    void OnApplicationQuit()
    {
        Tracker.getInstance().endSession();
        Tracker.getInstance().end();
    }

    public void addEnemy()
    {
        _enemies++;
    }

    public int getEnemies()
    {
        return _enemies;
    }
}
