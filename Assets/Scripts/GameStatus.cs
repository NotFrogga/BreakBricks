using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class GameStatus : MonoBehaviour
{
    [SerializeField] private int life = 3;
    [SerializeField] int score = 0;
    [Range(0.1f, 10f)] [SerializeField] public float timeScale = 1.5f;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI lifeText;
    public TextMeshProUGUI highScoreGameOver;
    public TextMeshProUGUI scoreGameOver;
    public AudioClip gameOverClip;
    public MusicPlayer musicPlayer;

    private void Awake()
    {
        GameStatus[] gameStatuses = FindObjectsOfType<GameStatus>();

        if (gameStatuses.Length > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Start()
    {
        Time.timeScale = timeScale;
        scoreText.text = score.ToString();
        lifeText.text = life.ToString();
    }

    private void Update()
    {
        scoreText.text = score.ToString();
        lifeText.text = life.ToString();
        SetHighScore();
    }

    private void SetHighScore()
    {
        if (score > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", score);
        }
    }

    public void AddScore(int points, float multiplier)
    {
        score += Mathf.FloorToInt((float)points * multiplier);
    }

    public void LoseLife()
    {
        life--;
    }

    public void AddLife()
    {
        life++;
    }

    public int getLife()
    {
        return life;
    }

    public int getScore()
    {
        return score;
    }

    public void setScore(int _score)
    {
        score = _score;
    }

    public void setTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
    }

    public void ResetGameStatus()
    {
        Destroy(gameObject);
    }

    public void GameOver()
    {
        DisablePaddle();
        GameObject.FindGameObjectWithTag("MusicManager").GetComponent<AudioSource>().Stop();
        musicPlayer.canPlay = false;
        AudioSource.PlayClipAtPoint(gameOverClip, Camera.main.transform.position);
        EnableCanvasOnlyByTagName("GameOver");
        DisplayScores(score, PlayerPrefs.GetInt("HighScore", score));
    }

    private void DisablePaddle()
    {
        Paddle paddle = FindObjectOfType<Paddle>();
        paddle.SetCanMove(false);
    }

    public void NextLevel()
    {
        DisablePaddle();
        FindObjectOfType<Paddle>().TriggerPaddleWinAnimation(true);
        DisableBall();
        EnableCanvasOnlyByTagName("SuccessPanel");
    }

    private void DisableBall()
    {
        Ball ball = FindObjectOfType<Ball>();
        ball.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
    }

    private void DisplayScores(int score, int highScore)
    {
        scoreGameOver.text = score.ToString();
        highScoreGameOver.text = highScore.ToString();
    }

    private static void EnableCanvasOnlyByTagName(string tag)
    {
        GameObject mainCanvas = Resources.
                                FindObjectsOfTypeAll<GameObject>().
                                FirstOrDefault(g => g.CompareTag("MainCanvas"));
        foreach (Transform childCanvas in mainCanvas.transform)
        {
            if (!childCanvas.gameObject.CompareTag(tag))
            {
                childCanvas.gameObject.SetActive(false);
            }
            else
            {
                childCanvas.gameObject.SetActive(true);
            }
        }
    }

    public void RestoreCanvases()
    {
        GameObject mainCanvas = Resources.
                                FindObjectsOfTypeAll<GameObject>().
                                FirstOrDefault(g => g.CompareTag("MainCanvas"));

        foreach (Transform childCanvas in mainCanvas.transform)
        {
            if (childCanvas.gameObject.CompareTag("PausePanel") || childCanvas.gameObject.CompareTag("GameOver") || childCanvas.gameObject.CompareTag("SuccessPanel"))
            {
                childCanvas.gameObject.SetActive(false);
            }
            else
            {
                childCanvas.gameObject.SetActive(true);
            }
        }
    }

    public void OnClickTryAgain()
    {
        musicPlayer.canPlay = true;
        RestoreCanvases();
        FindObjectOfType<SceneLoader>().ReloadScene();
    }

    public void OnClickNextLevel()
    {
        FindObjectOfType<SceneLoader>().LoadNextScene();
    }
}
