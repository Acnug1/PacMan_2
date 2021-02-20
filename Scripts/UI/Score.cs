using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    [SerializeField] private Pacman _pacman;
    [SerializeField] private EatSpawner _eatSpawner;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _maxScoreText;

    private void OnEnable()
    {
        _pacman.ScoreChanged += OnScoreChanged;
    }

    private void OnDisable()
    {
        _pacman.ScoreChanged -= OnScoreChanged;
    }

    private void Start()
    {
        _scoreText.text = "0";
        _maxScoreText.text = _eatSpawner.EatsCount.ToString();
    }

    private void OnScoreChanged(int score)
    {
        _scoreText.text = score.ToString();
    }
}
