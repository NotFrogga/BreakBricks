using UnityEngine;

public class LoseCollider : MonoBehaviour
{
    SceneLoader sceneLoader;
    GameStatus gameStatus;
    Level level;
    Paddle paddle;
    [SerializeField] AudioClip audioClip;

    private void Start()
    {
        paddle = FindObjectOfType<Paddle>();
        level = FindObjectOfType<Level>();
        sceneLoader = FindObjectOfType<SceneLoader>();
        gameStatus = FindObjectOfType<GameStatus>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayClip();
        ContinueOrGameOver();
    }

    private void PlayClip()
    {
        if (audioClip != null)
        {
            AudioSource.PlayClipAtPoint(audioClip, transform.position);
        }
    }
    private void ContinueOrGameOver()
    {
        gameStatus.LoseLife();
        int life = gameStatus.getLife();
        if (life > 0)
        {
            level.UpdateScoreMultiplierOnLostLife();
            //gameStatus.setScore(level.GetScoreBeginingOfLevel());
            paddle.AnimateLoseLife();
        }
        else
        {
            //sceneLoader.LoadGameOverScene();
            gameStatus.GameOver();
        }
    }
}
