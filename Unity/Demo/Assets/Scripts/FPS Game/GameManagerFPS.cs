using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerFPS : MonoBehaviour
{

    public float gameDuration = 90f;
    private float timer;

    public GameObject winPanel;
    private bool gameEnded = false;

    // Start is called before the first frame update
    void Start()
    {
        timer = gameDuration;
        if (winPanel != null) winPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameEnded) return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            WinGame();
        }
    }

    void WinGame()
    {
        gameEnded = true;
        if (winPanel != null) winPanel.SetActive(true);

        // Puedes pausar el juego si quieres:
        Time.timeScale = 0f;

        // O cargar una escena de victoria:
        // SceneManager.LoadScene("WinScene");
    }
}
