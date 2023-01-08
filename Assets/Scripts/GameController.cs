using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public TextMeshProUGUI TimeUI;

    private LeaderBoardManager leaderBoardManager;
    private float levelTime;
    private int levelNumber;
    private int numberOfSwings;
    public bool isRunning;
    public bool isTitleScreen;
    private bool clickedTitle;

    [Header("Canvases")]
    public Canvas aliveCanvas;
    public Canvas deathCanvas;
    public Canvas winCanvas;
    public Canvas titleCanvas;


    // Start is called before the first frame update
    void Start()
    {
        leaderBoardManager = GetComponent<LeaderBoardManager>();
        ResetLevel();
        isRunning = false;
        levelNumber = SceneManager.GetActiveScene().buildIndex;
        isTitleScreen = true;
        clickedTitle = false;
        Time.timeScale = 0;

        titleCanvas.transform.Find("Image").gameObject.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = SceneManager.GetActiveScene().name;

        aliveCanvas.gameObject.SetActive(false);
        deathCanvas.gameObject.SetActive(false);
        winCanvas.gameObject.SetActive(false);
        titleCanvas.gameObject.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning)
        {
            levelTime += Time.deltaTime;
        }

        if (Input.anyKey)
        {
            clickedTitle = true;
        }
        if(clickedTitle && !Input.anyKey && isTitleScreen)
        {
            isRunning = true;
            isTitleScreen = false;
            clickedTitle = false;
            titleCanvas.gameObject.SetActive(false);
            aliveCanvas.gameObject.SetActive(true);
            Time.timeScale = 1;

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
        leaderBoardManager.InitializeEmptyLeaderboard();
        LeaderBoardManager.SubmitScore(Mathf.FloorToInt(levelTime * 1000), numberOfSwings, levelNumber);
        leaderBoardManager.UpdateLeaderboardUI(levelNumber);
        StartCoroutine(UpdateScoreboard());
    }


    IEnumerator UpdateScoreboard()
    {
        yield return new WaitForSeconds(1);
        leaderBoardManager.UpdateLeaderboardUI(levelNumber);
    }
}
