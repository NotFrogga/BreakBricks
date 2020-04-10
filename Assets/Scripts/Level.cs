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
    public bool isPaused;
    public bool allowChangeColor = false;
    int breakableBlocks;
    [SerializeField] AudioClip openPauseClip;
    [SerializeField] AudioClip closePauseClip;

    public float multiplier;

    public int levelLoaded;
    SceneLoader sceneLoader;
    GameStatus gameStatus;
    Paddle paddle;

    private void Awake()
    {
        Resources.FindObjectsOfTypeAll<GameStatus>().First().RestoreCanvases();
    }

    private void Start()
    {
        multiplier = 2f;
        levelLoaded = getLevelNumberFromScene(gameObject.scene);
        LevelData.setLastLevelLoaded(this);
        SaveSystem.SaveLevel();
        isPaused = false;
        paddle = FindObjectOfType<Paddle>();
        gameStatus = FindObjectOfType<GameStatus>();
        scoreBeginingOfLevel = gameStatus.getScore();
        sceneLoader = FindObjectOfType<SceneLoader>();
    }

    private void Update()
    {
        if (breakableBlocks == 0)
        {
            NextLevel();
        }
    }

    private void NextLevel()
    {
        gameStatus.NextLevel();
    }

    private int getLevelNumberFromScene(Scene scene)
    {
        string sceneName = scene.name;
        int number;
        foreach (char character in sceneName)
        {
            if (Char.IsDigit(character))
            {
                number = Convert.ToInt32(character);
                return number;
            }
        }
        return 0;
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
        Color ballColor = ball.GetComponent<SpriteRenderer>().color;
        ball.Destroy();
        Ball newBall = Instantiate(ballPrefab, new Vector3(paddle.transform.position.x, paddle.transform.position.y + .70f, paddle.transform.position.z), paddle.transform.rotation);
        newBall.GetComponent<SpriteRenderer>().color = ballColor;
    }

    public void Pause()
    {
        GameObject pausePanel = Resources
                                .FindObjectsOfTypeAll<GameObject>()
                                .FirstOrDefault(g => g.CompareTag("PausePanel"));
        if (isPaused)
        {
            if (closePauseClip != null)
            {
                AudioSource.PlayClipAtPoint(closePauseClip, Camera.main.transform.position);
            }
            Time.timeScale = 1f;
            isPaused = false;
            pausePanel.SetActive(false);
        } else if (!isPaused)
        {
            if (openPauseClip != null)
            {
                AudioSource.PlayClipAtPoint(openPauseClip, Camera.main.transform.position);
            }
            Time.timeScale = 0;
            isPaused = true;
            pausePanel.SetActive(true);
        }
    }

    public void Menu()
    {
        GameObject pausePanel = Resources
                        .FindObjectsOfTypeAll<GameObject>()
                        .FirstOrDefault(g => g.CompareTag("PausePanel"));
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1;
        FindObjectOfType<SceneLoader>().ResetFirstScene();
    }

    public float GetMultiplier()
    {
        return multiplier;
    }

    public void UpdateScoreMultiplierOnLostLife()
    {
        if (multiplier > .45f)
        {
            multiplier -= .1f;
        }
    }

    public void UpdateScoreMultiplierOnCollision(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Paddle>() != null && multiplier > .7f)
        {
            multiplier -= 0.05f;
        }
    }
}
