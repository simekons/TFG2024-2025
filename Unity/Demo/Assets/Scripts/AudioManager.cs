using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;
using FMODUnity;
using FMOD.Studio;
using UnityEngine.SceneManagement;

/// <summary>
/// AudioManager handles all the changes to the sounds in the game, scene by scene.
/// </summary>

public class AudioManager : MonoBehaviour
{
    public Dictionary<int, FMODUnity.StudioEventEmitter> events;
    public float[] audiometry;

    public static AudioManager instance { get; private set; }

    /// <summary>
    /// Initializes the Dictionaries that will be used and the instance.
    /// </summary>
    private void Awake()
    {
        events = new Dictionary<int, FMODUnity.StudioEventEmitter>();
        initializeAudiometry();

        if (instance != null)
        {
            print("There's already an instance of AudioManager");
        }
        instance = this;
        DontDestroyOnLoad(this);
    }

    /// <summary>
    /// Empty constructor.
    /// </summary>
    public AudioManager() { }

    /// <summary>
    /// Setup of all the sounds in the current scene and their volume.
    /// </summary>
    void Start() {
        if (SceneManager.GetActiveScene().name != "AUDIOMETRY") {
            // We first find all the objects in the current scene.
            List<GameObject> objectsInScene = new List<GameObject>();
            Scene scene = SceneManager.GetActiveScene();
            scene.GetRootGameObjects(objectsInScene);

            // Then we grab a reference to the FMOD Studio Event Emitter component they have (if they have it).
            for (int i = 0; i < objectsInScene.Count; i++) {
                if (objectsInScene[i].GetComponentInChildren<FMODUnity.StudioEventEmitter>()) {
                    events.Add(i, objectsInScene[i].GetComponentInChildren<FMODUnity.StudioEventEmitter>());
                }
            }

            // And we update the volume of the sound by the value of the audiometry.
            updateVolume();
        } 
    }

    private void Update()
    {
        for (int i = 0; i < 8; i++)
        {
            print("LA AUDIOMETRIA ES : " + i + " " + audiometry[i]);
        }
    }

    /// <summary>
    /// Updates the volume of the sounds according to the value received after the audiometry.
    /// </summary>
    private void updateVolume() {
        float value = 1.0f;
        for (int i = 0; i < events.Count; ++i) {
            events[i].EventInstance.setParameterByName("db", value);

            // ---------------------- It needs to check the frequency of the sound before updating it. ----------------------
        }
    }

    /// <summary>
    /// Method associated to the audiometry canvas. It will update the values of the Dictionary according to the answers of the user.
    /// </summary>
    /// <param name="index"> Frequency index. </param>
    /// <param name="volume"> Volume needed to hear the sound. </param>
    public void answer(int index, float volume) {

        audiometry[index] = volume;
    }

    private void initializeAudiometry() {
        audiometry = new float[8];
    }
}

//PARAMETER_DESCRIPTION parameter;
//emitter.EventDescription.getParameterDescriptionByName("db", out parameter);
//print("PARAMETER_ID 1: " + parameter.id + " VALUE: " + parameter.defaultvalue);
//FMODUnity.RuntimeManager.StudioSystem.setParameterByName("db", 0.5f);

//print("PARAMETER_ID 2: " + parameter.name + " VALUE: " + parameter.maximum);

//print("VALID : " + emitter.EventInstance.isValid());

//emitter.EventInstance.getChannelGroup(out ChannelGroup group);
//emitter.EventDescription.getUserPropertyByIndex(0, out USER_PROPERTY p);

//FMOD.Studio.System.create(out FMOD.Studio.System system);
//system.getSoundInfo("event:/event_1", out SOUND_INFO info);
//print("INFO: " + info.exinfo.defaultfrequency);
//group.getChannel(0, out Channel channel);

//channel.getFrequency(out float frequency);

//print("FRECUENCIA: " + frequency);