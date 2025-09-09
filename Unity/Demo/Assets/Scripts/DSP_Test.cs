using FMODUnity;
using FMOD;
using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Telemetry;

public class DSP_Test : MonoBehaviour
{
    int _windowSize = 512;
    FMOD.DSP_FFT_WINDOW _windowShape = FMOD.DSP_FFT_WINDOW.RECT;

    [SerializeField] private FMOD.Studio.EventInstance _event;
    [SerializeField] private FMODUnity.StudioEventEmitter _eventEmitter;
    FMODUnity.StudioEventEmitter[] _emitters;
    FMOD.DSP _dsp;
    FMOD.ChannelGroup _channel;

    private FMOD.System fmodSystem;

    void Start()
    {
        _emitters = Resources.FindObjectsOfTypeAll<FMODUnity.StudioEventEmitter>();
        fmodSystem = RuntimeManager.CoreSystem;

        prepare();
    }

    void Update()
    {
        foreach (var emitter in _emitters)
        {
            if (emitter.IsPlaying())
            {
                analyzeFrequency(emitter);
            }
        }
    }

    void analyzeFrequency(FMODUnity.StudioEventEmitter emitter)
    {
        EventInstance instance = emitter.EventInstance;
        RESULT result = instance.getChannelGroup(out ChannelGroup channelGroup);

        if (result != RESULT.OK || !channelGroup.hasHandle())
        {
            print($"El evento {emitter.gameObject.name} no tiene un canal de audio activo.");
            return;
        }

        // Verificar que el DSP está inicializado
        if (!_dsp.hasHandle())
        {
            print("DSP no inicializado. Creando un nuevo DSP..-");
            fmodSystem.createDSPByType(DSP_TYPE.FFT, out _dsp);
            _dsp.setParameterInt((int)DSP_FFT.WINDOWSIZE, _windowSize * 2);
        }

        // Verificar si el DSP ya está agregado
        int numDSPs;
        channelGroup.getNumDSPs(out numDSPs);
        bool found = false;

        for (int i = 0; i < numDSPs; i++)
        {
            channelGroup.getDSP(i, out DSP existingDSP);
            if (existingDSP.hasHandle() && existingDSP.Equals(_dsp))
            {
                found = true;
                break;
            }
        }

        if (!found)
        {
            print($"El DSP FFT no está vinculado al canal del evento {emitter.gameObject.name}. Intentando agregarlo...");

            RESULT addResult = channelGroup.addDSP(0, _dsp);

            if (addResult != RESULT.OK)
            {
                print($"Error al agregar el DSP FFT al canal: {addResult}");
                return;
            }

            print($"DSP FFT agregado correctamente al canal del evento {emitter.gameObject.name} ");
        }

        // Obtener datos FFT
        uint length;
        System.IntPtr data;
        result = _dsp.getParameterData((int)DSP_FFT.SPECTRUMDATA, out data, out length);
        result = _dsp.getParameterData((int)DSP_FFT.SPECTRUMDATA, out data, out length);

        print($"GetParameterData result: {result}, data pointer: {data}");

        if (result == RESULT.OK && data != System.IntPtr.Zero)
        {
            try
            {
                DSP_PARAMETER_FFT fftData = (DSP_PARAMETER_FFT)System.Runtime.InteropServices.Marshal.PtrToStructure(data, typeof(DSP_PARAMETER_FFT));

                if (fftData.numchannels == 0 || fftData.spectrum == null || fftData.spectrum.Length == 0 || fftData.spectrum[0] == null)
                {
                    print($"FFT no ha generado datos aún o numchannels es 0 para {emitter.gameObject.name}. Reiniciando el DSP...");

                    // Intentar reasignar el DSP si el canal cambió
                    RESULT reassignResult = instance.getChannelGroup(out ChannelGroup newChannelGroup);
                    if (reassignResult == RESULT.OK && newChannelGroup.hasHandle())
                    {
                        print($"Reasignando DSP al nuevo ChannelGroup para {emitter.gameObject.name}.");
                        newChannelGroup.addDSP(0, _dsp);
                    }
                    else
                    {
                        print($"No se pudo reasignar DSP para {emitter.gameObject.name}, error: {reassignResult}");
                    }

                    return; // Salimos y esperamos al siguiente frame
                }

                // Ahora continuamos con el procesamiento normal del espectro
                float[] spectrumData = fftData.spectrum[0];

                if (spectrumData.Length > 0)
                {
                    float maxAmplitude = 0;
                    foreach (float value in spectrumData)
                    {
                        if (value > maxAmplitude) maxAmplitude = value;
                    }

                    if (maxAmplitude > 0)
                    {
                        float peakFrequency = getPeakFrequency(spectrumData);
                        print($"Sonido en {emitter.gameObject.name}: Frecuencia Pico: {peakFrequency} Hz");
                    }
                    else
                    {
                        print($"FFT recibió datos, pero el espectro solo contiene ceros para {emitter.gameObject.name}.");
                    }
                }
                else
                {
                    print($"FFT ha procesado el audio, pero el espectro aún está vacío para {emitter.gameObject.name}.");
                }
            }
            catch (System.Exception ex)
            {
                print($"Error al convertir datos FFT: {ex.Message}");
            }
        }
        else
        {
            print($"No se pudo obtener datos FFT para {emitter.gameObject.name}, error: {result}");
        }

    }




    float getPeakFrequency(float[] spectrum)
    {
        int maxIndex = 0;
        float maxValue = 0;

        for (int i = 0; i < spectrum.Length; i++)
        {
            if (spectrum[i] > maxValue)
            {
                maxValue = spectrum[i];
                maxIndex = i;
            }
        }

        float frequency = maxIndex * (48000 / 2) / spectrum.Length;
        return frequency;
    }

    private void prepare()
    {
        if (_emitters.Length == 0)
        {
            print("No se encontraron StudioEventEmitters en la escena.");
            return;
        }

        _event = _emitters[0].EventInstance;

        if (!_event.isValid())
        {
            print("El EventInstance no es válido.");
            return;
        }

        print("Inicializando evento FMOD.");
        _event.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject.transform));
        _event.start();

        StartCoroutine(WaitForChannelGroup());
    }

    private IEnumerator WaitForChannelGroup()
    {
        print("Esperando a que FMOD asigne un canal...");

        float waitTime = 0f;
        while (waitTime < 1f) // Esperamos hasta 1 segundo
        {
            RESULT result = _event.getChannelGroup(out _channel);
            if (result == RESULT.OK && _channel.hasHandle())
            {
                print("ChannelGroup obtenido correctamente, agregando DSP.");
                break;
            }

            waitTime += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        if (!_channel.hasHandle())
        {
            print("No se pudo obtener el ChannelGroup después de 1 segundo.");
            yield break;
        }

        // Asegurar que el DSP está creado
        RESULT dspResult = FMODUnity.RuntimeManager.CoreSystem.createDSPByType(FMOD.DSP_TYPE.FFT, out _dsp);
        if (dspResult != RESULT.OK)
        {
            print($"Error al crear el DSP FFT: {dspResult}");
            yield break;
        }

        _dsp.setParameterInt((int)FMOD.DSP_FFT.WINDOWTYPE, (int)_windowShape);
        _dsp.setParameterInt((int)FMOD.DSP_FFT.WINDOWSIZE, _windowSize * 2);

        // 🔹 Agregamos el DSP al canal
        RESULT addResult = _channel.addDSP(0, _dsp);
        if (addResult != RESULT.OK)
        {
            print($"Error al agregar el DSP FFT al canal: {addResult}");
        }
        else
        {
            print("DSP FFT agregado correctamente");

        }
    }
}
