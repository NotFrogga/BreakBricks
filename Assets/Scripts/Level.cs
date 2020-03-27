using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    public int scoreBeginingOfLevel;
    public Ball ballPrefab;
    int breakableBlocks;
    SceneLoader sceneLoader;
    GameStatus gameStatus;
    Paddle paddle;

    private void Start()
    {
        paddle = FindObjectOfType<Paddle>();
        gameStatus = FindObjectOfType<GameStatus>();
        scoreBeginingOfLevel = gameStatus.getScore();
        sceneLoader = FindObjectOfType<SceneLoader>();
    }

    private void Update()
    {
        if (breakableBlocks == 0)
        {
            gameStatus.AddLife();
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

    public void RestoreBall()
    {
        Ball ball = FindObjectOfType<Ball>();
        ball.Destroy();
        Instantiate(ballPrefab, new Vector3(paddle.transform.position.x, -8.95f, paddle.transform.position.z), paddle.transform.rotation);
    }
}
