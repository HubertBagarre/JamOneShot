using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private Image bar;
    public static float maxScore;

    void Start()
    {
        UpdateScore(0);
    }

    public void Activate(bool value,int index,Color color)
    {
        gameObject.SetActive(value);
        playerNameText.text = $"Player {index}";
        bar.color = color;
    }
    
    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
        bar.fillAmount = (score+1)/(maxScore+1);
    }
}
