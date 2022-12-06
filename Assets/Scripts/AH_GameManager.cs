using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class AH_GameManager : MonoBehaviour
{
    //Counter
    public TextMeshProUGUI timeText;
    private float TimeCounter;

    //Life
    public Image lifeImage;
    public Sprite[] lifeSpriteArray;

    //Score
    public TextMeshProUGUI scoreText;
    public int scoreCounter;

    public bool gameOver;

    public GameObject[] skinArray;
    private CinemachineVirtualCamera cvCamera;

    //Script
    private AH_PlayerController playerControllerScript;

    void Awake()
    {
        LoadSkin_HighScore();
    }
    void Start()
    {
        playerControllerScript = FindObjectOfType<AH_PlayerController>();
        cvCamera = FindObjectOfType<CinemachineVirtualCamera>();
        cvCamera.Follow = skinArray[AH_DataPersistance.PlayerStats.skinSelected].transform; //We set the camera follow to the current skin Player we chose in the MainMenu scene
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
        float miliseconds = (TimeCounter % 1) * 1000;
        timeText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, miliseconds); //Order

        AH_DataPersistance.PlayerStats.minutes = minutes;
        AH_DataPersistance.PlayerStats.seconds = seconds;
        AH_DataPersistance.PlayerStats.miliseconds = miliseconds;
    }

    public void UpdateScore(int score)
    {
        scoreCounter += score;
        scoreText.text = scoreCounter.ToString();
        AH_DataPersistance.PlayerStats.score = scoreCounter;
    }

    public void GameOver()
    {
        if(AH_DataPersistance.PlayerStats.highscore == 0) //The first match, the highscore will be the same as the first score saved
        {
            AH_DataPersistance.PlayerStats.highscore = AH_DataPersistance.PlayerStats.score;
        }
        else
        {
            if(AH_DataPersistance.PlayerStats.highscore < AH_DataPersistance.PlayerStats.score) //If we already played the game, the highscore will be replaced when the score counter hit the highscore.
            {
                AH_DataPersistance.PlayerStats.highscore = AH_DataPersistance.PlayerStats.score;
            }
        }
        AH_DataPersistance.PlayerStats.SaveForFutureGames(); //Before we die we save the values in data persistance
        SceneManager.LoadScene("Game Over");
    }

    private void LoadSkin_HighScore() //To load the skin selected in the MainMenu scene
    {
        skinArray[AH_DataPersistance.PlayerStats.skinSelected].SetActive(true);
    }

}


