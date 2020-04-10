using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    [SerializeField] float screenWidthInUnits = 16f;
    [SerializeField] float minClamp;
    [SerializeField] float maxClamp;
    [SerializeField] float speed;
    public bool paddleLoseAnimEnded;
    public AudioClip slideClip;
    
    private bool canMove;
    SpriteRenderer spriteRenderer;
    GameObject leftTouchLimitGO;
    GameObject rightTouchLimitGO;
    Animator dialogueBoxAnimator;
    Animator paddleAnimator;
    bool isOpen;
    float leftTouchLimit;
    float rightTouchLimit;
    Level level;
    // Start is called before the first frame update
    void Start()
    {
        TriggerPaddleWinAnimation(false);
        canMove = true;
        leftTouchLimitGO = GameObject.FindGameObjectWithTag("Left");
        leftTouchLimit = leftTouchLimitGO.transform.position.x;
        rightTouchLimitGO = GameObject.FindGameObjectWithTag("Right");
        rightTouchLimit = rightTouchLimitGO.transform.position.x;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = FindObjectOfType<Ball>().GetComponent<SpriteRenderer>().color;
        dialogueBoxAnimator = GameObject.FindGameObjectWithTag("DialogueBox").GetComponent<Animator>();
        paddleAnimator = gameObject.GetComponent<Animator>();
        level = FindObjectOfType<Level>();
    }

    public void AnimateLoseLife()
    {
        StopAllCoroutines();
        StartCoroutine
            (
                AnimationDurationLoseLife
                (
                    1.5f,
                    value =>
                    {
                        paddleAnimator.SetBool("ballIsRespawning", value);
                        level.RestoreBall();
                    }
                 )
            );

    }

    IEnumerator AnimationDurationLoseLife(float seconds, Action<bool> callback)
    {
        if (paddleAnimator != null)
        {
            paddleAnimator.SetBool("ballIsRespawning", true);
        }
        yield return new WaitForSeconds(1.5f);
        callback(false);
    }

    // Update is called once per frame
    void Update()
    {
        isOpen = dialogueBoxAnimator.GetBool("isOpen");
        spriteRenderer.color = FindObjectOfType<Ball>().GetComponent<SpriteRenderer>().color;
        if (canMove && !isOpen)
        {
            MovePaddleByTouch();
        }
    }

    private void MovePaddleByMouseClick()
    {
        if (Input.GetMouseButton(0))
        {
            // Get mouse position in screen size and convert it to world point
            Vector3 vector3 = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(vector3);

            if (mousePos.x < leftTouchLimit)
            {

                Vector3 movL = Vector3.left * speed * Time.deltaTime;
                Vector3 paddleMovedToLeft = transform.position + movL;

                if (paddleMovedToLeft.x > minClamp)
                {
                    transform.Translate(movL);
                } else
                {

                }
            }
            else if (mousePos.x > rightTouchLimit)
            {
                Vector3 movR = Vector3.right * speed * Time.deltaTime;
                Vector3 paddleMovedToRight = transform.position + movR;
                if (paddleMovedToRight.x <= maxClamp)
                {
                    transform.Translate(movR);
                }
            }
        }
    }

    private void MovePaddleByTouch()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Stationary)
        {
            paddleAnimator.SetBool("isSliding", true);
            if (slideClip != null)
            {
                AudioSource.PlayClipAtPoint(slideClip, transform.position, .05f);
            }
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

            if (touchPosition.x < leftTouchLimit && touchPosition.y < leftTouchLimitGO.transform.position.y && transform.position.x >= minClamp)
            {
                Vector3 directionL = Vector3.left * speed * Time.deltaTime;
                transform.Translate(directionL);
            }
            else if (touchPosition.x > rightTouchLimit && touchPosition.y < leftTouchLimitGO.transform.position.y && transform.position.x <= maxClamp)
            {
                Vector3 directionR = Vector3.right * speed * Time.deltaTime;
                transform.Translate(directionR);
            }
        }
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            paddleAnimator.SetBool("isSliding", false);
        }
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minClamp, maxClamp);
        transform.position = clampedPosition;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StopAllCoroutines();
        StartCoroutine(DelayCollisionAnimation());
    }

    IEnumerator DelayCollisionAnimation()
    {
        paddleAnimator.SetBool("isHit", true);
        yield return new WaitForSeconds(.20f);
        paddleAnimator.SetBool("isHit", false);
    }

    public void SetCanMove(bool boolean)
    {
        canMove = boolean;
    }

    public void TriggerPaddleWinAnimation(bool boolean)
    {
        if (paddleAnimator != null)
        {
            paddleAnimator.SetBool("win", boolean);
        }
    }
}
