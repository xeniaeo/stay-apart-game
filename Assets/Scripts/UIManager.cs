using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("The UIManager is null");
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    public Text tooClose;
    public int tooCloseCount;
    public Text currentLevel;
    public Image RiskBarImage;
    public Text Score;
    public Text ScoreUpdate;
    public Text RiskUpdate;

    // Start is called before the first frame update
    void Start()
    {
        tooCloseCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        tooClose.text = "Too Close: " + tooCloseCount.ToString();

        currentLevel.text = "Level: " + GameManager.LevelReached.ToString() + " / " + GameManager.MaxLevel.ToString();

        Score.text = "Score: " + GameManager.TotalScore.ToString();

        SetRiskBarValue(GetRiskBarValue() + tooCloseCount * 0.00003f);
        if (GetRiskBarValue() == 1.0f)
            GameManager.Instance.GameOver("Lose");
    }

    public void SetRiskBarValue(float value)
    {
        RiskBarImage.fillAmount = value;
        if (RiskBarImage.fillAmount < 0.2f)
            RiskBarImage.color = Color.green;
        else if (RiskBarImage.fillAmount < 0.5f)
            RiskBarImage.color = Color.yellow;
        else
            RiskBarImage.color = Color.red;
    }

    public float GetRiskBarValue()
    {
        return RiskBarImage.fillAmount;
    }

    public void PostToScoreUpdate(string update, float time)
    {
        StartCoroutine(PostToUI(update, ScoreUpdate, time));
    }

    public void PostToRiskUpdate(string update, float time)
    {
        StartCoroutine(PostToUI(update, RiskUpdate, time));
    }

    IEnumerator PostToUI(string update, Text textHolder, float time)
    {
        textHolder.text = update;
        yield return new WaitForSeconds(time);
        textHolder.text = "";
    }
}
