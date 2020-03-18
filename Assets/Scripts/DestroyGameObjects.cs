using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGameObjects : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameStatus[] gameStatuses = FindObjectsOfType<GameStatus>();
        foreach (GameStatus gameStatus in gameStatuses)
        {
            Destroy(gameStatus);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
