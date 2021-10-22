using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private float _delayInSeconds;
    [SerializeField] private List<RotationInterval> _rotationIntervals;

    private int _currentIndex;
    private float _passedTime;
    private bool _active;

    private void Start()
    {
        Activate();
    }

    private void Update()
    {
        if (!_active)
            return;

        transform.Rotate(_rotationIntervals[_currentIndex].Angle * _rotationIntervals[_currentIndex].Speed * Time.deltaTime);

        _passedTime += Time.deltaTime;
        if (_rotationIntervals[_currentIndex].TimeSeconds <= _passedTime)
        {
            _passedTime = 0;
            if (_currentIndex == _rotationIntervals.Count - 1)
                _currentIndex = 0;
            else
                _currentIndex++;
        }
    }

    private async void Activate()
    {
        await Task.Delay((int)(_delayInSeconds * 1000));
        _active = true;
    }

}
