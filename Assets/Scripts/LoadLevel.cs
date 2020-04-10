using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadLevel : MonoBehaviour
{
    public int? lastLevel;
    void Start()
    {
        lastLevel = SaveSystem.LoadLevel();
    }
}
