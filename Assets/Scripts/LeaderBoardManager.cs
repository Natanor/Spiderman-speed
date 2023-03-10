using LootLocker.Requests;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Profiling.Memory.Experimental;
using UnityEngine.SocialPlatforms.Impl;
using Random = UnityEngine.Random;

public class LeaderBoardManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] leaderboardLines;
    public TMP_InputField nameInputField;
    public TextMeshProUGUI titleGUI;

    private string memberId;
    void Start()
    {
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (!response.success)
            {
                return;
            }

            memberId = response.player_id.ToString();

            if (nameInputField != null)
            {
                GetPlayerName();
            }
        });       
    }

    public void GetPlayerName()
    {
        LootLockerSDKManager.GetPlayerName((response) =>
        {
            if (response.success)
            {
                Debug.Log("Successfully set player name");
                if (response.name == "")
                {
                    nameInputField.text = "Guest " + Random.Range(0, 99999);
                    UpdatePlayerName();

                }
                else
                {
                    nameInputField.text = response.name;
                }
            }
            else
            {
                Debug.Log("Error setting player name");
            }
        });
    }

    public void UpdatePlayerName()
    {
        if (nameInputField != null)
        {
            SetPlayerName(nameInputField.text);
        }
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

    public void InitializeEmptyLeaderboard(int start=0)
    {
        start = Mathf.Max(start,0);
        for (int i = start; i < leaderboardLines.Length; i++)
        {
            UpdateLeaderboardRow(new LeaderboardRow("-", "-", "-", "-"), i);
        }
    }

    public void UpdateLeaderboardUI(int level)
    {
        string LevelID = "LEVEL" + level;

        titleGUI.text = "Leaderboard for level " + level;

        LootLockerSDKManager.GetMemberRank(LevelID, memberId, (response) =>
        {
            if (response.statusCode == 200)
            {
                int rank = response.rank;
                

                if(rank <= 7)
                {

                    // get Top 8
                    LootLockerSDKManager.GetScoreList(LevelID, 8, 0, (response) =>
                    {
                        if (response.statusCode == 200)
                        {
                            for(int i=0;  i < response.items.Length; i++)
                            {
                                LootLockerLeaderboardMember row = response.items[i];
                                UpdateLeaderboardRow(new LeaderboardRow(row.rank, row.player.name, row.score, row.metadata), i);
                            }
                            InitializeEmptyLeaderboard(response.items.Length);
                        }
                        else
                        {
                            Debug.Log("failed: " + response.Error);
                        }
                    });
                }
                else
                {
                    // get Top 5
                    LootLockerSDKManager.GetScoreList(LevelID, 5, 0, (response) =>
                    {
                        if (response.statusCode == 200)
                        {
                            for (int i = 0; i < response.items.Length; i++)
                            {
                                LootLockerLeaderboardMember row = response.items[i];
                                UpdateLeaderboardRow(new LeaderboardRow(row.rank, row.player.name, row.score, row.metadata), i);
                            }
                        }
                        else
                        {
                            Debug.Log("failed: " + response.Error);
                        }
                    });

                    //get around player
                    LootLockerSDKManager.GetScoreList("LEVEL" + level, 3, rank-2, (response) =>
                    {
                        if (response.statusCode == 200)
                        {
                            for (int i = 0; i < response.items.Length; i++)
                            {
                                LootLockerLeaderboardMember row = response.items[i];
                                UpdateLeaderboardRow(new LeaderboardRow(row.rank, row.player.name, row.score, row.metadata), i + 5);
                            }
                            InitializeEmptyLeaderboard(response.items.Length+5);
                        }
                        else
                        {
                            Debug.Log("failed: " + response.Error);
                        }
                    });
                }

                
            }
            else
            {
                Debug.Log("failed: " + response.Error);
            }
        });
    }

    private void UpdateLeaderboardRow(LeaderboardRow row, int pos)
    {
        GameObject line = leaderboardLines[pos];
        TextMeshProUGUI position = line.transform.Find("Position").gameObject.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI name = line.transform.Find("Name").gameObject.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI time = line.transform.Find("Time").gameObject.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI numberOfSwings = line.transform.Find("NumberOfSwings").gameObject.GetComponent<TextMeshProUGUI>();


        position.text = row.Position.ToString() + ".";
        name.text = row.Name;
        time.text = row.TimeString;
        numberOfSwings.text = row.NumberOfSwings;
    }
}


struct LeaderboardRow
{
    public readonly string Name;
    public readonly string NumberOfSwings;
    public readonly string TimeString;
    public readonly string Position;

    public LeaderboardRow(int Position, string Name, int TimeMs, string NumberOfSwings)
    {
        this.Name = Name;
        this.Position = Position.ToString();
        this.TimeString = TimeSpan.FromMilliseconds(TimeMs).ToString("mm\\:ss\\.fff");
        this.NumberOfSwings = NumberOfSwings;
    }

    public LeaderboardRow(string Position, string Name, string TimeMs, string NumberOfSwings)
    {
        this.Name = Name;
        this.Position = Position;
        this.TimeString = TimeMs;
        this.NumberOfSwings = NumberOfSwings;
    }
}