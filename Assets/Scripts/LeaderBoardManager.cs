using LootLocker.Requests;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Profiling.Memory.Experimental;
using UnityEngine.SocialPlatforms.Impl;

public class LeaderBoardManager : MonoBehaviour
{
    private bool didLogin;


    // Start is called before the first frame update
    void Start()
    {
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (!response.success)
            {
                didLogin = false;
                return;
            }
            didLogin = true;


            SetPlayerName("Natanor");
            SubmitScore(999, 3, "1");
        });       
    }


    public void SetPlayerName(string name)
    {
        if (didLogin)
        {
            LootLockerSDKManager.SetPlayerName(name, (response) =>
            {
                if (response.success)
                {
                    Debug.Log("Successfully set player name");
                }
                else
                {
                    Debug.Log("Error setting player name");
                }
            });
        }
    }

    public void SubmitScore(int timeTakenMs, int numberOfShots, string level)
    {
        if (didLogin)
        {

            LootLockerSDKManager.SubmitScore("", timeTakenMs, level, numberOfShots.ToString(), (response) =>
            {
                if (response.statusCode == 200)
                {
                    Debug.Log("Successful");
                }
                else
                {
                    Debug.Log("failed: " + response.Error);
                }
            });
        }
    }

    public void getScores(string level)
    {
        int count = 50;

        LootLockerSDKManager.GetScoreList(level, count, 0, (response) =>
        {
            if (response.statusCode == 200)
            {
                Debug.Log("Successful");
            }
            else
            {
                Debug.Log("failed: " + response.Error);
            }
        });
    }

}
