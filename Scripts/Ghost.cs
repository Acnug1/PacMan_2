using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Ghost : MonoBehaviour, ICustomUpdatable
{
    [SerializeField] private Tilemap _level;
    [SerializeField] private Pacman _pacman;

    public void CustomUpdate() // каждую итерацию мы шагаем в нашу точку
    {
        transform.position = GetNextPositionInPathToTarget(_level.WorldToCell(_pacman.transform.position)) + new Vector3(0.5f, 0.5f); // задает позицию для перемещения к пакману (позицию пакмана указываем внутри ячейки сетки)
    }

    // каждый кадр строится путь от пакмана к призраку и призрак движется по одной точке этого построенного пути
    private Vector3Int GetNextPositionInPathToTarget(Vector3Int target) // возвращает точку в которую мы должны переместиться, чтобы попасть к этой точке
    {
        var _startPosition = _level.WorldToCell(transform.position); // каждый кадр получаем стартовую позицию призрака в ячейке сетки
        var _openList = new Queue<Vector3Int>(); // создаем очередь, это открытый список (если заменим на Stack получим алгоритм поиска в глубину, т.е. он работает в обратном порядке)
        var _closedList = new List<Vector3Int>(); // список (динамический массив) - это закрытый список. Туда мы помещаем ноды, в которых мы уже были, чтобы не ходить по одним и тем же путям

        _openList.Enqueue(target); // добавляем в очередь нашу цель (открытый список). Начинаем путь от цели (пакмана)
        _closedList.Add(target); // в закрытый список добавляем цель пакмана (чтобы заново не попасть в эту точку)

        while (_openList.Count > 0) // пока очередь не пустая (пока есть точки в открытом списке), т.е. мы не нашли путь к призраку
        {
            var currentPosition = _openList.Dequeue(); // достаем из нашей очереди первый элемент и излекаем его оттуда в текущую позицию (строим путь к призраку)
     
            for (int x = -1; x <= 1; x++) // проверяем точки слева, справа
            {
                for (int y = -1; y <= 1; y++) // сверху, снизу
                {
                    if (x != 0 && y != 0) // кроме диагонали
                        continue;

                    var reserchedPoint = currentPosition + new Vector3Int(x, y, 0); // генерируем точку рядом (точка для изучения)

                    if (_level.GetTile(reserchedPoint) != null) // если в reserchedPoint находится припятствие
                        continue; // пропускаем эту точку

                    if (_closedList.Contains(reserchedPoint)) // если эта точка находится в закрытом списке (мы посещали её раньше)
                        continue; // пропускаем эту точку

                    if (reserchedPoint == _startPosition) // если эта точка находится в позиции призрака, то есть мы достигли цели
                        return currentPosition; // если путь найден возвращаем текущую позицию

                    _openList.Enqueue(reserchedPoint); // продолжаем строить путь и добавляем в очередь нашу исследуемую точку
                    _closedList.Add(reserchedPoint); // добавляем эту точку в закрытый список, чтобы мы больше её не посещали
                }
            }
        }

        return _startPosition; // если путь не найден возвращаем стартовую позицию
    }
}
