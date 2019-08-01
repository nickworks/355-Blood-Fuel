using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour {

    public Text message;
    public Text score;

	void Start () {

        
	}
    public void Init(DriverPlayer driver) {
        if (driver.car.currentFuel <= 0) message.text = "No Fuel";
        string text = "Score : " + (int)driver.score;
        string[] chars = text.Split();
        score.text = string.Join(" ", chars);
    }
    public void PlayAgain()
    {
        SceneManager.LoadScene("LevelGen");
    }
    public void ToTitle()
    {
        SceneManager.LoadScene("Title");
    }
}
