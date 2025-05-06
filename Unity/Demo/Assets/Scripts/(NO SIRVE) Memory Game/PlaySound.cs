//using UnityEngine;
//using UnityEngine.UI;
//using FMODUnity;

//public class PlaySound : MonoBehaviour
//{
//    public int orden;

//    private Button buttonToPress;
//    private ColorBlock colorBlock;

//    private bool isButtonPressed = false;

//    private float pressTime = 0.0f;
//    private float timer = 0.0f;

//    [field: Header("Sound")]
//    [field: SerializeField] public EventReference sound { get; private set; }

//    void Start()
//    {
//        buttonToPress = GetComponent<Button>();
//        colorBlock = buttonToPress.colors;
//    }

//    void Update()
//    {
//        if (isButtonPressed)
//        {
//            timer += Time.deltaTime;

//            if (timer >= pressTime)
//            {
//                ReleaseButton();
//            }
//        }
//    }


//    void ReleaseButton()
//    {
//        colorBlock.disabledColor = colorBlock.normalColor;
//        buttonToPress.colors = colorBlock;

//        isButtonPressed = false;

//        timer = 0.0f;
//        pressTime = 0.0f;
//    }

//    public void playInTime(int time)
//    {
//        Invoke("play", time);
//    }

//    private void play()
//    {
//        RuntimeManager.PlayOneShot(sound);

//        colorBlock.disabledColor = colorBlock.pressedColor;
//        buttonToPress.colors = colorBlock;

//        isButtonPressed = true;

//        pressTime += 1.0f;
//    }

//    public void playButton()
//    {
//        RuntimeManager.PlayOneShot(sound);
//        Memory.instance.takeCombination(orden);
//    }

//    public void playSpecificSound(int soundIndex)
//    {
//        if(soundIndex >= 0 && soundIndex < FModEvents.instance.sounds.Length)
//            RuntimeManager.PlayOneShot(FModEvents.instance.sounds[soundIndex], transform.position);
//    }
//}
