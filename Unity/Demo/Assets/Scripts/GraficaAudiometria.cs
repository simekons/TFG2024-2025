using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PythonAudiograma : MonoBehaviour
{
    public RawImage outputImage; // UI RawImage para mostrar la gráfica

    void Start()
    {
        // Ruta completa del script y los archivos
        string scriptPath = @"D:\Desktop\TFG2023-2024\Grafica\main.py";
        string dataPath = @"D:\Desktop\TFG2023-2024\Grafica\datos.txt";
        string outputPath = @"D:\Desktop\TFG2023-2024\Grafica\audiogram.png";

        // Configurar el ProcessStartInfo
        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = "python", // Cambiar a "python3" si es necesario
            Arguments = $"\"{scriptPath}\"",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        Process process = Process.Start(psi);
        string output = process.StandardOutput.ReadToEnd(); // Capturar salida estándar
        string error = process.StandardError.ReadToEnd();   // Capturar errores
        process.WaitForExit();

        if (!string.IsNullOrEmpty(error))
        {
            UnityEngine.Debug.LogError($"Error al ejecutar el script: {error}");
            return;
        }

        UnityEngine.Debug.Log($"Script ejecutado correctamente: {output}");

        // Verificar si el archivo de imagen se generó correctamente
        if (File.Exists(outputPath))
        {
            byte[] imageBytes = File.ReadAllBytes(outputPath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageBytes);

            outputImage.texture = texture; // Mostrar la imagen en la UI
        }
        else
        {
            UnityEngine.Debug.LogError($"No se encontró el archivo de salida en {outputPath}");
        }
    }
}
