using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    public int scoreBeginingOfLevel;
    int breakableBlocks;
    SceneLoader sceneLoader;
    GameStatus gameStatus;

    private void Start()
    {
        gameStatus = FindObjectOfType<GameStatus>();
        scoreBeginingOfLevel = gameStatus.getScore();
        sceneLoader = FindObjectOfType<SceneLoader>();
    }

    private void Update()
    {
        if (breakableBlocks == 0)
        {
            sceneLoader.LoadNextScene();
        }
    }
    public void AddBreakableBlock()
    {
        breakableBlocks++;
    }

    public void ReduceBrokenBlock()
    {
        breakableBlocks--;
    }

    public int GetBreakableBlocks()
    {
        return breakableBlocks;
    }

    public int GetScoreBeginingOfLevel()
    {
        return scoreBeginingOfLevel;
    }
}
