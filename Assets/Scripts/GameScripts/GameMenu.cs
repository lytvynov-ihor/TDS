using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    public GameObject menuImg = null;
    void Start()
    {
        Time.timeScale = 0f;
        menuImg.SetActive(true);
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        menuImg.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}