using LootLocker.Requests;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Profiling.Memory.Experimental;
using UnityEngine.SocialPlatforms.Impl;

public class LeaderBoardManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (!response.success)
            {
                return;
            }


            SetPlayerName("Natanor");
            getScores(1);
        });       
    }


    static public void SetPlayerName(string name)
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

    static public void SubmitScore(int timeTakenMs, int numberOfShots, int level)
    {

        LootLockerSDKManager.SubmitScore("", timeTakenMs, "LEVEL"+level, numberOfShots.ToString(), (response) =>
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

    static public void getScores(int level)
    {
        int count = 50;

        LootLockerSDKManager.GetScoreList("LEVEL"+level, count, 0, (response) =>
        {
            if (response.statusCode == 200)
            {
                Debug.Log("Successful");
                Debug.Log(response.items);
            }
            else
            {
                Debug.Log("failed: " + response.Error);
            }
        });
    }

}
