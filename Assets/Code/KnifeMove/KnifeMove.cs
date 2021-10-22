using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KnifeMove : MonoBehaviour
{
    private const float DELTA_POSTITION = 0.01f;


    [SerializeField] private Transform _knifeTransform;
    [Tooltip("True - нож начнет движение из _positions[0]\nFalse - нож начнет движение из позиции в эдиторе")]
    [SerializeField] private bool _startMovingFromPositionZero;

    [SerializeField] private bool _isMoving;

    [Tooltip("First field - time intervals, second field - dynamic speed")]
    [SerializeField] private List<SpeedInterval> _speedList;

    private float _speed;
    private float _time;
    private Vector3 _lastPosition;
    int _index = 0;
    int _indexInList;

    private void Start()
    {
        if (_startMovingFromPositionZero)
            _knifeTransform.position = _speedList.First().Position;

        _speed = _speedList.First().Speed;
        _lastPosition = _knifeTransform.position;
        _index = 1;
    }
    private void Update()
    {
        if (!_isMoving)
            return;
        //Считаем новую позицию
        float x = (_speedList[_index].Position.x - _lastPosition.x) * _speed * Time.deltaTime + _knifeTransform.position.x;
        float y = (_speedList[_index].Position.y - _lastPosition.y) * _speed * Time.deltaTime + _knifeTransform.position.y;
        float z = (_speedList[_index].Position.z - _lastPosition.z) * _speed * Time.deltaTime + _knifeTransform.position.z;

        //Двигаем объъект
        _knifeTransform.position = new Vector3(x, y, z);

        //Проверка на прибытие в точку
        if (Vector3.Distance(_knifeTransform.position, _speedList[_index].Position) < DELTA_POSTITION)
        {
            _lastPosition = _speedList[_index].Position;
            _index++;
            if (_index >= _speedList.Count) //Проверка на исключение выхода за рамки массива
                _index = 0;
            _speed = _speedList[_index].Speed;
        }
    }
}
