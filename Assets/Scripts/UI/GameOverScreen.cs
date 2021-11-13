using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private Pacman _pacman;
    [SerializeField] private TMP_Text _gameOverText;

    private void OnEnable()
    {
        _pacman.GameOver += OnGameOver;
    }

    private void OnDisable()
    {
        _pacman.GameOver -= OnGameOver;
    }

    private void Start()
    {
        _gameOverText.gameObject.SetActive(false);
    }

    private void OnGameOver()
    {
        _gameOverText.gameObject.SetActive(true);
        Time.timeScale = 0;
    }
}
