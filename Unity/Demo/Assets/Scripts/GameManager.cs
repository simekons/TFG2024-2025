using FMODUnity;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance => _instance;

    private int[] leftHZ;
    private int[] rightHZ;

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
    }

    public void ChangeScene(string nameScene)
    {
        SceneManager.LoadScene(nameScene, LoadSceneMode.Single);
    }

    public void SetAudiometry(int[] audiometryLeft, int[] audiometryRight)
    {
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
}
