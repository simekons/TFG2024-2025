using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class RunPythonScript : MonoBehaviour
{
    private void OnEnable()
    {
        // Ruta del script de Python
        string pythonScriptPath = "Assets/Python/main.py";
        string pythonExePath = "python";

        // Ejecutar el script
        RunPython(pythonExePath, pythonScriptPath);

        // Cargar la imagen generada
        LoadGeneratedImage();
    }

    void RunPython(string pythonExePath, string scriptPath)
    {
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = pythonExePath,
            Arguments = scriptPath,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };

        using (Process process = Process.Start(startInfo))
        {
            process.WaitForExit(); // Espera a que el script termine
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            if (!string.IsNullOrEmpty(error))
            {
                UnityEngine.Debug.LogError($"Python Error: {error}");
            }
            else
            {
                UnityEngine.Debug.Log($"Python Output: {output}");
            }
        }
    }

    void LoadGeneratedImage()
    {
        // Ruta de la imagen generada
        string imagePath = "Assets/Python/audiogram.png";

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
