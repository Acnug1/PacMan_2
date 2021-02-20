using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loop : MonoBehaviour
{
    [SerializeField] private float _secondsPerUpdate; // задержка между каждым обновлением (количество секунд между вызовом CustomUpdate)

    private IEnumerable<ICustomUpdatable> _updatables; // перечисление реализаций интерфейса ICustomUpdatable (и Pacman и Ghosts реализуют этот интерфейс для использования одного и того же кастомного Update)
    private float _lastUpdateTime; // время, когда мы последний раз вызывали CustomUpdate

    private void Start()
    {
        _updatables = GetComponentsInChildren<ICustomUpdatable>(); // получаем все доступные реализации интерфейса ICustomUpdatable и записываем в перечислитель (реализуемые пакманом и призраками)
        _lastUpdateTime = Time.time; // запоминаем текущее время в качестве времени последнего вызова Update
    }

    private void Update()
    {
        if (Time.time - _lastUpdateTime >= _secondsPerUpdate) // если прошло больше времени, чем указанная нами задержка между кадрами
        {
            foreach (var updatable in _updatables) // для всех реализаций интерфейса ICustomUpdatable (пакмана и всех призраков)
                updatable.CustomUpdate(); // вызываем кастомный Update в качестве метода (в данном случае каждые _secondsPerUpdate секунды)

            _lastUpdateTime = Time.time; // запоминаем время последнего вызова Update
        }
    }
}
