using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using System;
using Unity.VisualScripting.FullSerializer;
using System.IO;

public class AudiometryManager : MonoBehaviour
{
    [field: Header("Left Ear")]
    [field: SerializeField] public EventReference leftEar { get; private set; }
    [field: Header("Right Ear")]
    [field: SerializeField] public EventReference rightEar { get; private set; }

    public static AudiometryManager instance { get; private set; }

    private EventInstance left;
    private EventInstance right;
    private EventInstance actualEar;

    int[] ordenSonidos;
    int[] guardarLeftHZ;
    int[] guardarRightHZ;
    int sonidoActual;
    int volumen;
    bool ear;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one AudioManager in scene");
        }
        instance = this;
    }

    void Start()
    {
        left = CreateEventInstance(leftEar);
        right = CreateEventInstance(rightEar);
        RandomSounds();

        guardarLeftHZ = new int[6];
        guardarRightHZ = new int[6];
    }

    public int GetSonidoActual()
    {
        return sonidoActual;
    }

    public EventInstance CreateEventInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        return eventInstance;
    }

    public void StartAudiometry(bool leftEar)
    {
        sonidoActual = 0;
        volumen = 0;

        ear = leftEar;

        if(leftEar)
            actualEar = left;
        else
            actualEar = right;

        actualEar.setParameterByName("HZ", ordenSonidos[sonidoActual]);
        actualEar.setParameterByName("dB", volumen);
        actualEar.start();
    }

    public void StopAudiometry()
    {
        actualEar.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    public int SubirVolumen()
    {
        volumen = (volumen + 10 > 130) ? 130 : volumen + 10;
        actualEar.setParameterByName("dB", volumen);
        return volumen;
    }

    public void NextSound()
    {
        if (ear)
            guardarLeftHZ[ordenSonidos[sonidoActual] - 1] = volumen;
        else
            guardarRightHZ[ordenSonidos[sonidoActual] - 1] = volumen;

        volumen = 0;
        sonidoActual++;
        AudiometryUIManager.instance.SetTextVol();
        actualEar.setParameterByName("HZ", ordenSonidos[sonidoActual]);
        actualEar.setParameterByName("dB", volumen);
    }

    private void RandomSounds()
    {
        List<int> frecuencias = new List<int> { 1, 2, 3, 4, 5, 6 };
        ordenSonidos = new int[6];
        System.Random random = new System.Random();

        for (int i = 0; i < ordenSonidos.Length; i++)
        {
            int num = random.Next(frecuencias.Count);
            ordenSonidos[i] = frecuencias[num]; 
            frecuencias.RemoveAt(num);
        }
    }

    public void SaveResultados()
    {
        int[] frecuencias = new int[] { 250, 500, 1000, 2000, 4000, 8000 };

        // Generar el texto para el archivo
        string izquierda = "Left\n";
        for (int i = 0; i < guardarLeftHZ.Length; i++)
        {
            izquierda += $"{frecuencias[i]} {guardarLeftHZ[i]}\n";
        }
        Debug.Log(izquierda);

        string derecha = "Right\n";
        for (int i = 0; i < guardarRightHZ.Length; i++)
        {
            derecha += $"{frecuencias[i]} {guardarRightHZ[i]}\n";
        }
        Debug.Log(derecha);

        string resultados = izquierda + derecha;

        // Ruta para guardar el archivo
        string path = "Assets/Python/datos.txt";

        // Guardar el archivo
        File.WriteAllText(path, resultados);

        GameManager.Instance.SetAudiometry(guardarLeftHZ, guardarRightHZ);
    }
}
