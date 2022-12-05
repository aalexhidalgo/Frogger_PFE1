using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class AH_MenuManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;

    public Image skinImage;
    public Sprite[] skinArray;
    public int skinSelected;

    public void StartButton()
    {
        AH_DataPersistance.PlayerStats.SaveForFutureGames();
        SceneManager.LoadScene("Game");
    }

    public void ExitButton()
    {
        //Maria
    }

    void Start()
    {
        Load_Time_Score();
    }

    private void Load_Time_Score() //Load score in the stats panel
    {
        if (PlayerPrefs.HasKey("Score"))
        {
            scoreText.text = (PlayerPrefs.GetInt("Score")).ToString();
            timeText.text = string.Format("{0:00}:{1:00}:{2:000}", (PlayerPrefs.GetFloat("Minutes")), (PlayerPrefs.GetFloat("Seconds")), (PlayerPrefs.GetFloat("Miliseconds")));
        }
    }

    public void Skin_Selection(string direction)
    {
        if (direction == "right")
        {
            skinSelected++;

            if (skinSelected >= skinArray.Length)
            {
                skinSelected = 0;
            }
        }

        if (direction == "left")
        {
            skinSelected--;

            if (skinSelected < 0)
            {
                skinSelected = 2;
            }
        }

        skinImage.sprite = skinArray[skinSelected];
        AH_DataPersistance.PlayerStats.skinSelected = skinSelected;
    }
}
