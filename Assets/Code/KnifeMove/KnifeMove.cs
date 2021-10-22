using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeMove : MonoBehaviour
{
    [SerializeField] private Transform _knifeTransform;
    [Tooltip("True - нож начнет движение из _positions[0]\nFalse - нож начнет движение из позиции в эдиторе")]
    [SerializeField] private bool _startMovingFromPositionZero;

    [SerializeField] private bool _isMoving;
    [SerializeField] private Vector3[] _positions = new Vector3[0];

    [Tooltip("First field - time intervals, second field - dynamic speed")]
    [SerializeField] private List<SpeedInterval> _speedList;

    private float _speed;
    private float _time;
    private Vector3 _lastPosition;
    int _index = 0;
    int _indexInList;

    private void Start()
    {
        if (_startMovingFromPositionZero) {
            _knifeTransform.position = _positions[0];
        }
            _lastPosition = _knifeTransform.position;
        StartCoroutine(SwitchSpeed());
    }
    IEnumerator SwitchSpeed()
    {
        while (_isMoving == true)
        {
            _speed = _speedList[_indexInList].Speed;
            yield return new WaitForSeconds(_speedList[_indexInList].Time);
            if (_indexInList >= _speedList.Count - 1)
            {
                _indexInList = 0;
            }
            else
            {
                _indexInList++;
            }
        }
    }
    private void Update()
    {
        if (_isMoving)
        {
            //Считаем новую позицию
            float x = (_positions[_index].x - _lastPosition.x) * _speed * Time.deltaTime + _knifeTransform.position.x;
            float y = (_positions[_index].y - _lastPosition.y) * _speed * Time.deltaTime + _knifeTransform.position.y;
            float z = (_positions[_index].z - _lastPosition.z) * _speed * Time.deltaTime + _knifeTransform.position.z;

            //Двигаем объъект
            _knifeTransform.position = new Vector3(x,y,z);

            //Проверка на прибытие в точку
            if (Vector3.Distance(_knifeTransform.position,_positions[_index]) < 0.2f)
            {
                _lastPosition = _positions[_index];
                _index++;
                if (_index >= _positions.Length) //Проверка на исключение выхода за рамки массива
                {
                    _index = 0;
                }
            }
        }
    }
}
