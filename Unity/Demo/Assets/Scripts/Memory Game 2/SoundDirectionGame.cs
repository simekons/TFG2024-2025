using System.Collections;
using System.Collections.Generic;
using Telemetry;
using UnityEngine;

public class SoundDirectionGame : MonoBehaviour
{

    [SerializeField] private PlaySound[] emitters; // 0: Izquierda, 1: Centro, 2: Derecha
    [SerializeField] private GameObject exitPanel;
    [SerializeField] private float initialDelay = 1.5f;
    [SerializeField] private float minDelay = 0.4f;
    [SerializeField] private float delayDecrease = 0.2f;

    private Queue<(string message, float duration)> messageQueue = new Queue<(string, float)>();
    private List<int> sequence = new List<int>();

    private int currentInputIndex = 0;
    private int sequenceLength = 1;
    private int roundsAtCurrentLength = 0;
    private int playIndex = 0;
    private float currentDelay;
    private float nextPlayTime;
    private float messageTimer = 0f;
    private bool isPlayingSequence = false;
    private bool justFailed = false;

    private int failed = 0;
    private int correct = 0;

    private enum GameState { PlayingSequence, WaitingForInput, Idle, ShowingMessage }
    private GameState state = GameState.Idle;

    private void Start()
    {
        currentDelay = initialDelay;
        Tracker.getInstance().startGameMemory();
        startNewSequence();
    }

    private void startNewSequence()
    {
        sequence.Clear();
        for (int i = 0; i < sequenceLength; i++)
            sequence.Add(Random.Range(0, emitters.Length));

        playIndex = 0;

        handleMessages();

        messageTimer = 0f;
        state = GameState.ShowingMessage;

        nextPlayTime = Time.time + 1f; // Retraso antes de la primera ronda
        isPlayingSequence = true;
        //state = GameState.PlayingSequence;
    }

    private void Update()
    {
        switch (state)
        {
            case GameState.PlayingSequence:
                handleSequencePlayback();
                break;

            case GameState.WaitingForInput:
                handleInput();
                break;

            case GameState.ShowingMessage:
                if (messageQueue.Count > 0)
                {
                    var current = messageQueue.Peek();
                    messageTimer += Time.deltaTime;
                    UIManager.instance.ShowMessage(current.message);

                    if (messageTimer >= current.duration)
                    {
                        messageQueue.Dequeue();
                        messageTimer = 0f;
                    }
                }
                else
                {
                    UIManager.instance.ClearMessage();
                    nextPlayTime = Time.time + 1f;
                    state = GameState.PlayingSequence;
                }
                break;
        }
    }

    private void handleSequencePlayback()
    {
        if (Time.time >= nextPlayTime && playIndex < sequence.Count)
        {
            emitters[sequence[playIndex]].PlayRandomSound();
            Debug.Log(sequence[playIndex]);
            playIndex++;
            nextPlayTime = Time.time + currentDelay;
        }

        if (playIndex >= sequence.Count)
        {
            isPlayingSequence = false;
            currentInputIndex = 0;
            state = GameState.WaitingForInput;
        }
    }

    private void handleInput()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            checkInput(0);
        if (Input.GetKeyDown(KeyCode.S))
            checkInput(1);
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            checkInput(2);
    }

    private void checkInput(int input)
    {
        if (sequence[currentInputIndex] == input)
        {
            Debug.Log("SEQUENCE: " + sequence.Count);
            currentInputIndex++;
            if (currentInputIndex == sequence.Count)
                successfulSequence();
        }
        else
            wrongSequence();
    }

    private void successfulSequence()
    {
        correct++;
        Tracker.getInstance().sequenceCorrectEvent(correct);
        Debug.Log("Correcto!");

        roundsAtCurrentLength++;

        if(sequenceLength == 1)
        {
            sequenceLength = 2;
            roundsAtCurrentLength = 0;
        }
        else if(roundsAtCurrentLength == 2)
        {
            sequenceLength++;
            roundsAtCurrentLength = 0;
        }
        else
            currentDelay = Mathf.Max(minDelay, currentDelay - delayDecrease);

        if (sequenceLength >= 6)
        {
            endGame();
            return;
        }

        state = GameState.Idle;
        startNewSequence();
    }

    private void wrongSequence()
    {
        failed++;
        Debug.Log("Fallaste, reiniciando secuencia");
        Tracker.getInstance().sequenceFailedEvent(failed);
        justFailed = true;
        state = GameState.Idle;
        startNewSequence();
    }

    private void handleMessages()
    {
        messageQueue.Clear();

        if(justFailed)
        {
            messageQueue.Enqueue(("¡Has fallado! Vamos a repetir otra secuencia con la misma dificultad", 3f));
            justFailed = false;
        }
        else if (sequenceLength == 1 && roundsAtCurrentLength == 0)
        {
            messageQueue.Enqueue(("Así se juega: \nEscucha de dónde viene el sonido y pulsa la tecla correcta (A / S / D)", 5f));
            messageQueue.Enqueue(("En breve, escucharás un sonido", 3f));
        }
        else if (roundsAtCurrentLength == 0)
            messageQueue.Enqueue(("Ahora aumentaremos en 1 los sonidos que escucharás", 3f));
        else
        {
            if(sequenceLength > 1)
                messageQueue.Enqueue(("Ahora incrementaremos la velocidad", 3f));
        }
    }

    private void endGame()
    {
        if (exitPanel != null)
            exitPanel.SetActive(true);

        state = GameState.ShowingMessage;
        Tracker.getInstance().maxSequenceEvent(sequenceLength);
    }
}
