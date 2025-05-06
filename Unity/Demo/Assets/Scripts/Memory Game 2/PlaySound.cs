using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class PlaySound : MonoBehaviour
{
    public void PlayRandomSound()
    {
        if (FModEvents.instance == null || FModEvents.instance.sounds.Length == 0)
        {
            Debug.LogWarning("No FMOD sounds configured!");
            return;
        }

        int index = Random.Range(0, FModEvents.instance.sounds.Length);
        EventReference sound = FModEvents.instance.sounds[index];

        if (!sound.IsNull)
        {
            FMOD.Studio.EventInstance instance = RuntimeManager.CreateInstance(sound);
            instance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
            instance.start();
            instance.release();
        }
    }
}
