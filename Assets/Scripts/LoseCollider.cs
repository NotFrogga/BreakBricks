using UnityEngine;

public class LoseCollider : MonoBehaviour
{
    SceneLoader sceneLoader;
    GameStatus gameStatus;
    Level level;

    private void Start()
    {
        level = FindObjectOfType<Level>();
        sceneLoader = FindObjectOfType<SceneLoader>();
        gameStatus = FindObjectOfType<GameStatus>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ContinueOrGameOver();
    }

    private void ContinueOrGameOver()
    {
        int life = gameStatus.getLife();
        gameStatus.LoseLife();
        if (life > 1)
        {
            gameStatus.setScore(level.GetScoreBeginingOfLevel());
            level.RestoreBall();
        }
        else
        {
            sceneLoader.LoadGameOverScene();
        }
    }
}
