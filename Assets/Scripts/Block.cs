using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    //[SerializeField] AudioClip audioClip;
    [SerializeField] int scorePerBlock = 83;
    [SerializeField] GameObject sparkle;
    public bool isBroken = false;
    Level level;
    GameStatus gameStatus;

    private void Start()
    {

        level = FindObjectOfType<Level>();
        if (gameObject.tag == "White" || gameObject.tag == "Red")
        {
            level.AddBreakableBlock();
        }

        gameStatus = FindObjectOfType<GameStatus>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        DestroyBlock(collision);
    }

    private void DestroyBlock(Collision2D collision)
    {
        if (BallCanDestroy(collision))
        {
            //AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position);
            TriggerSparklesVFX();
            Destroy(gameObject);
            level.ReduceBrokenBlock();
            gameStatus.AddScore(scorePerBlock);
            isBroken = true;
        }
    }

    private void TriggerSparklesVFX()
    {
        GameObject sparkleVFX = Instantiate(sparkle, transform.position, transform.rotation);
        Destroy(sparkleVFX, 2f);
    }

    private bool BallCanDestroy(Collision2D collision)
    {
       SpriteRenderer spriteRenderer = collision.gameObject.GetComponent<SpriteRenderer>();
       
        if (gameObject.tag == "Red" && spriteRenderer.color == Color.red)
        {
            return true;
        }
        else if (gameObject.tag == "White" && spriteRenderer.color == Color.white)
        {
            return true;
        }
        return false;
    }
}