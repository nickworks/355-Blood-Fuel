using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public void ButtonPlay()
    {
        SceneManager.LoadScene("LevelGen");
    }
    public void ButtonHowTo()
    {
        SceneManager.LoadScene("HowTo");
    }
    public void ButtonCredits()
    {
        SceneManager.LoadScene("Credits");
    }
    public void ButtonExit()
    {
        Application.Quit();
    }
}
