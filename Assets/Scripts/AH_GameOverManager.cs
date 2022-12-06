using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class AH_GameOverManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI timeText;
    #region Buttons
    public void RestartButton()
    {
        SceneManager.LoadScene("AH_Game");
    }

    public void ExitButton()
    {
        SceneManager.LoadScene("AH_Main_Menu");
    }
    #endregion
    private void Start()
    {
        Load_Score_Time();
    }

    public void Load_Score_Time() //It shows the game results
    {
        if(PlayerPrefs.HasKey("Score"))
        {
            scoreText.text = (PlayerPrefs.GetInt("Score")).ToString();
            highScoreText.text = (PlayerPrefs.GetInt("High_Score")).ToString();
            timeText.text = string.Format("{0:00}:{1:00}:{2:000}", (PlayerPrefs.GetFloat("Minutes")), (PlayerPrefs.GetFloat("Seconds")), (PlayerPrefs.GetFloat("Miliseconds")));
        }
    }
}
