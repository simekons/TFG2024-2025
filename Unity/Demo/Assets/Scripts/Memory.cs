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

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one AudioManager in scene");
        }
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        randomNumbers = new List<int>();
        for (int i = 0; i < count; i++)
        {
            int randomNumber = Random.Range(0, 5);
            randomNumbers.Add(randomNumber);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!recorded)
        {
            buttonsInteract(false);
            Invoke("buttonsInteractT", countMem);
            if (countMem <= count)
            {
                for (int i = 0; i < countMem; i++)
                {
                    buttons[randomNumbers[i]].GetComponent<PlaySound>().playInTime(i);
                }
                recorded = true;
            }
            else
            {
                buttonsInteract(false);
                panel.SetActive(true);
            }
        }
    }

    public void takeCombination(int num)
    {
        if (randomNumbers[correctButton] == num)
        {
            correctButton++;
        }
        else
        {
            correctButton = 0;
        }
        if (correctButton == countMem)
        {
            Invoke("nextCode", 0.8f);
            correctButton = 0;
        }
    }

    private void nextCode()
    {
        recorded = false;
        countMem++;
    }

    private void buttonsInteractT()
    {
        buttonsInteract(true);
    }

    private void buttonsInteract(bool interact)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].GetComponent<Button>().interactable = interact;
        }
    }

    public void startGame()
    {
        recorded = false;
    }
}
