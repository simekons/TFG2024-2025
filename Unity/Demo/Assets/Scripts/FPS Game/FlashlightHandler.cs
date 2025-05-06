using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightHandler : MonoBehaviour
{

    public Light flashlight;           // Referencia a la luz
    public KeyCode toggleKey = KeyCode.F; // Tecla para activar/desactivar

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            flashlight.enabled = !flashlight.enabled;
        }
    }
}
