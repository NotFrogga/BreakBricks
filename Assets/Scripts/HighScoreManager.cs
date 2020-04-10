using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreManager : MonoBehaviour
{
    public TextMeshProUGUI highScoreTitle;
    public TextMeshProUGUI highScoreText;
    public ContinueLoader continueLoader;
    // Start is called before the first frame update
    void Start()
    {
        if (!continueLoader.gameObject.activeSelf || PlayerPrefs.GetInt("HighScore", 0) == 0)
        {
            highScoreText.text = "";
            highScoreTitle.text = "";
        }
        else
        {
            highScoreText.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
        }
    }
}
