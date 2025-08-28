using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class UIManager : MonoBehaviour
    {
        public TextMeshProUGUI scoreText;

        private void OnEnable()
        {
            GameManager.OnScoreChange += UpdateScore;
        }
        private void OnDisable()
        {
            GameManager.OnScoreChange -= UpdateScore;
        }
        private void UpdateScore(int score)
        {
            scoreText.text = $"Score: {score}";
        }
    }
}
