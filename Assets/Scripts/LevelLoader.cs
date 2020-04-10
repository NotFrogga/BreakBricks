using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    int? level;
    // Start is called before the first frame update
    void Start()
    {
        level = FindObjectOfType<LoadLevel>().lastLevel;
        int levelButton = getLevelNumberFromGO(gameObject.tag);
        if (level != null && levelButton <= level)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private int getLevelNumberFromGO(string tagName)
    {
        int number;
        foreach (char character in tagName)
        {
            if (Char.IsDigit(character))
            {
                number = Convert.ToInt32(character);
                return number;
            }
        }
        return 0;
    }
}
