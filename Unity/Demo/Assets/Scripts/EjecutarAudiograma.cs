using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class EjecutarAudiograma : MonoBehaviour
{
    private Process proceso;
    private bool procesoTerminado = false;

    private void OnEnable()
    {
        Ejecutar();
    }

    private void Ejecutar()
    {
        // Ruta del ejecutable
        string rutaExe = Path.Combine(Application.streamingAssetsPath, "Audiograma", "audiogramaGenerator.exe");

        // Ruta del archivo de datos
        string rutaDatos = Application.streamingAssetsPath + "/Audiograma";

        proceso = new Process();
        proceso.StartInfo.FileName = rutaExe;

        // Pasamos la ruta de datos como argumento
        proceso.StartInfo.Arguments = $"\"{rutaDatos}\"";

        proceso.StartInfo.UseShellExecute = false;
        proceso.StartInfo.CreateNoWindow = true;

        proceso.EnableRaisingEvents = true;
        proceso.Exited += Proceso_Terminado;

        proceso.Start();

        UnityEngine.Debug.Log("Proceso iniciado: " + rutaExe + " con datos: " + rutaDatos);
    }


    private void Proceso_Terminado(object sender, EventArgs e)
    {
        // Esto ocurre en otro hilo
        procesoTerminado = true;
    }

    private void Update()
    {
        if (procesoTerminado)
        {
            procesoTerminado = false;
            MiFuncionCuandoTermine();
        }
    }

    private void MiFuncionCuandoTermine()
    {
        // Ruta de la imagen generada
        string imagePath = Path.Combine(Application.streamingAssetsPath, "Audiograma", "audiogram.png");

        if (File.Exists(imagePath))
        {
            byte[] imageData = File.ReadAllBytes(imagePath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageData);

            // Asignar la textura a la imagen de la UI
            GetComponent<RawImage>().texture = texture;
        }
        else
        {
            UnityEngine.Debug.LogError("La imagen generada no se encontró en la ruta especificada.");
        }
    }
}
