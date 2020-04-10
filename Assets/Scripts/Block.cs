using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] int scorePerBlock = 50;
    [SerializeField] GameObject sparkle;
    [SerializeField] GameObject blockDestroy;
    public bool isBroken = false;
    public AudioClip audioClip;
    Level level;
    GameStatus gameStatus;
    Vector3 initialPosition;
    SpriteRenderer spriteRenderer;
    Vector3 scale;
    Color paddleBlue = new Color(.3971f, .7823f, .9905f, 1f);
    Color paddleRed = new Color(.6862f, .2274f, .1647f, 1f);

    private void Start()
    {
        scale = transform.localScale;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        initialPosition = transform.position;
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

    IEnumerator ScaleBlock()
    {
        transform.localScale += new Vector3(.1f, .1f, transform.localScale.z);
        yield return new WaitForSeconds(.2f);
        transform.localScale = scale;
    }

    private void DestroyBlock(Collision2D collision)
    {
        if (BallCanDestroy(collision))
        {
            AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position);
            TriggerSparklesVFX();
            Destroy(gameObject);
            level.ReduceBrokenBlock();
            gameStatus.AddScore(scorePerBlock, level.GetMultiplier());
            isBroken = true;
        }
        else
        {
            transform.localScale = scale;
            StopAllCoroutines();
            StartCoroutine(ScaleBlock());
        }
    }

    private void TriggerSparklesVFX()
    {
        if (sparkle != null)
        {
            GameObject sparkleVFX = Instantiate(sparkle, transform.position, transform.rotation);
            Destroy(sparkleVFX, 2f);
        }
        if (blockDestroy != null)
        {
            GameObject blockAnimation = Instantiate(blockDestroy, transform.position, transform.rotation);
            Destroy(blockAnimation, .5f);

        }

    }

    private bool BallCanDestroy(Collision2D collision)
    {
       SpriteRenderer ballSprite = collision.gameObject.GetComponent<SpriteRenderer>();
        if (ballSprite.color == paddleRed && gameObject.tag == "Red")
        {
            return true;
        }
        else if (ballSprite.color == paddleBlue && gameObject.tag == "White")
        { 
            return true;
        }

        return false;
    }
}