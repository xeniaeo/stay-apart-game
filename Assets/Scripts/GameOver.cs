using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public Button HomeBtn;

    public Text LevelReached;
    private int _levelReached;
    private int _maxLevel;
    public Text Score;

    // Start is called before the first frame update
    void Start()
    {
        HomeBtn.GetComponent<Button>().onClick.AddListener(() => GoHome());

        _levelReached = GameManager.LevelReached;
        _maxLevel = GameManager.MaxLevel;


        if (GameManager.AllLevelsCleared)
        {
            LevelReached.text = "You just save the world!\n" + "All " + _maxLevel.ToString() + " levels completed!";
        }
        else
        {
            if (_levelReached < 3)
                LevelReached.text = "Oh well...\n" + "You've reached level " + _levelReached.ToString() + "!";
            else if (_levelReached < 5)
                LevelReached.text = "Not bad!\n" + "You've reached level " + _levelReached.ToString() + "!";
            else
                LevelReached.text = "Good job!\n" + "You've reached level " + _levelReached.ToString() + "!";
        }

        Score.text = "Score: " + GameManager.TotalScore.ToString();
    }

    private void GoHome()
    {
        SceneManager.LoadScene("Home", LoadSceneMode.Single);
    }
}
