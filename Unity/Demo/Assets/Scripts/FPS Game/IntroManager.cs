using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroManager : MonoBehaviour
{

    public GameObject introPanel;
    private float realTimeElapsed = 0f;
    private bool counting = true;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0f; // Pausa el juego al inicio
        introPanel.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!counting) return;

        realTimeElapsed += Time.unscaledDeltaTime; // Tiempo real, aunque Time.timeScale sea 0

        if (realTimeElapsed >= 10f)
        {
            introPanel.SetActive(false);
            Time.timeScale = 1f;
            counting = false;
        }
    }
}
