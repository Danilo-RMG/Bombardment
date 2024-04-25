using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
  private static readonly int SCORE_FACTOR = 10;
  [SerializeField] private TextMeshProUGUI scoreLabel;
  [SerializeField] private TextMeshProUGUI highestScoreLabel;
   void Start()
    {
     scoreLabel.text = GetScoreString();
     highestScoreLabel.text = GetHighestScoreString();
    }
   void Update()
    {
     scoreLabel.text = GetScoreString();
     highestScoreLabel.text = GetHighestScoreString();   
    }
   private string GetScoreString()
    {
     return (GameManager.Instance.GetScore() * SCORE_FACTOR).ToString();
    }
   private string GetHighestScoreString()
    {
     return (GameManager.Instance.GetHighestScore() * SCORE_FACTOR).ToString();
    }
}
