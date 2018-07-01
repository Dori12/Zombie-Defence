using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUIButton : MonoBehaviour {

    [SerializeField] private GameObject StartOptionUI;
    [SerializeField] private GameObject SettingOptionUI;

    bool isClickQuit;

    private void Start()
    {
        isClickQuit = false;
    }

    public void Click_StartButton()
    {
        StartOptionUI.SetActive(true);
        SettingOptionUI.SetActive(false);
    }
    public void Click_SettingButton()
    {
        StartOptionUI.SetActive(false);
        SettingOptionUI.SetActive(true);
    }
    public void Click_QuitButton()
    {
        if(isClickQuit == false)
        {
            isClickQuit = true;
            Invoke("GameQuit", 0.5f);
        }
    }
    public void Click_SinglePlayButton()
    {
        SceneManager.LoadScene(1);
    }
    void GameQuit()
    {
        Application.Quit();
    }
}
