using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Audiometry : MonoBehaviour
{
    public Slider[] m_sliders;

    /// <summary>
    /// Assign a delegate method to each one of the sliders.
    /// </summary>
    public void Start() {
        for (int i = 0; i < m_sliders.Length; i++) {
            m_sliders[i].onValueChanged.AddListener(delegate { sliderValueChanged(); });
        }
    }

    /// <summary>
    /// Method that's invoked whenever the slider value changes.
    /// </summary>
    public void sliderValueChanged() {
        int aux = 125;
        for (int i = 0; i < m_sliders.Length; i++) {
            Debug.Log("SLIDER DE " + aux + " TIENE EL VALOR DE: " + m_sliders[i].value);
            aux *= 2;
            AudioManager.instance.answer(i, m_sliders[i].value);
        }
    }
}
