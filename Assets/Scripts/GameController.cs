using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public TextMeshProUGUI TimeUI;

    private LeaderBoardManager leaderBoardManager;
    private float levelTime;
    private int levelNumber;
    private int numberOfSwings;
    public bool isRunning;

    [Header("Canvases")]
    public Canvas aliveCanvas;
    public Canvas deathCanvas;
    public Canvas winCanvas;





    // Start is called before the first frame update
    void Start()
    {
        leaderBoardManager = GetComponent<LeaderBoardManager>();
        ResetLevel();
        isRunning = true;
        levelNumber = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning)
        {
            levelTime += Time.deltaTime;
        }

        UpdateUI();
    }

    public void SetDeathCanvas()
    {
        deathCanvas.gameObject.SetActive(true);
        aliveCanvas.gameObject.SetActive(false);
    }

    void UpdateUI()
    {
        TimeUI.text = "Time: " + TimeSpan.FromSeconds(levelTime).ToString("mm\\:ss\\.fff"); 
    }

    public void AddSwing()
    {
        if (isRunning)
        {
            numberOfSwings++;
        }
    }

    public float getTime()
    {
        return levelTime;
    }

    void ResetLevel()
    {
        levelTime = 0;
        numberOfSwings = 0;
    }

    public void StartLevel()
    {
        isRunning= true;
    }
    public void StopLevel()
    {
        isRunning = false;
    }

    public void WinLevel()
    {
        StopLevel();
        LeaderBoardManager.SubmitScore(Mathf.FloorToInt(levelTime * 1000), numberOfSwings, levelNumber);
    }
}
