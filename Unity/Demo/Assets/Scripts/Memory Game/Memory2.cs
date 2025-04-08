using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Memory2 : MonoBehaviour
{
    public int totalRounds = 5; // Número total de rondas del juego
    private int currentRound = 3; // Empieza con 3 sonidos
    private int currentIndex = 0;
    private bool isPlayingSequence = false;

    private List<int> sequence = new List<int>();
    private List<int> soundSequence = new List<int>();

    [SerializeField] private PlaySound[] emitters; // 0: Arriba, 1: Abajo, 2: Izquierda, 3: Derecha
    [SerializeField] private GameObject panel;

    public static Memory2 instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Memory instance in scene");
        }
        instance = this;
    }

    private void Start()
    {
        GenerateSequence();
        StartCoroutine(PlaySequence());
    }

    private void GenerateSequence()
    {
        sequence.Clear();
        soundSequence.Clear();

        for (int i = 0; i < totalRounds; i++)
        {
            sequence.Add(Random.Range(0, emitters.Length)); // Elige aleatoriamente una dirección

            int soundIndex = Random.Range(0, FModEvents.instance.sounds.Length); // Sonido aleatorio fijo
            soundSequence.Add(soundIndex);
        }
    }

    private IEnumerator PlaySequence()
    {
        isPlayingSequence = true;
        for (int i = 0; i < currentRound; i++)
        {
            int soundIndex = soundSequence[i]; // Usar la nota ya seleccionada
            emitters[sequence[i]].playSpecificSound(soundIndex);
            yield return new WaitForSeconds(1f);
        }
        isPlayingSequence = false;
        currentIndex = 0;
    }

    public void CheckInput(int input)
    {
        if (isPlayingSequence) return; // No permitir entrada durante la secuencia

        if (sequence[currentIndex] == input)
        {
            currentIndex++;
            if (currentIndex == currentRound)
            {
                if (currentRound < totalRounds)
                {
                    currentRound++;
                    StartCoroutine(PlaySequence());
                }
                else
                {
                    Debug.Log("Juego completado");
                    panel.SetActive(true);
                }
                currentIndex = 0;
            }
        }
        else
        {
            Debug.Log("Secuencia incorrecta, reiniciando");
            currentIndex = 0;
            StartCoroutine(PlaySequence());
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            CheckInput(0);
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            CheckInput(1);
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            CheckInput(2);
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            CheckInput(3);
    }
}
