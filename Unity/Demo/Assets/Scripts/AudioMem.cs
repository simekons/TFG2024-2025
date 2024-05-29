using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioMem : MonoBehaviour
{
    [Header("Volume")]
    [Range(0, 1)]
    public float masterVolume = 1;

    private Bus masterBus;
    private EventInstance musicEventInstance;

    public static AudioMem instance { get; private set; }


    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("Found more than one AudioManager in scene");
        }
        instance = this;

        masterBus = RuntimeManager.GetBus("bus:/");
    }

    public void SetMusicParameter(string parameterName, float parameterValue)
    {
        musicEventInstance.setParameterByName(parameterName, parameterValue);
    }

    private void Update()
    {
        masterBus.setVolume(masterVolume);
    }

    public void playOneShot(EventReference sound)
    {
        RuntimeManager.PlayOneShot(sound);
    }

    public EventInstance CreateEventInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        return eventInstance;
    }
}
