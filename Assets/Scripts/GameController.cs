using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public TextMeshProUGUI TimeUI;

    private float levelTime;
    private int levelNumber;
    private float numberOfSwings;
    private bool isRunning;



    // Start is called before the first frame update
    void Start()
    {
        ResetLevel();
        isRunning = true;
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
}
