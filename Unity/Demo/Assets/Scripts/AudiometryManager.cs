using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using System;
using Unity.VisualScripting.FullSerializer;

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
            guardarLeftHZ[sonidoActual] = volumen;
        else
            guardarRightHZ[sonidoActual] = volumen;

        volumen = 0;
        sonidoActual++;
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

    public String PrintResulatdos()
    {
        List<int> frecuencias = new List<int> { 250, 500, 1000, 2000, 4000, 8000 };


        String izquierda = "Izquierda: ";
        for (int i = 0; i < guardarLeftHZ.Length; i++)
        {
            izquierda = izquierda + frecuencias[i].ToString() + " Hz = " + guardarLeftHZ[i].ToString() + " dB, ";
        }
        Debug.Log(izquierda);

        String derecha = "Derecha: ";
        for (int i = 0; i < guardarRightHZ.Length; i++)
        {
            derecha = derecha + frecuencias[i].ToString() + " Hz = " + guardarRightHZ[i].ToString() + " dB, ";
        }
        Debug.Log(derecha);

        return (izquierda + "\n" + derecha + "\n");
    }
}
