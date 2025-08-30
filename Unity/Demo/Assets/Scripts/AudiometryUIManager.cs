using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AudiometryUIManager : MonoBehaviour
{
    public static AudiometryUIManager instance { get; private set; }

    [SerializeField] private GameObject botonVol, botonStart, botonNext, botonExit, graphicImage, ear, leftCross, rightCross;
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
            ear.SetActive(false);
            leftCross.SetActive(false);
            rightCross.SetActive(false);

            botonStart.SetActive(true);
            end = true;

            AudiometryManager.instance.StopAudiometry();

            text.text = "Ahora vamos a la derecha";
            text.alignment = TextAlignmentOptions.Center;

            left = false;
        }
        else if (AudiometryManager.instance.GetSonidoActual() >= 5)
        {
            botonVol.SetActive(false);
            botonNext.SetActive(false);
            ear.SetActive(false);
            leftCross.SetActive(false);
            rightCross.SetActive(false);
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
        ear.SetActive(true);
        end = false;

        if(left)
            leftCross.SetActive(true);
        else
            rightCross.SetActive(true);

            text.text = "";

        AudiometryManager.instance.StartAudiometry(left);
    }

    public void SubirVolumen()
    {
        int vol = AudiometryManager.instance.SubirVolumen();

        if (vol >= 130)
            textVol.text = "MAX";
    }

    public void SetTextVol()
    {
        textVol.text = "+ Volumen";
    }
}