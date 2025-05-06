using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Memory : MonoBehaviour
{
    public int count = 5;

    private List<int> randomNumbers;
    private int countMem = 3;
    private int correctButton = 0;
    private bool recorded = true;
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private GameObject panel;

    public static Memory instance = null;

    /// <summary>
    /// Instance the Memory manager.
    /// </summary>
    private void Awake()
    {
        if (instance != null) 
            Debug.LogError("Found more than one AudioManager in scene");
        
        instance = this;
    }

    /// <summary>
    /// Initialize the random sequence.
    /// </summary>
    void Start()
    {
        randomNumbers = new List<int>();
        for (int i = 0; i < count; i++) {
            int randomNumber = Random.Range(0, 5);
            randomNumbers.Add(randomNumber);
        }
    }

    /// <summary>
    /// Keeps track of the current sequence input.
    /// </summary>
    void Update() {
        if (!recorded) {
            buttonsInteract(false);
            Invoke("buttonsInteractT", countMem);

            if (countMem <= count) {
                for (int i = 0; i < countMem; i++) {
                    //buttons[randomNumbers[i]].GetComponent<PlaySound>().playInTime(i);
                }
                recorded = true;
            }
            else {
                buttonsInteract(false);
                panel.SetActive(true);
            }
        }
    }

    /// <summary>
    /// Check if the sequence input is correct.
    /// </summary>
    public void takeCombination(int num)
    {
        if (randomNumbers[correctButton] == num)
            correctButton++;
        else {
            correctButton = 0;
            Invoke("restart", 1.0f);
        }
        if (correctButton == countMem) {
            Invoke("nextCode", 0.8f);
            correctButton = 0;
        }
    }

    private void restart()
    {
        recorded = false;
    }

    /// <summary>
    /// Enlarges the sequence by one.
    /// </summary>
    private void nextCode()
    {
        restart();
        countMem++;
    }

    /// <summary>
    /// Button interaction.
    /// </summary>
    private void buttonsInteractT()
    {
        buttonsInteract(true);
    }

    /// <summary>
    /// Button interacion.
    /// </summary>
    private void buttonsInteract(bool interact)
    {
        for (int i = 0; i < buttons.Length; i++)
            buttons[i].GetComponent<Button>().interactable = interact;
    }

    /// <summary>
    /// Starts the game.
    /// </summary>
    public void startGame()
    {
        restart();
    }
}
