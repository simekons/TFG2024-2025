using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    private Toggle button;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Toggle>();

        button.onValueChanged.AddListener(GameManager.Instance.ApplyAudiometryToGlobalFrequencies);
    }
}
