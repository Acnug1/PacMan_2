using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Animator))]

public class PacmanController : MonoBehaviour, ICustomUpdatable
{
    [SerializeField] private Tilemap _level;
    [SerializeField] private SpriteRenderer _sprite;

    private Vector3Int celledPosition; // позиция ячейки
    private Vector3Int _direction = Vector3Int.right; // зададим начальное направление;
    private bool isDead = false;

    private void Start()
    {
        celledPosition = _level.WorldToCell(transform.position); // помещаем текущую позицию пакмана в позицию ячейки (работаем с координатами сетки)
        transform.position = celledPosition + new Vector3(0.5f, 0.5f); // перемещаем в эту ячейку нашего пакмана и делаем оффсет на 0.5f
    }

    private void Update() // каждый кадр отслеживаем нажатие клавиши пользователем
    {
        if (Input.GetKeyDown(KeyCode.W) && !isDead) // задаем направление для перемещения
            _direction = Vector3Int.up;
        else if (Input.GetKeyDown(KeyCode.S) && !isDead)
            _direction = Vector3Int.down;
        else if (Input.GetKeyDown(KeyCode.D) && !isDead)
            _direction = Vector3Int.right;
        else if (Input.GetKeyDown(KeyCode.A) && !isDead)
            _direction = Vector3Int.left;
    }

    private void ApplyDirection() // применяем направление для пакмана (после окончания предыдущей итерации CustomUpdate)
    {
        if (_direction.x >= 0) // если его позиция перемещения больше или равна нулю
        {
            _sprite.flipX = true; // отражаем его спрайт по оси Х
        }
        else
        {
            _sprite.flipX = false; // иначе оставляем все как есть
        }

        //UP - 0, 1, 0
        //1) 0, 1, 0 ==  90
        //2) 0, -1, 0 == -90
        //3) 0, 0, 0 == 0
        transform.rotation = Quaternion.Euler(0, 0, 90f * _direction.y); // если пакман движется вверх или вниз задаем угол поворота пакмана по оси Z на -90, 0 или 90
    }

    public void CustomUpdate() // реализуем интерфейс CustomUpdate
    {
        ApplyDirection(); // применяем выбранное направление

        var nextPosition = celledPosition + _direction; // находим позицию ячейки при следующем перемещении в данном направлении

        if (_level.GetTile(nextPosition) == null) // если позиция ячейки на следующем шаге свободна для перемещения (там нет препятствия)
        {
            celledPosition = nextPosition; // если препятствий нет задаем новую позицию для ячейки перемещения pacman
            transform.position = celledPosition + new Vector3(0.5f, 0.5f); // перемещаем в эту ячейку нашего пакмана и делаем оффсет на 0.5f
        }
    }

    public void OnDeathPackman()
    {
        isDead = true;
        _direction = Vector3Int.zero; // он останавливается
    }
}
