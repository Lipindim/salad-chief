
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;


public class BallActivator: MonoBehaviour
{
    [SerializeField] private float _spawnDelayInSenond = 3;
    [SerializeField] private float _leftAndRightTargetAngle;
    [SerializeField] private Transform[] _targets = new Transform[3];
    [SerializeField] private NewBall[] _balls;
    [SerializeField] private float _xOffset;
    [SerializeField] private float _yOffset;
    

    private int _currentBallIndex = 0;
    
    private void Start()
    {
        foreach (var ball in _balls)
        {
            ball.SetTarget(_targets.Select(x => x.position).ToArray(), _leftAndRightTargetAngle);
            ball.SetOffsets(_xOffset, _yOffset);
        }

        _balls[_currentBallIndex].gameObject.SetActive(true);
        _balls[_currentBallIndex].Throwed += OnBallThrowed;
    }

    private async void OnBallThrowed()
    {
        await Task.Delay((int)(_spawnDelayInSenond * 1000));
        _currentBallIndex++;
        if (_currentBallIndex == _balls.Length)
            return;

        _balls[_currentBallIndex].gameObject.SetActive(true);
        _balls[_currentBallIndex].Throwed += OnBallThrowed;
    }
}

