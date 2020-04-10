using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex + 1 == SceneManager.sceneCountInBuildSettings - 1)
        {
            LoadGameOverScene();
        }
        else
        {
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
    }

    public void LoadFirstScene()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadSceneLevel(int level)
    {
        FindObjectOfType<MusicManager>().ResetMusicManager();
        SceneManager.LoadScene("Level " + level.ToString());
    }

    public void LoadCreditsScene()
    {
        SceneManager.LoadScene("Credits");
    }

    public void ReloadScene()
    {
        Resources.FindObjectsOfTypeAll<GameStatus>().First().ResetGameStatus();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadGameOverScene()
    {
        GameStatus gameStatus = FindObjectOfType<GameStatus>();
        gameStatus.ResetGameStatus();
        int lastSceneIndex = SceneManager.sceneCountInBuildSettings - 1;
        SceneManager.LoadScene(lastSceneIndex);
    }

    public void ResetFirstScene()
    {
        GameStatus gameStatus = FindObjectOfType<GameStatus>();
        gameStatus.ResetGameStatus();
        LoadFirstScene();
    }

    //FOR DEV PURPOSES
    public void LoadLevel1()
    {
        SceneManager.LoadScene("Level 1");
    }

    //FOR DEV PURPOSES
    public void LoadLevel2()
    {
        SceneManager.LoadScene("Level 2");
    }
}
