using FMODUnity;
using FMOD;
using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

public class DSP_Test : MonoBehaviour
{
    int _windowSize = 512;
    FMOD.DSP_FFT_WINDOW _windowShape = FMOD.DSP_FFT_WINDOW.RECT;

    [SerializeField] private FMOD.Studio.EventInstance _event;
    [SerializeField] private FMODUnity.StudioEventEmitter _eventEmitter;
    FMODUnity.StudioEventEmitter[] _emitters;
    FMOD.DSP _dsp;
    FMOD.ChannelGroup _channel;
    FMOD.DSP_PARAMETER_FFT _fftparam;

    float[] _samples;

    private FMOD.System fmodSystem;

    void Start()
    {
        _emitters = Resources.FindObjectsOfTypeAll<FMODUnity.StudioEventEmitter>();
        prepare();
        _samples = new float[_windowSize];

        fmodSystem = RuntimeManager.CoreSystem;

        fmodSystem.createDSPByType(DSP_TYPE.FFT, out _dsp);
        _dsp.setParameterInt((int)DSP_FFT.WINDOWSIZE, 512);
    }
    void Update()
    {
        foreach(var emitter in _emitters)
        {
            if (emitter.IsPlaying())
            {
                analyzeFrequency(emitter);
            }
        }
        //getSpectrumData();
    }

    void analyzeFrequency(FMODUnity.StudioEventEmitter emitter)
    {
        EventInstance instance = emitter.EventInstance;
        RESULT result = instance.getChannelGroup(out ChannelGroup channelGroup);

        if (result != RESULT.OK || !channelGroup.hasHandle())
        {
            print($"El evento {emitter.gameObject.name} no tiene un canal de audio activo.");
            return; // Salimos de la función si no hay canal válido
        }

        bool dspAdded = false;
        int numDSPs;
        channelGroup.getNumDSPs(out numDSPs);

        for (int i = 0; i < numDSPs; i++)
        {
            channelGroup.getDSP(i, out DSP existingDSP);
            if (existingDSP.hasHandle() && existingDSP.Equals(_dsp))
            {
                dspAdded = true;
                break;
            }
        }

        if (!dspAdded)
        {
            print("Añadiendo DSP FFT.");
            channelGroup.addDSP(CHANNELCONTROL_DSP_INDEX.HEAD, _dsp);
        }

        uint length;
        System.IntPtr data;
        result = _dsp.getParameterData((int)DSP_FFT.SPECTRUMDATA, out data, out length);

        print($"GetParameterData result: {result}, data pointer: {data}");

        if (result == RESULT.OK && data != System.IntPtr.Zero)
        {
            try
            {
                DSP_PARAMETER_FFT fftData = (DSP_PARAMETER_FFT)System.Runtime.InteropServices.Marshal.PtrToStructure(data, typeof(DSP_PARAMETER_FFT));
                if (fftData.spectrum != null && fftData.spectrum[0] != null)
                {
                    float[] spectrumData = fftData.spectrum[0];
                    float peakFrequency = getPeakFrequency(spectrumData);
                    print($"Sonido en {emitter.gameObject.name}: Frecuencia Pico: {peakFrequency} Hz");
                }
                else
                {
                    print($"FFT no ha generado datos aún para {emitter.gameObject.name}.");
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
        _event.start();  // Inicia el sonido

        // Asegurar que el sonido se está reproduciendo antes de obtener el canal
        StartCoroutine(WaitForChannelGroup());
    }

    private IEnumerator WaitForChannelGroup()
    {
        yield return new WaitForSeconds(0.1f); // Esperar a que FMOD asigne un canal

        RESULT result = _event.getChannelGroup(out _channel);
        if (result != RESULT.OK || !_channel.hasHandle())
        {
            print("No se pudo obtener el ChannelGroup después de iniciar el evento.");
            yield break;
        }

        print("ChannelGroup obtenido correctamente, agregando DSP.");

        FMODUnity.RuntimeManager.CoreSystem.createDSPByType(FMOD.DSP_TYPE.FFT, out _dsp);
        _dsp.setParameterInt((int)FMOD.DSP_FFT.WINDOWTYPE, (int)_windowShape);
        _dsp.setParameterInt((int)FMOD.DSP_FFT.WINDOWSIZE, _windowSize * 2);

        _channel.addDSP(0, _dsp);
    }
}