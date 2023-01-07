using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public int currentLevelIndex;


    public void LoadNextScene()
    {
        currentLevelIndex++;
        SceneManager.LoadScene(currentLevelIndex);
    }

    public void LoadMainMenu()
    {
        currentLevelIndex = 0;
        SceneManager.LoadScene(currentLevelIndex);
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(currentLevelIndex);
    }
}
