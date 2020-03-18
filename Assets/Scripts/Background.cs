using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = FindObjectOfType<Ball>().GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {
        spriteRenderer.color = FindObjectOfType<Ball>().GetComponent<SpriteRenderer>().color;
    }
}
