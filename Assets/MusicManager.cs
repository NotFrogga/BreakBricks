using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private void Awake()
    {
        MusicManager[] gameStatuses = FindObjectsOfType<MusicManager>();

        if (gameStatuses.Length > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    public void ResetMusicManager()
    {
        Destroy(gameObject);
    }
}
