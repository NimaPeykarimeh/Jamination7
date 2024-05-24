using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighScoreManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI highScoreText;
    [SerializeField] TextMeshProUGUI currentScore;
    [SerializeField] GameManager gameManager;
    private int highScore;


    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    private void Start()
    {
        // Load the high score from PlayerPrefs
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        print(highScore);
        //UpdateHighScoreText();
    }

    // Call this method to update the high score
    public void UpdateHighScore()
    {
        if (gameManager.totalPoint > highScore)
        {
            highScore = gameManager.totalPoint;
            // Save the new high score to PlayerPrefs
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }
        UpdateHighScoreText();
    }

    // Update the high score text on UI
    private void UpdateHighScoreText()
    {

        currentScore.text = "Score: " + gameManager.totalPoint.ToString();
        highScoreText.text = "High Score: " + highScore.ToString();

    }
}
