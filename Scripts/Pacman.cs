using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PacmanController))]

public class Pacman : MonoBehaviour
{
    [SerializeField] private EatSpawner _eatSpawner;

    private PacmanController _pacmanController;
    private Animator _animator;
    private int _score;

    public event UnityAction GameOver;
    public event UnityAction<int> ScoreChanged;

    private void Start()
    {
        _pacmanController = GetComponent<PacmanController>();
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Ghost ghost)) // если пакман сталкивается с призраком
        {
            _pacmanController.OnDeathPackman(); // вызываем метод смерти пакмана
            _animator.SetTrigger("Death");
            StartCoroutine(DeathPacman());
        }

        if (collision.TryGetComponent(out Eat eat)) // если пакман сталкивается с едой
        {
            Destroy(eat.gameObject); // еда уничтожается
            _score++; // увеличиваем количество очков
            ScoreChanged?.Invoke(_score); // вызываем событие для отображения очков

            if (_score >= _eatSpawner.EatsCount)
                GameOver?.Invoke();
        }
    }

    private IEnumerator DeathPacman()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(1.5f);

        yield return waitForSeconds;

        GameOver?.Invoke(); // выходит окно окончания игры
    }
}
