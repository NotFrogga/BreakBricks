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

    SpriteRenderer spriteRenderer;
    GameObject leftTouchLimitGO;
    GameObject rightTouchLimitGO;
    Animator animator;
    bool isOpen;
    float leftTouchLimit;
    float rightTouchLimit;
    // Start is called before the first frame update
    void Start()
    {
        leftTouchLimitGO = GameObject.FindGameObjectWithTag("Left");
        leftTouchLimit = leftTouchLimitGO.transform.position.x;
        rightTouchLimitGO = GameObject.FindGameObjectWithTag("Right");
        rightTouchLimit = rightTouchLimitGO.transform.position.x;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = FindObjectOfType<Ball>().GetComponent<SpriteRenderer>().color;
        animator = GameObject.FindGameObjectWithTag("DialogueBox").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        isOpen = animator.GetBool("isOpen");
        spriteRenderer.color = FindObjectOfType<Ball>().GetComponent<SpriteRenderer>().color;
        MovePaddleByTouch();
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
        if (!isOpen && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Stationary)
        {
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

            if (touchPosition.x < leftTouchLimit && transform.position.x >= minClamp)
            {
                Vector3 directionL = Vector3.left * speed * Time.deltaTime;
                transform.Translate(directionL);
            }
            else if (touchPosition.x > rightTouchLimit && transform.position.x <= maxClamp)
            {
                Vector3 directionR = Vector3.right * speed * Time.deltaTime;
                transform.Translate(directionR);
            }
        }
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minClamp, maxClamp);
        transform.position = clampedPosition;
    }
}
