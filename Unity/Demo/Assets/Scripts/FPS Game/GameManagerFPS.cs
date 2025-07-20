using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerFPS : MonoBehaviour
{

    public float gameDuration = 5f;
    private float timer;

    public GameObject winPanel;
    public GameObject menuPanel;
    public float timeBeforeMenu = 3f;

    private bool gameEnded = false;
    private float winTimer = 0f;
    private bool showingWinPanel = false;

    // Start is called before the first frame update
    void Start()
    {
        timer = gameDuration;
        if (winPanel != null) winPanel.SetActive(false);
        if (menuPanel != null) menuPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameEnded)
        {
            if (showingWinPanel)
            {
                winTimer += Time.deltaTime;
                if (winTimer >= timeBeforeMenu)
                {
                    if (winPanel != null) winPanel.SetActive(false);
                    if (menuPanel != null) menuPanel.SetActive(true);

                    Time.timeScale = 0f;
                    showingWinPanel = false;
                }
            }
            return;
        }

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

        Cursor.lockState = CursorLockMode.None;
        showingWinPanel = true;
    }
}
