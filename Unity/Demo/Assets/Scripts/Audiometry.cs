using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Audiometry : MonoBehaviour
{
    /// <summary>
    /// Sliders that are going to calibrate the volume of the tones.
    /// </summary>
    public Slider[] m_sliders;

    /// <summary>
    /// Name of the scene that's going next.
    /// </summary>
    [SerializeField]
    private string m_scene;

    /// <summary>
    /// The Event Emitter of the tones.
    /// </summary>
    [SerializeField]
    private FMODUnity.StudioEventEmitter m_tones;

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
            // Debug.Log("SLIDER DE " + aux + " TIENE EL VALOR DE: " + m_sliders[i].value);
            aux *= 2;
            AudioManager.instance.answer(i, m_sliders[i].value);
        }
    }

    /// <summary>
    /// Method that's going to switch between the different tones.
    /// </summary>
    /// <param name="index"></param>
    public void playSound(int index)
    {
        m_tones.EventInstance.setParameterByName("hz", index);
        m_tones.EventInstance.setParameterByName("db", calculateVolume(m_sliders[index].value));
        m_tones.Stop();
        m_tones.Play();

        // print("EL VALOR ES: " + value);
        // m_tones.EventInstance.getParameterByName("hz", out float value);
    }

    /// <summary>
    /// Given the sound, its frequency and the volume needed to hear it, we calculate the resulting volume.
    /// </summary>
    private float calculateVolume(float volume)
    {
        volume *= 70;
        volume -= 60; 
        return volume;
    }

    /// <summary>
    /// Method that changes the current scene to the next.
    /// </summary>
    public void changeScene()
    {
        m_tones.Stop();
        SceneManager.LoadScene(m_scene);
    }
}
