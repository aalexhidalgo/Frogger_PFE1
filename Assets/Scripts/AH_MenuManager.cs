using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class AH_MenuManager : MonoBehaviour
{
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI titleDescriptionText;
    public string[] descriptionArray;
    public string[] titleDescriptionArray;
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
        Load_Data();
    }

    public void Skin_Selection(string direction) //To change the skin of our character, it also shows a short description of every one of them
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
        descriptionText.text = descriptionArray[skinSelected];
        titleDescriptionText.text = titleDescriptionArray[skinSelected];
        AH_DataPersistance.PlayerStats.skinSelected = skinSelected;
    }

    public void Load_Data() //To load the data saved in PlayerPrefs
    {
        if (PlayerPrefs.HasKey("Skin_Selected"))
        {
            AH_DataPersistance.PlayerStats.highscore = PlayerPrefs.GetInt("High_Score");
            AH_DataPersistance.PlayerStats.skinSelected = PlayerPrefs.GetInt("Skin_Selected");
            skinImage.sprite = skinArray[AH_DataPersistance.PlayerStats.skinSelected];
            descriptionText.text = descriptionArray[AH_DataPersistance.PlayerStats.skinSelected];
            titleDescriptionText.text = titleDescriptionArray[AH_DataPersistance.PlayerStats.skinSelected];
        }
    }
}
