using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueLoader : MonoBehaviour
{
    public int? levelData;
    void Awake()
    {
        levelData = SaveSystem.LoadLevel();
        if (levelData == null || levelData == 0)
        {
            gameObject.SetActive(false);
        }
    }
}
