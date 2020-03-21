using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameStatus : MonoBehaviour
{
    [SerializeField] private int life = 3;
    [SerializeField] int score = 0;
    [Range(0.1f, 10f)] [SerializeField] float timeScale = 1.5f;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI lifeText;

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
        scoreText = FindObjectOfType<TextMeshProUGUI>();
        scoreText.text = score.ToString();
        lifeText.text = life.ToString();
    }

    private void Update()
    {
        scoreText.text = score.ToString();
        lifeText.text = life.ToString();
    }

    public void AddScore(int points)
    {
        score += points;
    }

    public void LoseLife()
    {
        life--;
    }

    public int getLife()
    {
        return life;
    }

    public void ResetGameStatus()
    {
        Destroy(gameObject);
    }
}
