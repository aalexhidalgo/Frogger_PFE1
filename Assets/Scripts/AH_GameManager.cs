using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class AH_GameManager : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    private float TimeCounter;

    public Image lifeImage;
    public Sprite[] lifeSpriteArray;

    public TextMeshProUGUI scoreText;
    private int scoreCounter;

    private AH_PlayerController playerControllerScript;

    void Start()
    {
        playerControllerScript = FindObjectOfType<AH_PlayerController>();
    }

    void Update()
    {
        if (playerControllerScript.active == true) //The time doesn't start counting if we didn't move
        {
            UpdateTime();
        }
    }

    void UpdateTime()
    {
        TimeCounter += Time.deltaTime;
        float minutes = Mathf.FloorToInt(TimeCounter / 60);
        float seconds = Mathf.FloorToInt(TimeCounter % 60);
        float milliSeconds = (TimeCounter % 1) * 1000;
        timeText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliSeconds);
    }

    public void GameOver()
    {
        AH_DataPersistanceScript.SaveForFutureGames();
        SceneManager.LoadScene("Game Over");
    }

    public void UpdateScore(int score)
    {
        scoreCounter += score;
        scoreText.text = scoreCounter.ToString();
    }

}


