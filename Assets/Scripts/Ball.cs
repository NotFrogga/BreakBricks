﻿using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] Paddle paddle1;
    [SerializeField] float xLaunch = 3f;
    [SerializeField] float yLaunch = 15f;
    [SerializeField] float randomVelocity = .7f;
    [SerializeField] float maxSpeed = 70f;
    [SerializeField] bool allowChangeColor;


    //cached reference
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigidBody2D;
    GameObject leftTouchLimitGO;
    GameObject rightTouchLimitGO;
    Animator animator;
    bool notStarted = true;
    bool isOpen;

    Vector2 paddleToBallVector2;
    // Start is called before the first frame update
    void Start()
    {
        leftTouchLimitGO = GameObject.FindGameObjectWithTag("Left");
        rightTouchLimitGO = GameObject.FindGameObjectWithTag("Right");
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        spriteRenderer.color = Color.white;
        paddleToBallVector2 = transform.position - paddle1.transform.position;
        animator = GameObject.FindGameObjectWithTag("DialogueBox").GetComponent<Animator>();
        //Checks if dialogue panel is open
        isOpen = animator.GetBool("isOpen");
    }

    // Update is called once per frame
    void Update()
    {
        //Checks if dialogue panel is open
        isOpen = animator.GetBool("isOpen");
        if (isOpen == false)
        {
            if (notStarted)
            {
                LockToPaddle();
                LauchOnMouseOnTouch();
            }
            else
            {
                SetSpeedLimit();
                if (allowChangeColor)
                {
                    ChangeBallColorByTouch();
                }
            }
        }
    }

    private void SetSpeedLimit()
    {
        if (rigidBody2D.velocity.x > maxSpeed)
        {
            rigidBody2D.velocity = new Vector2(maxSpeed, rigidBody2D.velocity.y);
        }
        else if (rigidBody2D.velocity.y > maxSpeed)
        {
            rigidBody2D.velocity = new Vector2(rigidBody2D.velocity.x, maxSpeed);
        }
    }

    private void LauchOnMouseOnTouch()
    {
        if (IsDoubleTap() && notStarted)
        {
            rigidBody2D.velocity = new Vector2(Random.Range(-xLaunch, xLaunch), yLaunch);
            notStarted = false;
        }
    }

    private void LockToPaddle()
    {
        Vector2 paddlePos = new Vector2(paddle1.transform.position.x, paddle1.transform.position.y);
        transform.position = paddlePos + paddleToBallVector2;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!notStarted)
        {
            GetComponent<Rigidbody2D>().velocity += new Vector2(Random.Range(-randomVelocity, randomVelocity), Random.Range(-randomVelocity, randomVelocity));
        }
    }

    private void ChangeBallColorByMouseClick()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 vector3 = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(vector3);
            if (mousePos.x > leftTouchLimitGO.transform.position.x && mousePos.x < rightTouchLimitGO.transform.position.x)
            {
                if (spriteRenderer.color != Color.red)
                {
                    spriteRenderer.color = Color.red;
                }
                else
                {
                    spriteRenderer.color = Color.white;
                }
            }
        }
            
    }

    public void ChangeBallColorByTouch()
    {
        if (!notStarted && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Vector3 touchPos = Input.GetTouch(0).position;
            Vector3 touchPosToWP = Camera.main.ScreenToWorldPoint(touchPos);
            if (touchPosToWP.x > leftTouchLimitGO.transform.position.x && touchPosToWP.x < rightTouchLimitGO.transform.position.x)
                if (spriteRenderer.color != Color.red)
                {
                    spriteRenderer.color = Color.red;
                }
                else
                {
                    spriteRenderer.color = Color.white;
                }
        }
    }

    public bool IsDoubleTap()
    {
        bool result = false;
        float MaxTimeWait = 1;
        float VariancePosition = 1;

        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            float DeltaTime = Input.GetTouch(0).deltaTime;
            float DeltaPositionLenght = Input.GetTouch(0).deltaPosition.magnitude;

            if (DeltaTime > 0 && DeltaTime < MaxTimeWait && DeltaPositionLenght < VariancePosition)
                result = true;
        }
        return result;
    }
}
