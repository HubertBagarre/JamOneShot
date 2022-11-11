using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreDisplayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI playerNameText;
    
    void Start()
    {
        UpdateScore(0);
    }

    public void Activate(bool value,int index)
    {
        gameObject.SetActive(value);
        playerNameText.text = $"Player {index}";
    }
    
    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
    }
}
