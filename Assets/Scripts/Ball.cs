using System;
using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    
    [SerializeField] float xLaunch = 3f;
    [SerializeField] float yLaunch = 15f;
    [SerializeField] float randomVelocity = 5f;
    [SerializeField] float maxSpeed = 20f;
    [SerializeField] AudioClip launchAudioClip;

    int collidedWallOrPaddle = 0;

    //cached reference
    AudioSource audioSource;
    AudioSource paddleAudioSource;
    Color paddleBlue = new Color(.3971f, .7823f, .9905f, 1f);
    Color paddleRed = new Color(.6862f, .2274f, .1647f, 1f);
    Paddle paddle1;
    Level level;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigidBody2D;
    GameObject leftTouchLimitGO;
    GameObject rightTouchLimitGO;
    Animator dialogueBoxAnimator;
    Animator ballAnimator;
    [SerializeField] bool notStarted = true;
    TrailRenderer trail;
    bool isOpen;
    Vector3 scale;

    Vector2 paddleToBallVector2;
    // Start is called before the first frame update
    void Start()
    {

        paddleAudioSource = FindObjectOfType<Paddle>().GetComponent<AudioSource>();
        audioSource = gameObject.GetComponent<AudioSource>();
        level = FindObjectOfType<Level>();
        paddle1 = FindObjectOfType<Paddle>();
        leftTouchLimitGO = GameObject.FindGameObjectWithTag("Left");
        rightTouchLimitGO = GameObject.FindGameObjectWithTag("Right");
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        paddleToBallVector2 = transform.position - paddle1.transform.position;
        dialogueBoxAnimator = GameObject.FindGameObjectWithTag("DialogueBox").GetComponent<Animator>();
        //Checks if dialogue panel is open
        isOpen = dialogueBoxAnimator.GetBool("isOpen");
        ballAnimator = gameObject.GetComponent<Animator>();
        trail = gameObject.GetComponent<TrailRenderer>();
        SetTrailColor(trail);
    }

    // Update is called once per frame
    void Update()
    {
        //Checks if dialogue panel is open
        if (dialogueBoxAnimator != null)
        {
            isOpen = dialogueBoxAnimator.GetBool("isOpen");
        }
        if (isOpen == false)
        {
            if (notStarted)
            {
                LockToPaddle();
                LauchOnMouseOnTouch();
                //LaunchForTest();
            }
            else
            {
                //LaunchForTest();
                SetSpeedLimit();
                if (level.allowChangeColor)
                {
                    ChangeBallColorByTouch();
                }
            }
        }
    }

    private void SetSpeedLimit()
    {
        rigidBody2D.velocity = new Vector2(Mathf.Clamp(rigidBody2D.velocity.x, -maxSpeed, maxSpeed), Mathf.Clamp(rigidBody2D.velocity.y, -maxSpeed, maxSpeed));
    }

    private void LauchOnMouseOnTouch()
    {
        Vector2 touchPosition = new Vector2();
        if (Input.touchCount > 0)
        {
            touchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
        if (IsDoubleTap() && notStarted && touchPosition.x >= leftTouchLimitGO.transform.position.x && touchPosition.x <= rightTouchLimitGO.transform.position.x)
        {
            if (launchAudioClip != null)
            {
                    AudioSource.PlayClipAtPoint(launchAudioClip, transform.position);
            }
            rigidBody2D.velocity = new Vector2(UnityEngine.Random.Range(-xLaunch, xLaunch), yLaunch);
            notStarted = false;
        }
        }
    }

    private void LaunchForTest()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Random.Range(-xLaunch, xLaunch)
            rigidBody2D.velocity = new Vector2(0, yLaunch);
            notStarted = false;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            rigidBody2D.velocity = new Vector2(yLaunch, 0);
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
            AddRandomBounce(collision);
            level.UpdateScoreMultiplierOnCollision(collision);
            audioSource.Play();
            StopAllCoroutines();
            StartCoroutine(DelayCollisionAnimation());
        }
    }

    //If it does not collide with a block, and the velocity is only vertical or horizontal
    // Add Force to the ball in order to tweak it a bit.
    private void AddRandomBounce(Collision2D collision)
    {
        GameObject colliderGameObject = collision.collider.gameObject;
        bool horizontalSpeedIsFlat = rigidBody2D.velocity.x < 0.25f && rigidBody2D.velocity.x > -0.25f;
        bool verticalSpeedIsFlat = rigidBody2D.velocity.y < 0.25f && rigidBody2D.velocity.y > -0.25f;
        bool collidedWallPaddleUnbreakable = CollidedWallPaddleUnbreakable(colliderGameObject);

        Block blockCollider = collision.collider.gameObject.GetComponent<Block>();
        AppendColliderCount(horizontalSpeedIsFlat, verticalSpeedIsFlat, collidedWallPaddleUnbreakable);
        if (collidedWallOrPaddle > 3)
        {
            AddForceToStuckBall(horizontalSpeedIsFlat, verticalSpeedIsFlat);
        }
    }

    private void AddForceToStuckBall(bool horizontalSpeedIsFlat, bool verticalSpeedIsFlat)
    {
        if (horizontalSpeedIsFlat)
        {
            AddHorizontalVelocity();
        }
        else if (verticalSpeedIsFlat)
        {
            AddVerticalVelocity();
        }
        collidedWallOrPaddle = 0;
    }

    private void AppendColliderCount(bool horizontalSpeedIsFlat, bool verticalSpeedIsFlat, bool collidedWallPaddleUnbreakable)
    {
        if (collidedWallPaddleUnbreakable)
        {
            if (horizontalSpeedIsFlat || verticalSpeedIsFlat)
            {
                collidedWallOrPaddle++;
            }
        }
        else
        {
            collidedWallOrPaddle = 0;
        }
    }

    private bool CollidedWallPaddleUnbreakable(GameObject gameObject)
    {
        if (gameObject.CompareTag("Unbreakable") || gameObject.GetComponent<Paddle>() != null || gameObject.CompareTag("Wall"))
        {
            return true;
        }
        return false;
    }

    private void AddVerticalVelocity()
    {
        if (rigidBody2D.velocity.y <= 0.05f && rigidBody2D.velocity.y >= -0.05f)
        {
            rigidBody2D.AddForce(new Vector2(0, -30f));
        }
        else
        {
            rigidBody2D.AddForce(new Vector2(0, rigidBody2D.velocity.y * 100.00f));
        }
    }

    private void AddHorizontalVelocity()
    {
        if (rigidBody2D.velocity.x <= 0.005f && rigidBody2D.velocity.x >= -0.005f)
        {
            if (transform.position.x < 8f)
            {
                rigidBody2D.AddForce(new Vector2(20f, 0));
            }
            else
            {
                rigidBody2D.AddForce(new Vector2(-20f, 0));
            }
        }
        else
        {
            rigidBody2D.AddForce(new Vector2(rigidBody2D.velocity.x * 100f, 0));
        }
    }

    IEnumerator DelayCollisionAnimation()
    {
        ballAnimator.SetBool("isHit", true);
        yield return new WaitForSeconds(.25f);
        ballAnimator.SetBool("isHit", false);
    }

    //private void ChangeBallColorByMouseClick()
    //{
    //    if (Input.GetMouseButton(0))
    //    {
    //        Vector3 vector3 = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
    //        Vector3 mousePos = Camera.main.ScreenToWorldPoint(vector3);
    //        if (mousePos.x > leftTouchLimitGO.transform.position.x && mousePos.x < rightTouchLimitGO.transform.position.x)
    //        {
    //            if (spriteRenderer.color != Color.red)
    //            {
    //                spriteRenderer.color = Color.red;
    //            }
    //            else
    //            {
    //                spriteRenderer.color = Color.white;
    //            }
    //        }
    //    }
            
    //}

    public void ChangeBallColorByTouch()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Vector3 touchPos = Input.GetTouch(0).position;
            Vector3 touchPosToWP = Camera.main.ScreenToWorldPoint(touchPos);
            if (touchPosToWP.x > leftTouchLimitGO.transform.position.x && touchPosToWP.x < rightTouchLimitGO.transform.position.x)
                if (spriteRenderer.color != paddleRed)
                {
                    paddleAudioSource.Play();
                    spriteRenderer.color = paddleRed;
                    SetTrailColor(trail);

                }
                else
                {
                    paddleAudioSource.Play();
                    spriteRenderer.color = paddleBlue;
                    SetTrailColor(trail);
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

    private void SetTrailColor(TrailRenderer trail)
    {
        Color endTrailColor = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.5f);
        trail.startColor = spriteRenderer.color;
        trail.endColor = endTrailColor;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
