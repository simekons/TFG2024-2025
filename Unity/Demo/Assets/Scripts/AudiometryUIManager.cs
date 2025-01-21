using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AudiometryUIManager : MonoBehaviour
{
    public static AudiometryUIManager instance { get; private set; }

    [SerializeField] private GameObject botonVol, botonStart, botonNext, botonExit, graphicImage;
    [SerializeField] private TMP_Text text, textVol;

    private bool left, end;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one AudioManager in scene");
        }
        instance = this;
    }

    private void Start()
    {
        left = true;
        end = true;
    }

    private void Update()
    {
        while (end)
            return;

        if (AudiometryManager.instance.GetSonidoActual() >= 5 && left == true)
        {
            botonVol.SetActive(false);
            botonNext.SetActive(false);
            botonStart.SetActive(true);
            end = true;

            AudiometryManager.instance.StopAudiometry();

            text.text = "Ahora vamos a la derecha";
            text.fontSize = 18;

            left = false;
        }
        else if (AudiometryManager.instance.GetSonidoActual() >= 5)
        {
            botonVol.SetActive(false);
            botonNext.SetActive(false);
            botonExit.SetActive(true);
            end = true;

            AudiometryManager.instance.StopAudiometry();
            AudiometryManager.instance.SaveResultados();
            graphicImage.SetActive(true);
        }
    }

    public void EmpezarAudiometria()
    {
        botonExit.SetActive(false);
        botonVol.SetActive(true);
        botonNext.SetActive(true);
        botonStart.SetActive(false);
        end = false;

        if (left)
            text.text = "<- Izquierda";
        else
            text.text = "Derecha ->";

        text.fontSize = 36;

        AudiometryManager.instance.StartAudiometry(left);
    }

    public void SubirVolumen()
    {
        int vol = AudiometryManager.instance.SubirVolumen();

        if (vol >= 130)
            textVol.text = "MAX";
    }
}