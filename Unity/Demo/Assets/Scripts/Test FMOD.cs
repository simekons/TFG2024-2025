using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFMOD : MonoBehaviour
{
    public StudioEventEmitter emitter;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            emitter.Play();
        }
    }
}
