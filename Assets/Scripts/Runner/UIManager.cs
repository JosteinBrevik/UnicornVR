using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            DestroyImmediate(this);
        }
    }

    //singleton implementation
    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
                instance = new UIManager();
            
            return instance;
        }
    }

    protected UIManager()
    {
    }

    private float score = 0;
    private float speed = 0;


    public void ResetScore()
    {
        score = 0;
        speed = 0;
        UpdateScoreText();
    }
    
    public void SetScore(float value)
    {
        score = value;
        UpdateScoreText();
    }

    public void IncreaseScore(float value)
    {
        score += value;
        UpdateScoreText();
    }
    
    private void UpdateScoreText()
    {
        ScoreText.text = score.ToString();
    }

    private void UpdateSpeedText()
    {
        SpeedText.text = speed.ToString();
    }


    public void SetSpeed(float value)
    {
        speed = value;
        UpdateSpeedText();
    }



    public void SetStatus(string text)
    {
        StatusText.text = text;
    }

    public Text ScoreText, StatusText, SpeedText;



}
